using SDMobileXF.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbordagensTabbedPage : TabbedPage
    {
        public AbordagensTabbedPage()
        {
            InitializeComponent();
            this.Title = Textos.Instancia.Abordagens;
            this.Children[0].Title = Textos.Instancia.Cadastradas;
            this.Children[1].Title = Textos.Instancia.Pendentes;
        }
    }
}