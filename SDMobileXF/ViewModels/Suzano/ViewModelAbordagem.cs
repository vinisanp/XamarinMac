using SDMobileXF.Banco.Tabelas;
using SDMobileXF.Classes;
using SDMobileXF.Views;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Xamarin.Forms;
using static SDMobileXFDados.Enumerados;

namespace SDMobileXF.ViewModels
{
    public class ViewModelAbordagem : ViewModelBase
    {
        #region Variaveis

        private string _numero;
        private DateTime _date = DateTime.MinValue;
        private TimeSpan _hora = TimeSpan.Zero;
        private ModeloObj _fornecedor; //empresa observada
        private ModeloObj _unidadeAbordagem;
        private ModeloObj _gerenciaAbordagem;
        private ModeloObj _areaAbordagem;
        private ModeloObj _localAbordagem;
        
        private string _descAtividadeSituacao;

        private List<SEGURO_INSEGURO> _respostasSeguroInseguro = new List<SEGURO_INSEGURO>();

        //A - Equipamento de Proteção Individual e Coletivo
        private ObservableCollection<ModeloObj> _inerentesAtividadeLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _inerentesAtividade;
        private ObservableCollection<ModeloObj> _relacaoComRiscosLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _relacaoComRiscos;
        private ObservableCollection<ModeloObj> _conservacaoAdequadaLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _conservacaoAdequada;
        private ObservableCollection<ModeloObj> _utilizacaoCorretaFixacaoDistanciaLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _utilizacaoCorretaFixacaoDistancia;

        //B - Máquinas, Veículos, Equipamentos e Ferramentas
        private ObservableCollection<ModeloObj> _identificacaoRiscosMapeadosLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _identificacaoRiscosMapeados;
        private ObservableCollection<ModeloObj> _medidasPrevencaoLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _medidasPrevencao;
        private ObservableCollection<ModeloObj> _boasCondicoesUsoLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _boasCondicoesUso;
        private ObservableCollection<ModeloObj> _utilizacaoCorretaLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _utilizacaoCorreta;
        private ObservableCollection<ModeloObj> _destinadosAtividadeLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _destinadosAtividade;

        //C - Programa Bom Senso
        private ObservableCollection<ModeloObj> _localLimpoLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _localLimpo;
        private ObservableCollection<ModeloObj> _materiaisOrganizadosLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _materiaisOrganizados;
        private ObservableCollection<ModeloObj> _descarteResiduosLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _descarteResiduos;

        //D - Acidentes, Incidentes e Desvios
        private ObservableCollection<ModeloObj> _identificacaoTratativaRiscosLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _identificacaoTratativaRiscos;
        private ObservableCollection<ModeloObj> _linhaFogoLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _linhaFogo;
        private ObservableCollection<ModeloObj> _posturasErgoAdequadasLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _posturasErgoAdequadas;
        private ObservableCollection<ModeloObj> _concentracaoTarefaLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _concentracaoTarefa;

        //E - Planejamento, Procedimento e Instrução
        private ObservableCollection<ModeloObj> _conhecimentoProcedimentosLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _conhecimentoProcedimentos;
        private ObservableCollection<ModeloObj> _conhecimentoRiscosLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _conhecimentoRiscos;
        private ObservableCollection<ModeloObj> _direitoRecusaLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _direitoRecusa;
        private ObservableCollection<ModeloObj> _acoesEmergenciaLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _acoesEmergencia;
        private ObservableCollection<ModeloObj> _realizaProcessoRotinaLst = new ObservableCollection<ModeloObj>();
        private ModeloObj _realizaProcessoRotina;

        //Ativadores de Comportamento
        private ObservableCollection<COGNITIVO> _cognitivos = new ObservableCollection<COGNITIVO>();
        private COGNITIVO _cognitivo;
        private ObservableCollection<FISIOLOGICO> _fisiologicos = new ObservableCollection<FISIOLOGICO>();
        private FISIOLOGICO _fisiologico;
        private ObservableCollection<PSICOLOGICO> _psicologicos = new ObservableCollection<PSICOLOGICO>();
        private PSICOLOGICO _psicologico;
        private ObservableCollection<SOCIAL> _sociais = new ObservableCollection<SOCIAL>();
        private SOCIAL _social;

        private ModeloObj _observador;
        private ModeloObj _registradoPor;

        private Guid _idAbordagemSQLite = Guid.Empty;

        #endregion Variaveis


        #region Propriedades

        public DateTime DataMaxima => new DateTime(2999, 12, 31);

        public DateTime DataMinima => new DateTime(2020, 01, 01);

        public string CorTituloPicker { get; set; }

        public string Numero
        {
            get { return this._numero; }
            set { this.DefinirPropriedade(ref this._numero, value); }
        }

        public bool ExibirNumero
        {
            get { return !this.EmEdicao && !string.IsNullOrEmpty(this.Numero); }
        }

        public DateTime Data
        {
            get { return this._date; }
            set { this.DefinirPropriedade(ref this._date, value); }
        }

        public TimeSpan Hora
        {
            get { return this._hora; }
            set { this.DefinirPropriedade(ref this._hora, value); }
        }

        public ModeloObj Fornecedor
        {
            get { return this._fornecedor; }
            set { this.DefinirPropriedade(ref this._fornecedor, value); }
        }

        public ModeloObj UnidadeAbordagem
        {
            get { return this._unidadeAbordagem; }
            set { this.DefinirPropriedade(ref this._unidadeAbordagem, value); }
        }

        public ModeloObj GerenciaAbordagem
        {
            get { return this._gerenciaAbordagem; }
            set { this.DefinirPropriedade(ref this._gerenciaAbordagem, value); }
        }

        public ModeloObj AreaAbordagem
        {
            get { return this._areaAbordagem; }
            set { this.DefinirPropriedade(ref this._areaAbordagem, value); }
        }

        public ModeloObj LocalAbordagem
        {
            get { return this._localAbordagem; }
            set { this.DefinirPropriedade(ref this._localAbordagem, value); }
        }


        public string DescAtividadeSituacao
        {
            get { return this._descAtividadeSituacao; }
            set { this.DefinirPropriedade(ref this._descAtividadeSituacao, value); }
        }

        //A - Equipamento de Proteção Individual e Coletivo
        public ObservableCollection<ModeloObj> InerentesAtividadeLst
        {
            get { return this._inerentesAtividadeLst; }
            set { this.DefinirPropriedade(ref this._inerentesAtividadeLst, value); }
        }
        public ModeloObj InerentesAtividade
        {
            get { return this._inerentesAtividade; }
            set { this.DefinirPropriedade(ref this._inerentesAtividade, value); }
        }

        public ObservableCollection<ModeloObj> RelacaoComRiscosLst
        {
            get { return this._relacaoComRiscosLst; }
            set { this.DefinirPropriedade(ref this._relacaoComRiscosLst, value); }
        }
        public ModeloObj RelacaoComRiscos
        {
            get { return this._relacaoComRiscos; }
            set { this.DefinirPropriedade(ref this._relacaoComRiscos, value); }
        }

        public ObservableCollection<ModeloObj> ConservacaoAdequadaLst
        {
            get { return this._conservacaoAdequadaLst; }
            set { this.DefinirPropriedade(ref this._conservacaoAdequadaLst, value); }
        }
        public ModeloObj ConservacaoAdequada
        {
            get { return this._conservacaoAdequada; }
            set { this.DefinirPropriedade(ref this._conservacaoAdequada, value); }
        }

        public ObservableCollection<ModeloObj> UtilizacaoCorretaFixacaoDistanciaLst
        {
            get { return this._utilizacaoCorretaFixacaoDistanciaLst; }
            set { this.DefinirPropriedade(ref this._utilizacaoCorretaFixacaoDistanciaLst, value); }
        }
        public ModeloObj UtilizacaoCorretaFixacaoDistancia
        {
            get { return this._utilizacaoCorretaFixacaoDistancia; }
            set { this.DefinirPropriedade(ref this._utilizacaoCorretaFixacaoDistancia, value); }
        }

        //B - Máquinas, Veículos, Equipamentos e Ferramentas
        public ObservableCollection<ModeloObj> IdentificacaoRiscosMapeadosLst
        {
            get { return this._identificacaoRiscosMapeadosLst; }
            set { this.DefinirPropriedade(ref this._identificacaoRiscosMapeadosLst, value); }
        }
        public ModeloObj IdentificacaoRiscosMapeados
        {
            get { return this._identificacaoRiscosMapeados; }
            set { this.DefinirPropriedade(ref this._identificacaoRiscosMapeados, value); }
        }

        public ObservableCollection<ModeloObj> MedidasPrevencaoLst
        {
            get { return this._medidasPrevencaoLst; }
            set { this.DefinirPropriedade(ref this._medidasPrevencaoLst, value); }
        }
        public ModeloObj MedidasPrevencao
        {
            get { return this._medidasPrevencao; }
            set { this.DefinirPropriedade(ref this._medidasPrevencao, value); }
        }

        public ObservableCollection<ModeloObj> BoasCondicoesUsoLst
        {
            get { return this._boasCondicoesUsoLst; }
            set { this.DefinirPropriedade(ref this._boasCondicoesUsoLst, value); }
        }
        public ModeloObj BoasCondicoesUso
        {
            get { return this._boasCondicoesUso; }
            set { this.DefinirPropriedade(ref this._boasCondicoesUso, value); }
        }

        public ObservableCollection<ModeloObj> UtilizacaoCorretaLst
        {
            get { return this._utilizacaoCorretaLst; }
            set { this.DefinirPropriedade(ref this._utilizacaoCorretaLst, value); }
        }
        public ModeloObj UtilizacaoCorreta
        {
            get { return this._utilizacaoCorreta; }
            set { this.DefinirPropriedade(ref this._utilizacaoCorreta, value); }
        }

        public ObservableCollection<ModeloObj> DestinadosAtividadeLst
        {
            get { return this._destinadosAtividadeLst; }
            set { this.DefinirPropriedade(ref this._destinadosAtividadeLst, value); }
        }
        public ModeloObj DestinadosAtividade
        {
            get { return this._destinadosAtividade; }
            set { this.DefinirPropriedade(ref this._destinadosAtividade, value); }
        }

        //C - Programa Bom Senso
        public ObservableCollection<ModeloObj> LocalLimpoLst
        {
            get { return this._localLimpoLst; }
            set { this.DefinirPropriedade(ref this._localLimpoLst, value); }
        }
        public ModeloObj LocalLimpo
        {
            get { return this._localLimpo; }
            set { this.DefinirPropriedade(ref this._localLimpo, value); }
        }

        public ObservableCollection<ModeloObj> MateriaisOrganizadosLst
        {
            get { return this._materiaisOrganizadosLst; }
            set { this.DefinirPropriedade(ref this._materiaisOrganizadosLst, value); }
        }
        public ModeloObj MateriaisOrganizados
        {
            get { return this._materiaisOrganizados; }
            set { this.DefinirPropriedade(ref this._materiaisOrganizados, value); }
        }

        public ObservableCollection<ModeloObj> DescarteResiduosLst
        {
            get { return this._descarteResiduosLst; }
            set { this.DefinirPropriedade(ref this._descarteResiduosLst, value); }
        }
        public ModeloObj DescarteResiduos
        {
            get { return this._descarteResiduos; }
            set { this.DefinirPropriedade(ref this._descarteResiduos, value); }
        }

        //D - Acidentes, Incidentes e Desvios
        public ObservableCollection<ModeloObj> IdentificacaoTratativaRiscosLst
        {
            get { return this._identificacaoTratativaRiscosLst; }
            set { this.DefinirPropriedade(ref this._identificacaoTratativaRiscosLst, value); }
        }
        public ModeloObj IdentificacaoTratativaRiscos
        {
            get { return this._identificacaoTratativaRiscos; }
            set { this.DefinirPropriedade(ref this._identificacaoTratativaRiscos, value); }
        }

        public ObservableCollection<ModeloObj> LinhaFogoLst
        {
            get { return this._linhaFogoLst; }
            set { this.DefinirPropriedade(ref this._linhaFogoLst, value); }
        }
        public ModeloObj LinhaFogo
        {
            get { return this._linhaFogo; }
            set { this.DefinirPropriedade(ref this._linhaFogo, value); }
        }

        public ObservableCollection<ModeloObj> PosturasErgoAdequadasLst
        {
            get { return this._posturasErgoAdequadasLst; }
            set { this.DefinirPropriedade(ref this._posturasErgoAdequadasLst, value); }
        }
        public ModeloObj PosturasErgoAdequadas
        {
            get { return this._posturasErgoAdequadas; }
            set { this.DefinirPropriedade(ref this._posturasErgoAdequadas, value); }
        }

        public ObservableCollection<ModeloObj> ConcentracaoTarefaLst
        {
            get { return this._concentracaoTarefaLst; }
            set { this.DefinirPropriedade(ref this._concentracaoTarefaLst, value); }
        }
        public ModeloObj ConcentracaoTarefa
        {
            get { return this._concentracaoTarefa; }
            set { this.DefinirPropriedade(ref this._concentracaoTarefa, value); }
        }

        //E - Planejamento, Procedimento e Instrução
        public ObservableCollection<ModeloObj> ConhecimentoProcedimentosLst
        {
            get { return this._conhecimentoProcedimentosLst; }
            set { this.DefinirPropriedade(ref this._conhecimentoProcedimentosLst, value); }
        }
        public ModeloObj ConhecimentoProcedimentos
        {
            get { return this._conhecimentoProcedimentos; }
            set { this.DefinirPropriedade(ref this._conhecimentoProcedimentos, value); }
        }

        public ObservableCollection<ModeloObj> ConhecimentoRiscosLst
        {
            get { return this._conhecimentoRiscosLst; }
            set { this.DefinirPropriedade(ref this._conhecimentoRiscosLst, value); }
        }
        public ModeloObj ConhecimentoRiscos
        {
            get { return this._conhecimentoRiscos; }
            set { this.DefinirPropriedade(ref this._conhecimentoRiscos, value); }
        }

        public ObservableCollection<ModeloObj> DireitoRecusaLst
        {
            get { return this._direitoRecusaLst; }
            set { this.DefinirPropriedade(ref this._direitoRecusaLst, value); }
        }
        public ModeloObj DireitoRecusa
        {
            get { return this._direitoRecusa; }
            set { this.DefinirPropriedade(ref this._direitoRecusa, value); }
        }

        public ObservableCollection<ModeloObj> AcoesEmergenciaLst
        {
            get { return this._acoesEmergenciaLst; }
            set { this.DefinirPropriedade(ref this._acoesEmergenciaLst, value); }
        }
        public ModeloObj AcoesEmergencia
        {
            get { return this._acoesEmergencia; }
            set { this.DefinirPropriedade(ref this._acoesEmergencia, value); }
        }

        public ObservableCollection<ModeloObj> RealizaProcessoRotinaLst
        {
            get { return this._realizaProcessoRotinaLst; }
            set { this.DefinirPropriedade(ref this._realizaProcessoRotinaLst, value); }
        }
        public ModeloObj RealizaProcessoRotina
        {
            get { return this._realizaProcessoRotina; }
            set { this.DefinirPropriedade(ref this._realizaProcessoRotina, value); }
        }

        //Ativadores de Comportamento
        public ObservableCollection<COGNITIVO> Cognitivos
        {
            get { return this._cognitivos; }
            set { this.DefinirPropriedade(ref this._cognitivos, value); }
        }
        public string IdsCognitivos
        {
            get
            {
                string ids = string.Empty;
                Guid[] marcados = this.Cognitivos.Where(m => m.ST_MARCADO).Select(m2 => m2.ID_COGNITIVO).ToArray();
                if (marcados.Length > 0)
                    ids = string.Join(";", marcados);
                return ids;
            }
        }

        public ObservableCollection<FISIOLOGICO> Fisiologicos
        {
            get { return this._fisiologicos; }
            set { this.DefinirPropriedade(ref this._fisiologicos, value); }
        }
        public string IdsFisiologicos
        {
            get
            {
                string ids = string.Empty;
                Guid[] marcados = this.Fisiologicos.Where(m => m.ST_MARCADO).Select(m2 => m2.ID_FISIOLOGICO).ToArray();
                if (marcados.Length > 0)
                    ids = string.Join(";", marcados);
                return ids;
            }
        }

        public ObservableCollection<PSICOLOGICO> Psicologicos
        {
            get { return this._psicologicos; }
            set { this.DefinirPropriedade(ref this._psicologicos, value); }
        }
        public string IdsPsicologicos
        {
            get
            {
                string ids = string.Empty;
                Guid[] marcados = this.Psicologicos.Where(m => m.ST_MARCADO).Select(m2 => m2.ID_PSICOLOGICO).ToArray();
                if (marcados.Length > 0)
                    ids = string.Join(";", marcados);
                return ids;
            }
        }

        public ObservableCollection<SOCIAL> Sociais
        {
            get { return this._sociais; }
            set { this.DefinirPropriedade(ref this._sociais, value); }
        }
        public string IdsSociais
        {
            get
            {
                string ids = string.Empty;
                Guid[] marcados = this.Sociais.Where(m => m.ST_MARCADO).Select(m2 => m2.ID_SOCIAL).ToArray();
                if (marcados.Length > 0)
                    ids = string.Join(";", marcados);
                return ids;
            }
        }

        public ModeloObj Observador
        {
            get { return this._observador; }
            set { this.DefinirPropriedade(ref this._observador, value); }
        }

        public ModeloObj RegistradoPor
        {
            get { return this._registradoPor; }
            set { this.DefinirPropriedade(ref this._registradoPor, value); }
        }

        #endregion Propriedades


        #region Commands

        public Command SalvarCommand { get; }

        public Command CancelarCommand { get; }

        #endregion Commands


        #region Construtores

        public ViewModelAbordagem(Action<bool, string> retornoSalvar, Action retornoCancelar)
        {
            this.SalvarCommand = new Command(async () => { await this.Salvar(retornoSalvar); });
            this.CancelarCommand = new Command(() => { retornoCancelar?.Invoke(); });            

            this.Ocupado = true;

            this.CorTituloPicker = "Transparent";
            if (Device.RuntimePlatform == Device.UWP)
            {
                if (Config.Estilo == "Claro")
                    this.CorTituloPicker = "Black";
                else
                    this.CorTituloPicker = "#e2e4e6";
            }

            this.Titulo = this.Textos.Abordagem;
        }

        #endregion Construtores


        #region Metodos de Gravação

        private async Task Salvar(Action<bool, string> retornoSalvar)
        {
            this.Ocupado = true;
            this.EmEdicao = false;

            RetornoRequest ret = new RetornoRequest();

            string mensagem = string.Empty;
            try
            {
                string erro = await this.VerificarCamposObrigatorios();
                if (!string.IsNullOrEmpty(erro))
                    retornoSalvar?.Invoke(false, erro);
                else
                {
                    ret = await this.SalvarDados();

                    if (ret.Ok)
                        retornoSalvar?.Invoke(true, this.Numero);
                    else
                        retornoSalvar?.Invoke(false, ret.Erro);
                }
            }
            catch (Exception ex)
            {
                retornoSalvar?.Invoke(false, ex.Message);
            }
            
            this.Ocupado = false;
            this.EmEdicao = true;
        }

        private async Task<RetornoRequest> SalvarDados()
        {
            if (Util.TemAcessoInternet)
                return await this.SalvarDadosApi();
            else
                return await this.SalvarDadosBanco();
        }

        private async Task<RetornoRequest> SalvarDadosBanco()
        {
            ABORDAGEM_COMPORTAMENTAL a = new ABORDAGEM_COMPORTAMENTAL();

            if (this._idAbordagemSQLite != Guid.Empty)
                a.ID_ABORDAGEM = this._idAbordagemSQLite;
            else
                a.ID_ABORDAGEM = Guid.NewGuid();

            a.DATA          = this.Data.AddSeconds(this.Hora.TotalSeconds);
            a.ID_FORNECEDOR = this.Fornecedor.Id;
            a.ID_REGIONAL   = this.UnidadeAbordagem.Id;
            a.ID_GERENCIA   = this.GerenciaAbordagem.Id;
            a.ID_AREA       = this.AreaAbordagem.Id;
            a.ID_LOCAL      = this.LocalAbordagem.Id;
            a.DESCRICAO     = this.DescAtividadeSituacao;

            //A - Equipamento de Proteção Individual e Coletivo
            a.ID_INERENTES_ATIVIDADE                  = this.InerentesAtividade?.Id;
            a.ID_RELACAO_COM_RISCOS                   = this.RelacaoComRiscos?.Id;
            a.ID_CONSERVACAO_ADEQUADA                 = this.ConservacaoAdequada?.Id;
            a.ID_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA = this.UtilizacaoCorretaFixacaoDistancia?.Id;

            a.ST_INERENTES_ATIVIDADE_VER_AGIR                  = this.InerentesAtividade.Marcado();
            a.ST_RELACAO_COM_RISCOS_VER_AGIR                   = this.RelacaoComRiscos.Marcado();
            a.ST_CONSERVACAO_ADEQUADA_VER_AGIR                 = this.ConservacaoAdequada.Marcado();
            a.ST_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA_VER_AGIR = this.UtilizacaoCorretaFixacaoDistancia.Marcado();

            //B - Máquinas, Veículos, Equipamentos e Ferramentas
            a.ID_IDENTIFICACAO_RISCOS_MAPEADOS = this.IdentificacaoRiscosMapeados?.Id;
            a.ID_MEDIDAS_PREVENCAO             = this.MedidasPrevencao?.Id;
            a.ID_BOAS_CONDICOES_USO            = this.BoasCondicoesUso?.Id;
            a.ID_UTILIZACAO_CORRETA            = this.UtilizacaoCorreta?.Id;
            a.ID_DESTINADOS_ATIVIDADE          = this.DestinadosAtividade?.Id;

            a.ST_IDENTIFICACAO_RISCOS_MAPEADOS_VER_AGIR = this.IdentificacaoRiscosMapeados.Marcado();
            a.ST_MEDIDAS_PREVENCAO_VER_AGIR             = this.MedidasPrevencao.Marcado();
            a.ST_BOAS_CONDICOES_USO_VER_AGIR            = this.BoasCondicoesUso.Marcado();
            a.ST_UTILIZACAO_CORRETA_VER_AGIR            = this.UtilizacaoCorreta.Marcado();
            a.ST_DESTINADOS_ATIVIDADE_VER_AGIR          = this.DestinadosAtividade.Marcado();

            //C - Programa Bom Senso
            a.ID_LOCAL_LIMPO           = this.LocalLimpo?.Id;
            a.ID_MATERIAIS_ORGANIZADOS = this.MateriaisOrganizados?.Id;
            a.ID_DESCARTE_RESIDUOS     = this.DescarteResiduos?.Id;

            a.ST_LOCAL_LIMPO_VER_AGIR           = this.LocalLimpo.Marcado();
            a.ST_MATERIAIS_ORGANIZADOS_VER_AGIR = this.MateriaisOrganizados.Marcado();
            a.ST_DESCARTE_RESIDUOS_VER_AGIR     = this.DescarteResiduos.Marcado();

            //D - Acidentes, Incidentes e Desvios
            a.ID_IDENTIFICACAO_TRATATIVA_RISCOS = this.IdentificacaoTratativaRiscos?.Id;
            a.ID_LINHA_FOGO                     = this.LinhaFogo?.Id;
            a.ID_POSTURAS_ERGO_ADEQUADAS        = this.PosturasErgoAdequadas?.Id;
            a.ID_CONCENTRACAO_TAREFA            = this.ConcentracaoTarefa?.Id;

            a.ST_IDENTIFICACAO_TRATATIVA_RISCOS_VER_AGIR = this.IdentificacaoTratativaRiscos.Marcado();
            a.ST_LINHA_FOGO_VER_AGIR                     = this.LinhaFogo.Marcado();
            a.ST_POSTURAS_ERGO_ADEQUADAS_VER_AGIR        = this.PosturasErgoAdequadas.Marcado();
            a.ST_CONCENTRACAO_TAREFA_VER_AGIR            = this.ConcentracaoTarefa.Marcado();

            //E - Planejamento, Procedimento e Instrução
            a.ID_CONHECIMENTO_PROCEDIMENTOS = this.ConhecimentoProcedimentos?.Id;
            a.ID_CONHECIMENTO_RISCOS        = this.ConhecimentoRiscos?.Id;
            a.ID_DIREITO_RECUSA             = this.DireitoRecusa?.Id;
            a.ID_ACOES_EMERGENCIA           = this.AcoesEmergencia?.Id;
            a.ID_REALIZA_PROCESSOROTINA     = this.RealizaProcessoRotina?.Id;

            a.ST_CONHECIMENTO_PROCEDIMENTOS_VER_AGIR = this.ConhecimentoProcedimentos.Marcado();
            a.ST_CONHECIMENTO_RISCOS_VER_AGIR        = this.ConhecimentoRiscos.Marcado();
            a.ST_DIREITO_RECUSA_VER_AGIR             = this.DireitoRecusa.Marcado();
            a.ST_ACOES_EMERGENCIA_VER_AGIR           = this.AcoesEmergencia.Marcado();
            a.ST_REALIZA_PROCESSOROTINA_VER_AGIR     = this.RealizaProcessoRotina.Marcado();

            //Ativadores de Comportamento
            a.COGNITIVOS   = this.IdsCognitivos;
            a.FISIOLOGICOS = this.IdsFisiologicos;
            a.PSICOLOGICOS = this.IdsPsicologicos;
            a.SOCIAIS      = this.IdsSociais;

            a.ID_OBSERVADOR    = this.Observador?.Id;
            a.ID_REGISTRADOPOR = this.RegistradoPor?.Id;
        
            RetornoRequest ret = new RetornoRequest();

            try
            {
                await App.Banco.InserirOuAlterarAsync(a);

                ret.Codigo = (int)System.Net.HttpStatusCode.OK;
                ret.Ok = true;
                ret.Mensagem = string.Empty;
            }
            catch (Exception ex)
            {
                ret.Ok = false;
                ret.Erro = ex.Message;
            }

            return ret;
        }

        private async Task<RetornoRequest> SalvarDadosApi()
        {
            string url = string.Concat(App.__EnderecoWebApi, "/Abordagem/Inserir");
            DateTime data = this.Data.AddSeconds(this.Hora.TotalSeconds);

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

            parametros.Add("DATA", this.Data.AddSeconds(this.Hora.TotalSeconds).ToString("yyyyMMdd HHmmss"));

            parametros.Add("ID_FORNECEDOR", this.Fornecedor.Id.ToString());
            parametros.Add("ID_REGIONAL", this.UnidadeAbordagem.Id.ToString());
            parametros.Add("ID_GERENCIA", this.GerenciaAbordagem.Id.ToString());
            parametros.Add("ID_AREA", this.AreaAbordagem.Id.ToString());
            parametros.Add("ID_LOCAL", this.LocalAbordagem.Id.ToString());
            parametros.Add("DESCRICAO", this.DescAtividadeSituacao);

            //A - Equipamento de Proteção Individual e Coletivo
            parametros.Add("ID_INERENTES_ATIVIDADE", this.InerentesAtividade.IdStrNullSafe());
            parametros.Add("ID_RELACAO_COM_RISCOS", this.RelacaoComRiscos.IdStrNullSafe());
            parametros.Add("ID_CONSERVACAO_ADEQUADA", this.ConservacaoAdequada.IdStrNullSafe());
            parametros.Add("ID_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA", this.UtilizacaoCorretaFixacaoDistancia.IdStrNullSafe());

            parametros.Add("ST_INERENTES_ATIVIDADE_VER_AGIR", this.InerentesAtividade.MarcadoStr());
            parametros.Add("ST_RELACAO_COM_RISCOS_VER_AGIR", this.RelacaoComRiscos.MarcadoStr());
            parametros.Add("ST_CONSERVACAO_ADEQUADA_VER_AGIR", this.ConservacaoAdequada.MarcadoStr());
            parametros.Add("ST_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA_VER_AGIR", this.UtilizacaoCorretaFixacaoDistancia.MarcadoStr());

            //B - Máquinas, Veículos, Equipamentos e Ferramentas
            parametros.Add("ID_IDENTIFICACAO_RISCOS_MAPEADOS", this.IdentificacaoRiscosMapeados.IdStrNullSafe());
            parametros.Add("ID_MEDIDAS_PREVENCAO", this.MedidasPrevencao.IdStrNullSafe());
            parametros.Add("ID_BOAS_CONDICOES_USO", this.BoasCondicoesUso.IdStrNullSafe());
            parametros.Add("ID_UTILIZACAO_CORRETA", this.UtilizacaoCorreta.IdStrNullSafe());
            parametros.Add("ID_DESTINADOS_ATIVIDADE", this.DestinadosAtividade.IdStrNullSafe());

            parametros.Add("ST_IDENTIFICACAO_RISCOS_MAPEADOS_VER_AGIR", this.IdentificacaoRiscosMapeados.MarcadoStr());
            parametros.Add("ST_MEDIDAS_PREVENCAO_VER_AGIR", this.MedidasPrevencao.MarcadoStr());
            parametros.Add("ST_BOAS_CONDICOES_USO_VER_AGIR", this.BoasCondicoesUso.MarcadoStr());
            parametros.Add("ST_UTILIZACAO_CORRETA_VER_AGIR", this.UtilizacaoCorreta.MarcadoStr());
            parametros.Add("ST_DESTINADOS_ATIVIDADE_VER_AGIR", this.DestinadosAtividade.MarcadoStr());

            //C - Programa Bom Senso
            parametros.Add("ID_LOCAL_LIMPO", this.LocalLimpo.IdStrNullSafe());
            parametros.Add("ID_MATERIAIS_ORGANIZADOS", this.MateriaisOrganizados.IdStrNullSafe());
            parametros.Add("ID_DESCARTE_RESIDUOS", this.DescarteResiduos.IdStrNullSafe());

            parametros.Add("ST_LOCAL_LIMPO_VER_AGIR", this.LocalLimpo.MarcadoStr());
            parametros.Add("ST_MATERIAIS_ORGANIZADOS_VER_AGIR", this.MateriaisOrganizados.MarcadoStr());
            parametros.Add("ST_DESCARTE_RESIDUOS_VER_AGIR", this.DescarteResiduos.MarcadoStr());

            //D - Acidentes, Incidentes e Desvios
            parametros.Add("ID_IDENTIFICACAO_TRATATIVA_RISCOS", this.IdentificacaoTratativaRiscos.IdStrNullSafe());
            parametros.Add("ID_LINHA_FOGO", this.LinhaFogo.IdStrNullSafe());
            parametros.Add("ID_POSTURAS_ERGO_ADEQUADAS", this.PosturasErgoAdequadas.IdStrNullSafe());
            parametros.Add("ID_CONCENTRACAO_TAREFA", this.ConcentracaoTarefa.IdStrNullSafe());

            parametros.Add("ST_IDENTIFICACAO_TRATATIVA_RISCOS_VER_AGIR", this.IdentificacaoTratativaRiscos.MarcadoStr());
            parametros.Add("ST_LINHA_FOGO_VER_AGIR", this.LinhaFogo.MarcadoStr());
            parametros.Add("ST_POSTURAS_ERGO_ADEQUADAS_VER_AGIR", this.PosturasErgoAdequadas.MarcadoStr());
            parametros.Add("ST_CONCENTRACAO_TAREFA_VER_AGIR", this.ConcentracaoTarefa.MarcadoStr());

            //E - Planejamento, Procedimento e Instrução
            parametros.Add("ID_CONHECIMENTO_PROCEDIMENTOS", this.ConhecimentoProcedimentos.IdStrNullSafe());
            parametros.Add("ID_CONHECIMENTO_RISCOS", this.ConhecimentoRiscos.IdStrNullSafe());
            parametros.Add("ID_DIREITO_RECUSA", this.DireitoRecusa.IdStrNullSafe());
            parametros.Add("ID_ACOES_EMERGENCIA", this.AcoesEmergencia.IdStrNullSafe());
            parametros.Add("ID_REALIZA_PROCESSOROTINA", this.RealizaProcessoRotina.IdStrNullSafe());

            parametros.Add("ST_CONHECIMENTO_PROCEDIMENTOS_VER_AGIR", this.ConhecimentoProcedimentos.MarcadoStr());
            parametros.Add("ST_CONHECIMENTO_RISCOS_VER_AGIR", this.ConhecimentoRiscos.MarcadoStr());
            parametros.Add("ST_DIREITO_RECUSA_VER_AGIR", this.DireitoRecusa.MarcadoStr());
            parametros.Add("ST_ACOES_EMERGENCIA_VER_AGIR", this.AcoesEmergencia.MarcadoStr());
            parametros.Add("ST_REALIZA_PROCESSOROTINA_VER_AGIR", this.RealizaProcessoRotina.MarcadoStr());

            //Ativadores de Comportamento
            parametros.Add("COGNITIVOS", this.IdsCognitivos);
            parametros.Add("FISIOLOGICOS", this.IdsFisiologicos);
            parametros.Add("PSICOLOGICOS", this.IdsPsicologicos);
            parametros.Add("SOCIAIS", this.IdsSociais);

            if (this.Observador != null)
                parametros.Add("ID_OBSERVADOR", this.Observador.IdStrNullSafe());
            if (this.RegistradoPor != null)
                parametros.Add("ID_REGISTRADOPOR", this.RegistradoPor.IdStrNullSafe());
            
            if (!string.IsNullOrEmpty(this.Numero))
                parametros.Add("NU_ABORDAGEM", this.Numero);

            RetornoRequest ret = new RetornoRequest();

            FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

            using (HttpClient requisicao = new HttpClient())
            {
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);
                string conteudo = await resposta.Content.ReadAsStringAsync();
                conteudo = conteudo.Replace("\"", string.Empty);

                ret.Codigo = (int)resposta.StatusCode;
                ret.Ok = resposta.StatusCode == System.Net.HttpStatusCode.OK;

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string id = string.Empty;
                    if (conteudo.Contains("|"))
                    {
                        string[] valores = conteudo.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (valores.Length == 2)
                        {
                            id = valores[0];
                            this.Numero = valores[1];
                        }
                    }
                    else
                        this.Numero = conteudo;
                    ret.Mensagem = conteudo;
                }
                else
                    ret.Erro = conteudo;
            }

            return ret;
        }

        #endregion Metodos de Gravação


        #region Busca dos dados

        private async Task PreencherRespostas(ObservableCollection<ModeloObj> lst)
        {
            if(this._respostasSeguroInseguro == null || this._respostasSeguroInseguro.Count == 0)
                this._respostasSeguroInseguro = await App.Banco.BuscarSeguroInsegurosAsync();
            
            foreach (SEGURO_INSEGURO i in this._respostasSeguroInseguro)
                lst.Add(i.ToModeloObj());
        }

        public override async Task LoadAsync()
        {
            try
            {
                if (!this.TelaCarregada)
                {
                    this.Ocupado = true;
                    this.EmEdicao = true;

                    await this.LimparCampos();

                    //A - Equipamento de Proteção Individual e Coletivo
                    await this.PreencherRespostas(this.InerentesAtividadeLst);
                    await this.PreencherRespostas(this.RelacaoComRiscosLst);
                    await this.PreencherRespostas(this.ConservacaoAdequadaLst);
                    await this.PreencherRespostas(this.UtilizacaoCorretaFixacaoDistanciaLst);

                    //B - Máquinas, Veículos, Equipamentos e Ferramentas
                    await this.PreencherRespostas(this.IdentificacaoRiscosMapeadosLst);
                    await this.PreencherRespostas(this.MedidasPrevencaoLst);
                    await this.PreencherRespostas(this.BoasCondicoesUsoLst);
                    await this.PreencherRespostas(this.UtilizacaoCorretaLst);
                    await this.PreencherRespostas(this.DestinadosAtividadeLst);

                    //C - Programa Bom Senso
                    await this.PreencherRespostas(this.LocalLimpoLst);
                    await this.PreencherRespostas(this.MateriaisOrganizadosLst);
                    await this.PreencherRespostas(this.DescarteResiduosLst);

                    //D - Acidentes, Incidentes e Desvios
                    await this.PreencherRespostas(this.IdentificacaoTratativaRiscosLst);
                    await this.PreencherRespostas(this.LinhaFogoLst);
                    await this.PreencherRespostas(this.PosturasErgoAdequadasLst);
                    await this.PreencherRespostas(this.ConcentracaoTarefaLst);

                    //E - Planejamento, Procedimento e Instrução
                    await this.PreencherRespostas(this.ConhecimentoProcedimentosLst);
                    await this.PreencherRespostas(this.ConhecimentoRiscosLst);
                    await this.PreencherRespostas(this.DireitoRecusaLst);
                    await this.PreencherRespostas(this.AcoesEmergenciaLst);
                    await this.PreencherRespostas(this.RealizaProcessoRotinaLst);

                    List<COGNITIVO> cogs = await App.Banco.BuscarCognitivosAsync();
                    foreach (COGNITIVO c in cogs)
                        this.Cognitivos.Add(c);

                    List<FISIOLOGICO> fis = await App.Banco.BuscarFisiologicosAsync();
                    foreach (FISIOLOGICO f in fis)
                        this.Fisiologicos.Add(f);

                    List<PSICOLOGICO> psi = await App.Banco.BuscarPsicologicosAsync();
                    foreach (PSICOLOGICO p in psi)
                        this.Psicologicos.Add(p);

                    List<SOCIAL> soc = await App.Banco.BuscarSociaisAsync();
                    foreach (SOCIAL s in soc)
                        this.Sociais.Add(s);

                    this.TelaCarregada = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }

        public async Task LimparCampos()
        {
            this.Numero                = string.Empty;
            this.Data                  = DateTime.Today;
            this.Hora                  = DateTime.Now.TimeOfDay;
            this.DescAtividadeSituacao = null;
            this.Fornecedor            = null;
            this.UnidadeAbordagem      = null;
            this.GerenciaAbordagem     = null;
            this.AreaAbordagem         = null;
            this.LocalAbordagem        = null;


            //A - Equipamento de Proteção Individual e Coletivo
            this.InerentesAtividade                = null;
            this.RelacaoComRiscos                  = null;
            this.ConservacaoAdequada               = null;
            this.UtilizacaoCorretaFixacaoDistancia = null;

            //B - Máquinas, Veículos, Equipamentos e Ferramentas
            this.IdentificacaoRiscosMapeados = null;
            this.MedidasPrevencao            = null;
            this.BoasCondicoesUso            = null;
            this.UtilizacaoCorreta           = null;
            this.DestinadosAtividade         = null;

            //C - Programa Bom Sensov
            this.LocalLimpo           = null;
            this.MateriaisOrganizados = null;
            this.DescarteResiduos     = null;

            //D - Acidentes, Incidentes e Desvios
            this.IdentificacaoTratativaRiscos = null;
            this.LinhaFogo                    = null;
            this.PosturasErgoAdequadas        = null;
            this.ConcentracaoTarefa           = null;

            //E - Planejamento, Procedimento e Instrução
            this.ConhecimentoProcedimentos = null;
            this.ConhecimentoRiscos        = null;
            this.DireitoRecusa             = null;
            this.AcoesEmergencia           = null;
            this.RealizaProcessoRotina     = null;

            this.Observador = null;
            this.RegistradoPor = null;

            if (UsuarioLogado.Instancia.ID_VINCULO != Guid.Empty)
                this.RegistradoPor = await this.Vinculo();
            else
                this.RegistradoPor = null;
        }

        public void GetAbordagem(string idAbordagem, OrigemDados origem)
        {
            if (origem == OrigemDados.Api)
                this.GetAbordagemApi(idAbordagem);
            else if (origem == OrigemDados.SQLite)
                this.GetAbordagemBanco(idAbordagem);
            this.TelaCarregada = true;
        }

        public async void GetAbordagemBanco(string idAbordagem)
        {
            ABORDAGEM_COMPORTAMENTAL a = null;
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                this._idAbordagemSQLite = new Guid(idAbordagem);

                a = await App.Banco.BuscarAbordagemAsync(this._idAbordagemSQLite);

                this.Numero = string.Empty;
                this.Data = a.DATA.Date;
                this.Hora = a.DATA.TimeOfDay;
                this.DescAtividadeSituacao = a.DESCRICAO;

                FORNECEDOR fornecedor = await App.Banco.BuscarFornecedorAsync(a.ID_FORNECEDOR);
                if (fornecedor != null)
                    this.Fornecedor = fornecedor.ToModeloObj();

                REGIONAL regional = await App.Banco.BuscarRegionalAsync(a.ID_REGIONAL);
                if (regional != null)
                    this.UnidadeAbordagem = regional.ToModeloObj();

                GERENCIA gerencia = await App.Banco.BuscarGerenciaAsync(a.ID_GERENCIA);
                if (gerencia != null)
                    this.GerenciaAbordagem = gerencia.ToModeloObj();

                AREA area = await App.Banco.BuscarAreaAsync(a.ID_AREA);
                if (area != null)
                    this.AreaAbordagem = area.ToModeloObj();

                LOCAL local = await App.Banco.BuscarLocalAsync(a.ID_LOCAL);
                if (local != null)
                    this.LocalAbordagem = local.ToModeloObj();

                if (a.ID_OBSERVADOR.HasValue)
                {
                    VINCULO vinculo = await App.Banco.BuscarVinculoAsync(a.ID_OBSERVADOR.Value);
                    if (vinculo != null)
                        this.Observador = vinculo.ToModeloObj();
                }

                if (a.ID_REGISTRADOPOR.HasValue)
                {
                    VINCULO vinculo = await App.Banco.BuscarVinculoAsync(a.ID_REGISTRADOPOR.Value);
                    if (vinculo != null)
                        this.RegistradoPor = vinculo.ToModeloObj();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar a abordagem comportamental.");
            }
            finally
            {
                this.Ocupado = false;
            }
        }

        public async void GetAbordagemApi(string idAbordagem)
        {
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                string url = string.Concat(App.__EnderecoWebApi, "/Abordagem/RetornarAbordagem");

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                parametros.Add("idAbordagem", idAbordagem);

                FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                using (HttpClient requisicao = new HttpClient())
                {
                    HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        Abordagem a = Newtonsoft.Json.JsonConvert.DeserializeObject<Abordagem>(conteudo);

                        this.Numero = a.CODIGO;
                        this.Data = a.DATA;
                        this.Hora = a.HORA;
                        this.DescAtividadeSituacao = a.DESCRICAO;
                        this.Fornecedor = a.FORNECEDOR;
                        this.UnidadeAbordagem = a.UNIDADE;
                        this.GerenciaAbordagem = a.GERENCIA;
                        this.AreaAbordagem = a.AREA;
                        this.LocalAbordagem = a.LOCAL;

                        //A - Equipamento de Proteção Individual e Coletivo
                        this.InerentesAtividade = this.GetModelo(this.InerentesAtividadeLst, a.INERENTES_ATIVIDADE);
                        this.RelacaoComRiscos = this.GetModelo(this.RelacaoComRiscosLst, a.RELACAO_COM_RISCOS);
                        this.ConservacaoAdequada = this.GetModelo(this.ConservacaoAdequadaLst, a.CONSERVACAO_ADEQUADA);
                        this.UtilizacaoCorretaFixacaoDistancia = this.GetModelo(this.UtilizacaoCorretaFixacaoDistanciaLst, a.UTILIZACAO_CORRETA_FIXACAO_DISTANCIA);

                        //B - Máquinas, Veículos, Equipamentos e Ferramentas
                        this.IdentificacaoRiscosMapeados = this.GetModelo(this.IdentificacaoRiscosMapeadosLst, a.IDENTIFICACAO_RISCOS_MAPEADOS);
                        this.MedidasPrevencao = this.GetModelo(this.MedidasPrevencaoLst, a.MEDIDAS_PREVENCAO);
                        this.BoasCondicoesUso = this.GetModelo(this.BoasCondicoesUsoLst, a.BOAS_CONDICOES_USO);
                        this.UtilizacaoCorreta = this.GetModelo(this.UtilizacaoCorretaLst, a.UTILIZACAO_CORRETA);
                        this.DestinadosAtividade = this.GetModelo(this.DestinadosAtividadeLst, a.DESTINADOS_ATIVIDADE);

                        //C - Programa Bom Sensov
                        this.LocalLimpo = this.GetModelo(this.LocalLimpoLst, a.LOCAL_LIMPO);
                        this.MateriaisOrganizados = this.GetModelo(this.MateriaisOrganizadosLst, a.MATERIAIS_ORGANIZADOS);
                        this.DescarteResiduos = this.GetModelo(this.DescarteResiduosLst, a.DESCARTE_RESIDUOS);

                        //D - Acidentes, Incidentes e Desvios
                        this.IdentificacaoTratativaRiscos = this.GetModelo(this.IdentificacaoTratativaRiscosLst, a.IDENTIFICACAO_TRATATIVA_RISCOS);
                        this.LinhaFogo = this.GetModelo(this.LinhaFogoLst, a.LINHA_FOGO);
                        this.PosturasErgoAdequadas = this.GetModelo(this.PosturasErgoAdequadasLst, a.POSTURAS_ERGO_ADEQUADAS);
                        this.ConcentracaoTarefa = this.GetModelo(this.ConcentracaoTarefaLst, a.CONCENTRACAO_TAREFA);

                        //E - Planejamento, Procedimento e Instrução
                        this.ConhecimentoProcedimentos = this.GetModelo(this._conhecimentoProcedimentosLst, a.CONHECIMENTO_PROCEDIMENTOS);
                        this.ConhecimentoRiscos = this.GetModelo(this.ConhecimentoRiscosLst, a.CONHECIMENTO_RISCOS);
                        this.DireitoRecusa = this.GetModelo(this.DireitoRecusaLst, a.DIREITO_RECUSA);
                        this.AcoesEmergencia = this.GetModelo(this.AcoesEmergenciaLst, a.ACOES_EMERGENCIA);
                        this.RealizaProcessoRotina = this.GetModelo(this.RealizaProcessoRotinaLst, a.REALIZA_PROCESSOROTINA);

                        foreach (ModeloObj c in a.COGNITIVOS)
                        {
                            COGNITIVO m = this.Cognitivos.FirstOrDefault(i => i.ID_COGNITIVO == c.Id);
                            if (m != null)
                                m.ST_MARCADO = true;
                        }

                        foreach (ModeloObj c in a.FISIOLOGICOS)
                        {
                            FISIOLOGICO m = this.Fisiologicos.FirstOrDefault(i => i.ID_FISIOLOGICO == c.Id);
                            if (m != null)
                                m.ST_MARCADO = true;
                        }

                        foreach (ModeloObj c in a.PSICOLOGICOS)
                        {
                            PSICOLOGICO m = this.Psicologicos.FirstOrDefault(i => i.ID_PSICOLOGICO == c.Id);
                            if (m != null)
                                m.ST_MARCADO = true;
                        }

                        foreach (ModeloObj c in a.SOCIAIS)
                        {
                            SOCIAL m = this.Sociais.FirstOrDefault(i => i.ID_SOCIAL == c.Id);
                            if (m != null)
                                m.ST_MARCADO = true;
                        }

                        this.Observador = a.OBSERVADOR;
                        this.RegistradoPor = a.REGISTRADOPOR;

                        await this.Atualizar();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar a abordagem comportamental.");
            }
            finally
            {
                this.Ocupado = false;
            }
        }

        private ModeloObj GetModelo(ObservableCollection<ModeloObj> lst, ModeloObj modelo)
        {
            if (modelo != null)
                return this.GetModelo(lst, modelo.Id, modelo.Marcado);
            return null;
        }

        private ModeloObj GetModelo(ObservableCollection<ModeloObj> lst, Guid id, bool marcado)
        {
            if (lst != null)
            {
                ModeloObj ret = lst.Where(m => m.Id == id).FirstOrDefault();
                if (ret != null)
                    ret.Marcado = marcado;
                return ret;
            }
            return null;
        }

        #endregion Busca dos dados


        #region Validação da ocorrência

        private bool DataValida()
        {
            bool ok = this.Data != this.DataMinima;
            return ok;
        }

        private bool FornecedorValido()
        {
            bool ok = this.Fornecedor != null;
            return ok;
        }

        private bool UnidadeAbordagemValida()
        {
            bool ok = this.UnidadeAbordagem != null;
            return ok;
        }

        private bool GerenciaValida()
        {
            bool ok = this.GerenciaAbordagem != null;
            return ok;
        }

        private bool AreaValida()
        {
            bool ok = this.AreaAbordagem != null;
            return ok;
        }

        private bool LocalValido()
        {
            bool ok = this.LocalAbordagem != null;
            return ok;
        }

        private bool DescricaoValida()
        {
            bool ok = !string.IsNullOrEmpty(this.DescAtividadeSituacao);
            return ok;
        }


        private Task<string> VerificarCamposObrigatorios()
        {
            return Task.Run(() =>
            {
                string erro = string.Empty;

                if (!this.DataValida()) erro += Environment.NewLine + this.Textos.Data;
                if (!this.FornecedorValido()) erro += Environment.NewLine + this.Textos.EmpresaObservada;
                if (!this.UnidadeAbordagemValida()) erro += Environment.NewLine + this.Textos.UnidadeAbordagem;
                if (!this.GerenciaValida()) erro += Environment.NewLine + this.Textos.GerenciaAbordagem;
                if (!this.AreaValida()) erro += Environment.NewLine + this.Textos.AreaAbordagem;
                if (!this.LocalValido()) erro += Environment.NewLine + this.Textos.LocalAbordagem;
                if (!this.DescricaoValida()) erro += Environment.NewLine + this.Textos.DescAtividadeSituacao;

                if (!string.IsNullOrEmpty(erro))
                    erro = string.Concat(Environment.NewLine, this.Textos.CamposObrigatorios, ": ", Environment.NewLine, erro);

                return erro;
            });
        }

        #endregion Validação da ocorrência
    }
}
