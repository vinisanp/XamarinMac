using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class TURNO
    {
        [PrimaryKey]
        public Guid ID_TURNO { get; set; }
        public string CD_TURNO { get; set; }
        public string DS_TURNO { get; set; }

        [Ignore]
        public string CD_DS_TURNO { get { return string.Concat(this.CD_TURNO.ToStringNullSafe(), " - ", this.DS_TURNO.ToStringNullSafe()); } }

        public TURNO() { }

        public TURNO(ModeloObj modelo)
        {
            this.ID_TURNO = modelo.Id;
            this.CD_TURNO = modelo.Codigo;
            this.DS_TURNO = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_TURNO, this.CD_TURNO, this.DS_TURNO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_TURNO, " - ", this.CD_DS_TURNO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
