using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SDMobileXF.ViewModels
{
    public class ViewModelVisualizarOcorrenciasCsn : ViewModelBase
    {
        private DateTime _dtInicial = DateTime.Today.AddDays(-10);
        private DateTime _dtFinal = DateTime.Today;
        private List<OcorrenciaCsn> _lista;
        private string _qtdRegistros;
        

        public ViewModelVisualizarOcorrenciasCsn(Action<bool, string> RetornoPesquisar)
        {
            this.PesquisarCommand = new Command(async () =>
            {
                this.Ocupado = true;
                this.Lista = await this.GetDados();
                this.QtdRegistros = this.Lista.Count > 0 ? $"({this.Lista.Count})" : string.Empty;
                this.Ocupado = false;
                if (this.DtInicial > this.DtFinal)
                    RetornoPesquisar(false, Globalizacao.Traduz("Data Inicial > Data Final."));
                else if (this.Lista.Count == 0)
                    RetornoPesquisar(false, Globalizacao.Traduz("Não existem registros para o período selecionado."));
                else if (this.Lista.Count > 1000)
                    RetornoPesquisar(false, Globalizacao.Traduz("Mais de 1000 registros encontrados. Refine a sua pesquisa."));                

            });
            
            this.Titulo = this.Textos.Ocorrencias;
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                if(Util.TemAcessoInternet)
                    this.Lista = await this.GetDados();
                else
                    this.Lista = new List<OcorrenciaCsn>();
                this.QtdRegistros = this.Lista.Count > 0 ? $"({this.Lista.Count})" : string.Empty;
            }
            catch (Exception ex)
            {
                this.Lista = new List<OcorrenciaCsn>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }

        #region Propriedades

        public Command PesquisarCommand { get; private set; }

        public DateTime DtInicial
        {
            get { return this._dtInicial; }
            set { this.DefinirPropriedade(ref this._dtInicial, value); }
        }

        public DateTime DtFinal
        {
            get { return this._dtFinal; }
            set { this.DefinirPropriedade(ref this._dtFinal, value); }
        }

        public List<OcorrenciaCsn> Lista
        {
            get
            {
                return this._lista;
            }
            set
            {
                this.DefinirPropriedade(ref this._lista, value);
            }
        }

        public string QtdRegistros
        {
            get { return this._qtdRegistros; }
            set { this.DefinirPropriedade(ref this._qtdRegistros, value); }
            
        }

        #endregion Propriedades


        #region Pesquisa

        private async Task<List<OcorrenciaCsn>> GetDados()
        {
            List<OcorrenciaCsn> lstDados = new List<OcorrenciaCsn>();
            try
            {
                string url = string.Concat(App.__EnderecoWebApi, "/OcorrenciasCsn/RetornarOcorrencias");

                Dictionary<string, string> parametros = new Dictionary<string, string>();                
                parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                parametros.Add("idVinculo", UsuarioLogado.Instancia.ID_VINCULO.ToString());                
                parametros.Add("dtInicial", this.DtInicial.ToString("dd/MM/yyyy"));
                parametros.Add("dtFinal", this.DtFinal.ToString("dd/MM/yyyy"));
                parametros.Add("idsRegProfissional", UsuarioLogado.Instancia.IDS_REGISTRO_PROFISSIONAL);
                
                FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                using (HttpClient requisicao = new HttpClient())
                {
                    HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string conteudo = await resposta.Content.ReadAsStringAsync();
                        lstDados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OcorrenciaCsn>>(conteudo).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return lstDados;
        }

        #endregion Pesquisa
    }
}
