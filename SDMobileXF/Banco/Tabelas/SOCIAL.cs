using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class SOCIAL : ViewModelModelo
    {
        [PrimaryKey]
        public Guid ID_SOCIAL { get; set; }
        public string CD_SOCIAL { get; set; }
        public string DS_SOCIAL { get; set; }
        private bool _ST_MARCADO;
        public bool ST_MARCADO
        {
            get { return this._ST_MARCADO; }
            set { this.DefinirPropriedade(ref this._ST_MARCADO, value); }
        }

        [Ignore]
        public string CD_DS_SOCIAL { get { return string.Concat(this.CD_SOCIAL.ToStringNullSafe(), " - ", this.DS_SOCIAL.ToStringNullSafe()); } }

        public SOCIAL() { }

        public SOCIAL(ModeloObj modelo)
        {
            this.ID_SOCIAL = modelo.Id;
            this.CD_SOCIAL = modelo.Codigo;
            this.DS_SOCIAL = modelo.Descricao;
            this.ST_MARCADO = modelo.Marcado;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_SOCIAL, this.CD_SOCIAL, this.DS_SOCIAL, this.ST_MARCADO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_SOCIAL, " - ", this.CD_DS_SOCIAL);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
