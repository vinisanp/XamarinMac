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
    public class ViewModelSNA : ViewModelBase
    {
        #region Variaveis

        private string _numero;
        private DateTime _date = DateTime.MinValue;
        private TimeSpan _horarioInicial = TimeSpan.Zero;
        private TimeSpan _horarioFinal = TimeSpan.Zero;
        private ModeloObj _unidade;
        private ModeloObj _gerencia;
        private ModeloObj _area;
        private ModeloObj _local;
        private string _temaAbordado;

        private List<ModeloObj> _lstOtimoBomRegularRuim;

        private string _CEAvaliacaoDescritiva;
        private bool _CERuim;
        private bool _CERegular;
        private bool _CEBom;
        private string _CENumeroDNA;

        private string _CAAvaliacaoDescritiva;
        private bool _CARuim;
        private bool _CARegular;
        private bool _CABom;
        private string _CANumeroDNA;

        private string _RAFAvaliacaoDescritiva;
        private bool _RAFRuim;
        private bool _RAFRegular;
        private bool _RAFBom;
        private string _RAFNumeroDNA;

        private string _QATAvaliacaoDescritiva;
        private bool _QATRuim;
        private bool _QATRegular;
        private bool _QATBom;
        private string _QATNumeroDNA;

        private string _pontosPositivos;

        private ModeloObj _registradoPor;

        private Guid _idSnaSQLite = Guid.Empty;

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

        public TimeSpan HorarioInicial
        {
            get { return this._horarioInicial; }
            set { this.DefinirPropriedade(ref this._horarioInicial, value); }
        }

        public TimeSpan HorarioFinal
        {
            get { return this._horarioFinal; }
            set { this.DefinirPropriedade(ref this._horarioFinal, value); }
        }

        public ModeloObj Unidade
        {
            get { return this._unidade; }
            set { this.DefinirPropriedade(ref this._unidade, value); }
        }

        public ModeloObj Gerencia
        {
            get { return this._gerencia; }
            set { this.DefinirPropriedade(ref this._gerencia, value); }
        }

        public ModeloObj Area
        {
            get { return this._area; }
            set { this.DefinirPropriedade(ref this._area, value); }
        }

        public ModeloObj Local
        {
            get { return this._local; }
            set { this.DefinirPropriedade(ref this._local, value); }
        }

        public string TemaAbordado
        {
            get { return this._temaAbordado; }
            set { this.DefinirPropriedade(ref this._temaAbordado, value); }
        }

        public string CEAvaliacaoDescritiva
        {
            get { return this._CEAvaliacaoDescritiva; }
            set { this.DefinirPropriedade(ref this._CEAvaliacaoDescritiva, value); }
        }
        public bool CERuim { get { return this._CERuim; } set { this.DefinirPropriedade(ref this._CERuim, value); } }
        public bool CERegular { get { return this._CERegular; } set { this.DefinirPropriedade(ref this._CERegular, value); } }
        public bool CEBom { get { return this._CEBom; } set { this.DefinirPropriedade(ref this._CEBom, value); } }
        public string CENumeroDNA { get { return this._CENumeroDNA; } set { this.DefinirPropriedade(ref this._CENumeroDNA, value); } }

        public string CAAvaliacaoDescritiva
        {
            get { return this._CAAvaliacaoDescritiva; }
            set { this.DefinirPropriedade(ref this._CAAvaliacaoDescritiva, value); }
        }
        public bool CARuim { get { return this._CARuim; } set { this.DefinirPropriedade(ref this._CARuim, value); } }
        public bool CARegular { get { return this._CARegular; } set { this.DefinirPropriedade(ref this._CARegular, value); } }
        public bool CABom { get { return this._CABom; } set { this.DefinirPropriedade(ref this._CABom, value); } }
        public string CANumeroDNA { get { return this._CANumeroDNA; } set { this.DefinirPropriedade(ref this._CANumeroDNA, value); } }

        public string RAFAvaliacaoDescritiva
        {
            get { return this._RAFAvaliacaoDescritiva; }
            set { this.DefinirPropriedade(ref this._RAFAvaliacaoDescritiva, value); }
        }
        public bool RAFRuim { get { return this._RAFRuim; } set { this.DefinirPropriedade(ref this._RAFRuim, value); } }
        public bool RAFRegular { get { return this._RAFRegular; } set { this.DefinirPropriedade(ref this._RAFRegular, value); } }
        public bool RAFBom { get { return this._RAFBom; } set { this.DefinirPropriedade(ref this._RAFBom, value); } }
        public string RAFNumeroDNA { get { return this._RAFNumeroDNA; } set { this.DefinirPropriedade(ref this._RAFNumeroDNA, value); } }

        public string QATAvaliacaoDescritiva
        {
            get { return this._QATAvaliacaoDescritiva; }
            set { this.DefinirPropriedade(ref this._QATAvaliacaoDescritiva, value); }
        }
        public bool QATRuim { get { return this._QATRuim; } set { this.DefinirPropriedade(ref this._QATRuim, value); } }
        public bool QATRegular { get { return this._QATRegular; } set { this.DefinirPropriedade(ref this._QATRegular, value); } }
        public bool QATBom { get { return this._QATBom; } set { this.DefinirPropriedade(ref this._QATBom, value); } }
        public string QATNumeroDNA { get { return this._QATNumeroDNA; } set { this.DefinirPropriedade(ref this._QATNumeroDNA, value); } }

        public string PontosPositivos
        {
            get { return this._pontosPositivos; }
            set { this.DefinirPropriedade(ref this._pontosPositivos, value); }
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

        public ViewModelSNA(Action<bool, string> retornoSalvar, Action retornoCancelar)
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

            this.Titulo = this.Textos.SNA;
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
            SNA s = new SNA();

            if (this._idSnaSQLite != Guid.Empty)
                s.ID_SNA = this._idSnaSQLite;
            else
                s.ID_SNA = Guid.NewGuid();

            s.DT_DATA = this.Data.Date.AddSeconds(this.HorarioInicial.TotalSeconds);
            s.DT_HORARIO_INICIAL = this.Data.Date.AddSeconds(this.HorarioInicial.TotalSeconds);
            //s.DT_HORARIO_FINAL = this.Data.Date.Add(this.HorarioFinal);

            s.ID_REGIONAL   = this.Unidade.Id;
            s.ID_GERENCIA   = this.Gerencia.Id;
            s.ID_AREA       = this.Area.Id;
            s.ID_LOCAL      = this.Local.Id;

            s.DS_TEMA_ABORDADO = this.TemaAbordado;

            s.DS_CE_AVALIACAODESCRITIVA = this.CEAvaliacaoDescritiva;
            s.ST_CE_BOM                 = this.CEBom;
            s.ST_CE_REGULAR             = this.CERegular;
            s.ST_CE_RUIM                = this.CERuim;
            s.NU_DNA_CE                 = this.CENumeroDNA;

            s.DS_CA_AVALIACAODESCRITIVA = this.CAAvaliacaoDescritiva;
            s.ST_CA_BOM                 = this.CABom;
            s.ST_CA_REGULAR             = this.CARegular;
            s.ST_CA_RUIM                = this.CARuim;
            s.NU_DNA_CA                 = this.CANumeroDNA;

            s.DS_RAF_AVALIACAODESCRITIVA = this.RAFAvaliacaoDescritiva;
            s.ST_RAF_BOM                 = this.RAFBom;
            s.ST_RAF_REGULAR             = this.RAFRegular;
            s.ST_RAF_RUIM                = this.RAFRuim;
            s.NU_DNA_RAF                 = this.RAFNumeroDNA;

            s.DS_QAT_AVALIACAODESCRITIVA = this.QATAvaliacaoDescritiva;
            s.ST_QAT_BOM                 = this.QATBom;
            s.ST_QAT_REGULAR             = this.QATRegular;
            s.ST_QAT_RUIM                = this.QATRuim;
            s.NU_DNA_QAT                 = this.QATNumeroDNA;

            s.DS_PONTOS_POSITIVOS = this.PontosPositivos;

            s.ID_REGISTRADOPOR = this.RegistradoPor.Id;
        
            RetornoRequest ret = new RetornoRequest();

            try
            {
                await App.Banco.InserirOuAlterarAsync(s);

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
            string url = string.Concat(App.__EnderecoWebApi, "/SegurancaNaArea/Inserir");

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

            if (!string.IsNullOrEmpty(this.Numero))
                parametros.Add("NU_SNA", this.Numero);

            DateTime data = this.Data.AddSeconds(this.HorarioInicial.TotalSeconds);
            parametros.Add("DT_DATA", data.ToString("yyyyMMdd HHmmss"));
            parametros.Add("DT_HORARIO_INICIAL", data.ToString("yyyyMMdd HHmmss"));

            parametros.Add("ID_REGIONAL", this.Unidade.Id.ToString());
            parametros.Add("ID_GERENCIA", this.Gerencia.Id.ToString());
            parametros.Add("ID_AREA", this.Area.Id.ToString());
            parametros.Add("ID_LOCAL", this.Local.Id.ToString());

            parametros.Add("DS_TEMA_ABORDADO", this.TemaAbordado);

            parametros.Add("ID_CE", Util.GetIdRuimRegularBom(this.CEBom, this.CERegular, this.CERuim));
            parametros.Add("DS_CE_AVALIACAODESCRITIVA", this.CEAvaliacaoDescritiva);
            parametros.Add("NU_DNA_CE", this.CENumeroDNA);

            parametros.Add("ID_CA", Util.GetIdRuimRegularBom(this.CABom, this.CARegular, this.CARuim));
            parametros.Add("DS_CA_AVALIACAODESCRITIVA", this.CAAvaliacaoDescritiva);
            parametros.Add("NU_DNA_CA", this.CANumeroDNA);

            parametros.Add("ID_RAF", Util.GetIdRuimRegularBom(this.RAFBom, this.RAFRegular, this.RAFRuim));
            parametros.Add("DS_RAF_AVALIACAODESCRITIVA", this.RAFAvaliacaoDescritiva);
            parametros.Add("NU_DNA_RAF", this.RAFNumeroDNA);

            parametros.Add("ID_QAT", Util.GetIdRuimRegularBom(this.QATBom, this.QATRegular, this.QATRuim));
            parametros.Add("DS_QAT_AVALIACAODESCRITIVA", this.QATAvaliacaoDescritiva);
            parametros.Add("NU_DNA_QAT", this.QATNumeroDNA);

            parametros.Add("DS_PONTOS_POSITIVOS", this.PontosPositivos);
            parametros.Add("ID_REGISTRADOPOR", this.RegistradoPor.IdStrNullSafe());

            string teste = string.Empty;
            foreach (KeyValuePair<string, string> p in parametros)
            {
                if (!string.IsNullOrEmpty(p.Value))
                    teste += p.Key + ":" + p.Value + Environment.NewLine;
            }

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
                        string[] valores = conteudo.Split(new char[] { '|' });
                        if (valores.Length == 6)
                        {
                            id = valores[0];
                            this.Numero = valores[1];
                            this.CENumeroDNA = valores[2];
                            this.CANumeroDNA = valores[3];
                            this.RAFNumeroDNA = valores[4];
                            this.QATNumeroDNA = valores[5];
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

                    this._lstOtimoBomRegularRuim = Util.LstOtimoBomRegulaRuim;

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
            this.Numero         = string.Empty;
            this.Data           = DateTime.Today;
            this.HorarioInicial = DateTime.Now.TimeOfDay;
            this.HorarioFinal   = DateTime.Now.TimeOfDay;
            this.TemaAbordado   = null;
            this.Unidade        = null;
            this.Gerencia       = null;
            this.Area           = null;
            this.Local          = null;

            this.CEAvaliacaoDescritiva = null;
            this.CEBom                 = false;
            this.CERegular             = false;
            this.CERuim                = false;
            this.CENumeroDNA           = null;

            this.CAAvaliacaoDescritiva = null;
            this.CABom                 = false;
            this.CARegular             = false;
            this.CARuim                = false;
            this.CANumeroDNA           = null;

            this.RAFAvaliacaoDescritiva = null;
            this.RAFBom                 = false;
            this.RAFRegular             = false;
            this.RAFRuim                = false;
            this.RAFNumeroDNA           = null;

            this.QATAvaliacaoDescritiva = null;
            this.QATBom                 = false;
            this.QATRegular             = false;
            this.QATRuim                = false;
            this.QATNumeroDNA           = null;

            this.PontosPositivos = null;

            this.RegistradoPor = null;

            if (UsuarioLogado.Instancia.ID_VINCULO != Guid.Empty)
                this.RegistradoPor = await this.Vinculo();
            else
                this.RegistradoPor = null;
        }

        public void GetSNA(string id, OrigemDados origem)
        {
            if (origem == OrigemDados.Api)
                this.GetSNAApi(id);
            else if (origem == OrigemDados.SQLite)
                this.GetSNABanco(id);
            this.TelaCarregada = true;
        }

        public async void GetSNABanco(string id)
        {
            SNA s = null;
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                this._idSnaSQLite = new Guid(id);

                s = await App.Banco.BuscarSNAAsync(this._idSnaSQLite);

                this.Numero = string.Empty;
                this.Data = s.DT_DATA.Date;
                this.TemaAbordado = s.DS_TEMA_ABORDADO;
                if(s.DT_HORARIO_INICIAL.HasValue)
                    this.HorarioInicial = s.DT_HORARIO_INICIAL.Value.TimeOfDay;

                REGIONAL regional = await App.Banco.BuscarRegionalAsync(s.ID_REGIONAL);
                if (regional != null)
                    this.Unidade = regional.ToModeloObj();

                GERENCIA gerencia = await App.Banco.BuscarGerenciaAsync(s.ID_GERENCIA);
                if (gerencia != null)
                    this.Gerencia = gerencia.ToModeloObj();

                AREA area = await App.Banco.BuscarAreaAsync(s.ID_AREA);
                if (area != null)
                    this.Area = area.ToModeloObj();

                LOCAL local = await App.Banco.BuscarLocalAsync(s.ID_LOCAL);
                if (local != null)
                    this.Local = local.ToModeloObj();

                this.CEAvaliacaoDescritiva = s.DS_CE_AVALIACAODESCRITIVA;
                this.CEBom                 = s.ST_CE_BOM;
                this.CERegular             = s.ST_CE_REGULAR;
                this.CERuim                = s.ST_CE_RUIM;
                this.CENumeroDNA           = s.NU_DNA_CE;

                this.CAAvaliacaoDescritiva = s.DS_CA_AVALIACAODESCRITIVA;
                this.CABom                 = s.ST_CA_BOM;
                this.CARegular             = s.ST_CA_REGULAR;
                this.CARuim                = s.ST_CA_RUIM;
                this.CANumeroDNA           = s.NU_DNA_CA;

                this.RAFAvaliacaoDescritiva = s.DS_RAF_AVALIACAODESCRITIVA;
                this.RAFBom                 = s.ST_RAF_BOM;
                this.RAFRegular             = s.ST_RAF_REGULAR;
                this.RAFRuim                = s.ST_RAF_RUIM;
                this.RAFNumeroDNA           = s.NU_DNA_RAF;

                this.QATAvaliacaoDescritiva = s.DS_QAT_AVALIACAODESCRITIVA;
                this.QATBom                 = s.ST_QAT_BOM;
                this.QATRegular             = s.ST_QAT_REGULAR;
                this.QATRuim                = s.ST_QAT_RUIM;
                this.QATNumeroDNA           = s.NU_DNA_QAT;

                this.PontosPositivos = s.DS_PONTOS_POSITIVOS;

                if (s.ID_REGISTRADOPOR.HasValue)
                {
                    VINCULO vinculo = await App.Banco.BuscarVinculoAsync(s.ID_REGISTRADOPOR.Value);
                    if (vinculo != null)
                        this.RegistradoPor = vinculo.ToModeloObj();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar SNA.");
            }
            finally
            {
                this.Ocupado = false;
            }
        }

        public async void GetSNAApi(string id)
        {
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                string url = string.Concat(App.__EnderecoWebApi, "/SegurancaNaArea/RetornarSNA");

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                parametros.Add("idSNA", id);

                FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                using (HttpClient requisicao = new HttpClient())
                {
                    HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        Sna s = Newtonsoft.Json.JsonConvert.DeserializeObject<Sna>(conteudo);

                        this.Numero = s.CODIGO;
                        this.Data = s.DATA;
                        this.HorarioInicial = s.HORA;

                        this.Unidade = s.UNIDADE;
                        this.Gerencia = s.GERENCIA;
                        this.Area = s.AREA;
                        this.Local = s.LOCAL;
                        this.TemaAbordado = s.DS_TEMA_ABORDADO;

                        this.CEAvaliacaoDescritiva = s.DS_CE_AVALIACAODESCRITIVA;
                        this.CEBom                 = s.ID_CE.ToBoolBom();
                        this.CERegular             = s.ID_CE.ToBoolRegular();
                        this.CERuim                = s.ID_CE.ToBoolRuim();
                        this.CENumeroDNA           = s.NU_DNA_CE;

                        this.CAAvaliacaoDescritiva = s.DS_CA_AVALIACAODESCRITIVA;
                        this.CABom                 = s.ID_CA.ToBoolBom();
                        this.CARegular             = s.ID_CA.ToBoolRegular();
                        this.CARuim                = s.ID_CA.ToBoolRuim();
                        this.CANumeroDNA           = s.NU_DNA_CA;

                        this.RAFAvaliacaoDescritiva = s.DS_RAF_AVALIACAODESCRITIVA;
                        this.RAFBom                 = s.ID_RAF.ToBoolBom();
                        this.RAFRegular             = s.ID_RAF.ToBoolRegular();
                        this.RAFRuim                = s.ID_RAF.ToBoolRuim();
                        this.RAFNumeroDNA           = s.NU_DNA_RAF;

                        this.QATAvaliacaoDescritiva = s.DS_QAT_AVALIACAODESCRITIVA;
                        this.QATBom                 = s.ID_QAT.ToBoolBom();
                        this.QATRegular             = s.ID_QAT.ToBoolRegular();
                        this.QATRuim                = s.ID_QAT.ToBoolRuim();
                        this.QATNumeroDNA           = s.NU_DNA_QAT;

                        this.PontosPositivos = s.DS_PONTOS_POSITIVOS;
                        this.RegistradoPor = s.REGISTRADOPOR;

                        await this.Atualizar();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar SNA.");
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

        private bool UnidadeValida()
        {
            bool ok = this.Unidade != null;
            return ok;
        }

        private bool GerenciaValida()
        {
            bool ok = this.Gerencia != null;
            return ok;
        }

        private bool AreaValida()
        {
            bool ok = this.Area != null;
            return ok;
        }

        private bool LocalValido()
        {
            bool ok = this.Local != null;
            return ok;
        }

        private bool RegistradoPorValido()
        {
            bool ok = this.RegistradoPor != null;
            return ok;
        }


        private Task<string> VerificarCamposObrigatorios()
        {
            return Task.Run(() =>
            {
                string erro = string.Empty;

                if (!this.DataValida()) erro += Environment.NewLine + this.Textos.Data;
                if (!this.UnidadeValida()) erro += Environment.NewLine + this.Textos.UnidadeAbordagem;
                if (!this.GerenciaValida()) erro += Environment.NewLine + this.Textos.GerenciaDoDesvio;
                if (!this.AreaValida()) erro += Environment.NewLine + this.Textos.AreaDoDesvio;
                if (!this.LocalValido()) erro += Environment.NewLine + this.Textos.LocalDoDesvio;
                if (!this.RegistradoPorValido()) erro += Environment.NewLine + this.Textos.RegistradoPor;

                if (!string.IsNullOrEmpty(erro))
                    erro = string.Concat(Environment.NewLine, this.Textos.CamposObrigatorios, ": ", Environment.NewLine, erro);

                return erro;
            });
        }

        #endregion Validação da ocorrência
    }
}
