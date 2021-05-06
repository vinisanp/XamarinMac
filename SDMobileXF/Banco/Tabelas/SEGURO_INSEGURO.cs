using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class SEGURO_INSEGURO
    {
        [PrimaryKey]
        public Guid ID_SEGURO_INSEGURO { get; set; }
        public string CD_SEGURO_INSEGURO { get; set; }
        public string DS_SEGURO_INSEGURO { get; set; }

        [Ignore]
        public string CD_DS_SEGURO_INSEGURO { get { return string.Concat(this.CD_SEGURO_INSEGURO.ToStringNullSafe(), " - ", this.DS_SEGURO_INSEGURO.ToStringNullSafe()); } }

        public SEGURO_INSEGURO() { }

        public SEGURO_INSEGURO(ModeloObj modelo)
        {
            this.ID_SEGURO_INSEGURO = modelo.Id;
            this.CD_SEGURO_INSEGURO = modelo.Codigo;
            this.DS_SEGURO_INSEGURO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_SEGURO_INSEGURO, this.CD_SEGURO_INSEGURO, this.DS_SEGURO_INSEGURO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_SEGURO_INSEGURO, " - ", this.CD_DS_SEGURO_INSEGURO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
