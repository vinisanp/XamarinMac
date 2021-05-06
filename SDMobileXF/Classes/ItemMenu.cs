using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXF.Classes
{
    public class ItemMenu
    {
        public ItemMenu() { }
        public ItemMenu(string codigo, string nome, string descricao, string imagem, List<ItemMenu> itensFilho)
        {
            this.Codigo = codigo;
            this.Nome = nome;
            this.Descricao = descricao;
            this.Imagem = imagem;
            this.ItensFilho = itensFilho;            
        }

        public string Codigo { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; }

        public string Imagem { get; set; }

        public List<ItemMenu> ItensFilho { get; }
    }
}
