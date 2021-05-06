using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class AREA
    {
        [PrimaryKey]
        public Guid ID_AREA { get; set; }
        public string CD_AREA { get; set; }
        public string DS_AREA { get; set; }
        public Guid ID_GERENCIA { get; set; }

        [Ignore]
        public string CD_DS_AREA { get { return string.Concat(this.CD_AREA.ToStringNullSafe(), " - ", this.DS_AREA.ToStringNullSafe()); } }

        public AREA() { }

        public AREA(ModeloObj modelo)
        {
            this.ID_AREA = modelo.Id;
            this.CD_AREA = modelo.Codigo;
            this.DS_AREA = modelo.Descricao;
            this.ID_GERENCIA = modelo.IdPai;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_AREA, this.CD_AREA, this.DS_AREA, this.ID_GERENCIA); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_AREA, " - ", this.CD_DS_AREA);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
