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
    public partial class OcorrenciasPage : ContentPage
    {
        private double _lastScrollY;

        protected ViewModelRelato ViewModel => this.BindingContext as ViewModelRelato;

        public OcorrenciasPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");

            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelRelato(this.RetornoSalvar, this.RetornoCancelar);
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

        public OcorrenciasPage(string idOcorrencia, OrigemDados origem)
        {
            InitializeComponent();
            this.Padding = Util.Padding;
            this.ToolbarItems.Clear();
            this.BindingContext = new ViewModelRelato(null, null);
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
            if (tabela == Enumerados.Tabela.UnidadeRegional)
            {
                if ((this.ViewModel.UnidadeRegional != null && obj != null && this.ViewModel.UnidadeRegional.Codigo != obj.Codigo) || obj == null)
                {
                    this.ViewModel.Gerencia = null;
                    this.ViewModel.Area = null;
                    this.ViewModel.Local = null;
                }
                this.ViewModel.UnidadeRegional = obj;
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
            else if (tabela == Enumerados.Tabela.Tipo)
                this.ViewModel.Tipo = obj;
            else if (tabela == Enumerados.Tabela.Classificacao)
            {
                if ((this.ViewModel.Classificacao != null && obj != null && this.ViewModel.Classificacao.Codigo != obj.Codigo) || obj == null)
                    this.ViewModel.SubClassificacao = null;
                this.ViewModel.Classificacao = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.SubClassificacao, RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.SubClassificacao)
            {
                this.ViewModel.SubClassificacao = obj;
                this.ViewModel.SelecionarItemFilho(obj, Enumerados.Tabela.Categoria, RetornoItemSelecionado);
            }
            else if (tabela == Enumerados.Tabela.Categoria)
                this.ViewModel.Categoria = obj;
            else if (tabela == Enumerados.Tabela.Fornecedor)
                this.ViewModel.Fornecedor = obj;
        }

        private void RetornoItemSelecionadoComunicadoPor(ModeloObj obj, Enumerados.Tabela tabela)
        {
            if (tabela == Enumerados.Tabela.Vinculo)
                this.ViewModel.ComunicadoPor = obj;
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
            if (this.ViewModel.UnidadeRegional == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione a unidade do desvio!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Gerencia,
                                                       this.ViewModel.Textos.GerenciaDoDesvio,
                                                       this.ViewModel.UnidadeRegional.Id.ToString());

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

        private async void btnTipo_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Tipo,
                                                   this.ViewModel.Textos.TipoDaOcorrencia,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnClassificacao_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Classificacao,
                                                   this.ViewModel.Textos.ClassificacaoDaOcorrencia,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnSubClassificacao_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.Classificacao == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione classificação da ocorrência!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.SubClassificacao,
                                                   this.ViewModel.Textos.SubClassificacaoDaOcorrencia,
                                                   this.ViewModel.Classificacao.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnCategoria_Clicked(object sender, EventArgs e)
        {
            if (this.ViewModel.SubClassificacao == null)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Selecione subclassificação da ocorrência!"), this.ViewModel.Textos.Ok);
            else
            {
                PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                       Enumerados.Tabela.Categoria,
                                                       this.ViewModel.Textos.CategoriaDaOcorrencia,
                                                       this.ViewModel.SubClassificacao.Id.ToString());

                await this.Navigation.PushModalAsync(page);
            }
        }

        private async void btnFornecedor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionado,
                                                   Enumerados.Tabela.Fornecedor,
                                                   this.ViewModel.Textos.Fornecedor,
                                                   null);

            await this.Navigation.PushModalAsync(page);
        }

        private async void btnComunicadoPor_Clicked(object sender, EventArgs e)
        {
            PesquisarPage page = new PesquisarPage(this.RetornoItemSelecionadoComunicadoPor,
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
                    FileInfo fi = new FileInfo(foto.Path);

                    ItemImagem img = new ItemImagem();
                    img.IdImagem = Guid.NewGuid();
                    img.Caminho = foto.Path;
                    img.Data = fi.CreationTime;
                    img.Image = foto.GetStreamWithImageRotatedForExternalStorage().ToByteArray();
                    foto.Dispose();
                    this.ViewModel.Imagens.Add(img);
                    this.Carousel.CurrentItem = img;
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

                if (foto == null)
                    return;

                ItemImagem img = new ItemImagem();
                img.IdImagem = Guid.NewGuid();
                img.Caminho = foto.Path;
                img.Image = foto.GetStreamWithImageRotatedForExternalStorage().ToByteArray();
                foto.Dispose();
                this.ViewModel.Imagens.Add(img);
            }
            catch (Exception ex)
            {

            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ImagesViewerPage page = new ImagesViewerPage(this.ViewModel.ImagemAtual.ImageSource);

            await this.Navigation.PushAsync(page);
        }

        private void Carousel_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            if (this.ViewModel.Imagens.Count > 0)
                this.ViewModel.ImagemAtual = this.ViewModel.Imagens[e.CurrentPosition];
        }
    }
}