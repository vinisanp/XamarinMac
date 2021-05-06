using SDMobileXF.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterViewCsnPage : MasterDetailPage
    {
        public MasterViewCsnPage()
        {
            App.Log("MasterViewCsnPage");
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            this.lblBemVindo.Text = $"{Globalizacao.Traduz("Bem-vindo")} {UsuarioLogado.Instancia.NM_USUARIO}!";                
        }

        private void lvMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                this.IsPresented = false;
                ItemMenu menu = e.SelectedItem as ItemMenu;
                this.lvMenu.SelectedItem = null;

                if (menu.Codigo == "config")                    
                    this.Navigation.PushAsync(new ConfigPage());                    
                else if (menu.Codigo == "sobre")
                    this.Navigation.PushAsync(new SobrePage());
                else if (menu.Codigo == "sair")
                {
                    Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new LoginPage());
                    Xamarin.Forms.Application.Current.MainPage.SetDynamicResource(Xamarin.Forms.VisualElement.StyleProperty, "EstiloNavigationPage");
                }
            }
        }
    }
}