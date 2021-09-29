using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class Opa
    {
        public string ID_OPA { get; set; }
        public string NUMERO { get; set; }
        public DateTime DATA { get; set; }
        
        public ModeloObj UNIDADE { get; set; }
        public ModeloObj GERENCIA { get; set; }
        public ModeloObj AREA { get; set; }
        public ModeloObj LOCAL { get; set; }

        public ModeloObj ATIVIDADE { get; set; }
        public ModeloObj TAREFA_OBSERVADA { get; set; }

        public ModeloOpa ModeloOpa { get; set; }

        public ModeloObj AVALIADOR { get; set; }
        public ModeloObj TIPO_AVALIADOR { get; set; }
        public ModeloObj EMPRESA_FORNECEDOR { get; set; }

        public string DS_EMAIL_AVALIADOR { get; set; }
        public string DS_AVALIADO { get; set; }


        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_OPA, " - ", this.NUMERO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
