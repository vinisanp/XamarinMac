using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class CATEGORIA
    {
        [PrimaryKey]
        public Guid ID_CATEGORIA { get; set; }
        public string CD_CATEGORIA { get; set; }
        public string DS_CATEGORIA { get; set; }
        public Guid ID_SUBCLASSIFICACAO { get; set; }

        [Ignore]
        public string CD_DS_CATEGORIA { get { return string.Concat(this.CD_CATEGORIA.ToStringNullSafe(), " - ", this.DS_CATEGORIA.ToStringNullSafe()); } }

        public CATEGORIA() { }

        public CATEGORIA(ModeloObj modelo)
        {
            this.ID_CATEGORIA = modelo.Id;
            this.CD_CATEGORIA = modelo.Codigo;
            this.DS_CATEGORIA = modelo.Descricao;
            this.ID_SUBCLASSIFICACAO = modelo.IdPai;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_CATEGORIA, this.CD_CATEGORIA, this.DS_CATEGORIA, this.ID_SUBCLASSIFICACAO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_CATEGORIA, " - ", this.CD_DS_CATEGORIA);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
