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
    public partial class VisualizarOpasPage : ContentPage
    {
        protected ViewModelVisualizarOpas ViewModel => this.BindingContext as ViewModelVisualizarOpas;

        public VisualizarOpasPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();            
            this.BindingContext = new ViewModelVisualizarOpas(RetornoPesquisar);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await this.ViewModel.LoadAsync();
            
            if (!Util.TemAcessoInternet)
            {
                string msg = Globalizacao.Traduz("Não foi possível acessar a lista de observações positivas florestais.");
                msg = string.Concat(Environment.NewLine, msg, Globalizacao.Traduz("Verifique se o dispositivo possui conexão com a internet."));
                await DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Ok);
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                this.ViewModel.Ocupado = true;
                this.Navigation.PushAsync(new OpaPage((e.SelectedItem as SDMobileXFDados.Modelos.Opa).ID_OPA, OrigemDados.Api));
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