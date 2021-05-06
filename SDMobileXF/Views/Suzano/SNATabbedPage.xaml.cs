using SDMobileXF.Classes;
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
    public partial class SNATabbedPage : TabbedPage
    {
        public SNATabbedPage()
        {
            InitializeComponent();
            this.Title = Textos.Instancia.SNAs;
            this.Children[0].Title = Textos.Instancia.Cadastradas;
            this.Children[1].Title = Textos.Instancia.Pendentes;
        }
    }
}