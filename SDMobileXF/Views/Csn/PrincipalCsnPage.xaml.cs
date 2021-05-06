using SDMobileXF.Banco.Tabelas;
using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalCsnPage : ContentPage
    {
        protected ViewModelPricipal ViewModel => this.BindingContext as ViewModelPricipal;

        public PrincipalCsnPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");

            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelPricipal(this.MenuSelecionado);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await this.ViewModel.LoadAsync();
            App.Log("PrincipalCsnPage OnAppearing");

            await Task.Run(async () =>
            {
                if (Util.TemAcessoInternet)
                {
                    App.Log("Verificando registros cadastrados no modo offline");
                    if (!OffLine.Instancia.SincronizandoOcorrencias)
                        await OffLine.Instancia.SincronizarOcorrenciasCsnAsync();
                }
            });
        }

        private async void MenuSelecionado(ItemMenu menu)
        {
            if (menu != null)
            {
                if (menu.Codigo == "sobre")
                {
                    await this.Navigation.PushAsync(new SobrePage());
                }
                else if (menu.Codigo == "config")
                {
                    await this.Navigation.PushAsync(new ConfigPage());
                }                
                else if (menu.Codigo == "sair")
                {
                    Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new LoginPage());
                    Xamarin.Forms.Application.Current.MainPage.SetDynamicResource(Xamarin.Forms.VisualElement.StyleProperty, "EstiloNavigationPage");
                }
            }
        }

        private void lvMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as Xamarin.Forms.ListView).SelectedItem = null;
        }

        private void RegistrarDNA_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new OcorrenciasCsnPage());
        }

        private void AcompanharDNA_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new OcorrenciasCsnTabbedPage());
        }

        private void Sair_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new LoginPage());
            Xamarin.Forms.Application.Current.MainPage.SetDynamicResource(Xamarin.Forms.VisualElement.StyleProperty, "EstiloNavigationPage");
        }
    }
}