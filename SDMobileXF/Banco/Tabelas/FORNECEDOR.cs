using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class FORNECEDOR
    {
        [PrimaryKey]
        public Guid ID_FORNECEDOR { get; set; }
        public string CD_FORNECEDOR { get; set; }
        public string DS_FORNECEDOR { get; set; }

        [Ignore]
        public string CD_DS_FORNECEDOR { get { return string.Concat(this.CD_FORNECEDOR.ToStringNullSafe(), " - ", this.DS_FORNECEDOR.ToStringNullSafe()); } }

        public FORNECEDOR() { }

        public FORNECEDOR(ModeloObj modelo)
        {
            this.ID_FORNECEDOR = modelo.Id;
            this.CD_FORNECEDOR = modelo.Codigo;
            this.DS_FORNECEDOR = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_FORNECEDOR, this.CD_FORNECEDOR, this.DS_FORNECEDOR); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_FORNECEDOR, " - ", this.CD_DS_FORNECEDOR);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
