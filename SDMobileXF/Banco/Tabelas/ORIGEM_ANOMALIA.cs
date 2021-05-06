using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class ORIGEM_ANOMALIA
    {
        [PrimaryKey]
        public Guid ID_ORIGEM_ANOMALIA { get; set; }
        public string CD_ORIGEM_ANOMALIA { get; set; }
        public string DS_ORIGEM_ANOMALIA { get; set; }

        [Ignore]
        public string CD_DS_ORIGEM_ANOMALIA { get { return string.Concat(this.CD_ORIGEM_ANOMALIA.ToStringNullSafe(), " - ", this.DS_ORIGEM_ANOMALIA.ToStringNullSafe()); } }

        public ORIGEM_ANOMALIA() { }

        public ORIGEM_ANOMALIA(ModeloObj modelo)
        {
            this.ID_ORIGEM_ANOMALIA = modelo.Id;
            this.CD_ORIGEM_ANOMALIA = modelo.Codigo;
            this.DS_ORIGEM_ANOMALIA = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_ORIGEM_ANOMALIA, this.CD_ORIGEM_ANOMALIA, this.DS_ORIGEM_ANOMALIA); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_ORIGEM_ANOMALIA, " - ", this.CD_DS_ORIGEM_ANOMALIA);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
