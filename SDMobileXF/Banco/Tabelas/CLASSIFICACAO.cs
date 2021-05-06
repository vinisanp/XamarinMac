using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class CLASSIFICACAO
    {
        [PrimaryKey]
        public Guid ID_CLASSIFICACAO { get; set; }
        public string CD_CLASSIFICACAO { get; set; }
        public string DS_CLASSIFICACAO { get; set; }

        [Ignore]
        public string CD_DS_CLASSIFICACAO { get { return string.Concat(this.CD_CLASSIFICACAO.ToStringNullSafe(), " - ", this.DS_CLASSIFICACAO.ToStringNullSafe()); } }

        public CLASSIFICACAO() { }

        public CLASSIFICACAO(ModeloObj modelo)
        {
            this.ID_CLASSIFICACAO = modelo.Id;
            this.CD_CLASSIFICACAO = modelo.Codigo;
            this.DS_CLASSIFICACAO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_CLASSIFICACAO, this.CD_CLASSIFICACAO, this.DS_CLASSIFICACAO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_CLASSIFICACAO, " - ", this.CD_DS_CLASSIFICACAO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
