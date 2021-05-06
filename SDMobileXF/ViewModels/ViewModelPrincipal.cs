using SDMobileXF.Banco.Tabelas;
using SDMobileXF.Classes;
using SDMobileXF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SDMobileXF.ViewModels
{
    public class ViewModelPricipal : ViewModelBase
    {
        private ItemMenu _menuSelecionado;
        private List<ItemMenu> _menu;
        private readonly Action<ItemMenu> _retornoItemSelecionado;

        public List<ItemMenu> Menu
        {
            get
            {
                return this._menu;
            }
            set
            {
                this.DefinirPropriedade(ref this._menu, value);
            }
        }

        public ItemMenu MenuSelecionado
        {
            get
            {
                return this._menuSelecionado;
            }
            set
            {
                this.DefinirPropriedade(ref this._menuSelecionado, value);

                this._retornoItemSelecionado?.Invoke(value);
            }
        }

        public string TextoBemVindo
        {            
            get => $"{Globalizacao.Traduz("Bem-vindo")} {UsuarioLogado.Instancia.NM_USUARIO}!";            
        }

        public bool TemAcessoAbordagem
        {
            get => UsuarioLogado.Instancia.TEM_ACESSO_ABORDAGEM;
        }

        public bool TemAcessoSNA
        {
            get => UsuarioLogado.Instancia.TEM_ACESSO_SNA;
        }

        public bool TemAcessoInspecoes
        {
            get => UsuarioLogado.Instancia.TEM_ACESSO_INSPECOES;
        }

        public bool TemAcessoOPA
        {
            //get => UsuarioLogado.Instancia.TEM_ACESSO_OPA;
            get => true;
        }

        public ViewModelPricipal(Action<ItemMenu> itemSelecionado)
        {
            this._retornoItemSelecionado = itemSelecionado;
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                List<ItemMenu> menu = new List<ItemMenu>();

                //menu.Add(new ItemMenu("acompSna", Globalizacao.Traduz("Acompanhe seus SNAs"), Globalizacao.Traduz("Ver SNAs cadastradas"), "sna.png"));

                menu.Add(new ItemMenu("dna",
                                        Globalizacao.Traduz("De Olho na Área"),
                                        Globalizacao.Traduz("Registrar ocorrências"),
                                        "dna.png",
                                        new List<ItemMenu>() 
                                            { 
                                                new ItemMenu() { Codigo = "regdna", Nome = "Registrar Ocorrência" },
                                                new ItemMenu() { Codigo = "acompdna", Nome = "Acompanhar Ocorrências" },
                                            }));
            
                menu.Add(new ItemMenu("acompOrt", Globalizacao.Traduz("Abordagem Comportamental"), Globalizacao.Traduz("Abordagem Comportamental"), "ort.png",
                                        new List<ItemMenu>()
                                            {
                                                new ItemMenu() { Codigo = "regort", Nome = "Registrar ORT" },
                                                new ItemMenu() { Codigo = "acomport", Nome = "Acompanhar ORTs" },
                                            }));
                menu.Add(new ItemMenu("sna", Globalizacao.Traduz("SNA"), Globalizacao.Traduz("Segurança na Área"), "sna.png",
                                        new List<ItemMenu>()
                                            {
                                                new ItemMenu() { Codigo = "regsna", Nome = "Registrar SNA" },
                                                new ItemMenu() { Codigo = "acompsna", Nome = "Acompanhar SNAs" },
                                            }));
                ////menu.Add(new ItemMenu("config", Globalizacao.Traduz("Configurações"), Globalizacao.Traduz("Ajustes das configurações"), "configuracoes.png", new List<string>()));
                ////menu.Add(new ItemMenu("sobre", Globalizacao.Traduz("Sobre"), Globalizacao.Traduz("Informações sobre o aplicativo"), "informacoes.png"));
                menu.Add(new ItemMenu("sair", Globalizacao.Traduz("Sair"), Globalizacao.Traduz("Voltar para a tela inicial"), "sair.png", null));

                this.Menu = menu;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
