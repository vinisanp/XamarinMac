using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class TIPO_ANOMALIA
    {
        [PrimaryKey]
        public Guid ID_TIPO_ANOMALIA { get; set; }
        public string CD_TIPO_ANOMALIA { get; set; }
        public string DS_TIPO_ANOMALIA { get; set; }

        [Ignore]
        public string CD_DS_TIPO_ANOMALIA { get { return string.Concat(this.CD_TIPO_ANOMALIA.ToStringNullSafe(), " - ", this.DS_TIPO_ANOMALIA.ToStringNullSafe()); } }

        public TIPO_ANOMALIA() { }

        public TIPO_ANOMALIA(ModeloObj modelo)
        {
            this.ID_TIPO_ANOMALIA = modelo.Id;
            this.CD_TIPO_ANOMALIA = modelo.Codigo;
            this.DS_TIPO_ANOMALIA = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_TIPO_ANOMALIA, this.CD_TIPO_ANOMALIA, this.DS_TIPO_ANOMALIA); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_TIPO_ANOMALIA, " - ", this.CD_DS_TIPO_ANOMALIA);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
