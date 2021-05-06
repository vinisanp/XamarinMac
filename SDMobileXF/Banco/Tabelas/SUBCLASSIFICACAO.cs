using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class SUBCLASSIFICACAO
    {
        [PrimaryKey]
        public Guid ID_SUBCLASSIFICACAO { get; set; }
        public string CD_SUBCLASSIFICACAO { get; set; }
        public string DS_SUBCLASSIFICACAO { get; set; }
        public Guid ID_CLASSIFICACAO { get; set; }

        [Ignore]
        public string CD_DS_SUBCLASSIFICACAO { get { return string.Concat(this.CD_SUBCLASSIFICACAO.ToStringNullSafe(), " - ", this.DS_SUBCLASSIFICACAO.ToStringNullSafe()); } }

        public SUBCLASSIFICACAO() { }

        public SUBCLASSIFICACAO(ModeloObj modelo)
        {
            this.ID_SUBCLASSIFICACAO = modelo.Id;
            this.CD_SUBCLASSIFICACAO = modelo.Codigo;
            this.DS_SUBCLASSIFICACAO = modelo.Descricao;
            this.ID_CLASSIFICACAO = modelo.IdPai;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_SUBCLASSIFICACAO, this.CD_SUBCLASSIFICACAO, this.DS_SUBCLASSIFICACAO, this.ID_CLASSIFICACAO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_SUBCLASSIFICACAO, " - ", this.CD_DS_SUBCLASSIFICACAO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
