using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class Sna
    {
        public string ID_SNA { get; set; }
        public string CODIGO { get; set; }
        public DateTime DATA { get; set; }
        public TimeSpan HORA { get; set; }       

        public ModeloObj UNIDADE { get; set; }
        public ModeloObj GERENCIA { get; set; }
        public ModeloObj AREA { get; set; }
        public ModeloObj LOCAL { get; set; }

        public string DS_TEMA_ABORDADO { get; set; }

        public string DS_CE_AVALIACAODESCRITIVA { get; set; }
        public string ID_CE { get; set; }
        public string NU_DNA_CE { get; set; }

        public string DS_CA_AVALIACAODESCRITIVA { get; set; }
        public string ID_CA { get; set; }
        public string NU_DNA_CA { get; set; }

        public string DS_RAF_AVALIACAODESCRITIVA { get; set; }
        public string ID_RAF { get; set; }
        public string NU_DNA_RAF { get; set; }

        public string DS_QAT_AVALIACAODESCRITIVA { get; set; }
        public string ID_QAT { get; set; }
        public string NU_DNA_QAT { get; set; }

        public string DS_PONTOS_POSITIVOS { get; set; }

        public ModeloObj REGISTRADOPOR { get; set; }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_SNA, " - ", this.CODIGO, " - ", this.DS_TEMA_ABORDADO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
