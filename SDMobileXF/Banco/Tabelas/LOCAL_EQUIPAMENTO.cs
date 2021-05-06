using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class LOCAL_EQUIPAMENTO
    {
        [PrimaryKey]
        public Guid ID_LOCAL_EQUIPAMENTO { get; set; }
        public Guid ID_AREA_EQUIPAMENTO { get; set; }
        public string CD_LOCAL_EQUIPAMENTO { get; set; }
        public string DS_LOCAL_EQUIPAMENTO { get; set; }

        [Ignore]
        public string CD_DS_LOCAL_EQUIPAMENTO { get { return string.Concat(this.CD_LOCAL_EQUIPAMENTO.ToStringNullSafe(), " - ", this.DS_LOCAL_EQUIPAMENTO.ToStringNullSafe()); } }

        public LOCAL_EQUIPAMENTO() { }

        public LOCAL_EQUIPAMENTO(ModeloObj modelo)
        {
            this.ID_LOCAL_EQUIPAMENTO = modelo.Id;
            this.ID_AREA_EQUIPAMENTO = modelo.IdPai;
            this.CD_LOCAL_EQUIPAMENTO = modelo.Codigo;
            this.DS_LOCAL_EQUIPAMENTO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_LOCAL_EQUIPAMENTO, this.CD_LOCAL_EQUIPAMENTO, this.DS_LOCAL_EQUIPAMENTO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_LOCAL_EQUIPAMENTO, " - ", this.CD_DS_LOCAL_EQUIPAMENTO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
