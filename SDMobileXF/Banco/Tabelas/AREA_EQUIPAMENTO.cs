using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class AREA_EQUIPAMENTO
    {
        [PrimaryKey]
        public Guid ID_AREA_EQUIPAMENTO { get; set; }
        public Guid ID_GERENCIA_CSN { get; set; }
        public string CD_AREA_EQUIPAMENTO { get; set; }
        public string DS_AREA_EQUIPAMENTO { get; set; }

        [Ignore]
        public string CD_DS_AREA_EQUIPAMENTO { get { return string.Concat(this.CD_AREA_EQUIPAMENTO.ToStringNullSafe(), " - ", this.DS_AREA_EQUIPAMENTO.ToStringNullSafe()); } }

        public AREA_EQUIPAMENTO() { }

        public AREA_EQUIPAMENTO(ModeloObj modelo)
        {
            this.ID_AREA_EQUIPAMENTO = modelo.Id;
            this.ID_GERENCIA_CSN = modelo.IdPai;
            this.CD_AREA_EQUIPAMENTO = modelo.Codigo;
            this.DS_AREA_EQUIPAMENTO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_AREA_EQUIPAMENTO, this.CD_AREA_EQUIPAMENTO, this.DS_AREA_EQUIPAMENTO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_AREA_EQUIPAMENTO, " - ", this.CD_DS_AREA_EQUIPAMENTO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
