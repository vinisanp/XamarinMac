using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class FISIOLOGICO : ViewModelModelo
    {
        [PrimaryKey]
        public Guid ID_FISIOLOGICO { get; set; }
        public string CD_FISIOLOGICO { get; set; }
        public string DS_FISIOLOGICO { get; set; }
        private bool _ST_MARCADO;
        public bool ST_MARCADO
        {
            get { return this._ST_MARCADO; }
            set { this.DefinirPropriedade(ref this._ST_MARCADO, value); }
        }

        [Ignore]
        public string CD_DS_FISIOLOGICO { get { return string.Concat(this.CD_FISIOLOGICO.ToStringNullSafe(), " - ", this.DS_FISIOLOGICO.ToStringNullSafe()); } }

        public FISIOLOGICO() { }

        public FISIOLOGICO(ModeloObj modelo)
        {
            this.ID_FISIOLOGICO = modelo.Id;
            this.CD_FISIOLOGICO = modelo.Codigo;
            this.DS_FISIOLOGICO = modelo.Descricao;
            this.ST_MARCADO = modelo.Marcado;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_FISIOLOGICO, this.CD_FISIOLOGICO, this.DS_FISIOLOGICO, this.ST_MARCADO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_FISIOLOGICO, " - ", this.CD_DS_FISIOLOGICO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
