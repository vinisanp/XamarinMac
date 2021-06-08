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
    public class ViewModelOPA : ViewModelBase
    {
        #region Variaveis

        private string _numero;
        private DateTime _date = DateTime.MinValue;
        private TimeSpan _hora = TimeSpan.Zero;

        private ModeloObj _unidade;
        private ModeloObj _gerencia;
        private ModeloObj _area;
        private ModeloObj _local;

        private ModeloObj _atividade;
        private ModeloObj _tarefaObservada;
        private ModeloObj _avaliador;
        private ModeloObj _tipoAvaliador;

        private ObservableCollection<GrupoOpaVm> _grupos;

        private Guid _idSQLite = Guid.Empty;
        private List<CampoOpa> _campos;
        private string _classificacao;
        private string _nota;

        #endregion Variaveis


        #region Propriedades

        public override bool EmEdicao
        {
            get { return this._emEdicao; }
            set
            {
                if (this.Grupos != null)
                    foreach (GrupoOpaVm grupo in this.Grupos)
                        grupo.EmEdicao = this.EmEdicao;
                this.DefinirPropriedade(ref this._emEdicao, value);
            }
        }

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

        public ModeloObj Atividade
        {
            get { return this._atividade; }
            set { this.DefinirPropriedade(ref this._atividade, value); }
        }

        public ModeloObj TarefaObservada
        {
            get { return this._tarefaObservada; }
            set { this.DefinirPropriedade(ref this._tarefaObservada, value); }
        }

        public ModeloObj Avaliador
        {
            get { return this._avaliador; }
            set { this.DefinirPropriedade(ref this._avaliador, value); }
        }

        public ModeloObj TipoAvaliador
        {
            get { return this._tipoAvaliador; }
            set { this.DefinirPropriedade(ref this._tipoAvaliador, value); }
        }

        public ObservableCollection<GrupoOpaVm> Grupos
        {
            get { return this._grupos; }
            set { this.DefinirPropriedade(ref this._grupos, value); }
        }

        public string NotaFinal
        {
            get { return this._nota; }
            set { this.DefinirPropriedade(ref this._nota, value); }
        }

        public string Classificacao
        {
            get { return this._classificacao; }
            set { this.DefinirPropriedade(ref this._classificacao, value); }
        }

        #endregion Propriedades


        #region Commands

        public Command SalvarCommand { get; }

        public Command CancelarCommand { get; }

        #endregion Commands


        #region Construtores

        public ViewModelOPA(Action<bool, string, string> retornoSalvar, Action retornoCancelar)
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

            this.Titulo = this.Textos.Opas;
        }

        public void Calcular()
        {
            if (this.Grupos != null && this.Grupos.Count > 0)
                foreach (GrupoOpaVm grupo in this.Grupos)
                    grupo.Calcular();
            
            this.AtualizarOpa();
        }

        public void AtualizarOpa()
        {
            double nuNota = 0;

            if (this.Grupos != null && this.Grupos.Count > 0)
            {
                string nota = string.Empty;
                double soma = 0;
                foreach (GrupoOpaVm grupo in this.Grupos)
                {
                    soma += grupo.Media;
                    if (!grupo.Preenchido)
                    {
                        nota += $"Apuração incorreta bloco '{grupo.Titulo}'";
                        break;
                    }
                }
                if (string.IsNullOrEmpty(nota))
                {
                    nuNota = soma / (double)this.Grupos.Count / 100;
                    nota = nuNota.ToString("P");
                }
                this.NotaFinal = nota;
            }
            else
            {
                this.NotaFinal = string.Empty;
            }

            if (nuNota >= 0.8)
                this.Classificacao = "Manter Gestão da Rotina";
            else if (nuNota >= 0.5)
                this.Classificacao = "Monitorar e atuar sobre a Gestão de Risco";
            else
                this.Classificacao = "Desenvolver plano de ação para melhoria";

            if (!string.IsNullOrEmpty(this.NotaFinal))
                this.Titulo = this.Textos.Opas + " - " + this.Classificacao + " (" + this.NotaFinal + ")";
            else
                this.Titulo = this.Textos.Opas;
        }

        #endregion Construtores


        #region Metodos de Gravação

        private async Task Salvar(Action<bool, string, string> retornoSalvar)
        {
            this.Ocupado = true;
            this.EmEdicao = false;

            RetornoRequest ret = new RetornoRequest();

            string mensagem = string.Empty;
            try
            {
                string erro = await this.VerificarCamposObrigatorios();
                if (!string.IsNullOrEmpty(erro))
                    retornoSalvar?.Invoke(false, erro, this.Textos.Aviso);
                else
                {
                    ret = await this.SalvarDados();

                    if (ret.Ok)
                        retornoSalvar?.Invoke(true, this.Numero, this.Textos.Aviso);
                    else
                        retornoSalvar?.Invoke(false, ret.Erro, this.Textos.ErroAoSalvar);
                }
            }
            catch (Exception ex)
            {
                retornoSalvar?.Invoke(false, ex.Message, this.Textos.ErroAoSalvar);
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
            OPA o = new OPA();

            if (this._idSQLite != Guid.Empty)
                o.ID_OPA = this._idSQLite;
            else
                o.ID_OPA = Guid.NewGuid();

            o.DT_OPA = this.Data.Date.AddSeconds(this.Hora.TotalSeconds);

            o.ID_REGIONAL = this.Unidade.Id;
            o.ID_GERENCIA = this.Gerencia.Id;
            o.ID_AREA = this.Area.Id;
            o.ID_LOCAL = this.Local.Id;

            o.ID_ATIVIDADE = this.Atividade?.Id;
            o.ID_TAREFA = this.TarefaObservada?.Id;
            o.ID_AVALIADOR = this.Avaliador?.Id;
            o.ID_TIPO_AVALIADOR = this.TipoAvaliador?.Id;

            RetornoRequest ret = new RetornoRequest();

            try
            {
                await App.Banco.InserirOuAlterarAsync(o);

                foreach (GrupoOpaVm g in this.Grupos)
                {
                    foreach (CampoOpaVm c in g.Campos)
                    {

                        CAMPO_OPA cBanco = new CAMPO_OPA();
                        cBanco.ID_OPA = o.ID_OPA;
                        cBanco.ID_CAMPO = c.IdCampo;
                        cBanco.ID_CONFORME = c.Resposta?.Id;
                        cBanco.DS_COMENTARIO = c.Comentario;
                        cBanco.NU_DNA = c.NumeroDNA;
                        if (!string.IsNullOrEmpty(cBanco.DS_COMENTARIO) || !string.IsNullOrEmpty(cBanco.NU_DNA) || cBanco.ID_CONFORME.HasValue)
                        {
                            string cols = Newtonsoft.Json.JsonConvert.SerializeObject(c.Colunas);
                            cBanco.COLUNAS = cols;
                        }
                        await App.Banco.InserirOuAlterarAsync(cBanco);
                    }
                }

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
            string url = string.Concat(App.__EnderecoWebApi, "/Opa/Inserir");
            DateTime data = this.Data.AddSeconds(this.Hora.TotalSeconds);

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

            parametros.Add("DT_DATA", this.Data.AddSeconds(this.Hora.TotalSeconds).ToString("yyyyMMdd HHmmss"));

            parametros.Add("ID_REGIONAL", this.Unidade.Id.ToString());
            parametros.Add("ID_GERENCIA", this.Gerencia.Id.ToString());
            parametros.Add("ID_AREA", this.Area.Id.ToString());
            parametros.Add("ID_LOCAL", this.Local.Id.ToString());

            if (this.Atividade != null)
                parametros.Add("ID_ATIVIDADE", this.Atividade.Id.ToString());

            if (this.TarefaObservada != null)
                parametros.Add("ID_TAREFA", this.TarefaObservada.Id.ToString());

            if (this.Avaliador != null)
                parametros.Add("ID_AVALIADOR", this.Avaliador.IdStrNullSafe());

            if (this.TipoAvaliador != null)
                parametros.Add("ID_TIPO_AVALIADOR", this.TipoAvaliador.IdStrNullSafe());

            if (!string.IsNullOrEmpty(this.Numero))
                parametros.Add("NU_OPA", this.Numero);

            List<CampoOpa> campos = new List<CampoOpa>();
            foreach (GrupoOpaVm g in this.Grupos)
                foreach (CampoOpaVm c in g.Campos)
                    campos.Add(c.ToCampoOpa());

            parametros.Add("CAMPOS", Newtonsoft.Json.JsonConvert.SerializeObject(campos));

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
                        string[] valores = conteudo.Split(new string[] { "|" }, StringSplitOptions.None);
                        if (valores.Length > 2)
                        {
                            id = valores[0];
                            this.Numero = valores[1];
							for (int i = 2; i < valores.Length; i++)
								try 
                                {
                                    string idCampo = this._campos[i - 2].IdCampo;
                                    string grupo = this._campos[i - 2].Grupo;
									GrupoOpaVm grupoVm = this.Grupos.FirstOrDefault(g => g.Titulo == grupo);
                                    if(grupoVm != null)
									{
										CampoOpaVm campoVm = grupoVm.Campos.FirstOrDefault(c => c.IdCampo == idCampo);
                                        if (campoVm != null)
                                            campoVm.NumeroDNA = valores[i];
									}
                                } catch { }
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

                    await this.CarregarCampos();
                    await this.LimparCampos();
                    this.AtualizarOpa();
                    this.TelaCarregada = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }

        public void Trigger(List<CampoOpa> campos)
        {
            string tipo = @"
";

            string trigger = @"
CREATE OR REPLACE TRIGGER TGB_OPA
BEFORE INSERT OR UPDATE ON TB32488541CD1C485BB1E42B4F9D91
REFERENCING NEW AS NEW OLD AS OLD
FOR EACH ROW
DECLARE
   nuDigitos         NUMBER;
   nuSequencia       VARCHAR(4000);
   cIdDNA            VARCHAR(36);
   cNumeroDNA        VARCHAR(100);
   nmTabelaDNA       VARCHAR(30);
   nmColunaNumeroDNA VARCHAR(30);
   cIdTipo           VARCHAR(36);
   cIdClassificacao  VARCHAR(36);
   cIdSubClassif     VARCHAR(36);
   cIdCategoria      VARCHAR(36);
   cImagem           BLOB;

BEGIN

   nmTabelaDNA := 'TB5824888C9ED4419F90EBBEA77F71';
   nmColunaNumeroDNA := 'CLC388C0FFC35B4680AA4FF9DA4F39';

   IF INSERTING OR UPDATING THEN
                           
      SELECT NU_DIGITOS INTO nuDigitos FROM SEQUENCIA_TABELA_BASE WHERE id_tabela = '5824888c-9ed4-419f-90eb-bea77f717ec8';

      SELECT CL000000000000ABCD000000000000 INTO cIdTipo FROM TB40A18BA0B6F04B3ABD7A82AB8E05 WHERE CL000000000000ABCD000300000000 = 'SSO';
      SELECT CL000000000000ABCD000000000000 INTO cIdClassificacao FROM TB5E5BE4B0C71E4F3B97E471EFBA69 WHERE CL000000000000ABCD000200000000 = 'OPA';
";

            string ajustesNumero = string.Empty;
            string selectInspComDNA = "SELECT * FROM TBEC4866F9F91846A0933C5A8DA73A WHERE ";
            foreach (CampoOpa c in campos)
            {
                selectInspComDNA += string.Format("\r\n OR {0} IS NOT NULL ", c.Colunas["CaixaNumerica"]);

                string bloco = @"
      --{5}
      IF :NEW.{0} = '8196e394-50de-453d-8f40-a386ee068dd9' AND :OLD.{0} || '*' != '8196e394-50de-453d-8f40-a386ee068dd9*' THEN
         cIdDNA := fc_guid;
         UP_GERA_SEQUENCIA_TABELA ( cIdDNA, '00000000-0000-0000-0000-000000000000',
                                    nmTabelaDNA, nmColunaNumeroDNA, NULL, nuDigitos, nuSequencia);
         cNumeroDNA := To_Char(SYSDATE, 'YYYYMM') || LPad(nuSequencia, 5, '0');
         :NEW.{1} := cNumeroDNA;
         SELECT CL000000000000ABCD000000000000 INTO cIdSubClassif FROM TB7DDBD552E69D4721B68BE65F4735 WHERE CL000000000000ABCD000200000000 = '{2}';
         SELECT CL000000000000ABCD000000000000 INTO cIdCategoria FROM TB5710DEF79A934512910FDD238152 WHERE CL000000000000ABCD000200000000 = '{3}';
         INSERT INTO TB5824888C9ED4419F90EBBEA77F71 ( CL000000000000ABCD000000000000, CLC388C0FFC35B4680AA4FF9DA4F39, CL000000000000ABCD001100000000,
                                                      CL60727E23076D489085D693B3F656, CL747EE3D6B76D42259D9B248BC634, CLE8D2FB41B9E44AF4A02D3A14B9A2,
                                                      CL5CDDB175B6EC4FF193ACAFEA9B6D, CLBBBD21D7C00A4908BBF90A1D2E04, CL63AF72F4170C4210B50B55664424,
                                                      CL93D7CEADB7B84208BA44D204F82E, CLE2A5D31F697041BBA66417F21456, CLB2A1EDEBC0444A19AA2FA2E73FE5, 
                                                      CLCB425A1BE63C4C5BB928F5910374, CL1BA3CCE96716406A8CC8F5D5C4A8 )
          VALUES (cIdDNA                                , cNumeroDNA                            , :NEW.CL000000000000ABCD001100000000,
                  :NEW.CL5E7110542FF345269D7EB1FFFBB3   , :NEW.CL361D9FD735BF4B68AF69544EFF66   , :NEW.CLD172E6FAE63B4AD2AE76690C24F7,
                  :NEW.CLA74D67236DC54D42A48F0EDE0B78   , cIdTipo                               , cIdClassificacao                   ,
                  cIdSubClassif                         , cIdCategoria                          , :NEW.CL63AF72F4170C4210B50B55664424,
                  1                                     , :NEW.{4} );
      END IF;

";
                if (c.Titulo.StartsWith("1.1")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0090", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("1.2")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0090", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("1.3")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0090", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("1.4")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0090", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("1.5")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0090", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("1.6")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0090", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("1.7")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0090", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("1.8")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0091", c.Colunas["CaixaTexto"], c.Titulo);

                if (c.Titulo.StartsWith("2.1")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0092", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("2.2")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0092", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("2.3")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0093", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("2.4")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0094", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("2.5")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0095", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("2.6")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0092", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("2.7")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0096", c.Colunas["CaixaTexto"], c.Titulo);

                if (c.Titulo.StartsWith("3.1")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "PO", "0097", c.Colunas["CaixaTexto"] , c.Titulo);
                if (c.Titulo.StartsWith("3.2")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0098", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("3.4")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0099", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("3.5")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0100", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("3.6")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0099", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("3.7")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0102", c.Colunas["CaixaTexto"], c.Titulo);

                if (c.Titulo.StartsWith("4.1")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0103", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("4.2")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "FUO", "0104", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("4.3")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "UTO", "0105", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("4.5")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "UTO", "0105", c.Colunas["CaixaTexto"], c.Titulo);

                if (c.Titulo.StartsWith("5.1")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0106", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("5.2")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0106", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("5.3")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0106", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("5.5")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0110", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("5.6")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCO", "0106", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("5.7")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0107", c.Colunas["CaixaTexto"], c.Titulo);

                if (c.Titulo.StartsWith("6.1")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0108", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("6.2")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0108", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("6.3")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0108", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("6.4")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0109", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("6.5")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0108", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("6.6")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0109", c.Colunas["CaixaTexto"], c.Titulo);
                if (c.Titulo.StartsWith("6.7")) trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALO", "0109", c.Colunas["CaixaTexto"], c.Titulo);


            }

            trigger += @"
   END IF;
END TGB_OPA;
/
";
        }

        public async Task LimparCampos()
        {
            this.Numero = string.Empty;
            this.Data = DateTime.Today;
            this.Hora = DateTime.Now.TimeOfDay;
            this.Unidade = null;
            this.Gerencia = null;
            this.Area = null;
            this.Local = null;
            this.TarefaObservada = null;
            this.Atividade = null;
            this.Avaliador = null;
            this.TipoAvaliador = null;

            if (this.Grupos != null)
            {
                foreach (GrupoOpaVm g in this.Grupos)
                {
                    foreach (CampoOpaVm c in g.Campos)
                    {
                        c.Comentario = null;
                        c.Resposta = null;
                        c.NumeroDNA = null;
                    }
                }
            }

            if (UsuarioLogado.Instancia.ID_VINCULO != Guid.Empty)
                this.Avaliador = await this.Vinculo();

            await this.Atualizar();
        }

        public async Task CarregarCampos()
        {
            this._campos = new List<CampoOpa>();

            if (Util.TemAcessoInternet)
            {
                try
                {
                    string url = string.Concat(App.__EnderecoWebApi, "/Opa/Campos");

                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                    FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                    using (HttpClient requisicao = new HttpClient())
                    {
                        HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                        if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            Config.SalvarConfiguracao("CamposOpa", conteudo);
                            this._campos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CampoOpa>>(conteudo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao carregar campos da Inspeção de Segurança.");
                }
            }
            else
            {
                string conteudo = Config.CarregarConfiguracao("CamposOpa").ToStringNullSafe();
                if (!string.IsNullOrEmpty(conteudo))
                    this._campos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CampoOpa>>(conteudo);
            }

            IEnumerable<IGrouping<string, CampoOpa>> grupos = this._campos.GroupBy(c => c.Grupo);
            this.Grupos = new ObservableCollection<GrupoOpaVm>();
            foreach (IGrouping<string, CampoOpa> grupo in grupos)
                this.Grupos.Add(new GrupoOpaVm(grupo.Key, grupo.ToList(), this.LstConformeNaoConforme, this) { CorTituloPicker = this.CorTituloPicker, EmEdicao = this.EmEdicao });

            this.Trigger(this._campos);
        }

		public void GetOpa(string id, OrigemDados origem)
        {
            if (origem == OrigemDados.Api)
                this.GetApi(id);
            else if (origem == OrigemDados.SQLite)
                this.GetBanco(id);
            this.TelaCarregada = true;
        }

        public async void GetBanco(string id)
        {
            OPA o = null;
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                this._idSQLite = new Guid(id);

                o = await App.Banco.BuscarOpaAsync(this._idSQLite);

                this.Numero = string.Empty;
                this.Data = o.DT_OPA.Date;
                this.Hora = o.DT_OPA.TimeOfDay;

                REGIONAL regional = await App.Banco.BuscarRegionalAsync(o.ID_REGIONAL);
                if (regional != null)
                    this.Unidade = regional.ToModeloObj();

                GERENCIA gerencia = await App.Banco.BuscarGerenciaAsync(o.ID_GERENCIA);
                if (gerencia != null)
                    this.Gerencia = gerencia.ToModeloObj();

                AREA area = await App.Banco.BuscarAreaAsync(o.ID_AREA);
                if (area != null)
                    this.Area = area.ToModeloObj();

                LOCAL local = await App.Banco.BuscarLocalAsync(o.ID_LOCAL);
                if (local != null)
                    this.Local = local.ToModeloObj();

                if (o.ID_ATIVIDADE.HasValue)
                {
                    ATIVIDADE_INSPECAO atividade = await App.Banco.BuscarAtividadeInspecaoAsync(o.ID_ATIVIDADE.Value);
                    if (atividade != null)
                        this.Atividade = atividade.ToModeloObj();
                }

                if (o.ID_TAREFA.HasValue)
                {
					TAREFA_OBSERVADA tarefa = await App.Banco.BuscarTarefaObservadaAsync(o.ID_TAREFA.Value);
                    if (tarefa != null)
                        this.TarefaObservada = tarefa.ToModeloObj();
                }

                if (o.ID_AVALIADOR.HasValue)
                {
                    VINCULO vinculo = await App.Banco.BuscarVinculoAsync(o.ID_AVALIADOR.Value);
                    if (vinculo != null)
                        this.Avaliador = vinculo.ToModeloObj();
                }

                if (o.ID_TIPO_AVALIADOR.HasValue)
                {
                    TIPO_AVALIADOR tipo = await App.Banco.BuscarTipoAvaliadorAsync(o.ID_TIPO_AVALIADOR.Value);
                    if (tipo != null)
                        this.TipoAvaliador = tipo.ToModeloObj();
                }

                List<CAMPO_OPA> camposBanco = await App.Banco.BuscarCamposOpaAsync(o.ID_OPA);
                if (camposBanco != null && this.Grupos != null)
                {
                    foreach (GrupoOpaVm g in this.Grupos)
                    {
                        if (g.Campos.Count == 0)
                            g.CarregarCampos();

                        foreach (CampoOpaVm c in g.Campos)
                        {
                            CAMPO_OPA campoOpa = camposBanco.FirstOrDefault(a => a.ID_CAMPO == c.IdCampo);
                            c.Comentario = campoOpa.DS_COMENTARIO;
                            if(campoOpa.ID_CONFORME.HasValue)
                                c.DefinirRespostaSemAtualizarGrupo(campoOpa.ID_CONFORME.Value.ToString());
                            c.NumeroDNA = campoOpa.NU_DNA;
                            c.ComentarioVisivel = !string.IsNullOrEmpty(c.Comentario);
                        }
                    }
                }

                this.Calcular();

                await this.Atualizar();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar a Observação Positiva Florestal.");
            }
            finally
            {
                this.Ocupado = false;
            }
        }

        public async void GetApi(string id)
        {
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                string url = string.Concat(App.__EnderecoWebApi, "/Opa/Retornar");

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                parametros.Add("idOpa", id);

                FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                using (HttpClient requisicao = new HttpClient())
                {
                    HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
						Opa opa = Newtonsoft.Json.JsonConvert.DeserializeObject<Opa>(conteudo);

                        if (opa != null)
                        {
                            this.Numero = opa.NUMERO;
                            this.Data = opa.DATA;
                            this.Hora = opa.DATA.TimeOfDay;
                            this.Unidade = opa.UNIDADE;
                            this.Gerencia = opa.GERENCIA;
                            this.Area = opa.AREA;
                            this.Local = opa.LOCAL;
                            this.TarefaObservada = opa.TAREFA_OBSERVADA;
                            this.Atividade = opa.ATIVIDADE;
                            this.Avaliador = opa.AVALIADOR;
                            this.TipoAvaliador = opa.TIPO_AVALIADOR;

							if (opa.CAMPOS != null && this.Grupos != null)
							{
								foreach (GrupoOpaVm g in this.Grupos)
								{
                                    if (g.Campos.Count == 0)
                                        g.CarregarCampos();

									foreach (CampoOpaVm c in g.Campos)
									{
										CampoOpa campoOpa = opa.CAMPOS.FirstOrDefault(a => a.IdCampo == c.IdCampo);
										c.Comentario = campoOpa.Comentario;
                                        c.DefinirRespostaSemAtualizarGrupo(campoOpa.IdConforme);
										c.NumeroDNA = campoOpa.NumeroDNA;
                                        c.ComentarioVisivel = !string.IsNullOrEmpty(c.Comentario);
									}
								}
							}

							this.Calcular();

                            await this.Atualizar();
                        }
                        else
                            await this.LimparCampos();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar observação positiva florestal.");
            }
            finally
            {
                this.Ocupado = false;
            }
        }

        #endregion Busca dos dados


        #region Validação da OPA

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

        private bool AvaliadorValido()
        {
            bool ok = this.Avaliador != null;
            return ok;
        }

        private Task<string> VerificarCamposObrigatorios()
        {
            return Task.Run(() =>
            {
                string erro = string.Empty;

                if (!this.DataValida()) erro += Environment.NewLine + this.Textos.Data;
                if (!this.UnidadeValida()) erro += Environment.NewLine + this.Textos.UnidadeInspecao;
                if (!this.GerenciaValida()) erro += Environment.NewLine + this.Textos.GerenciaInspecao;
                if (!this.AreaValida()) erro += Environment.NewLine + this.Textos.AreaInspecao;
                if (!this.LocalValido()) erro += Environment.NewLine + this.Textos.LocalInspecao;
                if (!this.AvaliadorValido()) erro += Environment.NewLine + this.Textos.Avaliador;

                if (!string.IsNullOrEmpty(erro))
                    erro = string.Concat(Environment.NewLine, this.Textos.CamposObrigatorios, ": ", Environment.NewLine, erro);
                else
                {
                    int count = 0;
                    foreach (GrupoOpaVm grupo in this.Grupos)
                        if (!grupo.Preenchido)
                        {
                            count++;
                            erro += grupo.Titulo + Environment.NewLine;
                        }

                    if (count == 1)
                        erro = string.Concat("Necessário preenchimento do bloco: ", Environment.NewLine, erro);
                    else if(count > 1)
                        erro = string.Concat("Necessário preenchimento dos blocos: ", Environment.NewLine, erro);
                }

                return erro;
            });
        }

        #endregion Validação da OPA
    }
}
