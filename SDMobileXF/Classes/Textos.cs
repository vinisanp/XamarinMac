using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXF.Classes
{
    public class Textos
    {
        private static Textos _textos = null;
        public static Textos Instancia
        {
            get
            {
                if (_textos == null)
                    _textos = new Textos();
                return _textos;
            }
        }

        public string Estilo => Globalizacao.Traduz("Estilo");
        public string Escuro => Globalizacao.Traduz("Escuro");
        public string Claro => Globalizacao.Traduz("Claro");

        public string TamanhoDaFonte => Globalizacao.Traduz("Tamanho da fonte");
        public string Pequeno => Globalizacao.Traduz("Pequeno");
        public string Medio => Globalizacao.Traduz("Médio");
        public string Grande => Globalizacao.Traduz("Grande");

        public string Idioma => Globalizacao.Traduz("Idioma");

        public string LoginInfo => Globalizacao.Traduz("Digite seu usuário e senha");
        public string PlaceHolderUsuario => Globalizacao.Traduz("Usuário");
        public string PlaceHolderSenha => Globalizacao.Traduz("Senha");
        public string Entrar => Globalizacao.Traduz("Entrar");
        public string LembrarMe => Globalizacao.Traduz("Lembrar-me");
        public string Sincronizacao => Globalizacao.Traduz("Sincronização");
        public string Sincronizar => Globalizacao.Traduz("Sincronizar");
        public string LimparDados => Globalizacao.Traduz("Limpar Dados");
        public string AtualizandoTabelasProgresso => Globalizacao.Traduz("Atualizando tabelas, progresso: ");
        public string SelecioneOperiodo => Globalizacao.Traduz("Selecione o período");
        public string DataInicial => Globalizacao.Traduz("Data Inicial");
        public string DataFinal => Globalizacao.Traduz("Data Final");
        public string Cadastradas => Globalizacao.Traduz("Cadastradas");        
        public string Pendentes => Globalizacao.Traduz("Pendentes");
        public string Configuracoes => Globalizacao.Traduz("S.T. - Configurações");
        public string Informacoes => Globalizacao.Traduz("S.T. - Informações");

        public string OcorrenciasCadastradas => Globalizacao.Traduz("Ocorrências Cadastradas");
        public string OcorrenciasSeremEnviadas => Globalizacao.Traduz("Ocorrências a serem enviadas");
        public string Ocorrencia => Globalizacao.Traduz("S.T. - Ocorrência");
        public string Ocorrencias => Globalizacao.Traduz("S.T. - Ocorrências");

        public string AbordagensCadastradas => Globalizacao.Traduz("Abordagens Comp. Cadastradas");
        public string AbordagensSeremEnviadas => Globalizacao.Traduz("Abordagens a serem enviadas");
        public string Abordagem => Globalizacao.Traduz("S.T. - Abordagem Comp.");
        public string Abordagens => Globalizacao.Traduz("S.T. - Abordagens Comp.");

        public string SNAsCadastrados => Globalizacao.Traduz("SNAs Cadastradas");
        public string SNAsSeremEnviadas => Globalizacao.Traduz("SNAs a serem enviadas");
        public string SNA => Globalizacao.Traduz("S.T. - SNA");
        public string SNAs => Globalizacao.Traduz("S.T. - SNAs");

        public string Ok => Globalizacao.Traduz("Ok");
        public string Sim => Globalizacao.Traduz("Sim");
        public string Nao => Globalizacao.Traduz("Não");
        public string Aviso => Globalizacao.Traduz("Aviso");
        public string Erro => Globalizacao.Traduz("Erro");
        public string ErroAoSalvar => Globalizacao.Traduz("Erro ao salvar!");
        public string Excluir => Globalizacao.Traduz("Excluir");

        public string Numero => Globalizacao.Traduz("Número");
        public string TituloAnomalia => Globalizacao.Traduz("Título da Anomalia");
        public string Data => Globalizacao.Traduz("Data");
        public string DataOcorrencia => Globalizacao.Traduz("Data Ocorrência");
        public string Dia => Globalizacao.Traduz("Dia");
        public string Hora => Globalizacao.Traduz("Hora");
        public string RegiaoSetor => Globalizacao.Traduz("Região/Setor");
        public string Letra => Globalizacao.Traduz("Letra");
        public string Turno => Globalizacao.Traduz("Turno");
        public string AreasAnormalidade => Globalizacao.Traduz("Áreas da Anormalidade");
        public string GerenciaGeral => Globalizacao.Traduz("Gerência Geral");
        public string Coordenacao => Globalizacao.Traduz("Coordenação");
        public string Lotacao => Globalizacao.Traduz("Lotação");
        public string AreaEquipamento => Globalizacao.Traduz("Área/Equipamento");
        public string LocalEquipamento => Globalizacao.Traduz("Local/Equipamento");
        public string LocalAnomalia => Globalizacao.Traduz("Local da Anomalia");
        public string UnidadeRegional => Globalizacao.Traduz("Unidade do desvio");
        public string GerenciaDoDesvio => Globalizacao.Traduz("Gerência do desvio");
        public string AreaDoDesvio => Globalizacao.Traduz("Área do desvio");
        public string LocalDoDesvio => Globalizacao.Traduz("Local do desvio");
        public string DescricaoDaOcorrencia => Globalizacao.Traduz("Descrição da Ocorrência");
        public string DescricaoPreliminar => Globalizacao.Traduz("Descrição Preliminar da Anomalia (O que ocorreu?)");
        public string TipoDaOcorrencia => Globalizacao.Traduz("Tipo de Ocorrência");
        public string ClassificacaoDaOcorrencia => Globalizacao.Traduz("Classificação da Ocorrência");
        public string SubClassificacaoDaOcorrencia => Globalizacao.Traduz("Subclassificação da Ocorrência");
        public string CategoriaDaOcorrencia => Globalizacao.Traduz("Categoria da Ocorrência");
        public string Fornecedor => Globalizacao.Traduz("Fornecedor");
        public string AcoesImediatas => Globalizacao.Traduz("Ações Imediatas");
        public string ForamTomadasAcoesImediatas => Globalizacao.Traduz("Houve Ação Imediata?");
        public string DescricaoDasAcoesImediatas => Globalizacao.Traduz("Descrever a ação imediata");
        public string RemocaoSintomas => Globalizacao.Traduz("Ações Imediatas/Remoção dos Sintomas");
        public string NaoQueroMeIdentificar => Globalizacao.Traduz("Não quero me identificar");
        public string OrigemAnomalia => Globalizacao.Traduz("Origem da Anomalia");
        public string TipoAnomalia => Globalizacao.Traduz("Tipo de Anomalia");
        public string ClassificacaoTipo => Globalizacao.Traduz("Classificação/Tipo");
        public string CaractExposicao => Globalizacao.Traduz("Categorização da Exposição Ocupacional Anomalia");
        public string Probabilidade => Globalizacao.Traduz("(A) Probabilidade");
        public string Severidade => Globalizacao.Traduz("(B) Severidade");
        public string ProdutoAB => Globalizacao.Traduz("Produto A + B");
        public string ResultadoClassificacaoRisco => Globalizacao.Traduz("Resultado da Classificação de Risco");
        public string Responsaveis => Globalizacao.Traduz("Responsáveis");
        public string ComunicadoPor => Globalizacao.Traduz("Comunicado por");
        public string RegistradoPor => Globalizacao.Traduz("Registrado Por");
        public string RelatadoPor => Globalizacao.Traduz("Relatado Por");
        public string SupervisorImediato => Globalizacao.Traduz("Supervisor Imediado");
        public string Assinatura => Globalizacao.Traduz("Assinatura");

        public string UltimaAtualizacaoTabelas => Globalizacao.Traduz("Última atualização das tabelas: ");
        public string TotalRegistrosBaixados => Globalizacao.Traduz("Total de registros baixados: ");
        public string KbytesBaixados => Globalizacao.Traduz("Kbytes baixados: ");

        public string EmpresaObservada => Globalizacao.Traduz("Empresa Observada");
        public string UnidadeAbordagem => Globalizacao.Traduz("Unidade Abordagem");
        public string GerenciaAbordagem => Globalizacao.Traduz("Gerência Abordagem");
        public string AreaAbordagem => Globalizacao.Traduz("Área Abordagem");
        public string LocalAbordagem => Globalizacao.Traduz("Local Abordagem");
        public string DescAtividadeSituacao => Globalizacao.Traduz("Descrição da Atividade / Situação");
        public string EPIeEPC => Globalizacao.Traduz("A - Equipamento de Proteção Individual e Coletivo");
        public string InerentesAtividade => Globalizacao.Traduz("Inerentes a Atividade");
        public string VerAgir => Globalizacao.Traduz("Ver e Agir");
        public string RelacaoComRiscos => Globalizacao.Traduz("Relação com os Riscos");
        public string ConservacaoAdequada => Globalizacao.Traduz("Conservação Adequada");
        public string UtilizacaoCorretaFixacaoDistancia => Globalizacao.Traduz("Utilização Correta (Ex.: Fixação e/ou distância)");
        public string MaquinasVeiculosEquip => Globalizacao.Traduz("B - Máquinas, Veículos, Equipamentos e Ferramentas");
        public string IdentificacaoRiscosMapeados => Globalizacao.Traduz("Identificação dos Riscos Mapeados");
        public string MedidasPrevencao => Globalizacao.Traduz("Medidas de Prevenção (Ex.: Inspeção e/ou ajustes)");
        public string BoasCondicoesUso => Globalizacao.Traduz("Boas Condições de Uso");
        public string UtilizacaoCorreta => Globalizacao.Traduz("Utilização Correta");
        public string DestinadosAtividade => Globalizacao.Traduz("Destinados a Atividade (sem improvisação)");
        public string ProgramaBomCenso => Globalizacao.Traduz("C - Programa Bom Senso");
        public string LocalLimpo => Globalizacao.Traduz("Local Limpo");
        public string MateriaisOrganizados => Globalizacao.Traduz("Materiais Organizados");
        public string DescarteResiduos => Globalizacao.Traduz("Descarte de Resíduos");
        public string AcidentesIncidentesDesvios => Globalizacao.Traduz("D - Acidentes, Incidentes e Desvios");
        public string IdentificacaoTratativaRiscos => Globalizacao.Traduz("Identificação e tratativa dos riscos (Ex.: DNA e/ou Ver e Agir)");
        public string LinhaFogo => Globalizacao.Traduz("Linha de Fogo (Ex.: Distância, carga ou queda de algo)");
        public string PosturasErgoAdequadas => Globalizacao.Traduz("Posturas Ergonômicas Adequadas");
        public string ConcentracaoTarefa => Globalizacao.Traduz("Concentração na Tarefa");
        public string PlanejamentoProcedimentoInstrucao => Globalizacao.Traduz("E - Planejamento, Procedimento e Instrução");
        public string ConhecimentoProcedimentos => Globalizacao.Traduz("Conhecimento e Consulta dos Procedimentos");
        public string ConhecimentoRiscos => Globalizacao.Traduz("Conhecimento Riscos Identificados pelo Programa Linha Mestra");
        public string DireitoRecusa => Globalizacao.Traduz("Direito de Recusa");
        public string AcoesEmergencia => Globalizacao.Traduz("Ações em Emergência (Ex.: Rota de Fuga, Chuveiro, Telefone, Brigadista, Alergia)");
        public string RealizaProcessoRotina => Globalizacao.Traduz("Realiza os Processos de Rotina (Ex.: DDS e/ou Liberações)");
        public string AtividadesComportamento => Globalizacao.Traduz("Ativadores de Comportamento");
        public string Cognitivos => Globalizacao.Traduz("COGNITIVOS");
        public string Fisiologicos => Globalizacao.Traduz("FISIOLÓGICOS");
        public string Psicologicos => Globalizacao.Traduz("PSICOLÓGICOS");
        public string Sociais => Globalizacao.Traduz("SOCIAIS");
        public string ObsOrt1 => Globalizacao.Traduz("Reforço Positivo de Comportamento: Vale lembrar da importância desse ato, inclua nas suas abordagens.");
        public string ObsOrt2 => Globalizacao.Traduz("Forneça feedback positivo para os comportamentos adequados na atividade, fortalecendo os comportamentos seguros vistos na execução dentro de qualquer circunstância.");
        public string ObsOrt3 => Globalizacao.Traduz("O reforço positivo aumenta a probabilidade desta resposta ocorrer no futuro, o indivíduo aprende qual o comportamento desejável para alcançar determinado objetivo.");
        public string ObsOrt4 => Globalizacao.Traduz("Fechamento de Compromisso: Ao final da abordagem, esse passo é necessário para a mudança positiva.");
        public string ObsOrt5 => Globalizacao.Traduz("Estabeleça compromissos com o executante para que ele coloque em prática soluções (reais) propostas.");
        public string ObsOrt6 => Globalizacao.Traduz("Feche os compromissos para a mudança dos comportamentos de risco, fornecendo ajuda com informações, conhecimentos e recursos, se necessário.");
        public string ObsOrt7 => Globalizacao.Traduz("Dependendo da situação, repasse informações e/ou abra um DNA.");
        public string ObsOrt8 => Globalizacao.Traduz("O intuito do compromisso é reduzir a dependência e aumentar a capacidade de antecipar e resolver situações na rotina.");
        public string ObsOrt9 => Globalizacao.Traduz("Autonomia é algo que deve ser desenvolvido, trabalhado dentro dessa ação para aumentar o potencial de ver e agir.");
        public string NomeObservador => Globalizacao.Traduz("Nome do Observador");

        public string HorarioFinal => Globalizacao.Traduz("Horário Final");
        public string HorarioInicial => Globalizacao.Traduz("Horário Inicial");
        public string Dados => Globalizacao.Traduz("Dados");
        public string DadosAtividade => Globalizacao.Traduz("Dados da Atividade");
        public string Unidade => Globalizacao.Traduz("Unidade");
        public string Gerencia => Globalizacao.Traduz("Gerência");
        public string Area => Globalizacao.Traduz("Área");
        public string Local => Globalizacao.Traduz("Local");
        public string TemaAbordado => Globalizacao.Traduz("Tema Abordado");
        public string TextoSNA1 => Globalizacao.Traduz("INTERAÇÃO DA GESTÃO NA FRENTE DE TRABALHO");
        public string TextoSNA2 => Globalizacao.Traduz("A interação do gestor com sua equipe deve priorizar a abordagem do tema disponibilizado pela equipe de SSQV, porém é possível utilizar outro tema que seja pertinente para o objetivo do Programa. A avaliação dos itens abaixo sobre o tema abordado será de grande importância para que a relação com a equipe naquele momento seja completa.");
        public string ClimaEquipe => Globalizacao.Traduz("Clima da Equipe");
        public string TextoClimaEquipe => Globalizacao.Traduz("Inicie o Segurança na Área se apresentando caso alguém não o conheça, pergunte como as pessoas estão, se tem alguma nova pessoa na equipe, aniversariante no dia ou semana. Como estava o clima da equipe?");
        public string Ruim => Globalizacao.Traduz("Ruim");
        public string Regular => Globalizacao.Traduz("Regular");
        public string Bom => Globalizacao.Traduz("Bom");
        public string AvaliacaoDescritiva => Globalizacao.Traduz("Avaliação Descritiva");
        public string NumeroDNA => Globalizacao.Traduz("Número do DNA");
        public string ConteudoAbordado => Globalizacao.Traduz("Conteúdo Abordado");
        public string TextoConteudoAbordado => Globalizacao.Traduz("Analise qual o nível de compreensão do tema apresentado. Utilize perguntas abertas neste momento. Qual nível de entendimento da equipe em termos da aplicação prática do conteúdo?");
        public string RotinaAplicacaoFerramentas => Globalizacao.Traduz("Rotina da aplicação das ferramentas de segurança");
        public string TextoRAF => Globalizacao.Traduz("Escolha uma ou duas ferramentas de segurança e pergunte para a equipe em relação a aplicação prática: se tem feito sentido, se o tempo dedicado é visto como oportunidade de evitar ocorrências e promover melhorias de ambientes e comportamentos, e quais as dúvidas ou sugestões. Como a equipe classifica a efetividade das ferramentas de segurança? Gentileza listar as ferramentas perguntadas.");
        public string QualidadeAmbienteTrabalho => Globalizacao.Traduz("Qualidade dos ambientes de trabalho");
        public string TextoQAT => Globalizacao.Traduz("Você considera o ambiente limpo, organizado, com condições de favorecimento de um clima positivo, que reforce a valorização das pessoas? Em termos gerais nestes aspectos, como você considera a área que visitou?");
        public string ObservacoesGerais => Globalizacao.Traduz("Observações Gerais");
        public string PontosPositivos => Globalizacao.Traduz("Pontos Positivos");
        public string Responsavel => Globalizacao.Traduz("Responsável");

        public string Identificacao => Globalizacao.Traduz("Identificação");
        public string Localizacao => Globalizacao.Traduz("Localização");
        public string DadosOcorrencia => Globalizacao.Traduz("Dados da Ocorrência");
        public string AcoesTomadas => Globalizacao.Traduz("Ações Tomadas");
        public string ResponsavelPelaOcorrencia => Globalizacao.Traduz("Responsável pela Ocorrência");
        public string ResponsavelPelaAbordagem => Globalizacao.Traduz("Responsável pela Abordagem");

        public string CamposObrigatorios => Globalizacao.Traduz("Campos obrigatórios");

        public string RegistrarOcorrencia => Globalizacao.Traduz("Registrar Ocorrência");
        public string Salvar => Globalizacao.Traduz("Salvar");
        public string Cancelar => Globalizacao.Traduz("Cancelar");

        public string Carregando => Globalizacao.Traduz("Carregando...");

        public string DescricaoSituacao => Globalizacao.Traduz("Descrição da Situação");
        public string Foto => Globalizacao.Traduz("Foto");

        public string InspecoesCadastradas => Globalizacao.Traduz("Inspeções de Segurança Cadastradas");
        public string InspecoesSeremEnviadas => Globalizacao.Traduz("Inspeções de Segurança a serem enviadas");
        public string Inspecoes => Globalizacao.Traduz("S.T. - Inspeções de Segurança");
        public string InspecaoSeguranca => Globalizacao.Traduz("Inspeção de Segurança");
        public string UnidadeInspecao => Globalizacao.Traduz("Unidade da Inspeção");
        public string GerenciaInspecao => Globalizacao.Traduz("Gerência da Inspeção");
        public string AreaInspecao => Globalizacao.Traduz("Área da Inspeção");
        public string LocalInspecao => Globalizacao.Traduz("Local da Inspeção");
        public string TipoInspecao => Globalizacao.Traduz("Tipo de Inspeção");
        public string Atividade => Globalizacao.Traduz("Atividade");
        public string Participantes => Globalizacao.Traduz("Participantes");
        public string RealizadoPor => Globalizacao.Traduz("Realizado Por");

        public string Opas => Globalizacao.Traduz("S.T. - Observação Positiva Florestal");

        public string OpasSeremEnviadas => Globalizacao.Traduz("Observações Positivas a serem enviadas");
        
        public string Avaliador => Globalizacao.Traduz("Avaliador");
        public string TipoAvaliador => Globalizacao.Traduz("Tipo de Avaliador");
        public string Tarefa => Globalizacao.Traduz("Tarefa");
        public string Conforme => Globalizacao.Traduz("Conforme");
        public string NaoConforme => Globalizacao.Traduz("Não Conforme");
        public string NaoSeAplica => Globalizacao.Traduz("Não se Aplica");
        public string Media => Globalizacao.Traduz("Média");
        public string Pontuacao => Globalizacao.Traduz("Pontuação");
        public string NotaFinal => Globalizacao.Traduz("Nota Final");
        public string Classificacao => Globalizacao.Traduz("Classificação");
        public string Comentarios => Globalizacao.Traduz("Comentários");
        
    }
}
