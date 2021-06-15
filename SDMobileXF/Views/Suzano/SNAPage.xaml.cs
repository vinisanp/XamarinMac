using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using static SDMobileXFDados.Enumerados;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Linq;
using System.Collections.Generic;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SNAPage : ContentPage
    {
        private double _lastScrollY;

        protected ViewModelSNA ViewModel => this.BindingContext as ViewModelSNA;
        private bool _checkAlterando;

        public SNAPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");

            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelSNA(this.RetornoSalvar, this.RetornoCancelar);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHorarioInicial.Format = Globalizacao.FormatoHora;
            //this.pickerHorarioFinal.Format = Globalizacao.FormatoHora;

            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                {
                    await this.ViewModel.LoadAsync();
                }
            };
        }

        public SNAPage(string id, OrigemDados origem)
        {
            InitializeComponent();
            this.Padding = Util.Padding;
            this.ToolbarItems.Clear();
            this.BindingContext = new ViewModelSNA(null, null);
            this.Appearing += (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                    this.Carregar(id, origem);
            };
        }

        public void Carregar(string id, OrigemDados origem)
        {
            if (origem == OrigemDados.Api && !Util.TemAcessoInternet)
            {
                string msg = Globalizacao.Traduz("Não foi possível obter os dados.");
                msg = string.Concat(Environment.NewLine, msg, Globalizacao.Traduz("Verifique se o dispositivo possui conexão com a internet."));
                DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
            }
            else
                this.ViewModel.GetSNA(id, origem);
        }


        private async void RetornoSalvar(bool ok, string msg)
        {
            if (ok)
            {
                await this.scroll.ScrollToAsync(0, 0, true);
                if (string.IsNullOrEmpty(this.ViewModel.Numero))
                    msg = string.Format(Globalizacao.Traduz("SNA salvo com sucesso!"), this.ViewModel.Numero);
                else
                    msg = string.Format(Globalizacao.Traduz("SNA '{0}' salvo com sucesso!"), this.ViewModel.Numero);
                await DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
                await this.ViewModel.LimparCampos();                
            }
            else
                await DisplayAlert(this.ViewModel.Textos.ErroAoSalvar, msg, this.ViewModel.Textos.Ok);
        }

        private async void RetornoCancelar()
        {
            bool ret = await DisplayAlert(this.ViewModel.Textos.Aviso,
                                          Globalizacao.Traduz("Dados não salvos serão perdidos. Deseja continuar?"),
                                          this.ViewModel.Textos.Sim,
                                          this.ViewModel.Textos.Nao);

            if (ret)
                await this.Navigation.PopAsync();
        }

        private void RetornoItemSelecionado(ModeloObj obj, Enumerados.Tabela tabela)
        {
            if (tabela == Enumerados.Tabela.UnidadeRegional)
            {
                if ((this.ViewModel.Unidade != null && obj != null && this.ViewModel.Unidade.Codigo != obj.Codigo) || obj == null)
                {
                    this.ViewModel.Gerencia = null;
                    this.ViewModel.Area = null;
                    this.ViewModel.Local = null;
                }
                this.ViewModel.Unidade = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Gerencia, RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Gerencia)
            {
                if ((this.ViewModel.Gerencia != null && obj != null && this.ViewModel.Gerencia.Codigo != obj.Codigo) || obj == null)
                {
                    this.ViewModel.Area = null;
                    this.ViewModel.Local = null;
                }
                this.ViewModel.Gerencia = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Area, RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Area)
            {
                if ((this.ViewModel.Area != null && obj != null && this.ViewModel.Area.Codigo != obj.Codigo) || obj == null)
                    this.ViewModel.Local = null;
                this.ViewModel.Area = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Local, RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Local)
                this.ViewModel.Local = obj;
        }

        private void RetornoItemSelecionadoRegistradoPor(ModeloObj obj, Enumerados.Tabela tabela)
        {
            if (tabela == Enumerados.Tabela.Vinculo)
                this.ViewModel.RegistradoPor = obj;
        }


        private async void btnUnidade_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.UnidadeRegional,
                                                   this.ViewModel.Textos.UnidadeRegional,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnGerencia_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Unidade == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a unidade do desvio!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Gerencia,
                                                       this.ViewModel.Textos.GerenciaDoDesvio,
                                                       this.ViewModel.Unidade.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnArea_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Gerencia == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a gerência do desvio!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Area,
                                                       this.ViewModel.Textos.AreaDoDesvio,
                                                       this.ViewModel.Gerencia.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnLocal_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Area == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a área do desvio!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Local,
                                                       this.ViewModel.Textos.LocalDoDesvio,
                                                       this.ViewModel.Area.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnRegistradoPor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoRegistradoPor,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.RegistradoPor,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void scroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (this.scroll.ScrollY < this._lastScrollY &&
               this._lastScrollY > 1000 &&
               this.scroll.ScrollY > 0 &&
               (this.scroll.ScrollY / this._lastScrollY < .1))
            {
                await this.scroll.ScrollToAsync(0, this._lastScrollY, false);
            }
            this._lastScrollY = this.scroll.ScrollY;
        }

        
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {            
            IEnumerable<View> lista = ((sender as CheckBox).Parent as StackLayout).Children.Where(i => i is CheckBox);
            foreach (View elem in lista)
            {
                CheckBox chk = elem as CheckBox;
                if (chk != sender as CheckBox)
                {
                    chk.CheckedChanged -= CheckBox_CheckedChanged;
                    chk.IsChecked = false;
                    chk.CheckedChanged += CheckBox_CheckedChanged;
                }
            }
        }
    }
}