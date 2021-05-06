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
    public partial class InspecaoPage : ContentPage
    {
        private double _lastScrollY;

        protected ViewModelInspecao ViewModel => this.BindingContext as ViewModelInspecao;

        public InspecaoPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();

            App.Log("Instanciando tela de Inspeções!");

            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelInspecao(this.RetornoSalvar, this.RetornoCancelar);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHora.Format = Globalizacao.FormatoHora;

            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                    await this.ViewModel.LoadAsync();

                App.Log("Final OnAppering tela de Inspeções!");
            };

        }

        public InspecaoPage(string id, OrigemDados origem)
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();

            App.Log("Instanciando tela de Inspeções!");

            this.Padding = Util.Padding;
            this.ToolbarItems.Clear();
            this.BindingContext = new ViewModelInspecao(null, null);
            this.pickerData.Format = Globalizacao.FormatoData;
            this.pickerHora.Format = Globalizacao.FormatoHora;

            this.Appearing += async (sender, e) =>
            {
                if (!this.ViewModel.TelaCarregada)
                {
                    await this.ViewModel.LoadAsync();
                    this.CarregarInspecao(id, origem);

                    App.Log("Final OnAppering tela de Inspeções!");
                    this.ViewModel.Ocupado = false;
                }
            };
        }

        public void CarregarInspecao(string id, OrigemDados origem)
        {
            if (origem == OrigemDados.Api && !Util.TemAcessoInternet)
            {
                string msg = Globalizacao.Traduz("Não foi possível obter os dados.");
                msg = string.Concat(Environment.NewLine, msg, Globalizacao.Traduz("Verifique se o dispositivo possui conexão com a internet."));
                DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
            }
            else
                this.ViewModel.GetInspecao(id, origem);
        }

        private async void RetornoSalvar(bool ok, string msg)
        {
            if (ok)
            {
                await this.scroll.ScrollToAsync(0, 0, true);
                if (string.IsNullOrEmpty(this.ViewModel.Numero))
                    msg = string.Format(Globalizacao.Traduz("Inspeção de Segurança salva com sucesso!"), this.ViewModel.Numero);
                else
                    msg = string.Format(Globalizacao.Traduz("Inspeção de Segurança '{0}' salva com sucesso!"), this.ViewModel.Numero);
                await this.DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
                await this.ViewModel.LimparCampos();
            }
            else
                await this.DisplayAlert(this.ViewModel.Textos.ErroAoSalvar, msg, this.ViewModel.Textos.Ok);
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
            else if (tabela == Enumerados.Tabela.Fornecedor)
                this.ViewModel.Fornecedor = obj;
            else if (tabela == Enumerados.Tabela.TipoInspecao)
                this.ViewModel.Tipo = obj;
            else if (tabela == Enumerados.Tabela.AtividadeInspecao)
                this.ViewModel.Atividade = obj;
        }

        private void RetornoItemSelecionadoRegistradoPor(ModeloObj obj, Enumerados.Tabela tabela)
        {
            this.ViewModel.RealizadoPor = obj;
        }

        private void RetornoItemSelecionadoParticipantes(ModeloObj obj, Enumerados.Tabela tabela)
        {
            this.ViewModel.AddParticipantes(obj);
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
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a unidade da abordagem!"), this.ViewModel.Textos.Ok);
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
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a gerência da abordagem!"), this.ViewModel.Textos.Ok);
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
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a área da abordagem!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Local,
                                                       this.ViewModel.Textos.Local,
                                                       this.ViewModel.Area.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnFornecedor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Fornecedor,
                                                   this.ViewModel.Textos.EmpresaObservada,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnTipoInspecao_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.TipoInspecao,
                                                   this.ViewModel.Textos.TipoInspecao,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnAtividade_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.AtividadeInspecao,
                                                   this.ViewModel.Textos.Atividade,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnParticipantes_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoParticipantes,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.Participantes,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnRegistradoPor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoRegistradoPor,
                                                   Enumerados.Tabela.Vinculo,
                                                   this.ViewModel.Textos.RealizadoPor,
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

            if (this.scroll.ScrollY + 250 >= this.scroll.ContentSize.Height - this.scroll.Height)
                this.ViewModel.AddCampo();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as Xamarin.Forms.ListView).SelectedItem = null;
        }

        private void MenuItemRemoverParticipante_Clicked(object sender, EventArgs e)
        {
            if (sender is MenuItem m && m.CommandParameter is ModeloObj p)
                this.ViewModel.Participantes.Remove(p);
        }

        private async void btnGaleria_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Galeria", "Acesso à galeria não disponível.", "OK");
                    return;
                }

                MediaFile foto = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                    CompressionQuality = 40
                });

                if (foto != null)
                {
                    if (((ImageButton)sender).CommandParameter is CampoInspecaoVm campo)
                    {
                        campo.Image = foto.GetStreamWithImageRotatedForExternalStorage().ToByteArray();
                        campo.CaminhoImagem = foto.Path;
                    }
                    foto.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void btnTirarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Nenhuma Câmera", "Nenhuma Câmera disponível.", "OK");
                    return;
                }

                StoreCameraMediaOptions armazenamento = new StoreCameraMediaOptions()
                {
                    SaveToAlbum = true,
                    Name = $"SDST_Mobile_{DateTime.Now.ToAAAAMMDD_HHMINSS()}.jpg",
                    DefaultCamera = CameraDevice.Rear,
                    CompressionQuality = 40,
                    PhotoSize = PhotoSize.Small
                };
                MediaFile foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(armazenamento);

                if (foto != null)
                {
                    if (((ImageButton)sender).CommandParameter is CampoInspecaoVm campo)
                    {
                        campo.Image = foto.GetStreamWithImageRotatedForExternalStorage().ToByteArray();
                        campo.CaminhoImagem = foto.Path;
                    }
                    foto.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (((ImageButton)sender).CommandParameter is CampoInspecaoVm campo)
                campo.Image = null;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ImagesViewerPage page = new ImagesViewerPage(this.ViewModel.Campos[0].ImageSource);
        }
    }
}