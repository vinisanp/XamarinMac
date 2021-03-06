using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using System;
using static SDMobileXFDados.Enumerados;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizarOcorrenciasPage : ContentPage
    {
        protected ViewModelVisualizarOcorrencias ViewModel => this.BindingContext as ViewModelVisualizarOcorrencias;

        public VisualizarOcorrenciasPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();            
            this.BindingContext = new ViewModelVisualizarOcorrencias(RetornoPesquisar);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await this.ViewModel.LoadAsync();
            
            if (!Util.TemAcessoInternet)
            {
                string msg = Globalizacao.Traduz("Não foi possível acessar a lista de ocorrências.");
                msg = string.Concat(Environment.NewLine, msg, Globalizacao.Traduz("Verifique se o dispossitivo possui conexão com a internet."));
                await DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                this.ViewModel.Ocupado = true;
                this.Navigation.PushAsync(new OcorrenciasPage((e.SelectedItem as SDMobileXFDados.Modelos.Ocorrencia).ID_OCORRENCIA, OrigemDados.Api));
                this.lstOcorrencias.SelectedItem = null;
                this.ViewModel.Ocupado = false;
            }
        }

        private async void RetornoPesquisar(bool ok, string msg)
        {
            if (!ok)
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz(msg), this.ViewModel.Textos.Ok);            
        }
    }
}