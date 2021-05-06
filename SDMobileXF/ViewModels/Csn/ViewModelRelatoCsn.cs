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
    public class ViewModelRelatoCsn : ViewModelBase
    {
        #region Variaveis

        private string _numero;
        private string _tituloAnomalia;
        private DateTime _date = DateTime.MinValue;
        private TimeSpan _hora = TimeSpan.Zero;
        private ModeloObj _diaSemana;
        private ModeloObj _regiaoSetor;
        private ModeloObj _letra;
        private ModeloObj _turno;
        private ModeloObj _gerenciaGeral;
        private ModeloObj _gerencia;
        private ModeloObj _areaEquipamento;
        private ModeloObj _localEquipamento;
        private string _localAnomalia;
        private string _descricaoPreliminar;
        private bool? _acoesImediatas;
        private string _remocaoSintomas;
        private ModeloObj _origemAnomalia;
        private ModeloObj _tipoAnomalia;
        private ModeloObj _classificacaoTipo;
        private ModeloObj _probabilidade;
        private ModeloObj _severidade;
        private string _produtoAB;
        private ModeloObj _resultadoClassificacaoRisco;
        private ModeloObj _registradoPor;
        private ModeloObj _relatadoPor;
        private ModeloObj _supervisorImediato;
        private ModeloObj _assinatura;

        private Guid _idOcorrenciaSQLite = Guid.Empty;
        
        #endregion Variaveis


        #region Propriedades

        public DateTime DataMaxima => new DateTime(2999, 12, 31);

        public DateTime DataMinima => new DateTime(2020, 01, 01);

        public string Numero
        {
            get { return this._numero; }
            set { this.DefinirPropriedade(ref this._numero, value); }
        }

        public string TituloAnomalia
        {
            get { return this._tituloAnomalia; }
            set { this.DefinirPropriedade(ref this._tituloAnomalia, value); }
        }

        public bool ExibirNumero
        {
            get { return !this.EmEdicao && !string.IsNullOrEmpty(this.Numero); }
        }

        public DateTime Data
        {
            get { return this._date; }
            set 
            {
                this.DefinirPropriedade(ref this._date, value);
                this.Dia = Util.LstDiaSemana.FirstOrDefault(d => d.Codigo == ((int)this._date.DayOfWeek + 1).ToString());
            }
        }

        public TimeSpan Hora
        {
            get { return this._hora; }
            set { this.DefinirPropriedade(ref this._hora, value); }
        }

        public ModeloObj Dia
        {
            get { return this._diaSemana; }
            set { this.DefinirPropriedade(ref this._diaSemana, value); }
        }

        public ModeloObj RegiaoSetor
        {
            get { return this._regiaoSetor; }
            set { this.DefinirPropriedade(ref this._regiaoSetor, value); }
        }

        public ModeloObj Letra
        {
            get { return this._letra; }
            set { this.DefinirPropriedade(ref this._letra, value); }
        }

        public ModeloObj Turno
        {
            get { return this._turno; }
            set { this.DefinirPropriedade(ref this._turno, value); }
        }

        public ModeloObj GerenciaGeral
        {
            get { return this._gerenciaGeral; }
            set { this.DefinirPropriedade(ref this._gerenciaGeral, value); }
        }

        public ModeloObj Gerencia
        {
            get { return this._gerencia; }
            set { this.DefinirPropriedade(ref this._gerencia, value); }
        }

        public ModeloObj AreaEquipamento
        {
            get { return this._areaEquipamento; }
            set { this.DefinirPropriedade(ref this._areaEquipamento, value); }
        }

        public ModeloObj LocalEquipamento
        {
            get { return this._localEquipamento; }
            set { this.DefinirPropriedade(ref this._localEquipamento, value); }
        }

        public string LocalAnomalia
        {
            get { return this._localAnomalia; }
            set { this.DefinirPropriedade(ref this._localAnomalia, value); }
        }

        public string DescricaoPreliminar
        {
            get { return this._descricaoPreliminar; }
            set { this.DefinirPropriedade(ref this._descricaoPreliminar, value); }
        }

        public bool? AcoesImediatasSim
        {
            get
            {
                if (this._acoesImediatas.HasValue)
                    return this._acoesImediatas.Value;
                return false;
            }
            set
            {
                this._acoesImediatas = value;
                this.OnPropertyChanged("AcoesImediatasSim");
            }
        }

        public bool? AcoesImediatasNao
        {
            get
            {
                if (this._acoesImediatas.HasValue)
                    return !this._acoesImediatas.Value;
                return false;
            }
            set
            {
                if (value.HasValue)
                    this._acoesImediatas = !value;
                else
                    this._acoesImediatas = null;
                this.OnPropertyChanged("AcoesImediatasNao");
            }
        }

        public string RemocaoSintomas
        {
            get { return this._remocaoSintomas; }
            set { this.DefinirPropriedade(ref this._remocaoSintomas, value); }
        }

        public ModeloObj OrigemAnomalia
        {
            get { return this._origemAnomalia; }
            set { this.DefinirPropriedade(ref this._origemAnomalia, value); }
        }

        public ModeloObj TipoAnomalia
        {
            get { return this._tipoAnomalia; }
            set { this.DefinirPropriedade(ref this._tipoAnomalia, value); }
        }

        public ModeloObj ClassificacaoTipo
        {
            get { return this._classificacaoTipo; }
            set { this.DefinirPropriedade(ref this._classificacaoTipo, value); }
        }

        public ModeloObj Probabilidade
        {
            get { return this._probabilidade; }
            set
            {
                this.DefinirPropriedade(ref this._probabilidade, value);
                this.CalcularProdutoAB();
            }
        }

        public ModeloObj Severidade
        {
            get { return this._severidade; }
            set
            { 
                this.DefinirPropriedade(ref this._severidade, value);
                this.CalcularProdutoAB();
            }
        }

        private void CalcularProdutoAB()
        {
            int ab = 0;
            if (this.Probabilidade != null && this.Severidade != null)
            {
                int p = Convert.ToInt32(this.Probabilidade.Codigo);
                int s = Convert.ToInt32(this.Severidade.Codigo);
                ab = Util.CalcularProdutoAB(p, s);
            }
            if (ab > 0)
            {
                this.ProdutoAB = ab.ToString();
                ModeloObj m = Util.ResultadoRisco(ab);

                if (m != null)
                    this.ResultadoClassificacaoRisco = m;
            }
            else
            {
                this.ProdutoAB = string.Empty;
                this.ResultadoClassificacaoRisco = null;
            }
        }

        public string ProdutoAB
        {
            get { return this._produtoAB; }
            set { this.DefinirPropriedade(ref this._produtoAB, value); }
        }

        public ModeloObj ResultadoClassificacaoRisco
        {
            get { return this._resultadoClassificacaoRisco; }
            set { this.DefinirPropriedade(ref this._resultadoClassificacaoRisco, value); }
        }

        public ModeloObj RegistradoPor
        {
            get { return this._registradoPor; }
            set { this.DefinirPropriedade(ref this._registradoPor, value); }
        }

        public ModeloObj RelatadoPor
        {
            get { return this._relatadoPor; }
            set { this.DefinirPropriedade(ref this._relatadoPor, value); }
        }

        public ModeloObj SupervisorImediato
        {
            get { return this._supervisorImediato; }
            set { this.DefinirPropriedade(ref this._supervisorImediato, value); }
        }

        public ModeloObj Assinatura
        {
            get { return this._assinatura; }
            set { this.DefinirPropriedade(ref this._assinatura, value); }
        }

        #endregion Propriedades


        #region Commands

        public Command SalvarCommand { get; }

        public Command CancelarCommand { get; }

        public Command ApagarFotoCommand { get; }

        #endregion Commands


        #region Construtores

        public ViewModelRelatoCsn(Action<bool, string> retornoSalvar, Action retornoCancelar)
        {
            this.SalvarCommand = new Command(async () => { await this.Salvar(retornoSalvar); });
            this.CancelarCommand = new Command(() => { retornoCancelar?.Invoke(); });            

            this.Ocupado = true;

            this.Titulo = this.Textos.Ocorrencia;
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
                    {
                        retornoSalvar?.Invoke(true, this.Numero);
                    }
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
            OCORRENCIACSN o = new OCORRENCIACSN();

            if (this._idOcorrenciaSQLite != Guid.Empty)
                o.ID_OCORRENCIA = this._idOcorrenciaSQLite;
            else
                o.ID_OCORRENCIA = Guid.NewGuid();

            o.TITULO = this.TituloAnomalia;
            o.DATA = this.Data.AddSeconds(this.Hora.TotalSeconds);

            o.REGIAOSETOR = this.RegiaoSetor?.Id;
            o.LETRA = this.Letra?.Id;
            o.TURNO = this.Turno?.Id;

            o.GERENCIAGERAL = this.GerenciaGeral?.Id;
            o.GERENCIA = this.Gerencia?.Id;
            o.AREAEQUIPAMENTO = this.AreaEquipamento?.Id;
            o.LOCALEQUIPAMENTO = this.LocalEquipamento?.Id;

            o.LOCALANOMALIA = this.LocalAnomalia;
            o.DESCRICAOPRELIMINAR = this.DescricaoPreliminar;

            o.ST_ACAOIMEDIATA = this.AcoesImediatasSim;
            o.REMOCAOSINTOMAS = this.RemocaoSintomas;

            o.ORIGEMANOMALIA = this.OrigemAnomalia?.Id;
            o.TIPOANOMALIA = this.TipoAnomalia?.Id;
            o.CLASSIFICACAOTIPO = this.ClassificacaoTipo?.Id;

            o.PROBABILIDADE = this.Probabilidade?.Id;
            o.SEVERIDADE = this.Severidade?.Id;

            o.REGISTRADOPOR = this.RegistradoPor?.Id;
            o.RELATADOPOR = this.RelatadoPor?.Id;
            o.SUPERVISOR = this.SupervisorImediato?.Id;
            o.ASSINATURA = this.Assinatura?.Id;

            RetornoRequest ret = new RetornoRequest();

            try
            {
                await App.Banco.InserirOuAlterarAsync(o);

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
            string url = string.Concat(App.__EnderecoWebApi, "/OcorrenciasCsn/Inserir");
            DateTime data = this.Data.AddSeconds(this.Hora.TotalSeconds);

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

            if (!string.IsNullOrEmpty(this.Numero))
                parametros.Add("NU_OCORRENCIA", this.Numero);

            parametros.Add("DATA", this.Data.AddSeconds(this.Hora.TotalSeconds).ToString("yyyyMMdd HHmmss"));
            parametros.Add("TITULO", this.TituloAnomalia);
            parametros.Add("ID_DIA", this.Dia.IdStrNullSafe());

            parametros.Add("ID_REGIAO", this.RegiaoSetor.IdStrNullSafe());
            parametros.Add("ID_LETRA", this.Letra.IdStrNullSafe());
            parametros.Add("ID_TURNO", this.Turno.IdStrNullSafe());

            parametros.Add("ID_GERENCIA_GERAL", this.GerenciaGeral.IdStrNullSafe());
            parametros.Add("ID_GERENCIA", this.Gerencia.IdStrNullSafe());
            parametros.Add("ID_AREA_EQUIPAMENTO", this.AreaEquipamento.IdStrNullSafe());
            parametros.Add("ID_LOCAL_EQUIPAMENTO", this.LocalEquipamento.IdStrNullSafe());

            parametros.Add("LOCAL_ANOMALIA", this.LocalAnomalia);
            parametros.Add("DESCRICAO_PRELIMINAR", this.DescricaoPreliminar);

            if (this.AcoesImediatasSim.HasValue)
                parametros.Add("ACOES_IMEDIATAS", this.AcoesImediatasSim.Value.ToIdSimNao());
            
            parametros.Add("REMOCAO_SINTOMAS", this.RemocaoSintomas);
            parametros.Add("ID_ORIGEM", this.OrigemAnomalia.IdStrNullSafe());
            parametros.Add("ID_TIPO_ANOMALIA", this.TipoAnomalia.IdStrNullSafe());
            parametros.Add("ID_CLASSIFICACAO_TIPO", this.ClassificacaoTipo.IdStrNullSafe());
            parametros.Add("ID_PROBABILIDADE", this.Probabilidade.IdStrNullSafe());
            parametros.Add("ID_SEVERIDADE", this.Severidade.IdStrNullSafe());
            parametros.Add("ID_RESULTADO", this.ResultadoClassificacaoRisco.IdStrNullSafe());
            parametros.Add("PRODUTOAB", this.ProdutoAB);

            parametros.Add("ID_REGISTRADO_POR", this.RegistradoPor.IdStrNullSafe());
            parametros.Add("ID_RELATADO_POR", this.RelatadoPor.IdStrNullSafe());
            parametros.Add("ID_SUPERVISOR", this.SupervisorImediato.IdStrNullSafe());
            parametros.Add("ID_ASSINATURA", this.Assinatura.IdStrNullSafe());

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

        public override async Task LoadAsync()
        {
            try
            {
                if (!this.TelaCarregada)
                {
                    this.Ocupado = true;
                    this.EmEdicao = true;

                    await this.LimparCampos();

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
            this.Numero = string.Empty;
            this.TituloAnomalia = string.Empty;
            this.Dia = null;
            this.Data = DateTime.Today;
            this.Hora = DateTime.Now.TimeOfDay;
            this.RegiaoSetor = null;
            this.Letra = null;
            this.Turno = null;
            this.GerenciaGeral = null;
            this.Gerencia = null;
            this.AreaEquipamento = null;
            this.LocalEquipamento = null;
            this.LocalAnomalia = string.Empty;
            this.DescricaoPreliminar = string.Empty;
            this.AcoesImediatasSim = null;
            this.AcoesImediatasNao = null;
            this.RemocaoSintomas = string.Empty;
            this.OrigemAnomalia = null;
            this.TipoAnomalia = null;
            this.ClassificacaoTipo = null;
            this.Probabilidade = null;
            this.Severidade = null;
            this.ProdutoAB = string.Empty;
            this.ResultadoClassificacaoRisco = null;

            this.RegistradoPor = null;
            if (UsuarioLogado.Instancia.ID_VINCULO != Guid.Empty)
                this.RegistradoPor = await this.Vinculo();
            else
                this.RegistradoPor = null;
            this.RelatadoPor = null;
            this.SupervisorImediato = null;
            this.Assinatura = null;
        }

        public void GetOcorrencia(string idOcorrencia, OrigemDados origem)
        {
            if (origem == OrigemDados.Api)
                this.GetOcorrenciaApi(idOcorrencia);
            else if (origem == OrigemDados.SQLite)
                this.GetOcorrenciaBanco(idOcorrencia);
            this.TelaCarregada = true;
        }

        public async void GetOcorrenciaBanco(string idOcorrencia)
        {
            OCORRENCIACSN o = null;
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                this._idOcorrenciaSQLite = new Guid(idOcorrencia);

                o = await App.Banco.BuscarOcorrenciaCsnAsync(this._idOcorrenciaSQLite);

                this.Numero = string.Empty;
                this.TituloAnomalia = o.TITULO;
                this.Data = o.DATA.Date;
                this.Hora = o.DATA.TimeOfDay;

                if (o.REGIAOSETOR.HasValue)
                {
                    UNIDADE m = await App.Banco.BuscarUnidadeAsync(o.REGIAOSETOR.Value);
                    if (m != null)
                        this.RegiaoSetor = m.ToModeloObj();
                }
                if (o.LETRA.HasValue)
                {
                    LETRA m = await App.Banco.BuscarLetraAsync(o.LETRA.Value);
                    if (m != null)
                        this.Letra = m.ToModeloObj();
                }
                if (o.TURNO.HasValue)
                {
                    TURNO m = await App.Banco.BuscarTurnoAsync(o.TURNO.Value);
                    if (m != null)
                        this.Turno = m.ToModeloObj();
                }
                if (o.GERENCIAGERAL.HasValue)
                {
                    GERENCIA_GERAL_CSN m = await App.Banco.BuscarGerenciaGeralCsnAsync(o.GERENCIAGERAL.Value);
                    if (m != null)
                        this.GerenciaGeral = m.ToModeloObj();
                }
                if (o.GERENCIA.HasValue)
                {
                    GERENCIA_CSN m = await App.Banco.BuscarGerenciaCsnAsync(o.GERENCIA.Value);
                    if (m != null)
                        this.Gerencia = m.ToModeloObj();
                }
                if (o.AREAEQUIPAMENTO.HasValue)
                {
                    AREA_EQUIPAMENTO m = await App.Banco.BuscarAreaEquipamentoAsync(o.AREAEQUIPAMENTO.Value);
                    if (m != null)
                        this.AreaEquipamento = m.ToModeloObj();
                }
                if (o.LOCALEQUIPAMENTO.HasValue)
                {
                    LOCAL_EQUIPAMENTO m = await App.Banco.BuscarLocalEquipamentoAsync(o.LOCALEQUIPAMENTO.Value);
                    if (m != null)
                        this.LocalEquipamento = m.ToModeloObj();
                }
                this.LocalAnomalia = o.LOCALANOMALIA;
                this.DescricaoPreliminar = o.DESCRICAOPRELIMINAR;
                if (o.ST_ACAOIMEDIATA.HasValue)
                {
                    this.AcoesImediatasSim = o.ST_ACAOIMEDIATA.Value;
                    this.AcoesImediatasNao = !o.ST_ACAOIMEDIATA.Value;
                }
                this.RemocaoSintomas = o.REMOCAOSINTOMAS;
                if (o.ORIGEMANOMALIA.HasValue)
                {
                    ORIGEM_ANOMALIA m = await App.Banco.BuscarOrigemAnomaliaAsync(o.ORIGEMANOMALIA.Value);
                    if (m != null)
                        this.OrigemAnomalia = m.ToModeloObj();
                }
                if (o.TIPOANOMALIA.HasValue)
                {
                    TIPO_ANOMALIA m = await App.Banco.BuscarTipoAnomaliaAsync(o.TIPOANOMALIA.Value);
                    if (m != null)
                        this.TipoAnomalia = m.ToModeloObj();
                }
                if (o.CLASSIFICACAOTIPO.HasValue)
                {
                    CLASSIFICACAO_TIPO m = await App.Banco.BuscarClassificacaoTipoAsync(o.CLASSIFICACAOTIPO.Value);
                    if (m != null)
                        this.ClassificacaoTipo = m.ToModeloObj();
                }
                if (o.PROBABILIDADE.HasValue)
                {
                    PROBABILIDADE m = await App.Banco.BuscarProbabilidadeAsync(o.PROBABILIDADE.Value);
                    if (m != null)
                        this.Probabilidade = m.ToModeloObj();
                }
                if (o.SEVERIDADE.HasValue)
                {
                    SEVERIDADE m = await App.Banco.BuscarSeveridadeAsync(o.SEVERIDADE.Value);
                    if (m != null)
                        this.Severidade = m.ToModeloObj();
                }

                if (o.REGISTRADOPOR.HasValue)
                {
                    VINCULO m = await App.Banco.BuscarVinculoAsync(o.REGISTRADOPOR.Value);
                    if (m != null)
                        this.RegistradoPor = m.ToModeloObj();
                }
                if (o.RELATADOPOR.HasValue)
                {
                    VINCULO m = await App.Banco.BuscarVinculoAsync(o.RELATADOPOR.Value);
                    if (m != null)
                        this.RelatadoPor = m.ToModeloObj();
                }
                if (o.SUPERVISOR.HasValue)
                {
                    VINCULO m = await App.Banco.BuscarVinculoAsync(o.SUPERVISOR.Value);
                    if (m != null)
                        this.SupervisorImediato = m.ToModeloObj();
                }
                if (o.ASSINATURA.HasValue)
                {
                    VINCULO m = await App.Banco.BuscarVinculoAsync(o.ASSINATURA.Value);
                    if (m != null)
                        this.Assinatura = m.ToModeloObj();
                }
            }
            catch (Exception ex)
            { 
                throw new Exception("Erro ao carregar a ocorrência.");
            }
            finally
            {
                this.Ocupado = false;
            }
        }

        public async void GetOcorrenciaApi(string idOcorrencia)
        {
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                string url = string.Concat(App.__EnderecoWebApi, "/OcorrenciasCsn/RetornarOcorrencia");

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                parametros.Add("idOcorrencia", idOcorrencia);

                FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                using (HttpClient requisicao = new HttpClient())
                {
                    HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        OcorrenciaCsn o = Newtonsoft.Json.JsonConvert.DeserializeObject<OcorrenciaCsn>(conteudo);

                        this.Numero = o.CODIGO;
                        this.TituloAnomalia = o.TITULO;
                        this.Dia = o.DIA;
                        this.Data = o.DATA;
                        this.Hora = o.DATA.TimeOfDay;
                        this.RegiaoSetor = o.REGIAOSETOR;
                        this.Letra = o.LETRA;
                        this.Turno = o.TURNO;
                        this.GerenciaGeral = o.GERENCIAGERAL;
                        this.Gerencia = o.GERENCIA;
                        this.AreaEquipamento = o.AREAEQUIPAMENTO;
                        this.LocalEquipamento = o.LOCALEQUIPAMENTO;
                        this.LocalAnomalia = o.LOCALANOMALIA;
                        this.DescricaoPreliminar = o.DESCRICAOPRELIMINAR;
                        this.AcoesImediatasSim = o.ACAOIMEDIATA.IdStrNullSafe().ToBoolSim();
                        this.RemocaoSintomas = o.REMOCAOSINTOMAS;
                        this.OrigemAnomalia = o.ORIGEMANOMALIA;
                        this.TipoAnomalia = o.TIPOANOMALIA;
                        this.ClassificacaoTipo = o.CLASSIFICACAOTIPO;
                        this.Probabilidade = o.PROBABILIDADE;
                        this.Severidade = o.SEVERIDADE;
                        this.ProdutoAB = o.PRODUTOAB;
                        this.ResultadoClassificacaoRisco = o.RESULTADOCLASIFICACAO;

                        this.RegistradoPor = o.REGISTRADOPOR;
                        if (this.RegistradoPor != null)
                            await App.Banco.InserirOuAlterarAsync(new VINCULO(this.RegistradoPor));

                        this.RelatadoPor = o.RELATADOPOR;
                        if (this.RelatadoPor != null)
                            await App.Banco.InserirOuAlterarAsync(new VINCULO(this.RelatadoPor));

                        this.SupervisorImediato = o.SUPERVISOR;
                        if (this.SupervisorImediato != null)
                            await App.Banco.InserirOuAlterarAsync(new VINCULO(this.SupervisorImediato));

                        this.Assinatura = o.ASSINATURA;
                        if (this.Assinatura != null)
                            await App.Banco.InserirOuAlterarAsync(new VINCULO(this.Assinatura));

                        this.OnPropertyChanged("ExibirNumero");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar a ocorrência.");
            }
            finally
            {
                this.Ocupado = false;
            }
        }

        #endregion Busca dos dados


        #region Validação da ocorrência

        private bool DataValida()
        {
            bool ok = this.Data != this.DataMinima;
            return ok;
        }

        private Task<string> VerificarCamposObrigatorios()
        {
            return Task.Run(() =>
            {
                string erro = string.Empty;

                if (!this.DataValida()) erro += Environment.NewLine + this.Textos.Data;

                if (!string.IsNullOrEmpty(erro))
                    erro = string.Concat(Environment.NewLine, this.Textos.CamposObrigatorios, ": ", Environment.NewLine, erro);

                return erro;
            });
        }

        #endregion Validação da ocorrência
    }
}
