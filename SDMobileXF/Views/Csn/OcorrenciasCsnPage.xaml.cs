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

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OcorrenciasCsnPage : ContentPage
    {
        private double _lastScrollY;

        protected ViewModelRelatoCsn ViewModel => this.BindingContext as ViewModelRelatoCsn;

        public OcorrenciasCsnPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");

            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelRelatoCsn(this.RetornoSalvar, this.RetornoCancelar);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHora.Format = Globalizacao.FormatoHora;

            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                {
                    await this.ViewModel.LoadAsync();
                }
            };
        }

        public OcorrenciasCsnPage(string idOcorrencia, OrigemDados origem)
        {
            InitializeComponent();
            this.Padding = Util.Padding;
            this.ToolbarItems.Clear();
            this.BindingContext = new ViewModelRelatoCsn(null, null);
            this.Appearing += (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                    this.CarregarOcorrencia(idOcorrencia, origem);
            };
        }

        public void CarregarOcorrencia(string idOcorrencia, OrigemDados origem)
        {
            if (origem == OrigemDados.Api && !Util.TemAcessoInternet)
            {
                string msg = Globalizacao.Traduz("Não foi possível obter os dados.");
                msg = string.Concat(Environment.NewLine, msg, Globalizacao.Traduz("Verifique se o dispositivo possui conexão com a internet."));
                DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
            }
            else
                this.ViewModel.GetOcorrencia(idOcorrencia, origem);
        }

        private async void RetornoSalvar(bool ok, string msg)
        {
            if (ok)
            {
                await this.scrollRelato.ScrollToAsync(0, 0, true);
                if (string.IsNullOrEmpty(this.ViewModel.Numero))
                    msg = string.Format(Globalizacao.Traduz("Comunicado salvo com sucesso!"), this.ViewModel.Numero);
                else
                    msg = string.Format(Globalizacao.Traduz("Comunicado '{0}' salvo com sucesso!"), this.ViewModel.Numero);
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
            if (tabela == Enumerados.Tabela.Unidade)
                this.ViewModel.RegiaoSetor = obj;
            else if (tabela == Enumerados.Tabela.Letra)
                this.ViewModel.Letra = obj;
            else if (tabela == Enumerados.Tabela.TurnoAnomalia)
                this.ViewModel.Turno = obj;
            else if (tabela == Enumerados.Tabela.GerenciaGeralCsn)
            {
                this.ViewModel.GerenciaGeral = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.GerenciaCsn, RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.GerenciaCsn)
                this.ViewModel.Gerencia = obj;
            else if (tabela == Enumerados.Tabela.AreaEquipamento)
                this.ViewModel.AreaEquipamento = obj;
            else if (tabela == Enumerados.Tabela.LocalEquipamento)
                this.ViewModel.LocalEquipamento = obj;
            else if (tabela == Enumerados.Tabela.OrigemAnomalia)
                this.ViewModel.OrigemAnomalia = obj;
            else if (tabela == Enumerados.Tabela.TipoAnomalia)
                this.ViewModel.TipoAnomalia = obj;
            else if (tabela == Enumerados.Tabela.ClassificacaoTipo)
                this.ViewModel.ClassificacaoTipo = obj;
            else if (tabela == Enumerados.Tabela.Probabilidade)
                this.ViewModel.Probabilidade = obj;
            else if (tabela == Enumerados.Tabela.Severidade)
                this.ViewModel.Severidade = obj;
        }

        private void RetornoItemSelecionadoRegistradoPor(ModeloObj obj, Enumerados.Tabela tabela)
        {
            if (tabela == Enumerados.Tabela.Vinculo)
                this.ViewModel.RegistradoPor = obj;
        }
        private void RetornoItemSelecionadoRelatadoPor(ModeloObj obj, Enumerados.Tabela tabela)
        {
            if (tabela == Enumerados.Tabela.Vinculo)
                this.ViewModel.RelatadoPor = obj;
        }
        private void RetornoItemSelecionadoSupervisor(ModeloObj obj, Enumerados.Tabela tabela)
        {
            if (tabela == Enumerados.Tabela.Vinculo)
                this.ViewModel.SupervisorImediato = obj;
        }
        private void RetornoItemSelecionadoAssinatura(ModeloObj obj, Enumerados.Tabela tabela)
        {
            if (tabela == Enumerados.Tabela.Vinculo)
                this.ViewModel.Assinatura = obj;
        }

        private async void btnRegiaoSetor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Unidade,
                                                   this.ViewModel.Textos.RegiaoSetor,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnLetra_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Letra,
                                                   this.ViewModel.Textos.Letra,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnTurno_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.TurnoAnomalia,
                                                   this.ViewModel.Textos.Turno,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnGerenciaGeral_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.GerenciaGeralCsn,
                                                   this.ViewModel.Textos.GerenciaGeral,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnGerencia_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.GerenciaGeral == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione gerência geral!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.GerenciaCsn,
                                                       this.ViewModel.Textos.Gerencia,
                                                       this.ViewModel.GerenciaGeral.IdStrNullSafe());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnAreaEquipamento_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Gerencia == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione gerência!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.AreaEquipamento,
                                                       this.ViewModel.Textos.AreaEquipamento,
                                                       this.ViewModel.Gerencia.IdStrNullSafe());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnLocalEquipamento_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.AreaEquipamento == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a área!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.LocalEquipamento,
                                                       this.ViewModel.Textos.LocalEquipamento,
                                                       this.ViewModel.AreaEquipamento.IdStrNullSafe());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnOrigemAnomalia_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.OrigemAnomalia,
                                                   this.ViewModel.Textos.OrigemAnomalia,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnTipoAnomalia_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.TipoAnomalia,
                                                   this.ViewModel.Textos.TipoAnomalia,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnClassificacaoTipo_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.ClassificacaoTipo,
                                                   this.ViewModel.Textos.ClassificacaoTipo,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnProbabilidade_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Probabilidade,
                                                   this.ViewModel.Textos.Probabilidade,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnSeveridade_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Severidade,
                                                   this.ViewModel.Textos.Severidade,
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

        private async void btnRelatadoPor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoRelatadoPor,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.RelatadoPor,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnSupervisorImediato_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoSupervisor,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.SupervisorImediato,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnAssinatura_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoAssinatura,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.Assinatura,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void scrollRelato_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (this.scrollRelato.ScrollY < this._lastScrollY &&
               this._lastScrollY > 1000 &&
               this.scrollRelato.ScrollY > 0 &&
               (this.scrollRelato.ScrollY / this._lastScrollY < .1))
            {
                await this.scrollRelato.ScrollToAsync(0, this._lastScrollY, false);
            }
            this._lastScrollY = this.scrollRelato.ScrollY;
        }
    }
}