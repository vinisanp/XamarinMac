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
    public partial class PrincipalPage : ContentPage
    {
        protected ViewModelPricipal ViewModel => this.BindingContext as ViewModelPricipal;

        public PrincipalPage()
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
            App.Log("PrincipalPage OnAppearing");

            await Task.Run(async () =>
            {
                if (Util.TemAcessoInternet)
                {
                    App.Log("Verificando registros cadastrados no modo offline");
                    if (!OffLine.Instancia.SincronizandoOcorrencias)
                        await OffLine.Instancia.SincronizarOcorrenciasAsync();
                    if (!OffLine.Instancia.SincronizandoAbordagens)
                        await OffLine.Instancia.SincronizarAbordagensAsync();
                    if (!OffLine.Instancia.SincronizandoSnas)
                        await OffLine.Instancia.SincronizarSNAsAsync();
                    if (!OffLine.Instancia.SincronizandoInspecoes)
                        await OffLine.Instancia.SincronizarInspecoesAsync();
                    if (!OffLine.Instancia.SincronizandoOpas)
                        await OffLine.Instancia.SincronizarOpasAsync();
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
                else if (menu.Codigo == "regdna")
                {
                    await this.Navigation.PushAsync(new OcorrenciasPage());
                }
                else if (menu.Codigo == "acompdna")
                {
                    await this.Navigation.PushAsync(new OcorrenciasTabbedPage());
                }
                else if (menu.Codigo == "regort")
                {
                    await this.Navigation.PushAsync(new AbordagemPage());
                }
                else if (menu.Codigo == "acomport")
                {
                    await this.Navigation.PushAsync(new AbordagensTabbedPage());
                }
                else if (menu.Codigo == "regsna")
                {
                    await this.Navigation.PushAsync(new SNAPage());
                }
                else if (menu.Codigo == "acompsna")
                {
                    await this.Navigation.PushAsync(new SNATabbedPage());
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
            this.Navigation.PushAsync(new OcorrenciasPage());
        }

        private void AcompanharDNA_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new OcorrenciasTabbedPage());
        }

        private void RegistrarORT_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new AbordagemPage());
        }

        private void AcompanharORT_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new AbordagensTabbedPage());
        }

        private void RegistrarSNA_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new SNAPage());
        }

        private void AcompanharSNA_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new SNATabbedPage());
        }

        private void RegistrarInspecao_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new InspecaoPage());
        }

        private void AcompanharInspecao_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new InspecoesTabbedPage());
        }

        private void RegistrarOPA_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new OpaPage());
        }

        private void AcompanharOPA_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            this.Navigation.PushAsync(new OpasTabbedPage());
        }

        private void Sair_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.Ocupado = true;
            Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new LoginPage());
            Xamarin.Forms.Application.Current.MainPage.SetDynamicResource(Xamarin.Forms.VisualElement.StyleProperty, "EstiloNavigationPage");
        }
    }
}