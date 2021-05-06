using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class TIPO_INSPECAO
    {
        [PrimaryKey]
        public Guid ID_TIPO_INSPECAO { get; set; }
        public string CD_TIPO_INSPECAO { get; set; }
        public string DS_TIPO_INSPECAO { get; set; }

        [Ignore]
        public string CD_DS_TIPO_INSPECAO { get { return string.Concat(this.CD_TIPO_INSPECAO.ToStringNullSafe(), " - ", this.DS_TIPO_INSPECAO.ToStringNullSafe()); } }

        public TIPO_INSPECAO() { }

        public TIPO_INSPECAO(ModeloObj modelo)
        {
            this.ID_TIPO_INSPECAO = modelo.Id;
            this.CD_TIPO_INSPECAO = modelo.Codigo;
            this.DS_TIPO_INSPECAO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_TIPO_INSPECAO, this.CD_TIPO_INSPECAO, this.DS_TIPO_INSPECAO); }

        public override string ToString()
        {
            try 
            {
                return string.Concat(this.ID_TIPO_INSPECAO, " - ", this.CD_DS_TIPO_INSPECAO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
