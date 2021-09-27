using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class VINCULO
    {
        [PrimaryKey]
        public Guid ID_VINCULO { get; set; }
        public string NU_MATRICULA { get; set; }
        public string NM_NOME { get; set; }
        public string DS_EMAIL { get; set; }

        [Ignore]
        public string CD_DS_VINCULO { get { return string.Concat(this.NU_MATRICULA.ToStringNullSafe(), " - ", this.NM_NOME.ToStringNullSafe()); } }

        public VINCULO() { }

        public VINCULO(ModeloObj modelo)
        {
            this.ID_VINCULO = modelo.Id;
            this.NU_MATRICULA = modelo.Codigo;
            this.NM_NOME = modelo.Descricao;
            this.DS_EMAIL = modelo.ValorString;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_VINCULO, this.NU_MATRICULA, this.NM_NOME, this.DS_EMAIL); }

        public override string ToString()
        {
            try 
            {
                return string.Concat(this.ID_VINCULO, " - ", this.CD_DS_VINCULO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
