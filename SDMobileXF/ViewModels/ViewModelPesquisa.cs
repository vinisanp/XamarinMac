using SDMobileXF.Banco.Tabelas;
using SDMobileXF.Classes;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SDMobileXF.ViewModels
{
    public class ViewModelPesquisa : ViewModelBase
    {
        private ModeloObj _selecionado;
        private List<ModeloObj> _lista;
        private readonly Action _retornoItemSelecionado;
        private readonly Action<string> _retornoPesquisaEfetuada;
        private string _cabecalho;
        private readonly string _filtroIdPai;

        public Enumerados.Tabela Tabela { get; private set; }

        public string Cabecalho
        {
            get { return this._cabecalho; }
            set { this.DefinirPropriedade(ref this._cabecalho, value); }
        }

        public List<ModeloObj> Lista
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

        public ModeloObj Selecionado
        {
            get
            {
                return this._selecionado;
            }
            set
            {
                this.DefinirPropriedade(ref this._selecionado, value);

                if (!this.Ocupado)
                    this._retornoItemSelecionado?.Invoke();
            }
        }

        public Command ProcurarCommand { get; private set; }

        public Command CancelarCommand { get; private set; }

        public ViewModelPesquisa(Action itemSelecionado, Action<string> pesquisaEfetuada, Enumerados.Tabela tabela, string cabecalho) : this(itemSelecionado, pesquisaEfetuada, tabela, cabecalho, null)
        {
        }

        public ViewModelPesquisa(Action itemSelecionado, Action<string> pesquisaEfetuada, Enumerados.Tabela tabela, string cabecalho, string filtroIdPai)
        {
            this.Ocupado = true;

            this.ProcurarCommand = new Command<string>( async (string textoBusca) => { await this.Procurar(textoBusca); });
            this.CancelarCommand = new Command( async () => { await this.Cancelar(); });

            this._retornoItemSelecionado = itemSelecionado;
            this._retornoPesquisaEfetuada = pesquisaEfetuada;            
            this._cabecalho = cabecalho;
            this._filtroIdPai = filtroIdPai;
            this.Tabela = tabela;
        }

        private async Task Cancelar()
        {
            this.Ocupado = true;
            this.Lista = new List<ModeloObj>();
            this.Ocupado = false;
        }

        private async Task Procurar(string textoBusca)
        {
            this.Ocupado = true;
            try
            {
                int quant = 0;
                if (this.Tabela == Enumerados.Tabela.Vinculo)
                    quant = await this.GetQuantidade(textoBusca);

                if (quant > 1000)
                {
                    this.Lista = new List<ModeloObj>();
                    string msg = Globalizacao.Traduz("Mais de 1000 registros encontrados.");
                    msg = string.Concat(msg, Environment.NewLine, Globalizacao.Traduz("Refine a sua pesquisa para evitar travamentos."));
                    msg = string.Concat(msg, Environment.NewLine, Globalizacao.Traduz("Deseja continuar assim mesmo?"));
                    this._retornoPesquisaEfetuada?.Invoke(msg);
                }
                else
                {
                    this.Lista = await this.GetDados(textoBusca);
                    this._retornoPesquisaEfetuada?.Invoke(string.Empty);
                }
            }
            catch (Exception ex)
            {
                this.Lista = new List<ModeloObj>();
                Debug.WriteLine(ex.Message);
                this._retornoPesquisaEfetuada?.Invoke(string.Empty);
            }
            this.Ocupado = false;
        }

        public async Task ForcarProcurar(string textoBusca)
        {
            this.Ocupado = true;
            try
            {
                this.Lista = await this.GetDados(textoBusca);
                this._retornoPesquisaEfetuada?.Invoke(string.Empty);
            }
            catch (Exception ex)
            {
                this.Lista = new List<ModeloObj>();
                Debug.WriteLine(ex.Message);
                this._retornoPesquisaEfetuada?.Invoke(string.Empty);
            }
            this.Ocupado = false;
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                if (this.Tabela != Enumerados.Tabela.Fornecedor &&
                    this.Tabela != Enumerados.Tabela.Vinculo &&
                    this.Tabela != Enumerados.Tabela.CadastroBasico)
                    this.Lista = await this.GetDados(string.Empty);
                else
                    this.Lista = new List<ModeloObj>();
            }
            catch (Exception ex)
            {
                this.Lista = new List<ModeloObj>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }

        private async Task<List<ModeloObj>> GetDados(string textoBusca)
        {
            List<ModeloObj> lista;
            if (Util.TemAcessoInternet)
                lista = await this.GetDadosApi(textoBusca);
            else
                lista = await this.GetDadosBanco(textoBusca);
            
            return lista;
        }

        private async Task<List<ModeloObj>> GetDadosBanco(string textoBusca)
        {
            List<ModeloObj> lst = new List<ModeloObj>();
            if (this.Tabela == Enumerados.Tabela.UnidadeRegional)
            {
                List<REGIONAL> aux = await App.Banco.BuscarRegionaisAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Gerencia)
            {
                List<GERENCIA> aux = await App.Banco.BuscarGerenciasAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Area)
            {
                List<AREA> aux = await App.Banco.BuscarAreasAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Local)
            {
                List<LOCAL> aux = await App.Banco.BuscarLocaisAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Tipo)
            {
                List<TIPO> aux = await App.Banco.BuscarTiposAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Classificacao)
            {
                List<CLASSIFICACAO> aux = await App.Banco.BuscarClassificacoesAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.SubClassificacao)
            {
                List<SUBCLASSIFICACAO> aux = await App.Banco.BuscarSubClassificacoesAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Categoria)
            {
                List<CATEGORIA> aux = await App.Banco.BuscarCategoriasAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Fornecedor)
            {
                List<FORNECEDOR> aux = await App.Banco.BuscarFornecedoresAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Vinculo)
            {
                List<VINCULO> aux = await App.Banco.BuscarVinculosAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.RespostasSeguroInseguroNa)
            {
                List<SEGURO_INSEGURO> aux = await App.Banco.BuscarSeguroInsegurosAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.AtivadoresComportamentoCOGNITIVOS)
            {
                List<COGNITIVO> aux = await App.Banco.BuscarCognitivosAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.AtivadoresComportamentoFISIOLOGICOS)
            {
                List<FISIOLOGICO> aux = await App.Banco.BuscarFisiologicosAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.AtivadoresComportamentoPSICOLOGICOS)
            {
                List<PSICOLOGICO> aux = await App.Banco.BuscarPsicologicosAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.AtivadoresComportamentoSOCIAIS)
            {
                List<SOCIAL> aux = await App.Banco.BuscarSociaisAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.TipoInspecao)
            {
                List<TIPO_INSPECAO> aux = await App.Banco.BuscarTipoInspecoesAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.AtividadeInspecao)
            {
                List<ATIVIDADE_INSPECAO> aux = await App.Banco.BuscarAtividadeInspecoesAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Unidade)
            {
                List<UNIDADE> aux = await App.Banco.BuscarUnidadesAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Letra)
            {
                List<LETRA> aux = await App.Banco.BuscarLetrasAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.TurnoAnomalia)
            {
                List<TURNO> aux = await App.Banco.BuscarTurnosAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.GerenciaGeralCsn)
            {
                List<GERENCIA_GERAL_CSN> aux = await App.Banco.BuscarGerenciasGeraisCsnAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.GerenciaCsn)
            {
                List<GERENCIA_CSN> aux = await App.Banco.BuscarGerenciasCsnAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.AreaEquipamento)
            {
                List<AREA_EQUIPAMENTO> aux = await App.Banco.BuscarAreaEquipamentosAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.LocalEquipamento)
            {
                List<LOCAL_EQUIPAMENTO> aux = await App.Banco.BuscarLocalEquipamentosAsync(new Guid(this._filtroIdPai), textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.OrigemAnomalia)
            {
                List<ORIGEM_ANOMALIA> aux = await App.Banco.BuscarOrigemAnomaliasAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.TipoAnomalia)
            {
                List<TIPO_ANOMALIA> aux = await App.Banco.BuscarTipoAnomaliasAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.ClassificacaoTipo)
            {
                List<CLASSIFICACAO_TIPO> aux = await App.Banco.BuscarClassificacaoTiposAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Probabilidade)
            {
                List<PROBABILIDADE> aux = await App.Banco.BuscarProbabilidadesAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.Severidade)
            {
                List<SEVERIDADE> aux = await App.Banco.BuscarSeveridadesAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.TipoAvaliador)
            {
                List<TIPO_AVALIADOR> aux = await App.Banco.BuscarTiposAvaliadorAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }
            else if (this.Tabela == Enumerados.Tabela.TarefaObservada)
            {
                List<TAREFA_OBSERVADA> aux = await App.Banco.BuscarTarefasObservadasAsync(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }

            try
            {
                lst = lst.OrderBy(item => item.Codigo.PadLeft(20, '0')).ToList();
            }
            catch(Exception ex)
            {
                string erro = ex.Message;                
            }
            return lst;
        }

        private async Task<List<ModeloObj>> GetDadosApi(string textoBusca)
        {
            List<ModeloObj> lstDados = new List<ModeloObj>();

            string url = string.Concat(App.__EnderecoWebApi, "/Tabela/RetornarDados");
            if(App.__nivelDeProjeto == "CSN")
                url = string.Concat(App.__EnderecoWebApi, "/TabelaCsn/RetornarDados");


            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("tabela", this.Tabela.ToString());
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            if (!string.IsNullOrEmpty(this._filtroIdPai))
                parametros.Add("idRegistroPai", this._filtroIdPai);
            if (!string.IsNullOrEmpty(textoBusca))
                parametros.Add("filtro", textoBusca);

            if (App.__nivelDeProjeto == "Suzano")
            {
                if (this.Tabela == Enumerados.Tabela.Classificacao) //temporario enquanto o serviço nao é atualizado na suzano. A classificação ficou trazendo os dados ao contrario. Removido o NOT da consulta 
                    parametros.Add("papelsso", "true");
                else
                    parametros.Add("papelsso", UsuarioLogado.Instancia.PAPEL_SSO.ToString());
            }

            FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

            using (HttpClient requisicao = new HttpClient())
            {
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string conteudo = await resposta.Content.ReadAsStringAsync();
                    lstDados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModeloObj>>(conteudo);

                    if (this.Tabela == Enumerados.Tabela.Classificacao && UsuarioLogado.Instancia.PAPEL_SSO == false) //temporario enquanto o serviço nao é atualizado na suzano. A classificação ficou trazendo os dados ao contrario. Removido o NOT da consulta 
                    {
                        this.ApagarItemTemporario(ref lstDados, "DP");
                        this.ApagarItemTemporario(ref lstDados, "LM");
                        this.ApagarItemTemporario(ref lstDados, "LP");
                        this.ApagarItemTemporario(ref lstDados, "PAE");
                    }
                }
            }

            return lstDados;
        }


        private void ApagarItemTemporario(ref List<ModeloObj> lista, string codigo)
        {
            ModeloObj item = lista.FirstOrDefault(i => i.Codigo == codigo);
            if (item != null)
                lista.Remove(item);
        }

        private async Task<int> GetQuantidade(string textoBusca)
        {
            if (Util.TemAcessoInternet)
                return await this.GetQuantidadeApi(textoBusca);
            else
                return await App.Banco.QuantidadeVinculosAsync(textoBusca);
        }

        private async Task<int> GetQuantidadeApi(string textoBusca)
        {
            string url = string.Concat(App.__EnderecoWebApi, "/Tabela/RetornarQuantidade");

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("tabela", this.Tabela.ToString());
            parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
            if (!string.IsNullOrEmpty(this._filtroIdPai))
                parametros.Add("idRegistroPai", this._filtroIdPai);
            if (!string.IsNullOrEmpty(textoBusca))
                parametros.Add("filtro", textoBusca);

            FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

            using (HttpClient requisicao = new HttpClient())
            {
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string conteudo = await resposta.Content.ReadAsStringAsync();
                    int quant = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(conteudo);
                    return quant;
                }
            }

            return 0;
        }

    }
}
