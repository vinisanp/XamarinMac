using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SobrePage : ContentPage
    {
        protected ViewModelSobre ViewModel => this.BindingContext as ViewModelSobre;

        public SobrePage()
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();
            this.Padding = Util.Padding;
            this.BindingContext = new ViewModelSobre();
        }
    }
}