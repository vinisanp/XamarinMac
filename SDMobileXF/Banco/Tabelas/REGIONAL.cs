using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class REGIONAL
    {
        [PrimaryKey]
        public Guid ID_REGIONAL { get; set; }
        public string CD_REGIONAL { get; set; }
        public string DS_REGIONAL { get; set; }
        public Guid ID_SEGMENTO { get; set; }

        [Ignore]
        public string CD_DS_REGIONAL { get { return string.Concat(this.CD_REGIONAL.ToStringNullSafe(), " - ", this.DS_REGIONAL.ToStringNullSafe()); } }

        public REGIONAL() { }

        public REGIONAL(ModeloObj modelo)
        {
            this.ID_REGIONAL = modelo.Id;
            this.CD_REGIONAL = modelo.Codigo;
            this.DS_REGIONAL = modelo.Descricao;
            this.ID_SEGMENTO = modelo.IdPai;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_REGIONAL, this.CD_REGIONAL, this.DS_REGIONAL); }

        public override string ToString()
        {
            try 
            {
                return string.Concat(this.ID_REGIONAL, " - ", this.CD_DS_REGIONAL);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
