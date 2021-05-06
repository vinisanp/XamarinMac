using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class LOCAL
    {
        [PrimaryKey]
        public Guid ID_LOCAL { get; set; }
        public string CD_LOCAL { get; set; }
        public string DS_LOCAL { get; set; }
        public Guid ID_AREA { get; set; }

        [Ignore]
        public string CD_DS_LOCAL { get { return string.Concat(this.CD_LOCAL.ToStringNullSafe(), " - ", this.DS_LOCAL.ToStringNullSafe()); } }

        public LOCAL() { }

        public LOCAL(ModeloObj modelo)
        {
            this.ID_LOCAL = modelo.Id;
            this.CD_LOCAL = modelo.Codigo;
            this.DS_LOCAL = modelo.Descricao;
            this.ID_AREA = modelo.IdPai;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_LOCAL, this.CD_LOCAL, this.DS_LOCAL, this.ID_AREA); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_LOCAL, " - ", this.CD_DS_LOCAL);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
