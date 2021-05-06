using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class TIPO
    {
        [PrimaryKey]
        public Guid ID_TIPO { get; set; }
        public string CD_TIPO { get; set; }
        public string DS_TIPO { get; set; }

        [Ignore]
        public string CD_DS_TIPO { get { return string.Concat(this.CD_TIPO.ToStringNullSafe(), " - ", this.DS_TIPO.ToStringNullSafe()); } }

        public TIPO() { }

        public TIPO(ModeloObj modelo)
        {
            this.ID_TIPO = modelo.Id;
            this.CD_TIPO = modelo.Codigo;
            this.DS_TIPO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_TIPO, this.CD_TIPO, this.DS_TIPO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_TIPO, " - ", this.CD_DS_TIPO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
