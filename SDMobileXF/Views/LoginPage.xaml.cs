using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using SDMobileXFDados;
using System.Threading.Tasks;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        protected ViewModelLogin ViewModel => this.BindingContext as ViewModelLogin;

        public LoginPage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelLogin(this.RetornoLogin);

            if (Util.UWP)
                this.Title = this.ViewModel.Titulo + " - " + App.__nivelDeProjeto + " - " + App.__EnderecoWebApi;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await this.ViewModel.LoadAsync();

            if (!App.Sincronizado && Util.TemAcessoInternet)
            {
                App.Sincronizado = true;
                if(App.__nivelDeProjeto == "CSN")
                    await OffLine.Instancia.SincronizarTabelasBaseCsnAsync();
                else
                    await OffLine.Instancia.SincronizarTabelasBaseSuzanoAsync();
            }
        }

        private async void RetornoLogin(Enumerados.StatusLogin status)
        {
            if (status == Enumerados.StatusLogin.LoginAutorizado)
            {
                if (Config.TermoDeUsoApp)
                {
                    if (App.__nivelDeProjeto == "CSN")
                        await this.Navigation.PushAsync(new MasterViewCsnPage());
                    else if (App.__nivelDeProjeto == "Suzano")
                        await this.Navigation.PushAsync(new MasterViewPage());
                }
                else
                    await this.Navigation.PushAsync(new TermoDeUsoPage());
            }
            else if (status == Enumerados.StatusLogin.UsuarioInativo)
                await this.DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("O período de acesso para este usuário expirou."), this.ViewModel.Textos.Ok);
            else
                await this.DisplayAlert(this.ViewModel.Textos.Erro, Globalizacao.Traduz("Usuário ou senha inválidos."), this.ViewModel.Textos.Ok);
        }

        private void VisualizarSenha_Clicked(object sender, System.EventArgs e)
        {
            this.txtSenha.IsPassword = this.txtSenha.IsPassword ? false : true;
            this.imgOlho.Source = this.txtSenha.IsPassword ? "olhoaberto.png" : "olhofechado.png";
            if (!string.IsNullOrEmpty(this.txtSenha.Text))
                this.txtSenha.CursorPosition = this.txtSenha.Text.Length;
        }
    }
}