using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class ModeloObj
    {
        public Guid Id { get; set; }
        public Guid IdPai { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool Marcado { get; set; }
        public int valorInt { get; set; }

        public string CodigoDescricao 
        {
            get 
            {
                string ret = string.Empty;

                if (!string.IsNullOrEmpty(this.Codigo) && !string.IsNullOrEmpty(this.Descricao))
                    ret = $"{this.Codigo} - {this.Descricao}";
                else if (string.IsNullOrEmpty(this.Codigo) && !string.IsNullOrEmpty(this.Descricao))
                    ret = this.Descricao;
                else if (!string.IsNullOrEmpty(this.Codigo) && string.IsNullOrEmpty(this.Descricao))
                    ret = this.Codigo;

                return ret;
            } 
        }

        public ModeloObj()
        {
        }
         
        public ModeloObj(Guid id, string codigo, string descricao)
        {
            this.Id = id;
            this.Codigo = codigo;
            this.Descricao = descricao;
        }

        public ModeloObj(Guid id, string codigo, string descricao, Guid idPai) : this(id, codigo, descricao)
        {
            this.IdPai = idPai;
        }

        public ModeloObj(Guid id, string codigo, string descricao, bool marcado) : this(id, codigo, descricao)
        {
            this.Marcado = marcado;
        }

        public override string ToString()
        {
            return string.Concat(this.Id.ToString(), " - ", this.CodigoDescricao);
        }
    }
}
