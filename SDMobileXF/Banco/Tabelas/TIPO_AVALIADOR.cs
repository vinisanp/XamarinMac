using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class TIPO_AVALIADOR
    {
        [PrimaryKey]
        public Guid ID_TIPO_AVALIADOR { get; set; }
        public string CD_TIPO_AVALIADOR { get; set; }
        public string DS_TIPO_AVALIADOR { get; set; }

        [Ignore]
        public string CD_DS_TIPO_AVALIADOR { get { return string.Concat(this.CD_TIPO_AVALIADOR.ToStringNullSafe(), " - ", this.DS_TIPO_AVALIADOR.ToStringNullSafe()); } }

        public TIPO_AVALIADOR() { }

        public TIPO_AVALIADOR(ModeloObj modelo)
        {
            this.ID_TIPO_AVALIADOR = modelo.Id;
            this.CD_TIPO_AVALIADOR = modelo.Codigo;
            this.DS_TIPO_AVALIADOR = modelo.Descricao;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_TIPO_AVALIADOR, this.CD_TIPO_AVALIADOR, this.DS_TIPO_AVALIADOR); }

        public override string ToString()
        {
            try 
            {
                return string.Concat(this.ID_TIPO_AVALIADOR, " - ", this.CD_DS_TIPO_AVALIADOR);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
