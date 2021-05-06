using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class UNIDADE
    {
        [PrimaryKey]
        public Guid ID_UNIDADE { get; set; }
        public string CD_UNIDADE { get; set; }
        public string DS_UNIDADE { get; set; }

        [Ignore]
        public string CD_DS_UNIDADE { get { return string.Concat(this.CD_UNIDADE.ToStringNullSafe(), " - ", this.DS_UNIDADE.ToStringNullSafe()); } }

        public UNIDADE() { }

        public UNIDADE(ModeloObj modelo)
        {
            this.ID_UNIDADE = modelo.Id;
            this.CD_UNIDADE = modelo.Codigo;
            this.DS_UNIDADE = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_UNIDADE, this.CD_UNIDADE, this.DS_UNIDADE); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_UNIDADE, " - ", this.CD_DS_UNIDADE);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
