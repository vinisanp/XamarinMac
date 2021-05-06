using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class INFO
    {
        [PrimaryKey]
        public string NM_TABELA { get; set; }
        public DateTime DT_LAST_UPDATE { get; set; }
        public int NU_QUANT { get; set; }

        public INFO() { }

        public INFO(string nmTabela, DateTime ultimaAlteracao, int quant)
        {
            this.NM_TABELA = nmTabela;
            this.DT_LAST_UPDATE = ultimaAlteracao;
            this.NU_QUANT = quant;
        }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.NM_TABELA, ", ",this.NU_QUANT , " registros, ", this.DT_LAST_UPDATE.ToShortDateString(), " ", this.DT_LAST_UPDATE.ToLongTimeString()) ;
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
