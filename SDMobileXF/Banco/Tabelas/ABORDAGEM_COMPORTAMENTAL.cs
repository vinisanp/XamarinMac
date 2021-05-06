using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class ABORDAGEM_COMPORTAMENTAL
    {
        [PrimaryKey]
        public Guid ID_ABORDAGEM { get; set; }
        public DateTime DATA { get; set; }
        public Guid ID_FORNECEDOR { get; set; }
        public Guid ID_REGIONAL { get; set; }
        public Guid ID_GERENCIA { get; set; }
        public Guid ID_AREA { get; set; }
        public Guid ID_LOCAL { get; set; }
        public string DESCRICAO { get; set; }

        //A - Equipamento de Proteção Individual e Coletivo
        public Guid? ID_INERENTES_ATIVIDADE { get; set; }
        public bool ST_INERENTES_ATIVIDADE_VER_AGIR { get; set; }
        public Guid? ID_RELACAO_COM_RISCOS { get; set; }
        public bool ST_RELACAO_COM_RISCOS_VER_AGIR { get; set; }
        public Guid? ID_CONSERVACAO_ADEQUADA { get; set; }
        public bool ST_CONSERVACAO_ADEQUADA_VER_AGIR { get; set; }
        public Guid? ID_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA { get; set; }
        public bool ST_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA_VER_AGIR { get; set; }

        //B - Máquinas, Veículos, Equipamentos e Ferramentas
        public Guid? ID_IDENTIFICACAO_RISCOS_MAPEADOS { get; set; }
        public bool ST_IDENTIFICACAO_RISCOS_MAPEADOS_VER_AGIR { get; set; }
        public Guid? ID_MEDIDAS_PREVENCAO { get; set; }
        public bool ST_MEDIDAS_PREVENCAO_VER_AGIR { get; set; }
        public Guid? ID_BOAS_CONDICOES_USO { get; set; }
        public bool ST_BOAS_CONDICOES_USO_VER_AGIR { get; set; }
        public Guid? ID_UTILIZACAO_CORRETA { get; set; }
        public bool ST_UTILIZACAO_CORRETA_VER_AGIR { get; set; }
        public Guid? ID_DESTINADOS_ATIVIDADE { get; set; }
        public bool ST_DESTINADOS_ATIVIDADE_VER_AGIR { get; set; }

        //C - Programa Bom Senso
        public Guid? ID_LOCAL_LIMPO { get; set; }
        public bool ST_LOCAL_LIMPO_VER_AGIR { get; set; }
        public Guid? ID_MATERIAIS_ORGANIZADOS { get; set; }
        public bool ST_MATERIAIS_ORGANIZADOS_VER_AGIR { get; set; }
        public Guid? ID_DESCARTE_RESIDUOS { get; set; }
        public bool ST_DESCARTE_RESIDUOS_VER_AGIR { get; set; }

        //D - Acidentes, Incidentes e Desvios
        public Guid? ID_IDENTIFICACAO_TRATATIVA_RISCOS { get; set; }
        public bool ST_IDENTIFICACAO_TRATATIVA_RISCOS_VER_AGIR { get; set; }
        public Guid? ID_LINHA_FOGO { get; set; }
        public bool ST_LINHA_FOGO_VER_AGIR { get; set; }
        public Guid? ID_POSTURAS_ERGO_ADEQUADAS { get; set; }
        public bool ST_POSTURAS_ERGO_ADEQUADAS_VER_AGIR { get; set; }
        public Guid? ID_CONCENTRACAO_TAREFA { get; set; }
        public bool ST_CONCENTRACAO_TAREFA_VER_AGIR { get; set; }

        //E - Planejamento, Procedimento e Instrução
        public Guid? ID_CONHECIMENTO_PROCEDIMENTOS { get; set; }
        public bool ST_CONHECIMENTO_PROCEDIMENTOS_VER_AGIR { get; set; }
        public Guid? ID_CONHECIMENTO_RISCOS { get; set; }
        public bool ST_CONHECIMENTO_RISCOS_VER_AGIR { get; set; }
        public Guid? ID_DIREITO_RECUSA { get; set; }
        public bool ST_DIREITO_RECUSA_VER_AGIR { get; set; }
        public Guid? ID_ACOES_EMERGENCIA { get; set; }
        public bool ST_ACOES_EMERGENCIA_VER_AGIR { get; set; }
        public Guid? ID_REALIZA_PROCESSOROTINA { get; set; }
        public bool ST_REALIZA_PROCESSOROTINA_VER_AGIR { get; set; }

        public string COGNITIVOS { get; set; }
        public string FISIOLOGICOS { get; set; }
        public string PSICOLOGICOS { get; set; }
        public string SOCIAIS { get; set; }
        
        public Guid? ID_OBSERVADOR { get; set; }
        public Guid? ID_REGISTRADOPOR { get; set; }

        public string DS_SYNC { get; set; }

        [Ignore]
        public bool TemErro
        {
            get { return !string.IsNullOrEmpty(this.DS_SYNC); }
        }

        public ABORDAGEM_COMPORTAMENTAL() { }
    }
}
