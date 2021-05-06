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
using System.Collections.ObjectModel;
using System.Collections;
using SDMobileXF.Banco.Tabelas;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbordagemPage : ContentPage
    {
        private double _lastScrollY;

        protected ViewModelAbordagem ViewModel => this.BindingContext as ViewModelAbordagem;

        public AbordagemPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");

            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelAbordagem(this.RetornoSalvar, this.RetornoCancelar);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHora.Format = Globalizacao.FormatoHora;

            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                    await this.ViewModel.LoadAsync();
                this.AjustarListView(this.LstCognitivos);
                this.AjustarListView(this.LstFisiologicos);
                this.AjustarListView(this.LstPsicologicos);
                this.AjustarListView(this.LstSociais);
            };
        }

        public AbordagemPage(string idAbordagem, OrigemDados origem)
        {
            InitializeComponent();
            this.Padding = Util.Padding;
            this.ToolbarItems.Clear();
            this.BindingContext = new ViewModelAbordagem(null, null);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHora.Format = Globalizacao.FormatoHora;
            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                {
                    await this.ViewModel.LoadAsync();
                    this.CarregarAbordagem(idAbordagem, origem);
                }
                this.AjustarListView(this.LstCognitivos);
                this.AjustarListView(this.LstFisiologicos);
                this.AjustarListView(this.LstPsicologicos);
                this.AjustarListView(this.LstSociais);
            };
        }

        public void CarregarAbordagem(string idAbordagem, OrigemDados origem)
        {
            if (origem == OrigemDados.Api && !Util.TemAcessoInternet)
            {
                string msg = Globalizacao.Traduz("Não foi possível obter os dados.");
                msg = string.Concat(Environment.NewLine, msg, Globalizacao.Traduz("Verifique se o dispositivo possui conexão com a internet."));
                DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
            }
            else
                this.ViewModel.GetAbordagem(idAbordagem, origem);
        }

        private async void RetornoSalvar(bool ok, string msg)
        {
            if (ok)
            {
                await this.scrollORT.ScrollToAsync(0, 0, true);
                if (string.IsNullOrEmpty(this.ViewModel.Numero))
                    msg = string.Format(Globalizacao.Traduz("Abordagem Comportamental salva com sucesso!"), this.ViewModel.Numero);
                else
                    msg = string.Format(Globalizacao.Traduz("Abordagem Comportamental '{0}' salva com sucesso!"), this.ViewModel.Numero);
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
                if ((this.ViewModel.UnidadeAbordagem != null && obj != null && this.ViewModel.UnidadeAbordagem.Codigo != obj.Codigo) || obj == null)
                {
                    this.ViewModel.GerenciaAbordagem = null;
                    this.ViewModel.AreaAbordagem = null;
                    this.ViewModel.LocalAbordagem = null;
                }
                this.ViewModel.UnidadeAbordagem = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Gerencia, this.RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Gerencia)
            {
                if ((this.ViewModel.GerenciaAbordagem != null && obj != null && this.ViewModel.GerenciaAbordagem.Codigo != obj.Codigo) || obj == null)
                {
                    this.ViewModel.AreaAbordagem = null;
                    this.ViewModel.LocalAbordagem = null;
                }
                this.ViewModel.GerenciaAbordagem = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Area, this.RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Area)
            {
                if ((this.ViewModel.AreaAbordagem != null && obj != null && this.ViewModel.AreaAbordagem.Codigo != obj.Codigo) || obj == null)
                    this.ViewModel.LocalAbordagem = null;
                this.ViewModel.AreaAbordagem = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Local, this.RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Local)
                this.ViewModel.LocalAbordagem = obj;
            else if (tabela == Enumerados.Tabela.Fornecedor)
                this.ViewModel.Fornecedor = obj;
        }

        private void RetornoItemSelecionadoObservador(ModeloObj obj, Enumerados.Tabela tabela)
        {
            this.ViewModel.Observador = obj;
        }

        private void RetornoItemSelecionadoRegistradoPor(ModeloObj obj, Enumerados.Tabela tabela)
        {
            this.ViewModel.RegistradoPor = obj;
        }

        private async void btnFornecedor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Fornecedor,
                                                   this.ViewModel.Textos.EmpresaObservada,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnUnidade_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.UnidadeRegional,
                                                   this.ViewModel.Textos.UnidadeAbordagem,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnGerencia_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.UnidadeAbordagem == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a unidade da abordagem!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Gerencia,
                                                       this.ViewModel.Textos.GerenciaAbordagem,
                                                       this.ViewModel.UnidadeAbordagem.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnArea_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.GerenciaAbordagem == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a gerência da abordagem!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Area,
                                                       this.ViewModel.Textos.AreaAbordagem,
                                                       this.ViewModel.GerenciaAbordagem.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnLocal_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.AreaAbordagem == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a área da abordagem!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Local,
                                                       this.ViewModel.Textos.LocalAbordagem,
                                                       this.ViewModel.AreaAbordagem.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnObservador_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoObservador,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.ComunicadoPor,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnRegistradoPor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoRegistradoPor,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.RegistradoPor,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void scrollORT_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (this.scrollORT.ScrollY < this._lastScrollY &&
               this._lastScrollY > 1000 &&
               this.scrollORT.ScrollY > 0 &&
               (this.scrollORT.ScrollY / this._lastScrollY < .1))
            {
                await this.scrollORT.ScrollToAsync(0, this._lastScrollY, false);
            }
            this._lastScrollY = this.scrollORT.ScrollY;
        }

        private void AjustarListView(Xamarin.Forms.ListView lst)
        {
            if (lst != null && lst.ItemsSource != null)
            {
                double count = (lst.ItemsSource as IList).Count;
                int margin = 0;
                if (Device.RuntimePlatform != Device.UWP)
                    margin = 4;
                lst.HeightRequest = count * (lst.RowHeight + margin);
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as Xamarin.Forms.ListView).SelectedItem = null;
        }
    }
}