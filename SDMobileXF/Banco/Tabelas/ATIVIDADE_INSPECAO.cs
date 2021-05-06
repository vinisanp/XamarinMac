using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class ATIVIDADE_INSPECAO
    {
        [PrimaryKey]
        public Guid ID_ATIVIDADE_INSPECAO { get; set; }
        public string CD_ATIVIDADE_INSPECAO { get; set; }
        public string DS_ATIVIDADE_INSPECAO { get; set; }

        [Ignore]
        public string CD_DS_ATIVIDADE_INSPECAO { get { return string.Concat(this.CD_ATIVIDADE_INSPECAO.ToStringNullSafe(), " - ", this.DS_ATIVIDADE_INSPECAO.ToStringNullSafe()); } }

        public ATIVIDADE_INSPECAO() { }

        public ATIVIDADE_INSPECAO(ModeloObj modelo)
        {
            this.ID_ATIVIDADE_INSPECAO = modelo.Id;
            this.CD_ATIVIDADE_INSPECAO = modelo.Codigo;
            this.DS_ATIVIDADE_INSPECAO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_ATIVIDADE_INSPECAO, this.CD_ATIVIDADE_INSPECAO, this.DS_ATIVIDADE_INSPECAO); }

        public override string ToString()
        {
            try 
            {
                return string.Concat(this.ID_ATIVIDADE_INSPECAO, " - ", this.CD_DS_ATIVIDADE_INSPECAO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
