using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class LETRA
    {
        [PrimaryKey]
        public Guid ID_LETRA { get; set; }
        public string CD_LETRA { get; set; }
        public string DS_LETRA { get; set; }

        [Ignore]
        public string CD_DS_LETRA { get { return string.Concat(this.CD_LETRA.ToStringNullSafe(), " - ", this.DS_LETRA.ToStringNullSafe()); } }

        public LETRA() { }

        public LETRA(ModeloObj modelo)
        {
            this.ID_LETRA = modelo.Id;
            this.CD_LETRA = modelo.Codigo;
            this.DS_LETRA = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_LETRA, this.CD_LETRA, this.DS_LETRA); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_LETRA, " - ", this.CD_DS_LETRA);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
