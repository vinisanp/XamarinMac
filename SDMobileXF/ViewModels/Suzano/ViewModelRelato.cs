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
    public class ViewModelRelato : ViewModelBase
    {
        #region Variaveis

        private string _numero;
        private DateTime _date = DateTime.MinValue;
        private TimeSpan _hora = TimeSpan.Zero;
        private ModeloObj _unidadeRegional;
        private ModeloObj _gerencia;
        private ModeloObj _area;
        private ModeloObj _local;
        private string _descricao;
        private ModeloObj _tipo;
        private ModeloObj _classificacao;
        private ModeloObj _subclassificacao;
        private ModeloObj _categoria;
        private ModeloObj _fornecedor;
        private bool? _acoesImediatas;
        private string _descricaoAcoesImediatas;
        private bool _naoQueroMeIdentificar;
        private ModeloObj _comunicadoPor;
        private ModeloObj _registradoPor;
        private Guid _idOcorrenciaSQLite = Guid.Empty;
        private ObservableCollection<ItemImagem> _imagens = new ObservableCollection<ItemImagem>();
        private ItemImagem _imagemAtual;
        private bool _exibirFoto;

        #endregion Variaveis


        #region Propriedades

        public DateTime DataMaxima => new DateTime(2999, 12, 31);

        public DateTime DataMinima => new DateTime(2020, 01, 01);

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

        public ModeloObj UnidadeRegional
        {
            get { return this._unidadeRegional; }
            set { this.DefinirPropriedade(ref this._unidadeRegional, value); }
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

        public string Descricao
        {
            get { return this._descricao; }
            set { this.DefinirPropriedade(ref this._descricao, value); }
        }

        public ModeloObj Tipo
        {
            get { return this._tipo; }
            set { this.DefinirPropriedade(ref this._tipo, value); }
        }

        public ModeloObj Classificacao
        {
            get { return this._classificacao; }
            set 
            { 
                this.DefinirPropriedade(ref this._classificacao, value);
                if (value != null && (value.Codigo != "OC")) //observação comportamental não permite foto
                    this.ExibirFoto = true;
                else
                {
                    this.ExibirFoto = false;
                    this.Imagens.Clear();
                    this.ImagemAtual = null;
                }
            }
        }

        public ModeloObj SubClassificacao
        {
            get { return this._subclassificacao; }
            set { this.DefinirPropriedade(ref this._subclassificacao, value); }
        }

        public ModeloObj Categoria
        {
            get { return this._categoria; }
            set { this.DefinirPropriedade(ref this._categoria, value); }
        }

        public ModeloObj Fornecedor
        {
            get { return this._fornecedor; }
            set { this.DefinirPropriedade(ref this._fornecedor, value); }
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

                if (!this._acoesImediatas.HasValue || !this._acoesImediatas.Value)
                    this.DescricaoAcoesImediatas = string.Empty;

                this.OnPropertyChanged("AcoesImediatasSim");
                this.OnPropertyChanged("ExibirDescricaoAcoesImediatas");
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

                if (!this._acoesImediatas.HasValue || !this._acoesImediatas.Value)
                    this.DescricaoAcoesImediatas = string.Empty;

                this.OnPropertyChanged("AcoesImediatasNao");
                this.OnPropertyChanged("ExibirDescricaoAcoesImediatas");
            }
        }

        public bool ExibirDescricaoAcoesImediatas
        {
            get
            {
                if (this._acoesImediatas.HasValue)
                    return this._acoesImediatas.Value;
                return false;
            }
        }

        public string DescricaoAcoesImediatas
        {
            get { return this._descricaoAcoesImediatas; }
            set { this.DefinirPropriedade(ref this._descricaoAcoesImediatas, value); }
        }

        public bool NaoQueroMeIdentificar
        {
            get { return this._naoQueroMeIdentificar; }
            set
            {
                this.DefinirPropriedade(ref this._naoQueroMeIdentificar, value);

                if (this._naoQueroMeIdentificar)
                {
                    this.ComunicadoPor = null;
                    this.RegistradoPor = null;
                }
                this.OnPropertyChanged("ExibirIdentificacao");
            }
        }

        public ModeloObj ComunicadoPor
        {
            get { return this._comunicadoPor; }
            set { this.DefinirPropriedade(ref this._comunicadoPor, value); }
        }

        public ModeloObj RegistradoPor
        {
            get { return this._registradoPor; }
            set { this.DefinirPropriedade(ref this._registradoPor, value); }
        }

        public bool ExibirIdentificacao
        {
            get
            {
                if (this._naoQueroMeIdentificar)
                    return false;
                return true;
            }
        }

        public ObservableCollection<ItemImagem> Imagens
        {
            get { return this._imagens; }
            set { this.DefinirPropriedade(ref this._imagens, value); }
        }

        public ItemImagem ImagemAtual
        {
            get 
            { 
                if(this._imagemAtual == null && this.Imagens != null && this.Imagens.Count > 0)
                    this.ImagemAtual = this.Imagens.First();
                return this._imagemAtual; 
            }
            set { this.DefinirPropriedade(ref this._imagemAtual, value); }
        }

        public bool ExibirFoto
        {
            get { return this._exibirFoto; }
            set { this.DefinirPropriedade(ref this._exibirFoto, value); }
        }

        #endregion Propriedades


        #region Commands

        public Command SalvarCommand { get; }

        public Command CancelarCommand { get; }

        public Command ApagarFotoCommand { get; }



        #endregion Commands


        #region Construtores

        public ViewModelRelato(Action<bool, string> retornoSalvar, Action retornoCancelar)
        {
            this.SalvarCommand = new Command(async () => { await this.Salvar(retornoSalvar); });
            this.CancelarCommand = new Command(() => { retornoCancelar?.Invoke(); });            

            this.ApagarFotoCommand = new Command(() => 
            {
                this.ApagarFoto();
            });

            this.Ocupado = true;

            this.Titulo = this.Textos.Ocorrencia;

            this.Imagens = new ObservableCollection<ItemImagem>();
        }

        private void ApagarFoto()
        {
            this.Imagens.Remove(this.ImagemAtual);            
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


        public async Task SalvarImagensBanco(OCORRENCIA ocorrencia)
        {
            //List<IMAGEM_RELATO> imagensDoBanco = await App.Banco.BuscarImagensAsync(ocorrencia.ID_OCORRENCIA);
            //foreach (IMAGEM_RELATO img in imagensDoBanco)
            //    await App.Banco.ApagarAsync(img);

            foreach (ItemImagem itemImagem in this.Imagens)
            {
                IMAGEM_RELATO modelo = new IMAGEM_RELATO();
                modelo.ID_OCORRENCIA = ocorrencia.ID_OCORRENCIA;
                modelo.ID_IMAGEM = itemImagem.IdImagem;
                modelo.DATA = itemImagem.Data.Value;
                modelo.CAMINHO = itemImagem.Caminho;
                modelo.BYTES_IMAGEM = itemImagem.Image;
                await App.Banco.InserirAsync(modelo);
            }
        }

        public async Task<RetornoRequest> SalvarImagensApi(string id)
        {
            string url = string.Concat(App.__EnderecoWebApi, "/Ocorrencias/UploadImagem");

            RetornoRequest ret = new RetornoRequest();
            ret.Ok = true;

            for (int i = 0; i < this.Imagens.Count; i++)
            {
                ItemImagem item = this.Imagens[i];
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        using (MultipartFormDataContent formData = new MultipartFormDataContent())
                        {
                            CancellationToken cancellationToken = CancellationToken.None;

                            FileInfo fi = new FileInfo(item.Caminho);
                            string nmArquivo = string.Concat("SDST_Mobile_img_", i, "_", item.Data.Value.ToAAAAMMDD_HHMINSS(), fi.Extension);

                            HttpContent imageContent = new StreamContent(new MemoryStream(item.Image));
                            ContentDispositionHeaderValue conteudo = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = nmArquivo};
                            imageContent.Headers.ContentDisposition = conteudo;
                            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            formData.Add(imageContent);
                            client.DefaultRequestHeaders.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                            client.DefaultRequestHeaders.Add("ID_OCORRENCIA", id);
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
                    ret.Erro = ex.Message;
                }
            }

            return ret;
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
            OCORRENCIA o = new OCORRENCIA();

            if (this._idOcorrenciaSQLite != Guid.Empty)
                o.ID_OCORRENCIA = this._idOcorrenciaSQLite;
            else
                o.ID_OCORRENCIA = Guid.NewGuid();

            o.DATA = this.Data.AddSeconds(this.Hora.TotalSeconds);
            o.ID_REGIONAL = this.UnidadeRegional.Id;
            o.ID_GERENCIA = this.Gerencia.Id;
            o.ID_AREA = this.Area.Id;
            o.ID_LOCAL = this.Local.Id;
            o.DESCRICAO = this.Descricao;
            o.ID_TIPO = this.Tipo.Id;
            o.ID_CLASSIFICACAO = this.Classificacao.Id;
            o.ID_SUBCLASSIFICACAO = this.SubClassificacao.Id;
            if (this.Categoria != null)
                o.ID_CATEGORIA = this.Categoria.Id;
            if (this.Fornecedor != null)
                o.ID_FORNECEDOR = this.Fornecedor.Id;
            o.ST_ACAOIMEDIATA = this.AcoesImediatasSim;
            if (this.AcoesImediatasSim.HasValue && this.AcoesImediatasSim.Value)
                o.DS_ACAOIMEDIATA = this.DescricaoAcoesImediatas;
            o.ST_NAOQUEROIDENTIFICAR = this.NaoQueroMeIdentificar;
            if (!this.NaoQueroMeIdentificar)
            {
                if (this.ComunicadoPor != null)
                    o.ID_COMUNICADOPOR = this.ComunicadoPor.Id;
                if (this.RegistradoPor != null)
                    o.ID_REGISTRADOPOR = this.RegistradoPor.Id;
            }

            RetornoRequest ret = new RetornoRequest();

            try
            {
                await App.Banco.InserirOuAlterarAsync(o);                

                await this.SalvarImagensBanco(o);

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
            string url = string.Concat(App.__EnderecoWebApi, "/Ocorrencias/Inserir");
            DateTime data = this.Data.AddSeconds(this.Hora.TotalSeconds);

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

            parametros.Add("DATA", this.Data.AddSeconds(this.Hora.TotalSeconds).ToString("yyyyMMdd HHmmss"));
            parametros.Add("ID_REGIONAL", this.UnidadeRegional.Id.ToString());
            parametros.Add("ID_GERENCIA", this.Gerencia.Id.ToString());
            parametros.Add("ID_AREA", this.Area.Id.ToString());
            parametros.Add("ID_LOCAL", this.Local.Id.ToString());
            parametros.Add("DESCRICAO", this.Descricao);
            parametros.Add("ID_TIPO", this.Tipo.Id.ToString());
            parametros.Add("ID_CLASSIFICACAO", this.Classificacao.Id.ToString());
            parametros.Add("ID_SUBCLASSIFICACAO", this.SubClassificacao.Id.ToString());
            if (this.Categoria != null)
                parametros.Add("ID_CATEGORIA", this.Categoria.Id.ToString());
            if (this.Fornecedor != null)
                parametros.Add("ID_FORNECEDOR", this.Fornecedor.Id.ToString());
            if (this.AcoesImediatasSim.HasValue)
            {
                parametros.Add("ACOES_IMEDIATAS", this.AcoesImediatasSim.Value.ToIdSimNao());
                if (this.AcoesImediatasSim.Value)
                    parametros.Add("DS_ACOES_IMEDIATAS", this.DescricaoAcoesImediatas);
            }
            parametros.Add("NAO_QUERO_ME_IDENTIFICAR", this.NaoQueroMeIdentificar.ToNumber());
            if (!this.NaoQueroMeIdentificar)
            {
                if (this.ComunicadoPor != null)
                    parametros.Add("ID_COMUNICADO_POR", this.ComunicadoPor.Id.ToString());
                if (this.RegistradoPor != null)
                    parametros.Add("ID_REGISTRADO_POR", this.RegistradoPor.Id.ToString());
            }
            if (!string.IsNullOrEmpty(this.Numero))
                parametros.Add("NU_OCORRENCIA", this.Numero);

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

                    await this.SalvarImagensApi(id);
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


                    //if (Util.Debug)
                    //{
                    //    this.UnidadeRegional = (await App.Banco.BuscarRegionaisAsync()).FirstOrDefault().ToModeloObj();
                    //    this.Gerencia = (await App.Banco.BuscarGerenciasAsync()).FirstOrDefault().ToModeloObj();
                    //    this.Area = (await App.Banco.BuscarAreasAsync()).FirstOrDefault().ToModeloObj();
                    //    this.Local = (await App.Banco.BuscarLocaisAsync()).FirstOrDefault().ToModeloObj();
                    //    this.Tipo = (await App.Banco.BuscarTiposAsync()).FirstOrDefault().ToModeloObj();
                    //    this.Classificacao = (await App.Banco.BuscarClassificacoesAsync()).FirstOrDefault().ToModeloObj();
                    //    this.SubClassificacao = (await App.Banco.BuscarSubClassificacoesAsync()).FirstOrDefault().ToModeloObj();
                    //    this.Descricao = "asdf sdf sfk s hdfksjh kjs ghdfs asdf xdf asdf adsrg dfh wsery ty5e6 ";
                    //}

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
            this.Data = DateTime.Today;
            this.Hora = DateTime.Now.TimeOfDay;
            this.UnidadeRegional = null;
            this.Gerencia = null;
            this.Area = null;
            this.Local = null;
            this.Descricao = string.Empty;
            this.Tipo = null;
            this.Classificacao = null;
            this.SubClassificacao = null;
            this.Categoria = null;
            this.Fornecedor = null;
            this.AcoesImediatasSim = false;
            this.AcoesImediatasNao = true;
            this.DescricaoAcoesImediatas = string.Empty;
            this.ComunicadoPor = null;
            this.RegistradoPor = null;
            this.NaoQueroMeIdentificar = false;
            if (UsuarioLogado.Instancia.ID_VINCULO != Guid.Empty)
                this.RegistradoPor = await this.Vinculo();
            else
                this.RegistradoPor = null;

            this.Imagens.Clear();
            this.ImagemAtual = null;
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
            OCORRENCIA ocorrencia = null;
            List<IMAGEM_RELATO> imagensDoBanco = null;
            try
            {
                this.Ocupado = true;
                this.EmEdicao = false;
                this._idOcorrenciaSQLite = new Guid(idOcorrencia);

                ocorrencia = await App.Banco.BuscarOcorrenciaAsync(this._idOcorrenciaSQLite);

                this.Numero = string.Empty;
                this.Data = ocorrencia.DATA.Date;
                this.Hora = ocorrencia.DATA.TimeOfDay;
                this.Descricao = ocorrencia.DESCRICAO;

                REGIONAL regional = await App.Banco.BuscarRegionalAsync(ocorrencia.ID_REGIONAL);
                if (regional != null)
                    this.UnidadeRegional = regional.ToModeloObj();

                GERENCIA gerencia = await App.Banco.BuscarGerenciaAsync(ocorrencia.ID_GERENCIA);
                if (gerencia != null)
                    this.Gerencia = gerencia.ToModeloObj();

                AREA area = await App.Banco.BuscarAreaAsync(ocorrencia.ID_AREA);
                if (area != null)
                    this.Area = area.ToModeloObj();

                LOCAL local = await App.Banco.BuscarLocalAsync(ocorrencia.ID_LOCAL);
                if (local != null)
                    this.Local = local.ToModeloObj();

                TIPO tipo = await App.Banco.BuscarTipoAsync(ocorrencia.ID_TIPO);
                if (tipo != null)
                    this.Tipo = tipo.ToModeloObj();

                CLASSIFICACAO classificacao = await App.Banco.BuscarClassificacaoAsync(ocorrencia.ID_CLASSIFICACAO);
                if (classificacao != null)
                    this.Classificacao = classificacao.ToModeloObj();

                SUBCLASSIFICACAO subclassificacao = await App.Banco.BuscarSubClassificacaoAsync(ocorrencia.ID_SUBCLASSIFICACAO);
                if (subclassificacao != null)
                    this.SubClassificacao = subclassificacao.ToModeloObj();

                if (ocorrencia.ID_CATEGORIA.HasValue)
                {
                    CATEGORIA categoria = await App.Banco.BuscarCategoriaAsync(ocorrencia.ID_CATEGORIA.Value);
                    if (categoria != null)
                        this.Categoria = categoria.ToModeloObj();
                }

                if (ocorrencia.ID_FORNECEDOR.HasValue)
                {
                    FORNECEDOR fornecedor = await App.Banco.BuscarFornecedorAsync(ocorrencia.ID_FORNECEDOR.Value);
                    if (fornecedor != null)
                        this.Fornecedor = fornecedor.ToModeloObj();
                }

                if (ocorrencia.ST_ACAOIMEDIATA.HasValue)
                {
                    this.AcoesImediatasSim = ocorrencia.ST_ACAOIMEDIATA.Value;
                    this.AcoesImediatasNao = !ocorrencia.ST_ACAOIMEDIATA.Value;
                    this.DescricaoAcoesImediatas = ocorrencia.DS_ACAOIMEDIATA;
                }

                this.NaoQueroMeIdentificar = ocorrencia.ST_NAOQUEROIDENTIFICAR;

                if (!this.NaoQueroMeIdentificar)
                {
                    if (ocorrencia.ID_COMUNICADOPOR.HasValue)
                    {
                        VINCULO vinculo = await App.Banco.BuscarVinculoAsync(ocorrencia.ID_COMUNICADOPOR.Value);
                        if (vinculo != null)
                            this.ComunicadoPor = vinculo.ToModeloObj();
                    }

                    if (ocorrencia.ID_REGISTRADOPOR.HasValue)
                    {
                        VINCULO vinculo = await App.Banco.BuscarVinculoAsync(ocorrencia.ID_REGISTRADOPOR.Value);
                        if (vinculo != null)
                            this.RegistradoPor = vinculo.ToModeloObj();
                    }
                }

                if (this.Imagens.Count > 0)
                    this.Imagens.Clear();
                this.ImagemAtual = null;
                imagensDoBanco = await App.Banco.BuscarImagensAsync(this._idOcorrenciaSQLite);
                try
                {
                    foreach (IMAGEM_RELATO img in imagensDoBanco)
                        this.Imagens.Add(new ItemImagem() { IdImagem = img.ID_IMAGEM, Caminho = img.CAMINHO, Image = img.BYTES_IMAGEM, Data = img.DATA });
                }
                catch (Exception ex)
                {
                }
                if (this.Imagens.Count > 0)
                    this.ImagemAtual = this.Imagens.FirstOrDefault();
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
                string url = string.Concat(App.__EnderecoWebApi, "/Ocorrencias/RetornarOcorrencia");

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
                        Ocorrencia ocorrencia = Newtonsoft.Json.JsonConvert.DeserializeObject<Ocorrencia>(conteudo);

                        this.Numero = ocorrencia.CODIGO;
                        this.Data = ocorrencia.DATA;
                        this.Hora = ocorrencia.HORA;
                        this.UnidadeRegional = ocorrencia.UNIDADEREGIONAL;
                        this.Gerencia = ocorrencia.GERENCIA;
                        this.Area = ocorrencia.AREA;
                        this.Local = ocorrencia.LOCAL;
                        this.Descricao = ocorrencia.DESCRICAO;
                        this.Tipo = ocorrencia.TIPO;
                        this.Classificacao = ocorrencia.CLASSIFICACAO;
                        this.SubClassificacao = ocorrencia.SUBCLASSIFICACAO;
                        this.Categoria = ocorrencia.CATEGORIA;
                        this.Fornecedor = ocorrencia.FORNECEDOR;
                        if (!string.IsNullOrEmpty(ocorrencia.ACAOIMEDIATA))
                        {
                            this.AcoesImediatasSim = ocorrencia.ACAOIMEDIATA.ToBoolSim();
                            this.AcoesImediatasNao = !ocorrencia.ACAOIMEDIATA.ToBoolSim();
                        }
                        this.DescricaoAcoesImediatas = ocorrencia.DESCRICAOACOESIMEDIATAS;

                        this.ComunicadoPor = ocorrencia.COMUNICADOPOR;
                        if(this.ComunicadoPor != null)
                            await App.Banco.InserirOuAlterarAsync(new VINCULO(this.ComunicadoPor));

                        this.RegistradoPor = ocorrencia.REGISTRADOPOR;
                        if (this.RegistradoPor != null)
                            await App.Banco.InserirOuAlterarAsync(new VINCULO(this.RegistradoPor));

                        this.NaoQueroMeIdentificar = ocorrencia.NAOQUEROMEIDENTIFICAR;
                        
                        this.OnPropertyChanged("ExibirNumero");

                        if (ocorrencia.ANEXOS != null)
                        {
                            if (this.Imagens.Count > 0)
                                this.Imagens.Clear();
                            //this.ImagemAtual = null;
                            foreach (DocAnexo a in ocorrencia.ANEXOS)
                            {
                                this.Imagens.Add(new ItemImagem() { IdImagem = Guid.NewGuid(), Caminho = a.NM_ANEXO, Image = a.BI_ANEXO, Data = null });
                            }
                            //if(this.Imagens.Count > 0)
                            //    this.ImagemAtual = this.Imagens.FirstOrDefault();
                        }
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

        private bool UnidadeRegionalValida()
        {
            bool ok = this.UnidadeRegional != null;
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

        private bool DescricaoValida()
        {
            bool ok = !string.IsNullOrEmpty(this.Descricao);
            return ok;
        }

        private bool ClassificacaoValida()
        {
            bool ok = this.Classificacao != null;
            return ok;
        }

        private bool SubClassificacaoValida()
        {
            bool ok = this.Classificacao != null && this.Classificacao.Codigo != "DQ" && this.SubClassificacao != null;
            return ok;
        }

        private bool TipoValido()
        {
            bool ok = this.Tipo != null;
            return ok;
        }

        private bool AcoesImediatasValida()
        {
            bool ok = this.AcoesImediatasSim == true || this.AcoesImediatasNao == true;
            return ok;
        }

        private bool DescricaoAcoesImediatasValida()
        {
            bool ok = (this.AcoesImediatasSim == true && !string.IsNullOrEmpty(this.DescricaoAcoesImediatas)) || this.AcoesImediatasNao == true;
            return ok;
        }

        private Task<string> VerificarCamposObrigatorios()
        {
            return Task.Run(() =>
            {
                string erro = string.Empty;

                if (!this.DataValida()) erro += Environment.NewLine + this.Textos.Data;
                if (!this.UnidadeRegionalValida()) erro += Environment.NewLine + this.Textos.UnidadeRegional;
                if (!this.GerenciaValida()) erro += Environment.NewLine + this.Textos.GerenciaDoDesvio;
                if (!this.AreaValida()) erro += Environment.NewLine + this.Textos.AreaDoDesvio;
                if (!this.LocalValido()) erro += Environment.NewLine + this.Textos.LocalDoDesvio;
                if (!this.DescricaoValida()) erro += Environment.NewLine + this.Textos.DescricaoDaOcorrencia;
                if (!this.TipoValido()) erro += Environment.NewLine + this.Textos.TipoDaOcorrencia;
                if (!this.ClassificacaoValida()) erro += Environment.NewLine + this.Textos.ClassificacaoDaOcorrencia;
                if (!this.SubClassificacaoValida()) erro += Environment.NewLine + this.Textos.SubClassificacaoDaOcorrencia;
                if (!this.AcoesImediatasValida()) erro += Environment.NewLine + this.Textos.ForamTomadasAcoesImediatas;
                if (!this.DescricaoAcoesImediatasValida()) erro += Environment.NewLine + this.Textos.DescricaoDasAcoesImediatas;

                //if (this.NaoQueroMeIdentificar == false)
                //{
                //    if (this.RegistradoPor == null) erro += Environment.NewLine + this.Textos.RegistradoPor;
                //    if (this.ComunicadoPor == null) erro += Environment.NewLine + this.Textos.ComunicadoPor;
                //}

                if (!string.IsNullOrEmpty(erro))
                    erro = string.Concat(Environment.NewLine, this.Textos.CamposObrigatorios, ": ", Environment.NewLine, erro);

                return erro;
            });
        }

        #endregion Validação da ocorrência
    }
}
