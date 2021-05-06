using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class PSICOLOGICO : ViewModelModelo
    {
        [PrimaryKey]
        public Guid ID_PSICOLOGICO { get; set; }
        public string CD_PSICOLOGICO { get; set; }
        public string DS_PSICOLOGICO { get; set; }
        private bool _ST_MARCADO;
        public bool ST_MARCADO
        {
            get { return this._ST_MARCADO; }
            set { this.DefinirPropriedade(ref this._ST_MARCADO, value); }
        }

        [Ignore]
        public string CD_DS_PSICOLOGICO { get { return string.Concat(this.CD_PSICOLOGICO.ToStringNullSafe(), " - ", this.DS_PSICOLOGICO.ToStringNullSafe()); } }

        public PSICOLOGICO() { }

        public PSICOLOGICO(ModeloObj modelo)
        {
            this.ID_PSICOLOGICO = modelo.Id;
            this.CD_PSICOLOGICO = modelo.Codigo;
            this.DS_PSICOLOGICO = modelo.Descricao;
            this.ST_MARCADO = modelo.Marcado;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_PSICOLOGICO, this.CD_PSICOLOGICO, this.DS_PSICOLOGICO, this.ST_MARCADO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_PSICOLOGICO, " - ", this.CD_DS_PSICOLOGICO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
