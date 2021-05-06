using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class COGNITIVO : ViewModelModelo
    {

        [PrimaryKey]
        public Guid ID_COGNITIVO { get; set; }
        public string CD_COGNITIVO { get; set; }
        public string DS_COGNITIVO { get; set; }
        private bool _ST_MARCADO;
        public bool ST_MARCADO
        {
            get { return this._ST_MARCADO; }
            set { this.DefinirPropriedade(ref this._ST_MARCADO, value); }
        }

        [Ignore]
        public string CD_DS_COGNITIVO { get { return string.Concat(this.CD_COGNITIVO.ToStringNullSafe(), " - ", this.DS_COGNITIVO.ToStringNullSafe()); } }

        public COGNITIVO() { }

        public COGNITIVO(ModeloObj modelo)
        {
            this.ID_COGNITIVO = modelo.Id;
            this.CD_COGNITIVO = modelo.Codigo;
            this.DS_COGNITIVO = modelo.Descricao;
            this.ST_MARCADO = modelo.Marcado;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_COGNITIVO, this.CD_COGNITIVO, this.DS_COGNITIVO, this.ST_MARCADO); }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_COGNITIVO, " - ", this.CD_DS_COGNITIVO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
