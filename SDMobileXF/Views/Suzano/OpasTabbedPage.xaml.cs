﻿using SDMobileXF.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpasTabbedPage : TabbedPage
    {
        public OpasTabbedPage()
        {
            InitializeComponent();
            this.Title = Textos.Instancia.Inspecoes;
            this.Children[0].Title = Textos.Instancia.Cadastradas;
            this.Children[1].Title = Textos.Instancia.Pendentes;
        }
    }
}