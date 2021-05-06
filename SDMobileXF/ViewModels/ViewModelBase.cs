using SDMobileXF.Classes;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using SDMobileXF.Banco.Tabelas;

namespace SDMobileXF.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected string _titulo;
        protected bool _ocupado = false;
        protected bool _emEdicao = false;
        protected List<ModeloObj> _lstConformeNaoConforme;

        public Textos Textos => Textos.Instancia;

        public string Logo512
        {
            get
            {
                if (App.__nivelDeProjeto == "CSN")
                    return "LogoCsn512.png"; 
                else if (App.__nivelDeProjeto == "Suzano")
                    return "LogoSuzano512.png";

                return string.Empty;
            }
        }

        public List<ModeloObj> LstConformeNaoConforme
        {
            get
            {
                if (this._lstConformeNaoConforme == null)
                {
                    this._lstConformeNaoConforme = new List<ModeloObj>
                    {
                        new ModeloObj(new Guid("9e0fd48b-3811-44d6-bcbf-fa6577c8211d"), "C", "Conforme"),
                        new ModeloObj(new Guid("8196e394-50de-453d-8f40-a386ee068dd9"), "NC", "Não Conforme"),
                        new ModeloObj(new Guid("2f837df0-d09d-46a6-8d66-5667f76c3a57"), "N/A", "Não se aplica")
                    };
                }

                return this._lstConformeNaoConforme;
            }
        }

        public ViewModelBase()
        {
        }

        public virtual async Task Atualizar()
        {
            this.OnPropertyChanged(string.Empty);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string nomePropriedade = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        protected bool DefinirPropriedade<T>(ref T variavel, T valor, [CallerMemberName] string nomePropriedade = null)
        {
            if (EqualityComparer<T>.Default.Equals(variavel, valor))
                return false;

            variavel = valor;
            this.OnPropertyChanged(nomePropriedade);
            return true;
        }

        public bool TelaCarregada { get; set; }

        public string Titulo
        {
            get 
            {
                if (string.IsNullOrEmpty(this._titulo))
                {
                    if(App.__nivelDeProjeto.Length <= 4)
                        this._titulo = "Segurança do Trabalho " + App.__nivelDeProjeto;
                    else
                        this._titulo = "S.T. " + App.__nivelDeProjeto;
                }
                return this._titulo; 
            }
            set { this.DefinirPropriedade(ref this._titulo, value); }
        }

        public virtual bool Ocupado
        {
            get { return this._ocupado; }
            set { this.DefinirPropriedade(ref this._ocupado, value); }
        }

        public virtual bool EmEdicao
        {
            get { return this._emEdicao; }
            set { this.DefinirPropriedade(ref this._emEdicao, value); }
        }

        public bool LabelPickerVisivel
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                    case Device.Android:
                        return true;
                    case Device.UWP:
                        return false;
                    default:
                        return true;
                }
            }
        }

        public virtual Task LoadAsync()
        {
            return Task.FromResult(true);
        }

        public virtual Task LoadAsync(object[] args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Busca item filho baseado no pai para efetuar carga da informação filha caso so tenha um registro
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="gerencia"></param>
        /// <param name="retornoItemSelecionado"></param>
        public async void SelecionarItemFilho(ModeloObj objPai, Enumerados.Tabela tabelaFilha, Action<ModeloObj, Enumerados.Tabela> retornoItemSelecionado)
        {
            if (Util.TemAcessoInternet)
                this.SelecionarItemFilhoApi(objPai, tabelaFilha, retornoItemSelecionado);
            else
                this.SelecionarItemFilhoBanco(objPai, tabelaFilha, retornoItemSelecionado);
        }

        /// <summary>
        /// Busca item filho baseado no pai para efetuar carga da informação filha caso so tenha um registro
        /// fonte de dados: SQLite
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="gerencia"></param>
        /// <param name="retornoItemSelecionado"></param>
        public async void SelecionarItemFilhoBanco(ModeloObj objPai, Enumerados.Tabela tabelaFilha, Action<ModeloObj, Enumerados.Tabela> retornoItemSelecionado)
        {
            if (tabelaFilha == Enumerados.Tabela.Gerencia)
            {
                List<GERENCIA> lst = await App.Banco.BuscarGerenciasAsync(objPai.Id, string.Empty);
                if (lst != null && lst.Count == 1)
                    retornoItemSelecionado(lst[0].ToModeloObj(), tabelaFilha);
            }
            else if (tabelaFilha == Enumerados.Tabela.Area)
            {
                List<AREA> lst = await App.Banco.BuscarAreasAsync(objPai.Id, string.Empty);
                if (lst != null && lst.Count == 1)
                    retornoItemSelecionado(lst[0].ToModeloObj(), tabelaFilha);
            }
            else if (tabelaFilha == Enumerados.Tabela.Local)
            {
                List<LOCAL> lst = await App.Banco.BuscarLocaisAsync(objPai.Id, string.Empty);
                if (lst != null && lst.Count == 1)
                    retornoItemSelecionado(lst[0].ToModeloObj(), tabelaFilha);
            }
            else if (tabelaFilha == Enumerados.Tabela.SubClassificacao)
            {
                List<SUBCLASSIFICACAO> lst = await App.Banco.BuscarSubClassificacoesAsync(objPai.Id, string.Empty);
                if (lst != null && lst.Count == 1)
                    retornoItemSelecionado(lst[0].ToModeloObj(), tabelaFilha);
            }
            else if (tabelaFilha == Enumerados.Tabela.Categoria)
            {
                List<CATEGORIA> lst = await App.Banco.BuscarCategoriasAsync(objPai.Id, string.Empty);
                if (lst != null && lst.Count == 1)
                    retornoItemSelecionado(lst[0].ToModeloObj(), tabelaFilha);
            }
        }
        
        /// <summary>
        /// Busca item filho baseado no pai para efetuar carga da informação filha caso so tenha um registro
        /// fonte de dados: API
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="gerencia"></param>
        /// <param name="retornoItemSelecionado"></param>
        public async void SelecionarItemFilhoApi(ModeloObj objPai, Enumerados.Tabela tabelaFilha, Action<ModeloObj, Enumerados.Tabela> retornoItemSelecionado)
        {
            ModeloObj objFilho = new ModeloObj();

            string url = string.Concat(SDMobileXF.App.__EnderecoWebApi, "/Tabela/RetornarSeUnicoFilho");

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("tabela", tabelaFilha.ToString());
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("idRegistroPai", objPai.Id.ToString());

            FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

            using (HttpClient requisicao = new HttpClient())
            {
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string conteudo = await resposta.Content.ReadAsStringAsync();
                    objFilho = Newtonsoft.Json.JsonConvert.DeserializeObject<ModeloObj>(conteudo);
                    if (objFilho != null)
                        retornoItemSelecionado(objFilho, tabelaFilha);
                }
            }

        }

        public async Task<ModeloObj> Vinculo()
        {
            if (Util.TemAcessoInternet)
            {
                ModeloObj modelo = await this.VinculoApi();
                if(modelo != null)
                    await App.Banco.InserirOuAlterarAsync(new VINCULO(modelo));
                return modelo;
            }
            else
                return await this.VinculoBanco();
        }

        public async Task<ModeloObj> VinculoBanco()
        {
            VINCULO v = await App.Banco.BuscarVinculoAsync(UsuarioLogado.Instancia.ID_VINCULO);
            return v.ToModeloObj();
        }

        public async Task<ModeloObj> VinculoApi()
        {
            List<ModeloObj> lstDados = new List<ModeloObj>();

            string url = string.Concat(App.__EnderecoWebApi, "/Tabela/RetornarDados");
            if(App.__nivelDeProjeto == "CSN")
                url = string.Concat(App.__EnderecoWebApi, "/TabelaCsn/RetornarDados");

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("tabela", Enumerados.Tabela.Vinculo.ToString());
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            parametros.Add("idRegistro", UsuarioLogado.Instancia.ID_VINCULO.ToString());

            FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

            using (HttpClient requisicao = new HttpClient())
            {
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string conteudo = await resposta.Content.ReadAsStringAsync();
                    lstDados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModeloObj>>(conteudo);
                }
            }

            if (lstDados != null && lstDados.Count > 0)
                return lstDados.FirstOrDefault();

            return null;
        }

    }
}
