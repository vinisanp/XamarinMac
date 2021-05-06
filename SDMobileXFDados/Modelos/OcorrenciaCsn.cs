using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class OcorrenciaCsn
    {
        public string ID_OCORRENCIA { get; set; }
        public string CODIGO { get; set; }
        public string TITULO { get; set; }
        public DateTime DATA { get; set; }    
        public ModeloObj DIA { get; set; }
        public ModeloObj REGIAOSETOR { get; set; }
        public ModeloObj LETRA { get; set; }
        public ModeloObj TURNO { get; set; }
        public ModeloObj GERENCIAGERAL { get; set; }
        public ModeloObj GERENCIA { get; set; }
        public ModeloObj AREAEQUIPAMENTO { get; set; }
        public ModeloObj LOCALEQUIPAMENTO { get; set; }
        public string DESCRICAOPRELIMINAR { get; set; }
        public string LOCALANOMALIA { get; set; }
        public ModeloObj ACAOIMEDIATA { get; set; }
        public string REMOCAOSINTOMAS { get; set; }
        public ModeloObj ORIGEMANOMALIA { get; set; }
        public ModeloObj TIPOANOMALIA { get; set; }
        public ModeloObj CLASSIFICACAOTIPO { get; set; }
        public ModeloObj PROBABILIDADE { get; set; }
        public ModeloObj SEVERIDADE { get; set; }
        public string PRODUTOAB { get; set; }
        public ModeloObj RESULTADOCLASIFICACAO { get; set; }
        public ModeloObj REGISTRADOPOR { get; set; }
        public ModeloObj RELATADOPOR { get; set; }
        public ModeloObj SUPERVISOR { get; set; }
        public ModeloObj ASSINATURA { get; set; }
        public int STATUS { get; set; }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_OCORRENCIA, " - ", this.CODIGO, " - ", this.TITULO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
