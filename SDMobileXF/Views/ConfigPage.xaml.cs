using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using SDMobileXFDados.Modelos;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage : ContentPage
    {
        protected ViewModelConfig ViewModel => this.BindingContext as ViewModelConfig;

        public ConfigPage()
        {
            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelConfig(this.EstiloSelecionado, this.TamanhoSelecionado, this.IdiomaSelecionado);
            //if (Device.RuntimePlatform == Device.UWP)
            //    this.imgIdioma.Margin = new Thickness(0, 22, 0, 0);
            //this.imgIdioma.HeightRequest = 32;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await this.ViewModel.LoadAsync();
        }

        private void EstiloSelecionado(ModeloObj estilo)
        {
            if (estilo != null && Config.Estilo != estilo.Codigo)
            {
                Config.Estilo = estilo.Codigo;
                App app = Application.Current as SDMobileXF.App;
                app.AplicarEstilo();
            }
        }

        private void TamanhoSelecionado(ModeloObj tam)
        {
            if (tam != null && Config.TamanhoFonte != tam.Codigo)
            {
                Config.TamanhoFonte = tam.Codigo;
                App app = Application.Current as SDMobileXF.App;
                app.AplicarEstilo();
            }
        }

        private async void IdiomaSelecionado(Idioma idioma)
        {
            if (idioma != null && Globalizacao.CodigoIdiomaAtual != idioma.Codigo)
            {
                Globalizacao.CodigoIdiomaAtual = idioma.Codigo;
                await this.ViewModel.Atualizar();
            }
        }
    }
}