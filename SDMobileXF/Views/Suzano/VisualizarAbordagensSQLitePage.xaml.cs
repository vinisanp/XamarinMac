using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using SDMobileXF.Banco.Tabelas;
using static SDMobileXFDados.Enumerados;
using Xamarin.Essentials;
using System;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizarAbordagensSQLitePage : ContentPage
    {
        protected ViewModelVisualizarAbordagensSQLite ViewModel => this.BindingContext as ViewModelVisualizarAbordagensSQLite;

        public VisualizarAbordagensSQLitePage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();            
            this.BindingContext = new ViewModelVisualizarAbordagensSQLite(this.RetornoPesquisar, this.ConfirmarExcluir);
        }

        private async void RegistrosSincronizados(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e != null && e.ProgressPercentage > 0 && e.UserState.ToStringNullSafe() == this.ViewModel.Textos.Abordagens)
                await this.ViewModel.LoadAsync();
        }

		protected override void OnDisappearing()
        {
            OffLine.Instancia.RegistrosSincronizados -= this.RegistrosSincronizados;
        }

		protected override async void OnAppearing()
        {
            base.OnAppearing();

            await this.ViewModel.LoadAsync();
            OffLine.Instancia.RegistrosSincronizados -= this.RegistrosSincronizados;
            OffLine.Instancia.RegistrosSincronizados += this.RegistrosSincronizados;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                this.ViewModel.Ocupado = true;
                this.Navigation.PushAsync(new AbordagemPage((e.SelectedItem as ABORDAGEM_COMPORTAMENTAL).ID_ABORDAGEM.ToString(), OrigemDados.SQLite));
                this.lstOcorrencias.SelectedItem = null;
                this.ViewModel.Ocupado = false;
            }
        }

        private async void RetornoPesquisar(bool ok, string msg)
        {
            if (!ok)            
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz(msg), this.ViewModel.Textos.Ok);
        }

        private async void ConfirmarExcluir(ABORDAGEM_COMPORTAMENTAL a)
        {
            string msg = "A abordagem comportamental '{0}' da data {1} será excluida e não poderá mais ser sincronizada.";
            msg = string.Format(msg, a.DESCRICAO, a.DATA.ToString());
            msg = string.Concat(msg, Environment.NewLine, Globalizacao.Traduz("Deseja continuar assim mesmo?"));
            bool ok = await DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Sim, this.ViewModel.Textos.Nao);
            if (ok)
                await this.ViewModel.ExclusaoConfirmada(a);
        }
    }
}