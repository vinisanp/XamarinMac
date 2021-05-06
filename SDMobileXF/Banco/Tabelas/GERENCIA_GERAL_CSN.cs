using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class GERENCIA_GERAL_CSN
    {
        [PrimaryKey]
        public Guid ID_GERENCIA_GERAL_CSN { get; set; }
        public string CD_GERENCIA_GERAL_CSN { get; set; }
        public string DS_GERENCIA_GERAL_CSN { get; set; }

        [Ignore]
        public string CD_DS_GERENCIA_GERAL_CSN { get { return string.Concat(this.CD_GERENCIA_GERAL_CSN.ToStringNullSafe(), " - ", this.DS_GERENCIA_GERAL_CSN.ToStringNullSafe()); } }

        public GERENCIA_GERAL_CSN() { }

        public GERENCIA_GERAL_CSN(ModeloObj modelo)
        {
            this.ID_GERENCIA_GERAL_CSN = modelo.Id;
            this.CD_GERENCIA_GERAL_CSN = modelo.Codigo;
            this.DS_GERENCIA_GERAL_CSN = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_GERENCIA_GERAL_CSN, this.CD_GERENCIA_GERAL_CSN, this.DS_GERENCIA_GERAL_CSN); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_GERENCIA_GERAL_CSN, " - ", this.CD_DS_GERENCIA_GERAL_CSN);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
