using SDMobileXF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermoDeUsoPage : ContentPage
    {
        protected ViewModelTermoUso ViewModel => this.BindingContext as ViewModelTermoUso;
        public TermoDeUsoPage()
        {
            InitializeComponent();
            this.BindingContext = new ViewModelTermoUso();
        }

        private async void AbrirAplicativo(object sender, EventArgs e)
        {
            if (App.__nivelDeProjeto == "CSN")
                await this.Navigation.PushAsync(new MasterViewCsnPage());
            else if (App.__nivelDeProjeto == "Suzano")
                await this.Navigation.PushAsync(new MasterViewPage());
        }
    }
}