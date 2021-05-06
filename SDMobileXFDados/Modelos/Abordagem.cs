using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class Abordagem
    {
        public string ID_ABORDAGEM { get; set; }
        public string CODIGO { get; set; }
        public DateTime DATA { get; set; }
        public TimeSpan HORA { get; set; }       
        public ModeloObj FORNECEDOR { get; set; } 
        public ModeloObj UNIDADE { get; set; }
        public ModeloObj GERENCIA { get; set; }
        public ModeloObj AREA { get; set; }
        public ModeloObj LOCAL { get; set; }
        public string DESCRICAO { get; set; }

        //A - Equipamento de Proteção Individual e Coletivo
        public ModeloObj INERENTES_ATIVIDADE { get; set; }
        public ModeloObj RELACAO_COM_RISCOS { get; set; }
        public ModeloObj CONSERVACAO_ADEQUADA { get; set; }
        public ModeloObj UTILIZACAO_CORRETA_FIXACAO_DISTANCIA { get; set; }

        //B - Máquinas, Veículos, Equipamentos e Ferramentas
        public ModeloObj IDENTIFICACAO_RISCOS_MAPEADOS { get; set; }
        public ModeloObj MEDIDAS_PREVENCAO { get; set; }
        public ModeloObj BOAS_CONDICOES_USO { get; set; }
        public ModeloObj UTILIZACAO_CORRETA { get; set; }
        public ModeloObj DESTINADOS_ATIVIDADE { get; set; }

        //C - Programa Bom Senso
        public ModeloObj LOCAL_LIMPO { get; set; }
        public ModeloObj MATERIAIS_ORGANIZADOS { get; set; }
        public ModeloObj DESCARTE_RESIDUOS { get; set; }

        //D - Acidentes, Incidentes e Desvios
        public ModeloObj IDENTIFICACAO_TRATATIVA_RISCOS { get; set; }
        public ModeloObj LINHA_FOGO { get; set; }
        public ModeloObj POSTURAS_ERGO_ADEQUADAS { get; set; }
        public ModeloObj CONCENTRACAO_TAREFA { get; set; }

        //E - Planejamento, Procedimento e Instrução
        public ModeloObj CONHECIMENTO_PROCEDIMENTOS { get; set; }
        public ModeloObj CONHECIMENTO_RISCOS { get; set; }
        public ModeloObj DIREITO_RECUSA { get; set; }
        public ModeloObj ACOES_EMERGENCIA { get; set; }
        public ModeloObj REALIZA_PROCESSOROTINA { get; set; }

        public List<ModeloObj> COGNITIVOS { get; set; }
        public List<ModeloObj> FISIOLOGICOS { get; set; }
        public List<ModeloObj> PSICOLOGICOS { get; set; }
        public List<ModeloObj> SOCIAIS { get; set; }

        public ModeloObj OBSERVADOR { get; set; }
        public ModeloObj REGISTRADOPOR { get; set; }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_ABORDAGEM, " - ", this.CODIGO, " - ", this.DESCRICAO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
