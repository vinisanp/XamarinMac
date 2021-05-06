using SDMobileXF.Classes;
using SDMobileXF.ViewModels;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace SDMobileXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PesquisarPage : ContentPage
    {
        private readonly Action<ModeloObj, Enumerados.Tabela> _retornoItemSelecionado;

        public ViewModelPesquisa ViewModel => this.BindingContext as ViewModelPesquisa;

        public PesquisarPage(Action<ModeloObj, Enumerados.Tabela> itemSelecionado, Enumerados.Tabela tabela, string cabecalho, string filtroIdPai)
        {
            Xamarin.Forms.Application.Current.On<Windows>().SetImageDirectory("Assets");
            InitializeComponent();            
            this.BindingContext = new ViewModelPesquisa(this.RetornoItemSelecionado, this.RetornoPesquisaEfetuada, tabela, cabecalho, filtroIdPai);
            this._retornoItemSelecionado = itemSelecionado;           
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();     
            
            await this.ViewModel.LoadAsync();
        }

        private async void RetornoItemSelecionado()
        {
            this._retornoItemSelecionado?.Invoke(this.ViewModel.Selecionado, this.ViewModel.Tabela);
            await this.Navigation.PopModalAsync();
        }

        private async void RetornoPesquisaEfetuada(string retorno)
        {
            if (!this.ViewModel.Lista.Any())
            {
                if (string.IsNullOrEmpty(retorno))
                    await DisplayAlert(this.ViewModel.Textos.Aviso, Globalizacao.Traduz("Nenhum registro encontrado com o filtro informado!"), this.ViewModel.Textos.Ok);
                else
                {
                    bool ok = await DisplayAlert(this.ViewModel.Textos.Aviso, retorno, this.ViewModel.Textos.Sim, this.ViewModel.Textos.Nao);
                    if (ok)
                        await this.ViewModel.ForcarProcurar(this.barraPesquisa.Text);
                }
            }
        }

        private void barraPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue) && e.OldTextValue?.Length > 1)            
                this.ViewModel.ProcurarCommand.Execute(string.Empty);            
        }

        private async void FecharTela(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }
    }
}