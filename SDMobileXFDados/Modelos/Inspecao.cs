using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class Inspecao
    {
        public string ID_INSPECAO { get; set; }
        public string NUMERO { get; set; }
        public DateTime DATA { get; set; }
        
        public ModeloObj UNIDADE { get; set; }
        public ModeloObj GERENCIA { get; set; }
        public ModeloObj AREA { get; set; }
        public ModeloObj LOCAL { get; set; }

        public ModeloObj FORNECEDOR { get; set; }

        public ModeloObj TIPO { get; set; }
        public ModeloObj ATIVIDADE { get; set; }

        public List<ModeloObj> PARTICIPANTES { get; set; }

        public List<CampoInspecao> CAMPOS { get; set; }
               
        public ModeloObj REALIZADOPOR { get; set; }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_INSPECAO, " - ", this.NUMERO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
