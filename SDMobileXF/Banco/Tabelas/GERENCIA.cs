using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class GERENCIA
    {
        [PrimaryKey]
        public Guid ID_GERENCIA { get; set; }
        public string CD_GERENCIA { get; set; }
        public string DS_GERENCIA { get; set; }
        public Guid ID_REGIONAL { get; set; }

        [Ignore]
        public string CD_DS_GERENCIA { get { return string.Concat(this.CD_GERENCIA.ToStringNullSafe(), " - ", this.DS_GERENCIA.ToStringNullSafe()); } }

        public GERENCIA() { }

        public GERENCIA(ModeloObj modelo)
        {
            this.ID_GERENCIA = modelo.Id;
            this.CD_GERENCIA = modelo.Codigo;
            this.DS_GERENCIA = modelo.Descricao;
            this.ID_REGIONAL = modelo.IdPai;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_GERENCIA, this.CD_GERENCIA, this.DS_GERENCIA, this.ID_REGIONAL); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_GERENCIA, " - ", this.CD_DS_GERENCIA);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
