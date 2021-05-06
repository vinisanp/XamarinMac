using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class CLASSIFICACAO_TIPO
    {
        [PrimaryKey]
        public Guid ID_CLASSIFICACAO_TIPO { get; set; }
        public string CD_CLASSIFICACAO_TIPO { get; set; }
        public string DS_CLASSIFICACAO_TIPO { get; set; }

        [Ignore]
        public string CD_DS_CLASSIFICACAO_TIPO { get { return string.Concat(this.CD_CLASSIFICACAO_TIPO.ToStringNullSafe(), " - ", this.DS_CLASSIFICACAO_TIPO.ToStringNullSafe()); } }

        public CLASSIFICACAO_TIPO() { }

        public CLASSIFICACAO_TIPO(ModeloObj modelo)
        {
            this.ID_CLASSIFICACAO_TIPO = modelo.Id;
            this.CD_CLASSIFICACAO_TIPO = modelo.Codigo;
            this.DS_CLASSIFICACAO_TIPO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_CLASSIFICACAO_TIPO, this.CD_CLASSIFICACAO_TIPO, this.DS_CLASSIFICACAO_TIPO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_CLASSIFICACAO_TIPO, " - ", this.CD_DS_CLASSIFICACAO_TIPO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
