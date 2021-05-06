using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class SEVERIDADE
    {
        [PrimaryKey]
        public Guid ID_SEVERIDADE { get; set; }
        public string CD_SEVERIDADE { get; set; }
        public string DS_SEVERIDADE { get; set; }

        [Ignore]
        public string CD_DS_SEVERIDADE { get { return string.Concat(this.CD_SEVERIDADE.ToStringNullSafe(), " - ", this.DS_SEVERIDADE.ToStringNullSafe()); } }

        public SEVERIDADE() { }

        public SEVERIDADE(ModeloObj modelo)
        {
            this.ID_SEVERIDADE = modelo.Id;
            this.CD_SEVERIDADE = modelo.Codigo;
            this.DS_SEVERIDADE = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_SEVERIDADE, this.CD_SEVERIDADE, this.DS_SEVERIDADE); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_SEVERIDADE, " - ", this.CD_DS_SEVERIDADE);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
