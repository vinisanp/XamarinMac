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
    public class ViewModelInspecao : ViewModelBase
    {
        #region Variaveis

        private string _numero;
        private DateTime _date = DateTime.MinValue;
        private TimeSpan _hora = TimeSpan.Zero;
        private ModeloObj _unidade;
        private ModeloObj _gerencia;
        private ModeloObj _area;
        private ModeloObj _local;
        private ModeloObj _fornecedor;
        private ModeloObj _tipo;
        private ModeloObj _atividade;
        private ModeloObj _realizadoPor;
        private ObservableCollection<ModeloObj> _participantes;
        private ObservableCollection<CampoInspecaoVm> _campos;
        private ObservableCollection<CampoInspecaoVm> _camposVisiveis;

        private Guid _idSQLite = Guid.Empty;

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

        public ModeloObj Fornecedor
        {
            get { return this._fornecedor; }
            set { this.DefinirPropriedade(ref this._fornecedor, value); }
        }

        public ModeloObj RealizadoPor
        {
            get { return this._realizadoPor; }
            set { this.DefinirPropriedade(ref this._realizadoPor, value); }
        }

        public ObservableCollection<ModeloObj> Participantes
        {
            get { return this._participantes; }
            set { this._participantes = value; }
        }
        public async void AddParticipantes(ModeloObj m)
        {
            if (!this._participantes.Any(p => p.Id == m.Id))
            {
                this._participantes.Add(m);
                int ret = await App.Banco.InserirOuAlterarAsync(new VINCULO(m));
                this.OnPropertyChanged("Participantes");
            }
        }
        public string IdsParticipantes
        {
            get
            {
                string ids = string.Empty;
                Guid[] marcados = this.Participantes.Select(p => p.Id).ToArray();
                if (marcados.Length > 0)
                    ids = string.Join(";", marcados);
                return ids;
            }
        }

        public ModeloObj Tipo
        {
            get { return this._tipo; }
            set { this.DefinirPropriedade(ref this._tipo, value); }
        }

        public ModeloObj Atividade
        {
            get { return this._atividade; }
            set { this.DefinirPropriedade(ref this._atividade, value); }
        }

        public ObservableCollection<CampoInspecaoVm> Campos
        {
            get { return this._campos; }
            set { this.DefinirPropriedade(ref this._campos, value); }
        }

        public ObservableCollection<CampoInspecaoVm> CamposVisiveis
        {
            get { return this._camposVisiveis; }
            set { this.DefinirPropriedade(ref this._camposVisiveis, value); }
        }

        #endregion Propriedades


        #region Commands

        public Command SalvarCommand { get; }

        public Command CancelarCommand { get; }

        #endregion Commands


        #region Construtores

        public ViewModelInspecao(Action<bool, string> retornoSalvar, Action retornoCancelar)
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

            this.Titulo = this.Textos.InspecaoSeguranca;
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
            INSPECAO i = new INSPECAO();

            if (this._idSQLite != Guid.Empty)
                i.ID_INSPECAO = this._idSQLite;
            else
                i.ID_INSPECAO = Guid.NewGuid();

            i.DT_DATA         = this.Data.Date.AddSeconds(this.Hora.TotalSeconds);
            i.ID_REGIONAL     = this.Unidade.Id;
            i.ID_GERENCIA     = this.Gerencia.Id;
            i.ID_AREA         = this.Area.Id;
            i.ID_LOCAL        = this.Local.Id;
            i.ID_FORNECEDOR   = this.Fornecedor.Id;
            i.ID_TIPO         = this.Tipo?.Id;
            i.ID_ATIVIDADE    = this.Atividade?.Id;
            i.PARTICIPANTES   = this.IdsParticipantes;
            i.ID_REALIZADOPOR = this.RealizadoPor?.Id;

            RetornoRequest ret = new RetornoRequest();

            try
            {
                await App.Banco.InserirOuAlterarAsync(i);

                foreach (CampoInspecaoVm c in this.Campos)
                {
                    CAMPO_INSPECAO cBanco = new CAMPO_INSPECAO();
                    cBanco.ID_INSPECAO = i.ID_INSPECAO;
                    cBanco.ID_CAMPO = c.IdCampo;
                    cBanco.ID_CONFORME = c.Resposta?.Id;
                    cBanco.DS_SITUACAO = c.Descricao;
                    cBanco.NU_DNA = c.NumeroDNA;
                    if (!string.IsNullOrEmpty(cBanco.DS_SITUACAO) || !string.IsNullOrEmpty(cBanco.NU_DNA) || cBanco.ID_CONFORME.HasValue)
                    {
                        string cols = Newtonsoft.Json.JsonConvert.SerializeObject(c.Colunas);
                        cBanco.COLUNAS = cols;
                    }
                    cBanco.CAMINHO = c.CaminhoImagem;
                    cBanco.BYTES_IMAGEM = c.Image;

                    await App.Banco.InserirOuAlterarAsync(cBanco);
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
            string url = string.Concat(App.__EnderecoWebApi, "/Inspecao/Inserir");
            DateTime data = this.Data.AddSeconds(this.Hora.TotalSeconds);

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

            parametros.Add("DT_DATA", this.Data.AddSeconds(this.Hora.TotalSeconds).ToString("yyyyMMdd HHmmss"));

            parametros.Add("ID_REGIONAL", this.Unidade.Id.ToString());
            parametros.Add("ID_GERENCIA", this.Gerencia.Id.ToString());
            parametros.Add("ID_AREA", this.Area.Id.ToString());
            parametros.Add("ID_LOCAL", this.Local.Id.ToString());
            parametros.Add("ID_FORNECEDOR", this.Fornecedor.Id.ToString());

            if (this.Tipo != null)
                parametros.Add("ID_TIPO", this.Tipo.Id.ToString());

            if (this.Atividade != null)
                parametros.Add("ID_ATIVIDADE", this.Atividade.Id.ToString());

            if (this.RealizadoPor != null)
                parametros.Add("ID_REALIZADOPOR", this.RealizadoPor.IdStrNullSafe());
            
            if (!string.IsNullOrEmpty(this.Numero))
                parametros.Add("NU_INSPECAO", this.Numero);

            if (this.Participantes != null && this.Participantes.Count > 0)
            {
                string participantes = Newtonsoft.Json.JsonConvert.SerializeObject(this.Participantes.Select(p => p.Id.ToString()).ToList());
                parametros.Add("PARTICIPANTES", participantes);
            }

            List<CampoInspecao> campos = new List<CampoInspecao>();
            foreach (CampoInspecaoVm c in this.Campos)
                campos.Add(c.ToCampoInspecao());

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
                                try { this.Campos[i - 2].NumeroDNA = valores[i]; } catch { }
                        }
                    }
                    else
                        this.Numero = conteudo;
                    ret.Mensagem = conteudo;

                    Task.Run(() => { this.EnviarImagens(id); });
                }
                else
                    ret.Erro = conteudo;
            }

            return ret;
        }

		public void AddCampo()
		{
            if (this.CamposVisiveis == null)
                this.CamposVisiveis = new ObservableCollection<CampoInspecaoVm>();

            if (this.CamposVisiveis.Count == 0)
                this.CamposVisiveis.Add(this.Campos.FirstOrDefault());
            else if (this.CamposVisiveis.Count < this.Campos.Count)
                this.CamposVisiveis.Add(this.Campos[this.CamposVisiveis.Count]);
        }

		public async void EnviarImagens(string id)
        {
            RetornoRequest ret = new RetornoRequest();
            ret.Ok = true;

            for (int i = 0; i < this.Campos.Count; i++)
            {
                CampoInspecaoVm item = this.Campos[i];
                if (item.Image != null)
                {
                    try
                    {
                        string url = string.Concat(App.__EnderecoWebApi, "/Inspecao/UploadImagem");
                        using (HttpClient client = new HttpClient())
                        {
                            using (MultipartFormDataContent formData = new MultipartFormDataContent())
                            {
                                CancellationToken cancellationToken = CancellationToken.None;

                                FileInfo fi = new FileInfo(item.CaminhoImagem);
                                string nmArquivo = string.Concat("SDST_Mobile_img_", i, "_", this.Data.ToAAAAMMDD_HHMINSS(), fi.Extension);

                                HttpContent imageContent = new StreamContent(new MemoryStream(item.Image));
                                ContentDispositionHeaderValue conteudo = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = nmArquivo };
                                imageContent.Headers.ContentDisposition = conteudo;
                                imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                formData.Add(imageContent);
                                client.DefaultRequestHeaders.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                                client.DefaultRequestHeaders.Add("ID_INSPECAO", id);
                                client.DefaultRequestHeaders.Add("NM_COLUNA", item.Colunas["Multimidia"]);
                                HttpResponseMessage resposta = await client.PostAsync(url, formData);
                                string conteudoResposta = await resposta.Content.ReadAsStringAsync();

                                ret.Codigo = (int)resposta.StatusCode;
                                if (resposta.StatusCode != System.Net.HttpStatusCode.Created)
                                {
                                    ret.Ok = false;
                                    ret.Erro = string.Concat(ret.Erro, conteudoResposta, Environment.NewLine);
                                }
                                else
                                    ret.Mensagem = string.Concat(ret.Mensagem, conteudoResposta, Environment.NewLine);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ret.Ok = false;
                        ret.Erro = ex.Message;
                    }
                }
            }
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
                    await this.CarregarCampos();

                    this.TelaCarregada = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }

        private void Trigger(List<CampoInspecao> campos)
        {
            string tipo = @"
      SELECT CL000000000000ABCD000000000000 INTO cIdTipo FROM TB40A18BA0B6F04B3ABD7A82AB8E05 WHERE CL000000000000ABCD000300000000 = 'SSO';
";
            string classificacao = @"
      SELECT CL000000000000ABCD000000000000 INTO cIdClassificacao FROM TB5E5BE4B0C71E4F3B97E471EFBA69 WHERE CL000000000000ABCD000200000000 = 'IP';
";

            string trigger = @"
CREATE OR REPLACE TRIGGER TGB_INSPECAO
BEFORE INSERT OR UPDATE ON TBEC4866F9F91846A0933C5A8DA73A
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

" + tipo + classificacao;

            string imagens = string.Empty;
            string ajustesNumero = string.Empty;
            string selectInspComDNA = "SELECT * FROM TBEC4866F9F91846A0933C5A8DA73A WHERE ";
            foreach (CampoInspecao c in campos)
            {
                string ajusteNumero = @"                                                                                                                                                                                
      IF INSP.{0} IS NOT NULL THEN                      
         SELECT Count(*) INTO nCount 
           FROM TB5824888C9ED4419F90EBBEA77F71
          WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = INSP.{0};
         IF nCount > 1 THEN
            up_log('AJUSTES DNA INSP SNA', nCount || ' - ' || INSP.CL000000000000ABCD000000000000, 24); 
            up_log('AJUSTES DNA INSP SNA', 'SELECT * FROM TB5824888C9ED4419F90EBBEA77F71 WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = ''' || INSP.{0} || ''' AND CL1BA3CCE96716406A8CC8F5D5C4A8 = ''' || INSP.{1} || '''', 24);
            FOR DNA IN (SELECT * FROM TB5824888C9ED4419F90EBBEA77F71
                         WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = INSP.{0}
                           AND CL1BA3CCE96716406A8CC8F5D5C4A8 = INSP.{1})
            LOOP                                                                                 
               SELECT Max(CLC388C0FFC35B4680AA4FF9DA4F39) + 1 INTO cNumeroDNA
                 FROM TB5824888C9ED4419F90EBBEA77F71
                WHERE CLC388C0FFC35B4680AA4FF9DA4F39 LIKE SubStr(INSP.{0}, 0, 6) || '%';  
               up_log('AJUSTES DNA INSP SNA', 'SELECT Max(CLC388C0FFC35B4680AA4FF9DA4F39) + 1 FROM TB5824888C9ED4419F90EBBEA77F71 WHERE CLC388C0FFC35B4680AA4FF9DA4F39 LIKE ''' || SubStr(INSP.{0}, 0, 6) || '%''', 24);
               up_log('AJUSTES DNA INSP SNA', 'DNA: ' || DNA.CLC388C0FFC35B4680AA4FF9DA4F39 || ' -> ' || cNumeroDNA, 24); 
               --UPDATE TB5824888C9ED4419F90EBBEA77F71 SET CLC388C0FFC35B4680AA4FF9DA4F39 = cNumeroDNA WHERE CL000000000000ABCD000000000000 = DNA.CL000000000000ABCD000000000000;
               --UPDATE TBEC4866F9F91846A0933C5A8DA73A SET {0} = cNumeroDNA WHERE CL000000000000ABCD000000000000 = INSP.CL000000000000ABCD000000000000;  
            END LOOP;
         END IF;
      END IF;
";
                ajustesNumero += string.Format(ajusteNumero, c.Colunas["CaixaNumerica"], c.Colunas["CaixaTexto"]);

                selectInspComDNA += string.Format("\r\n OR {0} IS NOT NULL ", c.Colunas["CaixaNumerica"]);

                string bloco = @"
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
                                                      CL93D7CEADB7B84208BA44D204F82E, CLE2A5D31F697041BBA66417F21456, CL871D3C3461C9405399EB2D2ECB38,
                                                      CL7EE5A052A86F45B983A5AD831585, CLB2A1EDEBC0444A19AA2FA2E73FE5, CLCB425A1BE63C4C5BB928F5910374,
                                                      CL1BA3CCE96716406A8CC8F5D5C4A8)
          VALUES (cIdDNA                                , cNumeroDNA                            , :NEW.CL000000000000ABCD001100000000,
                  :NEW.CLFC0A5487EF054DD987189FC7488B   , :NEW.CLB902473D1C6B490195C14E304540   , :NEW.CL062725DEC7BB4040927156C70452,
                  :NEW.CL8451E852C2CD4E3AA24FC66E2FF4   , cIdTipo                               , cIdClassificacao                   ,
                  cIdSubClassif                         , cIdCategoria                          , :NEW.CL6F32E8F8649E432BA8C4A77DA584,
                  :NEW.CLD4206822651B4EEE99E940FC3AC1   , :NEW.CLD4206822651B4EEE99E940FC3AC1   , 1,
                  :NEW.{4});
      END IF;

";
                if (c.Pergunta.StartsWith("1. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "VTI", "0078", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("2. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "CP", "0075", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("3. ") ||
                         c.Pergunta.StartsWith("4. ") ||
                         c.Pergunta.StartsWith("6. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALI", "0068", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("5. ") ||
                         c.Pergunta.StartsWith("16. ") ||
                         c.Pergunta.StartsWith("19. ") ||
                         c.Pergunta.StartsWith("21. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALI", "0070", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("7. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DCI", "0074", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("8. ") ||
                         c.Pergunta.StartsWith("12. ") ||
                         c.Pergunta.StartsWith("17. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "MAI", "0080", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("9. ") ||
                         c.Pergunta.StartsWith("23. ") ||
                         c.Pergunta.StartsWith("24. ") ||
                         c.Pergunta.StartsWith("26. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "MAI", "0072", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("10. ") ||
                         c.Pergunta.StartsWith("13. ") ||
                         c.Pergunta.StartsWith("25. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALI", "0073", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("11. ") ||
                         c.Pergunta.StartsWith("14. ") ||
                         c.Pergunta.StartsWith("18. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "FUI", "0076", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("15. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALI", "0069", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("22. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "OI", "0079", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("27. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "DOI", "0082", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("28. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALI", "0077", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("29. ") ||
                         c.Pergunta.StartsWith("30. ") ||
                         c.Pergunta.StartsWith("31. ") ||
                         c.Pergunta.StartsWith("32. ") ||
                         c.Pergunta.StartsWith("33. ") ||
                         c.Pergunta.StartsWith("34. ") ||
                         c.Pergunta.StartsWith("35. ") ||
                         c.Pergunta.StartsWith("36. ") ||
                         c.Pergunta.StartsWith("37. ") ||
                         c.Pergunta.StartsWith("38. ") ||
                         c.Pergunta.StartsWith("39. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALI", "0071", c.Colunas["CaixaTexto"]);
                else if (c.Pergunta.StartsWith("40. "))
                    trigger += string.Format(bloco, c.Colunas["CaixaOpcao"], c.Colunas["CaixaNumerica"], "ALI", "0081", c.Colunas["CaixaTexto"]);

                imagens += string.Format(@"

         IF :NEW.{0} IS NOT NULL AND :NEW.{1} IS NOT NULL THEN
            SELECT CL000000000000ABCD000000000000 INTO cIdDNA FROM TB5824888C9ED4419F90EBBEA77F71 WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = :NEW.{0};
            DELETE DOC_ANEXO 
             WHERE NM_TABELA = 'TB5824888C9ED4419F90EBBEA77F71' 
               AND NM_COLUNA = 'CL7477547A7A82406C8C10CA08C1DA' 
               AND ID_REGISTRO = cIdDNA 
               AND CL000000000000ABCD000000000000 = :NEW.{1};
   
            SELECT BI_DOCUMENTO INTO cImagem FROM MULTIMIDIA
             WHERE NM_TABELA = 'TBEC4866F9F91846A0933C5A8DA73A'
               AND NM_COLUNA = '{1}'
               AND CL000000000000ABCD000000000000 = :NEW.{1};
   
            INSERT INTO DOC_ANEXO (CL000000000000ABCD000000000000, ID_REGISTRO, NM_TABELA, NM_COLUNA, NM_ANEXO, DS_ANEXO, DS_TIPO_ANEXO, BI_ANEXO)
            VALUES (:NEW.{1}, cIdDNA, 'TB5824888C9ED4419F90EBBEA77F71', 'CL7477547A7A82406C8C10CA08C1DA', 'imagemSNA.jpg', 'imagemSNA.jpg', '.jpg', cImagem);
   
         END IF;", c.Colunas["CaixaNumerica"], c.Colunas["Multimidia"]);
            }

            trigger += string.Format(@"
      IF UPDATING THEN
         {0}
      END IF;
   END IF;
END TGB_INSPECAO;
/
", imagens);
        }

        public async Task LimparCampos()
        {
            this.Numero     = string.Empty;
            this.Data       = DateTime.Today;
            this.Hora       = DateTime.Now.TimeOfDay;
            this.Unidade    = null;
            this.Gerencia   = null;
            this.Area       = null;
            this.Local      = null;
            this.Fornecedor = null;
            this.Tipo       = null;
            this.Atividade  = null;

            this.Participantes = new ObservableCollection<ModeloObj>();

            this.RealizadoPor = null;

            if (this.Campos != null)
            {
                foreach (CampoInspecaoVm c in this.Campos)
                {
                    c.CaminhoImagem = null;
                    c.Descricao = null;
                    c.Resposta = null;
                    c.NumeroDNA = null;
                    c.Image = null;
                }
            }

            this.CamposVisiveis = null;

            if (UsuarioLogado.Instancia.ID_VINCULO != Guid.Empty)
                this.RealizadoPor = await this.Vinculo();

            await this.Atualizar();
        }

        public async Task CarregarCampos()
        {
            List<CampoInspecao> campos = new List<CampoInspecao>();

            if (Util.TemAcessoInternet)
            {
                try
                {
                    string url = string.Concat(App.__EnderecoWebApi, "/Inspecao/Campos");

                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                    FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                    using (HttpClient requisicao = new HttpClient())
                    {
                        HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                        if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            Config.SalvarConfiguracao("CamposInspecao", conteudo);
                            campos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CampoInspecao>>(conteudo);
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
                string conteudo = Config.CarregarConfiguracao("CamposInspecao").ToStringNullSafe();
                if(!string.IsNullOrEmpty(conteudo))
                    campos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CampoInspecao>>(conteudo);
            }

            this.Campos = new ObservableCollection<CampoInspecaoVm>();
            foreach (CampoInspecao c in campos)
                this.Campos.Add(new CampoInspecaoVm(c, this.LstConformeNaoConforme));

            this.Trigger(campos);
        }

        public void GetInspecao(string id, OrigemDados origem)
        {
            if (origem == OrigemDados.Api)
                this.GetApi(id);
            else if (origem == OrigemDados.SQLite)
                this.GetBanco(id);
            this.TelaCarregada = true;
        }

        public async void GetBanco(string id)
        {
            INSPECAO i = null;
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                this._idSQLite = new Guid(id);

                i = await App.Banco.BuscarInspecaoAsync(this._idSQLite);

                this.Numero = string.Empty;
                this.Data = i.DT_DATA.Date;
                this.Hora = i.DT_DATA.TimeOfDay;

                REGIONAL regional = await App.Banco.BuscarRegionalAsync(i.ID_REGIONAL);
                if (regional != null)
                    this.Unidade = regional.ToModeloObj();

                GERENCIA gerencia = await App.Banco.BuscarGerenciaAsync(i.ID_GERENCIA);
                if (gerencia != null)
                    this.Gerencia = gerencia.ToModeloObj();

                AREA area = await App.Banco.BuscarAreaAsync(i.ID_AREA);
                if (area != null)
                    this.Area = area.ToModeloObj();

                LOCAL local = await App.Banco.BuscarLocalAsync(i.ID_LOCAL);
                if (local != null)
                    this.Local = local.ToModeloObj();

                FORNECEDOR fornecedor = await App.Banco.BuscarFornecedorAsync(i.ID_FORNECEDOR);
                if (fornecedor != null)
                    this.Fornecedor = fornecedor.ToModeloObj();

                if (i.ID_TIPO.HasValue)
                {
                    TIPO_INSPECAO tipo = await App.Banco.BuscarTipoInspecaoAsync(i.ID_TIPO.Value);
                    if (tipo != null)
                        this.Tipo = tipo.ToModeloObj();
                }

                if (i.ID_ATIVIDADE.HasValue)
                {
                    ATIVIDADE_INSPECAO atividade = await App.Banco.BuscarAtividadeInspecaoAsync(i.ID_ATIVIDADE.Value);
                    if (atividade != null)
                        this.Atividade = atividade.ToModeloObj();
                }

                if (i.ID_REALIZADOPOR.HasValue)
                {
                    VINCULO vinculo = await App.Banco.BuscarVinculoAsync(i.ID_REALIZADOPOR.Value);
                    if (vinculo != null)
                        this.RealizadoPor = vinculo.ToModeloObj();
                }

                this.Participantes = new ObservableCollection<ModeloObj>();
                if (!string.IsNullOrEmpty(i.PARTICIPANTES))
                {
                    string[] lstParticipantes = i.PARTICIPANTES.Split(';');
                    foreach (string idParticipante in lstParticipantes)
                    {
                        VINCULO p = await App.Banco.BuscarVinculoAsync(new Guid(idParticipante));
                        if (p != null)
                            this.AddParticipantes(p.ToModeloObj());
                    }
                }

                List<CAMPO_INSPECAO> camposBanco = await App.Banco.BuscarCamposInspecaoAsync(i.ID_INSPECAO);
                foreach (var campoBanco in camposBanco)
                {
                    CampoInspecaoVm campoVm = this.Campos.FirstOrDefault(c => c.IdCampo == campoBanco.ID_CAMPO);
                    if (campoVm != null)
                    {
                        campoVm.Descricao = campoBanco.DS_SITUACAO;
                        if (campoBanco.ID_CONFORME.HasValue)
                            campoVm.Resposta = this.LstConformeNaoConforme.FirstOrDefault(r => r.Id == campoBanco.ID_CONFORME.Value);
                        campoVm.NumeroDNA = campoBanco.NU_DNA;
                        campoVm.CaminhoImagem = campoBanco.CAMINHO;
                        campoVm.Image = campoBanco.BYTES_IMAGEM;

                        if (!string.IsNullOrEmpty(campoBanco.COLUNAS))
                            campoVm.Colunas = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(campoBanco.COLUNAS);
                    }
                }

                await this.Atualizar();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar a Inspeção de Seguraça.");
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
                string url = string.Concat(App.__EnderecoWebApi, "/Inspecao/Retornar");

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                parametros.Add("idInspecao", id);

                FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                using (HttpClient requisicao = new HttpClient())
                {
                    HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        Inspecao insp = Newtonsoft.Json.JsonConvert.DeserializeObject<Inspecao>(conteudo);

                        this.Numero     = insp.NUMERO;
                        this.Data       = insp.DATA;
                        this.Hora       = insp.DATA.TimeOfDay;
                        this.Unidade    = insp.UNIDADE;
                        this.Gerencia   = insp.GERENCIA;
                        this.Area       = insp.AREA;
                        this.Local      = insp.LOCAL;
                        this.Fornecedor = insp.FORNECEDOR;
                        this.Tipo       = insp.TIPO;
                        this.Atividade  = insp.ATIVIDADE;

                        this.RealizadoPor = insp.REALIZADOPOR;

                        this.Participantes = new ObservableCollection<ModeloObj>();
                        foreach (ModeloObj p in insp.PARTICIPANTES)
                            this.AddParticipantes(p);

                        this.Campos = new ObservableCollection<CampoInspecaoVm>();
                        foreach (CampoInspecao c in insp.CAMPOS)
                            this.Campos.Add(new CampoInspecaoVm(c, this.LstConformeNaoConforme));

                        await this.Atualizar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar a Inspeção de Segurança.");
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

        private bool FornecedorValido()
        {
            bool ok = this.Fornecedor != null;
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
            bool ok = this.RealizadoPor != null;
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
                if (!this.FornecedorValido()) erro += Environment.NewLine + this.Textos.Fornecedor;
                if (!this.RegistradoPorValido()) erro += Environment.NewLine + this.Textos.RegistradoPor;

				foreach (CampoInspecaoVm c in this.Campos)
				{
                    if (c.Resposta != null && c.Resposta.Id == new Guid("8196e394-50de-453d-8f40-a386ee068dd9") && string.IsNullOrEmpty(c.Descricao))
                        erro += string.Concat(Environment.NewLine, c.TituloDescricaoSituacao, " (", c.Pergunta, ")");
                }

                if (!string.IsNullOrEmpty(erro))
                    erro = string.Concat(Environment.NewLine, this.Textos.CamposObrigatorios, ": ", Environment.NewLine, erro);

                return erro;
            });
        }

        #endregion Validação da ocorrência
    }
}
