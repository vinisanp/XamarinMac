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
using System.Linq;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpaPage : ContentPage
    {
        private double _lastScrollY;

        protected ViewModelOPA ViewModel => this.BindingContext as ViewModelOPA;

        public OpaPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();

            App.Log("Instanciando tela de OPA!");

            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelOPA(this.RetornoSalvar, this.RetornoCancelar);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHora.Format = Globalizacao.FormatoHora;

            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                    await this.ViewModel.LoadAsync();

                App.Log("Final OnAppering tela de OPA!");
            };

        }

        public OpaPage(string id, OrigemDados origem)
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();

            App.Log("Instanciando tela de OPA!");

            this.Padding = Util.Padding;
            this.ToolbarItems.Clear();
            this.BindingContext = new ViewModelOPA(null, null);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHora.Format = Globalizacao.FormatoHora;

            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                {
                    await this.ViewModel.LoadAsync();
                    this.CarregarOpa(id, origem);

                    App.Log("Final OnAppering tela de OPA!");
                    this.ViewModel.Ocupado = false;
                }
            };
        }

        public void CarregarOpa(string id, OrigemDados origem)
        {
            if (origem == OrigemDados.Api && !Util.TemAcessoInternet)
            {
                string msg = Globalizacao.Traduz("Não foi possível obter os dados.");
                msg = string.Concat(Environment.NewLine, msg, Globalizacao.Traduz("Verifique se o dispositivo possui conexão com a internet."));
                DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
            }
            else
                this.ViewModel.GetOpa(id, origem);
        }

        private async void RetornoSalvar(bool ok, string msg, string titulo)
        {
            if (ok)
            {
                await this.scroll.ScrollToAsync(0, 0, true);
                if (string.IsNullOrEmpty(this.ViewModel.Numero))
                    msg = string.Format(Globalizacao.Traduz("Observação Positiva Florestal salva com sucesso!"), this.ViewModel.Numero);
                else
                    msg = string.Format(Globalizacao.Traduz("Observação Positiva Florestal '{0}' salva com sucesso!"), this.ViewModel.Numero);
                await this.DisplayAlert(titulo, msg, this.ViewModel.Textos.Ok);
                await this.ViewModel.LimparCampos();
            }
            else
                await this.DisplayAlert(titulo, msg, this.ViewModel.Textos.Ok);
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
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Gerencia, this.RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Gerencia)
            {
                if ((this.ViewModel.Gerencia != null && obj != null && this.ViewModel.Gerencia.Codigo != obj.Codigo) || obj == null)
                {
                    this.ViewModel.Area = null;
                    this.ViewModel.Local = null;
                }
                this.ViewModel.Gerencia = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Area, this.RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Area)
            {
                if ((this.ViewModel.Area != null && obj != null && this.ViewModel.Area.Codigo != obj.Codigo) || obj == null)
                    this.ViewModel.Local = null;
                this.ViewModel.Area = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Local, this.RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Local)
                this.ViewModel.Local = obj;
            else if (tabela == Enumerados.Tabela.AtividadeInspecao)
                this.ViewModel.Atividade = obj;
            else if (tabela == Enumerados.Tabela.TarefaObservada)
                this.ViewModel.TarefaObservada = obj;
            else if (tabela == Enumerados.Tabela.Vinculo)
                this.ViewModel.Avaliador = obj;
            else if (tabela == Enumerados.Tabela.TipoAvaliador)
                this.ViewModel.TipoAvaliador = obj;
        }

        private async void btnUnidade_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.UnidadeRegional,
                                                   this.ViewModel.Textos.Unidade,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnGerencia_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Unidade == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a unidade!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Gerencia,
                                                       this.ViewModel.Textos.Gerencia,
                                                       this.ViewModel.Unidade.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnArea_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Gerencia == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a gerência!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Area,
                                                       this.ViewModel.Textos.Area,
                                                       this.ViewModel.Gerencia.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnLocal_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Area == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a área!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Local,
                                                       this.ViewModel.Textos.Local,
                                                       this.ViewModel.Area.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnAtividade_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.AtividadeInspecao,
                                                   this.ViewModel.Textos.Atividade,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnTarefaObservada_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.TarefaObservada,
                                                   this.ViewModel.Textos.Tarefa,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnAvaliador_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.Avaliador,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnTipoAvaliador_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.TipoAvaliador,
                                                   this.ViewModel.Textos.TipoAvaliador,
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

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as Xamarin.Forms.ListView).SelectedItem = null;
        }
	}
}