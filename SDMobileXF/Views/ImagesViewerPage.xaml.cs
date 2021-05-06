using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagesViewerPage : ContentPage
    {
        protected ImageSource _imageSource = null;

        public ImagesViewerPage(ImageSource imageSource)
        {
            InitializeComponent();
            this.Padding = Util.Padding;
            this._imageSource = imageSource;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            this.imgView.Source = this._imageSource;
        }
    }
}