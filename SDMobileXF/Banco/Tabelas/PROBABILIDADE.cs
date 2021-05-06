using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class PROBABILIDADE
    {
        [PrimaryKey]
        public Guid ID_PROBABILIDADE { get; set; }
        public string CD_PROBABILIDADE { get; set; }
        public string DS_PROBABILIDADE { get; set; }

        [Ignore]
        public string CD_DS_PROBABILIDADE { get { return string.Concat(this.CD_PROBABILIDADE.ToStringNullSafe(), " - ", this.DS_PROBABILIDADE.ToStringNullSafe()); } }

        public PROBABILIDADE() { }

        public PROBABILIDADE(ModeloObj modelo)
        {
            this.ID_PROBABILIDADE = modelo.Id;
            this.CD_PROBABILIDADE = modelo.Codigo;
            this.DS_PROBABILIDADE = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_PROBABILIDADE, this.CD_PROBABILIDADE, this.DS_PROBABILIDADE); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_PROBABILIDADE, " - ", this.CD_DS_PROBABILIDADE);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
