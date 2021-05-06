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
    public partial class VisualizarOcorrenciasSQLitePage : ContentPage
    {
        protected ViewModelVisualizarOcorrenciasSQLite ViewModel => this.BindingContext as ViewModelVisualizarOcorrenciasSQLite;

        public VisualizarOcorrenciasSQLitePage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();            
            this.BindingContext = new ViewModelVisualizarOcorrenciasSQLite(this.RetornoPesquisar, this.ConfirmarExcluir);
        }

        private async void RegistrosSincronizados(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e != null && e.ProgressPercentage > 0 && e.UserState.ToStringNullSafe() == this.ViewModel.Textos.Ocorrencias)
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
                this.Navigation.PushAsync(new OcorrenciasPage((e.SelectedItem as OCORRENCIA).ID_OCORRENCIA.ToString(), OrigemDados.SQLite));
                this.lstOcorrencias.SelectedItem = null;
                this.ViewModel.Ocupado = false;
            }
        }

        private async void RetornoPesquisar(bool ok, string msg)
        {
            if (!ok)            
                await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz(msg), this.ViewModel.Textos.Ok);
        }

        private async void ConfirmarExcluir(OCORRENCIA o)
        {
            string msg = "A ocorrência '{0}' da data {1} será excluida e não poderá mais ser sincronizada.";
            msg = string.Format(msg, o.DESCRICAO, o.DATA.ToString());
            msg = string.Concat(msg, Environment.NewLine, Globalizacao.Traduz("Deseja continuar assim mesmo?"));
            bool ok = await DisplayAlert(this.ViewModel.Textos.Aviso, msg, this.ViewModel.Textos.Sim, this.ViewModel.Textos.Nao);
            if (ok)
                await this.ViewModel.ExclusaoConfirmada(o);
        }
    }
}