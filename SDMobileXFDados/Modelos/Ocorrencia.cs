using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class Ocorrencia
    {
        public string ID_OCORRENCIA { get; set; }
        public string CODIGO { get; set; }
        public DateTime DATA { get; set; }
        public TimeSpan HORA { get; set; }        
        public ModeloObj UNIDADEREGIONAL { get; set; }
        public ModeloObj GERENCIA { get; set; }
        public ModeloObj AREA { get; set; }
        public ModeloObj LOCAL { get; set; }
        public string DESCRICAO { get; set; }
        public ModeloObj TIPO { get; set; }
        public ModeloObj CLASSIFICACAO { get; set; }
        public ModeloObj SUBCLASSIFICACAO { get; set; }
        public ModeloObj CATEGORIA { get; set; }
        public ModeloObj FORNECEDOR { get; set; }
        public string ACAOIMEDIATA { get; set; }        
        public string DESCRICAOACOESIMEDIATAS { get; set; }
        public bool NAOQUEROMEIDENTIFICAR { get; set; }
        public ModeloObj COMUNICADOPOR { get; set; }
        public ModeloObj REGISTRADOPOR { get; set; }
        public int STATUS { get; set; }
        public List<DocAnexo> ANEXOS { get; set; }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_OCORRENCIA, " - ", this.CODIGO, " - ", this.DESCRICAO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
