using SDMobileXF.Banco.Tabelas;
using SDMobileXF.ViewModels;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
    public class OffLine
    {
        #region Variáveis

        private double _count = 0;
        private double _percentual = 0;
        private double _total = 0;
        public event ProgressChangedEventHandler ProgressoAlterado;
        public event ProgressChangedEventHandler RegistrosSincronizados;
        private static OffLine _instancia;
        private ConcurrentDictionary<Enumerados.Tabela, List<ModeloObj>> _dadosApi = new ConcurrentDictionary<Enumerados.Tabela, List<ModeloObj>>();

        #endregion Variáveis


        #region Propriedades

        public bool Executando { get; private set; }
        public bool SincronizandoOcorrencias { get; private set; }
        public bool SincronizandoAbordagens { get; private set; }
        public bool SincronizandoSnas { get; private set; }
        public bool SincronizandoInspecoes { get; private set; }
        public bool SincronizandoOpas { get; private set; }

        public static OffLine Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new OffLine();
                return _instancia;
            }
        }

        public double Percentual
        {
            get
            {
                return this._percentual;
            }
            private set
            {
                this._percentual = value;
                if(this.ProgressoAlterado != null)
                    this.ProgressoAlterado.Invoke(this, new ProgressChangedEventArgs(Convert.ToInt32(this._percentual * 100), null));
            }
        }

        public long TamDadosBaixados { get; private set; }

        public TimeSpan Tempo { get; private set; }

        #endregion Propriedades


        #region Métodos Tabelas Base

        private async Task<List<ModeloObj>> GetDadosApi(Enumerados.Tabela tabela)
        {
            App.Log("Baixando " + tabela.ToString());

            List<ModeloObj> lstDados = new List<ModeloObj>();

            string url = string.Concat(App.__EnderecoWebApi, "/Tabela/RetornarDados");
            if(App.__nivelDeProjeto == "CSN")
                url = string.Concat(App.__EnderecoWebApi, "/TabelaCsn/RetornarDados");

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("tabela", tabela.ToString());
            parametros.Add("idChaveSessao", Guid.Empty.ToString());
            //if(tabela == Enumerados.Tabela.Vinculo)
            //    parametros.Add("idRegistro", UsuarioLogado.Instancia.ID_VINCULO.ToString());

            FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

            using (HttpClient requisicao = new HttpClient())
            {
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    this.TamDadosBaixados += resposta.Content.Headers.ContentLength.Value;
                    string conteudo = await resposta.Content.ReadAsStringAsync();
                    lstDados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModeloObj>>(conteudo);
                }
            }

            this._count++;
            this.Percentual = this._count / this._total * 100;

            return lstDados;
        }

        private async Task AtualizarDadosNoSQLITE(Enumerados.Tabela tabela)
        {
            try
            {
                if (this._dadosApi.ContainsKey(tabela))
                {
                    List<ModeloObj> lst = this._dadosApi[tabela];

                    List<object> lstInserir = new List<object>();
                    List<object> lstAlterar = new List<object>();
                    HashSet<Guid> ids = await App.Banco.BuscarIdsAsync(tabela);

                    foreach (ModeloObj m in lst)
                    {
                        if (tabela == Enumerados.Tabela.Vinculo && m.Id == Guid.Empty)
                            continue;

                        this.AdicionarNasListas(lstInserir, lstAlterar, ids, m, tabela);
                    }

                    if (lstInserir.Any())
                        await App.Banco.InserirAsync(lstInserir);
                    if (lstAlterar.Any())
                        await App.Banco.AlterarAsync(lstAlterar);

                    await App.Banco.InserirOuAlterarAsync(new INFO(tabela.ToString(), DateTime.Now, lst.Count));

                    this._count++;
                    this.Percentual = this._count / this._total * 100;
                }
                App.Log("Tabela " + tabela.ToString() + " atualizada no SQLITE");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                App.Log("Erro ao atualizar no SQLITE " + tabela.ToString() + " - " + ex.Message);
            }
        }

        private void AdicionarNasListas(List<object> lstInserir, List<object> lstAlterar, HashSet<Guid> ids, ModeloObj m, Enumerados.Tabela tabela)
        {
            object obj = null;

            if (tabela == Enumerados.Tabela.UnidadeRegional)                          obj = m.ToRegional();
            else if (tabela == Enumerados.Tabela.Gerencia)                            obj = m.ToGerencia();
            else if (tabela == Enumerados.Tabela.Area)                                obj = m.ToArea();
            else if (tabela == Enumerados.Tabela.Local)                               obj = m.ToLocal();
            else if (tabela == Enumerados.Tabela.Tipo)                                obj = m.ToTipo();
            else if (tabela == Enumerados.Tabela.Classificacao)                       obj = m.ToClassificacao();
            else if (tabela == Enumerados.Tabela.SubClassificacao)                    obj = m.ToSubClassificacao();
            else if (tabela == Enumerados.Tabela.Categoria)                           obj = m.ToCategoria();
            else if (tabela == Enumerados.Tabela.Fornecedor)                          obj = m.ToFornecedor();
            else if (tabela == Enumerados.Tabela.Vinculo)                             obj = m.ToVinculo();
            else if (tabela == Enumerados.Tabela.RespostasSeguroInseguroNa)           obj = m.ToSeguroInseguro();
            else if (tabela == Enumerados.Tabela.AtivadoresComportamentoCOGNITIVOS)   obj = m.ToCognitivos();
            else if (tabela == Enumerados.Tabela.AtivadoresComportamentoFISIOLOGICOS) obj = m.ToFisiologicos();
            else if (tabela == Enumerados.Tabela.AtivadoresComportamentoPSICOLOGICOS) obj = m.ToPsicologicos();
            else if (tabela == Enumerados.Tabela.AtivadoresComportamentoSOCIAIS)      obj = m.ToSociais();
            else if (tabela == Enumerados.Tabela.TipoInspecao)                        obj = m.ToTipoInspecao();
            else if (tabela == Enumerados.Tabela.AtividadeInspecao)                   obj = m.ToAtividadeInspecao();
            else if (tabela == Enumerados.Tabela.Unidade)                             obj = m.ToUnidade();
            else if (tabela == Enumerados.Tabela.Letra)                               obj = m.ToLetra();
            else if (tabela == Enumerados.Tabela.TurnoAnomalia)                       obj = m.ToTurno();
            else if (tabela == Enumerados.Tabela.GerenciaGeralCsn)                    obj = m.ToGerenciaGeralCsn();
            else if (tabela == Enumerados.Tabela.GerenciaCsn)                         obj = m.ToGerenciaCsn();
            else if (tabela == Enumerados.Tabela.AreaEquipamento)                     obj = m.ToAreaEquipamento();
            else if (tabela == Enumerados.Tabela.LocalEquipamento)                    obj = m.ToLocalEquipamento();
            else if (tabela == Enumerados.Tabela.OrigemAnomalia)                      obj = m.ToOrigemAnomalia();
            else if (tabela == Enumerados.Tabela.TipoAnomalia)                        obj = m.ToTipoAnomalia();
            else if (tabela == Enumerados.Tabela.ClassificacaoTipo)                   obj = m.ToClassificacaoTipo();
            else if (tabela == Enumerados.Tabela.Probabilidade)                       obj = m.ToProbabilidade();
            else if (tabela == Enumerados.Tabela.Severidade)                          obj = m.ToSeveridade();
            else if (tabela == Enumerados.Tabela.TipoAvaliador)                       obj = m.ToTipoAvaliador();
            else if (tabela == Enumerados.Tabela.TarefaObservada)                     obj = m.ToTarefaObservada();

            if (obj != null)
            {
                if (ids.Contains(m.Id))
                    lstAlterar.Add(obj);
                else
                    lstInserir.Add(obj);
            }
        }

        private async Task BaixarDadosApiAsync(Enumerados.Tabela tabela)
        {
            if (_dadosApi.ContainsKey(tabela))
                this._dadosApi[tabela] = await this.GetDadosApi(tabela);
            else
                this._dadosApi.TryAdd(tabela, await this.GetDadosApi(tabela));
        }

        public Task LimparAsync()
        {
            return Task.Run(() => { this.Limpar(); });
        }

        public void Limpar()
        {
            if (OffLine.Instancia.Executando)
                return;

            this.Executando = true;
            this.Percentual = 0;

            try
            {
                App.Banco.DroparTabelas();
                App.Banco.CriarTabelasSeNaoExistir();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            this.Executando = false;
            App.Sincronizado = false;
        }


        private async Task SincronizarTabelasBase(List<Enumerados.Tabela> tabelas)
        {
            App.Log("Inicio SincronizarTabelasBase");

            if (OffLine.Instancia.Executando)
                return;

            this.Executando = true;
            this.TamDadosBaixados = 0;
            this.Percentual = 0;

            try
            {
                DateTime inicio = DateTime.Now;

                this._count = 0;

                //multiplica o count da lista por 2 por que tem o metodo q baixa e outro q grava no sqlite
                //e o percentual na tela de config considera os dois, o total ainda é somado com mais 2 
                //pois ainda tem o vinculo q é executado depois (sem usar a lista de tabelas)
                this._total = (tabelas.Count * 2) + 2;

                Parallel.ForEach(tabelas, t => { this.BaixarDadosApiAsync(t).Wait(); });
                foreach (Enumerados.Tabela t in tabelas)
                    await this.AtualizarDadosNoSQLITE(t);

                await this.BaixarDadosApiAsync(Enumerados.Tabela.Vinculo);
                await this.AtualizarDadosNoSQLITE(Enumerados.Tabela.Vinculo);

                int tamBytes = Convert.ToInt32(this.TamDadosBaixados / 1024);
                int registros = this._dadosApi.Sum(d => d.Value.Count);

                await App.Banco.InserirOuAlterarAsync(new INFO("kbytes", DateTime.Now, tamBytes));
                await App.Banco.InserirOuAlterarAsync(new INFO("registros", DateTime.Now, registros));

                this.Tempo = DateTime.Now - inicio;

                App.Log("Tempos total: " + this.Tempo.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            this.Executando = false;
            this.Percentual = 100;

            App.Log("Final SincronizarTabelasBase");
        }

        public Task SincronizarTabelasBaseSuzanoAsync()
        {
            return Task.Run(() => { this.SincronizarTabelasBaseSuzano(); });
        }

        public async void SincronizarTabelasBaseSuzano()
        {
			List<Enumerados.Tabela> tabelas = new List<Enumerados.Tabela>() { Enumerados.Tabela.UnidadeRegional, Enumerados.Tabela.Gerencia, Enumerados.Tabela.Area,
																			  Enumerados.Tabela.Local, Enumerados.Tabela.Tipo, Enumerados.Tabela.Classificacao,
																			  Enumerados.Tabela.SubClassificacao, Enumerados.Tabela.Categoria, Enumerados.Tabela.Fornecedor,
																			  Enumerados.Tabela.RespostasSeguroInseguroNa, Enumerados.Tabela.AtivadoresComportamentoCOGNITIVOS,
																			  Enumerados.Tabela.AtivadoresComportamentoFISIOLOGICOS, Enumerados.Tabela.AtivadoresComportamentoPSICOLOGICOS,
																			  Enumerados.Tabela.AtivadoresComportamentoSOCIAIS, Enumerados.Tabela.TipoInspecao,
																			  Enumerados.Tabela.AtividadeInspecao, Enumerados.Tabela.TipoAvaliador, Enumerados.Tabela.TarefaObservada };

			await SincronizarTabelasBase(tabelas);
        }

        public Task SincronizarTabelasBaseCsnAsync()
        {
            return Task.Run(() => { this.SincronizarTabelasBaseCsn(); });
        }

        public async void SincronizarTabelasBaseCsn()
        {
            List<Enumerados.Tabela> tabelas = new List<Enumerados.Tabela>() {   Enumerados.Tabela.Unidade, Enumerados.Tabela.Letra, Enumerados.Tabela.TurnoAnomalia,
                                                                                Enumerados.Tabela.GerenciaGeralCsn, Enumerados.Tabela.GerenciaCsn, Enumerados.Tabela.AreaEquipamento,
                                                                                Enumerados.Tabela.LocalEquipamento, Enumerados.Tabela.OrigemAnomalia, Enumerados.Tabela.TipoAnomalia,
                                                                                Enumerados.Tabela.ClassificacaoTipo, Enumerados.Tabela.Probabilidade, Enumerados.Tabela.Severidade};

            await SincronizarTabelasBase(tabelas);
        }

        #endregion Métodos Tabelas Base


        #region Métodos Ocorrencias

        public Task SincronizarOcorrenciasAsync()
        {
            return Task.Run(() => { this.SincronizarOcorrencias(); });
        }

        public async void SincronizarOcorrencias()
        {
            App.Log("Início OffLine.SincronizarOcorrencias");

            if (OffLine.Instancia.SincronizandoOcorrencias)
                return;

            if (Util.TemAcessoInternet)
            {
                OffLine.Instancia.SincronizandoOcorrencias = true;
                List<OCORRENCIA> lstDados = await App.Banco.BuscarOcorrenciasAsync();
                lstDados = lstDados.OrderBy(o => o.DATA).ToList();

                string url = string.Concat(App.__EnderecoWebApi, "/Ocorrencias/Inserir");
                int countSucesso = 0;
                int countErro = 0;

                foreach (OCORRENCIA o in lstDados)
                {
                    try
                    {
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                        parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

                        parametros.Add("DATA", o.DATA.ToString("yyyyMMdd HHmmss"));
                        parametros.Add("ID_REGIONAL", o.ID_REGIONAL.ToString());
                        parametros.Add("ID_GERENCIA", o.ID_GERENCIA.ToString());
                        parametros.Add("ID_AREA", o.ID_AREA.ToString());
                        parametros.Add("ID_LOCAL", o.ID_LOCAL.ToString());
                        parametros.Add("DESCRICAO", o.DESCRICAO);
                        parametros.Add("ID_TIPO", o.ID_TIPO.ToString());
                        parametros.Add("ID_CLASSIFICACAO", o.ID_CLASSIFICACAO.ToString());
                        parametros.Add("ID_SUBCLASSIFICACAO", o.ID_SUBCLASSIFICACAO.ToString());
                        if (o.ID_CATEGORIA.HasValue)
                            parametros.Add("ID_CATEGORIA", o.ID_CATEGORIA.ToString());
                        if (o.ID_FORNECEDOR.HasValue)
                            parametros.Add("ID_FORNECEDOR", o.ID_FORNECEDOR.ToString());
                        if (o.ST_ACAOIMEDIATA.HasValue)
                        {
                            parametros.Add("ACOES_IMEDIATAS", o.ST_ACAOIMEDIATA.Value.ToIdSimNao());
                            if (o.ST_ACAOIMEDIATA.Value)
                                parametros.Add("DS_ACOES_IMEDIATAS", o.DS_ACAOIMEDIATA);
                        }
                        parametros.Add("NAO_QUERO_ME_IDENTIFICAR", o.ST_NAOQUEROIDENTIFICAR.ToNumber());
                        if (!o.ST_NAOQUEROIDENTIFICAR)
                        {
                            if (o.ID_COMUNICADOPOR != null)
                                parametros.Add("ID_COMUNICADO_POR", o.ID_COMUNICADOPOR.ToString());
                            if (o.ID_REGISTRADOPOR != null)
                                parametros.Add("ID_REGISTRADO_POR", o.ID_REGISTRADOPOR.ToString());
                        }

                        FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                        using (HttpClient requisicao = new HttpClient())
                        {
                            HttpResponseMessage resposta = await requisicao.PostAsync(url, param);
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            conteudo = conteudo.Replace("\"", string.Empty);

                            if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                if (conteudo.Contains("|"))
                                {
                                    string[] valores = conteudo.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (valores.Length == 2)
                                    {
                                        string id = valores[0];
                                        ViewModelRelato vm = new ViewModelRelato(null, null);
                                        List<IMAGEM_RELATO> imagensDoBanco = await App.Banco.BuscarImagensAsync(o.ID_OCORRENCIA);
                                        foreach (IMAGEM_RELATO img in imagensDoBanco)
                                            vm.Imagens.Add(new ItemImagem() { IdImagem = img.ID_IMAGEM, Caminho = img.CAMINHO, Image = img.BYTES_IMAGEM, Data = img.DATA });
                                        RetornoRequest respostaImagem = await vm.SalvarImagensApi(id);
                                        if (respostaImagem.Ok)
                                            foreach (IMAGEM_RELATO img in imagensDoBanco)
                                                await App.Banco.ApagarAsync(img);
                                    }
                                }

                                await App.Banco.ApagarAsync(o);
                                countSucesso++;
                            }
                            else
                            {
                                o.DS_SYNC = conteudo.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                await App.Banco.InserirOuAlterarAsync(o);
                                countErro++;
                            }
                        }
                        App.Log("Enviada Ocorrência: " + o.ID_OCORRENCIA);
                    }
                    catch (Exception ex)
                    {
                        App.Log("Erro ao enviar Ocorrência: " + ex.Message);
                        countErro++;
                    }
                }

                if (countSucesso > 0 || countErro > 0)
                {
                    string msg = string.Empty;
                    if (countSucesso == 1)
                        msg = "1 ocorrência cadastrada enquanto o dispositivo estava sem conexão foi sincronizada com sucesso!";
                    else if (countSucesso > 1)
                        msg = $"{countSucesso} ocorrências cadastradas enquanto o dispositivo estava sem conexão foram sincronizadas com sucesso!";

                    if (countErro == 1)
                        msg = string.Concat("Erro ao tentar sincronizar 1 ocorrência que foi cadastrada enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);
                    else if (countErro > 1)
                        msg = string.Concat($"Erro ao tentar sincronizar {countErro} ocorrências que foram cadastradas enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);

                    App.Notificar("SDMobile - DNA", msg);
                }

                OffLine.Instancia.SincronizandoOcorrencias = false;
                System.ComponentModel.ProgressChangedEventArgs prog = new ProgressChangedEventArgs(countSucesso + countErro, Textos.Instancia.Ocorrencias);
                this.RegistrosSincronizados?.Invoke(null, prog);
            }

            this.Executando = false;
            App.Log("Final OffLine.SincronizarOcorrencias");
        }

        #endregion Métodos Ocorrencias


        #region Métodos Abordagens

        public Task SincronizarAbordagensAsync()
        {
            return Task.Run(() => { this.SincronizarAbordagens(); });
        }

        public async void SincronizarAbordagens()
        {
            App.Log("Início OffLine.SincronizarAbordagens");

            if (OffLine.Instancia.SincronizandoAbordagens)
                return;

            if (Util.TemAcessoInternet)
            {
                OffLine.Instancia.SincronizandoAbordagens = true;
                List<ABORDAGEM_COMPORTAMENTAL> lstDados = await App.Banco.BuscarAbordagensAsync();
                lstDados = lstDados.OrderBy(o => o.DATA).ToList();

                string url = string.Concat(App.__EnderecoWebApi, "/Abordagem/Inserir");
                int countSucesso = 0;
                int countErro = 0;

                foreach (ABORDAGEM_COMPORTAMENTAL a in lstDados)
                {
                    try
                    {
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                        parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

                        parametros.Add("DATA", a.DATA.ToString("yyyyMMdd HHmmss"));

                        parametros.Add("ID_FORNECEDOR", a.ID_FORNECEDOR.ToString());
                        parametros.Add("ID_REGIONAL", a.ID_REGIONAL.ToString());
                        parametros.Add("ID_GERENCIA", a.ID_GERENCIA.ToString());
                        parametros.Add("ID_AREA", a.ID_AREA.ToString());
                        parametros.Add("ID_LOCAL", a.ID_LOCAL.ToString());
                        parametros.Add("DESCRICAO", a.DESCRICAO);

                        //A - Equipamento de Proteção Individual e Coletivo
                        parametros.Add("ID_INERENTES_ATIVIDADE", a.ID_INERENTES_ATIVIDADE.ToString());
                        parametros.Add("ID_RELACAO_COM_RISCOS", a.ID_RELACAO_COM_RISCOS.ToString());
                        parametros.Add("ID_CONSERVACAO_ADEQUADA", a.ID_CONSERVACAO_ADEQUADA.ToString());
                        parametros.Add("ID_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA", a.ID_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA.ToString());

                        parametros.Add("ST_INERENTES_ATIVIDADE_VER_AGIR", a.ST_INERENTES_ATIVIDADE_VER_AGIR.ToNumber());
                        parametros.Add("ST_RELACAO_COM_RISCOS_VER_AGIR", a.ST_RELACAO_COM_RISCOS_VER_AGIR.ToNumber());
                        parametros.Add("ST_CONSERVACAO_ADEQUADA_VER_AGIR", a.ST_CONSERVACAO_ADEQUADA_VER_AGIR.ToNumber());
                        parametros.Add("ST_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA_VER_AGIR", a.ST_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA_VER_AGIR.ToNumber());

                        //B - Máquinas, Veículos, Equipamentos e Ferramentas
                        parametros.Add("ID_IDENTIFICACAO_RISCOS_MAPEADOS", a.ID_IDENTIFICACAO_RISCOS_MAPEADOS.ToString());
                        parametros.Add("ID_MEDIDAS_PREVENCAO", a.ID_MEDIDAS_PREVENCAO.ToString());
                        parametros.Add("ID_BOAS_CONDICOES_USO", a.ID_BOAS_CONDICOES_USO.ToString());
                        parametros.Add("ID_UTILIZACAO_CORRETA", a.ID_UTILIZACAO_CORRETA.ToString());
                        parametros.Add("ID_DESTINADOS_ATIVIDADE", a.ID_DESTINADOS_ATIVIDADE.ToString());

                        parametros.Add("ST_IDENTIFICACAO_RISCOS_MAPEADOS_VER_AGIR", a.ST_IDENTIFICACAO_RISCOS_MAPEADOS_VER_AGIR.ToNumber());
                        parametros.Add("ST_MEDIDAS_PREVENCAO_VER_AGIR", a.ST_MEDIDAS_PREVENCAO_VER_AGIR.ToNumber());
                        parametros.Add("ST_BOAS_CONDICOES_USO_VER_AGIR", a.ST_BOAS_CONDICOES_USO_VER_AGIR.ToNumber());
                        parametros.Add("ST_UTILIZACAO_CORRETA_VER_AGIR", a.ST_UTILIZACAO_CORRETA_VER_AGIR.ToNumber());
                        parametros.Add("ST_DESTINADOS_ATIVIDADE_VER_AGIR", a.ST_DESTINADOS_ATIVIDADE_VER_AGIR.ToNumber());

                        //C - Programa Bom Senso
                        parametros.Add("ID_LOCAL_LIMPO", a.ID_LOCAL_LIMPO.ToString());
                        parametros.Add("ID_MATERIAIS_ORGANIZADOS", a.ID_MATERIAIS_ORGANIZADOS.ToString());
                        parametros.Add("ID_DESCARTE_RESIDUOS", a.ID_DESCARTE_RESIDUOS.ToString());

                        parametros.Add("ST_LOCAL_LIMPO_VER_AGIR", a.ST_LOCAL_LIMPO_VER_AGIR.ToNumber());
                        parametros.Add("ST_MATERIAIS_ORGANIZADOS_VER_AGIR", a.ST_LOCAL_LIMPO_VER_AGIR.ToNumber());
                        parametros.Add("ST_DESCARTE_RESIDUOS_VER_AGIR", a.ST_LOCAL_LIMPO_VER_AGIR.ToNumber());

                        //D - Acidentes, Incidentes e Desvios
                        parametros.Add("ID_IDENTIFICACAO_TRATATIVA_RISCOS", a.ID_IDENTIFICACAO_TRATATIVA_RISCOS.ToString());
                        parametros.Add("ID_LINHA_FOGO", a.ID_LINHA_FOGO.ToString());
                        parametros.Add("ID_POSTURAS_ERGO_ADEQUADAS", a.ID_POSTURAS_ERGO_ADEQUADAS.ToString());
                        parametros.Add("ID_CONCENTRACAO_TAREFA", a.ID_CONCENTRACAO_TAREFA.ToString());

                        parametros.Add("ST_IDENTIFICACAO_TRATATIVA_RISCOS_VER_AGIR", a.ST_IDENTIFICACAO_TRATATIVA_RISCOS_VER_AGIR.ToNumber());
                        parametros.Add("ST_LINHA_FOGO_VER_AGIR", a.ST_LINHA_FOGO_VER_AGIR.ToNumber());
                        parametros.Add("ST_POSTURAS_ERGO_ADEQUADAS_VER_AGIR", a.ST_POSTURAS_ERGO_ADEQUADAS_VER_AGIR.ToNumber());
                        parametros.Add("ST_CONCENTRACAO_TAREFA_VER_AGIR", a.ST_CONCENTRACAO_TAREFA_VER_AGIR.ToNumber());

                        //E - Planejamento, Procedimento e Instrução
                        parametros.Add("ID_CONHECIMENTO_PROCEDIMENTOS", a.ID_CONHECIMENTO_PROCEDIMENTOS.ToString());
                        parametros.Add("ID_CONHECIMENTO_RISCOS", a.ID_CONHECIMENTO_RISCOS.ToString());
                        parametros.Add("ID_DIREITO_RECUSA", a.ID_DIREITO_RECUSA.ToString());
                        parametros.Add("ID_ACOES_EMERGENCIA", a.ID_ACOES_EMERGENCIA.ToString());
                        parametros.Add("ID_REALIZA_PROCESSOROTINA", a.ID_REALIZA_PROCESSOROTINA.ToString());

                        parametros.Add("ST_CONHECIMENTO_PROCEDIMENTOS_VER_AGIR", a.ST_CONHECIMENTO_PROCEDIMENTOS_VER_AGIR.ToNumber());
                        parametros.Add("ST_CONHECIMENTO_RISCOS_VER_AGIR", a.ST_CONHECIMENTO_RISCOS_VER_AGIR.ToNumber());
                        parametros.Add("ST_DIREITO_RECUSA_VER_AGIR", a.ST_DIREITO_RECUSA_VER_AGIR.ToNumber());
                        parametros.Add("ST_ACOES_EMERGENCIA_VER_AGIR", a.ST_ACOES_EMERGENCIA_VER_AGIR.ToNumber());
                        parametros.Add("ST_REALIZA_PROCESSOROTINA_VER_AGIR", a.ST_REALIZA_PROCESSOROTINA_VER_AGIR.ToNumber());

                        //Ativadores de Comportamento
                        parametros.Add("COGNITIVOS", a.COGNITIVOS);
                        parametros.Add("FISIOLOGICOS", a.FISIOLOGICOS);
                        parametros.Add("PSICOLOGICOS", a.PSICOLOGICOS);
                        parametros.Add("SOCIAIS", a.SOCIAIS);

                        parametros.Add("ID_OBSERVADOR", a.ID_OBSERVADOR.ToString());
                        parametros.Add("ID_REGISTRADOPOR", a.ID_REGISTRADOPOR.ToString());

                        FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                        using (HttpClient requisicao = new HttpClient())
                        {
                            HttpResponseMessage resposta = await requisicao.PostAsync(url, param);
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            conteudo = conteudo.Replace("\"", string.Empty);

                            if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                await App.Banco.ApagarAsync(a);
                                countSucesso++;
                            }
                            else
                            {
                                a.DS_SYNC = conteudo.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                await App.Banco.InserirOuAlterarAsync(a);
                                countErro++;
                            }
                        }
                        App.Log("Enviada Abordagem: " + a.ID_ABORDAGEM);
                    }
                    catch (Exception ex)
                    {
                        App.Log("Erro ao enviar Abordagem: " + ex.Message);
                        countErro++;
                    }
                }

                if (countSucesso > 0 || countErro > 0)
                {
                    string msg = string.Empty;
                    if (countSucesso == 1)
                        msg = "1 abordagem comportamental cadastrada enquanto o dispositivo estava sem conexão foi sincronizada com sucesso!";
                    else if (countSucesso > 1)
                        msg = $"{countSucesso} abordagens comportamentais enquanto o dispositivo estava sem conexão foram sincronizadas com sucesso!";

                    if (countErro == 1)
                        msg = string.Concat("Erro ao tentar sincronizar 1 abordagem comportamental que foi cadastrada enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);
                    else if (countErro > 1)
                        msg = string.Concat($"Erro ao tentar sincronizar {countErro} abordagens comportamentais que foram cadastradas enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);

                    App.Notificar("SDMobile - ORT", msg);
                }

                OffLine.Instancia.SincronizandoAbordagens = false;
                System.ComponentModel.ProgressChangedEventArgs prog = new ProgressChangedEventArgs(countSucesso + countErro, Textos.Instancia.Abordagens);
                this.RegistrosSincronizados?.Invoke(null, prog);
            }

            this.Executando = false;
            App.Log("Final OffLine.SincronizarAbordagens");
        }

        #endregion Métodos Abordagens


        #region Métodos Sna

        public Task SincronizarSNAsAsync()
        {
            return Task.Run(() => { this.SincronizarSNAs(); });
        }

        public async void SincronizarSNAs()
        {
            App.Log("Início OffLine.SincronizarSNAs");

            if (OffLine.Instancia.SincronizandoSnas)
                return;

            if (Util.TemAcessoInternet)
            {
                OffLine.Instancia.SincronizandoSnas = true;
                List<SNA> lstDados = await App.Banco.BuscarSNAsAsync();
                lstDados = lstDados.OrderBy(o => o.DT_DATA).ToList();

                string url = string.Concat(App.__EnderecoWebApi, "/SegurancaNaArea/Inserir");
                int countSucesso = 0;
                int countErro = 0;

                foreach (SNA s in lstDados)
                {
                    try
                    {
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                        parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

                        parametros.Add("DT_DATA", s.DT_DATA.ToString("yyyyMMdd HHmmss"));

                        if (s.DT_HORARIO_INICIAL.HasValue)
                            parametros.Add("DT_HORARIO_INICIAL", s.DT_HORARIO_INICIAL.Value.ToString("yyyyMMdd HHmmss"));

                        parametros.Add("ID_REGIONAL", s.ID_REGIONAL.ToString());
                        parametros.Add("ID_GERENCIA", s.ID_GERENCIA.ToString());
                        parametros.Add("ID_AREA", s.ID_AREA.ToString());
                        parametros.Add("ID_LOCAL", s.ID_LOCAL.ToString());

                        parametros.Add("DS_TEMA_ABORDADO", s.DS_TEMA_ABORDADO);

                        parametros.Add("ID_CE", Util.GetIdRuimRegularBom(s.ST_CE_BOM, s.ST_CE_REGULAR, s.ST_CE_RUIM));
                        parametros.Add("DS_CE_AVALIACAODESCRITIVA", s.DS_CE_AVALIACAODESCRITIVA);
                        parametros.Add("NU_DNA_CE", s.NU_DNA_CE);

                        parametros.Add("ID_CA", Util.GetIdRuimRegularBom(s.ST_CA_BOM, s.ST_CA_REGULAR, s.ST_CA_RUIM));
                        parametros.Add("DS_CA_AVALIACAODESCRITIVA", s.DS_CA_AVALIACAODESCRITIVA);
                        parametros.Add("NU_DNA_CA", s.NU_DNA_CA);

                        parametros.Add("ID_RAF", Util.GetIdRuimRegularBom(s.ST_RAF_BOM, s.ST_RAF_REGULAR, s.ST_RAF_RUIM));
                        parametros.Add("DS_RAF_AVALIACAODESCRITIVA", s.DS_RAF_AVALIACAODESCRITIVA);
                        parametros.Add("NU_DNA_RAF", s.NU_DNA_RAF);

                        parametros.Add("ID_QAT", Util.GetIdRuimRegularBom(s.ST_QAT_BOM, s.ST_QAT_REGULAR, s.ST_QAT_RUIM));
                        parametros.Add("DS_QAT_AVALIACAODESCRITIVA", s.DS_QAT_AVALIACAODESCRITIVA);
                        parametros.Add("NU_DNA_QAT", s.NU_DNA_QAT);

                        parametros.Add("DS_PONTOS_POSITIVOS", s.DS_PONTOS_POSITIVOS);
                        parametros.Add("ID_REGISTRADOPOR", s.ID_REGISTRADOPOR.IdStrNullSafe());

                        FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                        using (HttpClient requisicao = new HttpClient())
                        {
                            HttpResponseMessage resposta = await requisicao.PostAsync(url, param);
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            conteudo = conteudo.Replace("\"", string.Empty);

                            if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                await App.Banco.ApagarAsync(s);
                                countSucesso++;
                            }
                            else
                            {
                                s.DS_SYNC = conteudo.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                await App.Banco.InserirOuAlterarAsync(s);
                                countErro++;
                            }
                        }
                        App.Log("Enviado SNA: " + s.ID_SNA);
                    }
                    catch (Exception ex)
                    {
                        App.Log("Erro ao enviar SNA: " + ex.Message);
                        countErro++;
                    }
                }

                if (countSucesso > 0 || countErro > 0)
                {
                    string msg = string.Empty;
                    if (countSucesso == 1)
                        msg = "1 SNA cadastrada enquanto o dispositivo estava sem conexão foi sincronizada com sucesso!";
                    else if (countSucesso > 1)
                        msg = $"{countSucesso} SNAs enquanto o dispositivo estava sem conexão foram sincronizadas com sucesso!";

                    if (countErro == 1)
                        msg = string.Concat("Erro ao tentar sincronizar 1 SNA que foi cadastrada enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);
                    else if (countErro > 1)
                        msg = string.Concat($"Erro ao tentar sincronizar {countErro} SNAs que foram cadastradas enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);

                    App.Notificar("SDMobile - SNA", msg);
                }

                OffLine.Instancia.SincronizandoSnas = false;
                System.ComponentModel.ProgressChangedEventArgs prog = new ProgressChangedEventArgs(countSucesso + countErro, Textos.Instancia.SNAs);
                this.RegistrosSincronizados?.Invoke(null, prog);
            }

            this.Executando = false;
            App.Log("Final OffLine.SincronizarSNAs");
        }

        #endregion Métodos Sna


        #region Métodos Inspeção

        public Task SincronizarInspecoesAsync()
        {
            return Task.Run(() => { this.SincronizarInpecoes(); });
        }

        public async void SincronizarInpecoes()
        {
            App.Log("Início OffLine.SincronizarInpecoes");

            if (OffLine.Instancia.SincronizandoInspecoes)
                return;

            if (Util.TemAcessoInternet)
            {
                OffLine.Instancia.SincronizandoInspecoes = true;
                List<INSPECAO> lstDados = await App.Banco.BuscarInspecoesAsync();
                lstDados = lstDados.OrderBy(o => o.DT_DATA).ToList();

                string url = string.Concat(App.__EnderecoWebApi, "/Inspecao/Inserir");
                int countSucesso = 0;
                int countErro = 0;

                foreach (INSPECAO i in lstDados)
                {
                    try
                    {
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                        parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

                        if (!string.IsNullOrEmpty(i.NU_INSPECAO))
                            parametros.Add("NU_INSPECAO", i.NU_INSPECAO);

                        parametros.Add("DT_DATA", i.DT_DATA.ToString("yyyyMMdd HHmmss"));

                        parametros.Add("ID_REGIONAL", i.ID_REGIONAL.ToString());
                        parametros.Add("ID_GERENCIA", i.ID_GERENCIA.ToString());
                        parametros.Add("ID_AREA", i.ID_AREA.ToString());
                        parametros.Add("ID_LOCAL", i.ID_LOCAL.ToString());
                        parametros.Add("ID_FORNECEDOR", i.ID_FORNECEDOR.ToString());

                        if (i.ID_TIPO.HasValue)
                            parametros.Add("ID_TIPO", i.ID_TIPO.Value.ToString());

                        if (i.ID_ATIVIDADE.HasValue)
                            parametros.Add("ID_ATIVIDADE", i.ID_ATIVIDADE.Value.ToString());

                        if (i.ID_REALIZADOPOR.HasValue)
                            parametros.Add("ID_REALIZADOPOR", i.ID_REALIZADOPOR.Value.ToString());

                        if (!string.IsNullOrEmpty(i.PARTICIPANTES))
                        {
                            List<string> lst = i.PARTICIPANTES.Split(';').ToList();
                            string participantes = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                            parametros.Add("PARTICIPANTES", participantes);
                        }

                        List<CampoInspecao> camposApi = new List<CampoInspecao>();

                        List<CAMPO_INSPECAO> camposSqLite = await App.Banco.BuscarCamposInspecaoAsync(i.ID_INSPECAO);
                        foreach (CAMPO_INSPECAO campoSqLite in camposSqLite)
                        {
                            CampoInspecao campoApi = new CampoInspecao();

                            campoApi.IdCampo = campoSqLite.ID_CAMPO;
                            campoApi.Descricao = campoSqLite.DS_SITUACAO;
                            campoApi.NumeroDNA = campoSqLite.NU_DNA;

                            if (campoSqLite.ID_CONFORME.HasValue)
                                campoApi.IdConforme = campoSqLite.ID_CONFORME.Value.ToString();

                            if (!string.IsNullOrEmpty(campoApi.Descricao) || !string.IsNullOrEmpty(campoApi.NumeroDNA) || !string.IsNullOrEmpty(campoApi.IdConforme))
                            {
                                if (!string.IsNullOrEmpty(campoSqLite.COLUNAS))
                                {
                                    Dictionary<string, string> colunas = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(campoSqLite.COLUNAS);
                                    campoApi.Colunas = colunas;
                                }
                            }

                            camposApi.Add(campoApi);
                        }

                        parametros.Add("CAMPOS", Newtonsoft.Json.JsonConvert.SerializeObject(camposApi));

                        FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                        using (HttpClient requisicao = new HttpClient())
                        {
                            HttpResponseMessage resposta = await requisicao.PostAsync(url, param);
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            conteudo = conteudo.Replace("\"", string.Empty);

                            if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string idApi = string.Empty;
                                if (conteudo.Contains("|"))
                                {
                                    string[] valores = conteudo.Split(new string[] { "|" }, StringSplitOptions.None);
                                    if (valores.Length > 2)
                                    {
                                        idApi = valores[0];
                                        i.NU_INSPECAO = valores[1];
                                        for (int j = 2; j < valores.Length; j++)
                                            try { camposSqLite[j - 2].NU_DNA = valores[j]; } catch { }
                                    }
                                }

                                this.EnviarImagensInpecao(i, idApi, camposSqLite);
                                await App.Banco.ApagarAsync(i);
                                countSucesso++;
                            }
                            else
                            {
                                i.DS_SYNC = conteudo.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                await App.Banco.InserirOuAlterarAsync(i);
                                countErro++;
                            }
                        }
                        App.Log("Enviada Inspeção: " + i.ID_INSPECAO);
                    }
                    catch (Exception ex)
                    {
                        App.Log("Erro ao enviar Inspeção: " + ex.Message);
                        countErro++;
                    }
                }

                if (countSucesso > 0 || countErro > 0)
                {
                    string msg = string.Empty;
                    if (countSucesso == 1)
                        msg = "1 Inspeção de Segurança cadastrada enquanto o dispositivo estava sem conexão foi sincronizada com sucesso!";
                    else if (countSucesso > 1)
                        msg = $"{countSucesso} Inspeções de Segurança enquanto o dispositivo estava sem conexão foram sincronizadas com sucesso!";

                    if (countErro == 1)
                        msg = string.Concat("Erro ao tentar sincronizar 1 Inspeção de Segurança que foi cadastrada enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);
                    else if (countErro > 1)
                        msg = string.Concat($"Erro ao tentar sincronizar {countErro} Inspeções de Segurança que foram cadastradas enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);

                    App.Notificar("SDMobile - INSP", msg);
                }

                OffLine.Instancia.SincronizandoInspecoes = false;
                System.ComponentModel.ProgressChangedEventArgs prog = new ProgressChangedEventArgs(countSucesso + countErro, Textos.Instancia.Inspecoes);
                this.RegistrosSincronizados?.Invoke(null, prog);
            }

            this.Executando = false;
            App.Log("Final OffLine.SincronizarInpecoes");
        }

        private async void EnviarImagensInpecao(INSPECAO i, string idApi, List<CAMPO_INSPECAO> camposSqLite)
        {
            int count = 0;
            foreach (CAMPO_INSPECAO campoSqLite in camposSqLite)
            {
                if (campoSqLite.BYTES_IMAGEM != null)
                {
                    try
                    {
                        Dictionary<string, string> colunas = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(campoSqLite.COLUNAS);
                        string url = string.Concat(App.__EnderecoWebApi, "/Inspecao/UploadImagem");
                        using (HttpClient client = new HttpClient())
                        {
                            using (MultipartFormDataContent formData = new MultipartFormDataContent())
                            {
                                CancellationToken cancellationToken = CancellationToken.None;

                                FileInfo fi = new FileInfo(campoSqLite.CAMINHO);
                                string nmArquivo = string.Concat("SDST_Mobile_img_", count, "_", i.DT_DATA.ToAAAAMMDD_HHMINSS(), fi.Extension);

                                HttpContent imageContent = new StreamContent(new MemoryStream(campoSqLite.BYTES_IMAGEM));
                                ContentDispositionHeaderValue conteudo = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = nmArquivo };
                                imageContent.Headers.ContentDisposition = conteudo;
                                imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                formData.Add(imageContent);
                                client.DefaultRequestHeaders.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                                client.DefaultRequestHeaders.Add("ID_INSPECAO", idApi);
                                client.DefaultRequestHeaders.Add("NM_COLUNA", colunas["Multimidia"]);
                                try
                                {
                                    HttpResponseMessage resposta = await client.PostAsync(url, formData);
                                    string conteudoResposta = await resposta.Content.ReadAsStringAsync();
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                count++;
            }
        }

        #endregion Métodos Inspeção


        #region Métodos Ocorrencias Csn

        public Task SincronizarOcorrenciasCsnAsync()
        {
            return Task.Run(() => { this.SincronizarOcorrenciasCsn(); });
        }

        public async void SincronizarOcorrenciasCsn()
        {
            App.Log("Início OffLine.SincronizarOcorrenciasCsn");

            if (OffLine.Instancia.SincronizandoOcorrencias)
                return;

            if (Util.TemAcessoInternet)
            {
                OffLine.Instancia.SincronizandoOcorrencias = true;
                List<OCORRENCIACSN> lstDados = await App.Banco.BuscarOcorrenciasCsnAsync();
                lstDados = lstDados.OrderBy(o => o.DATA).ToList();

                string url = string.Concat(App.__EnderecoWebApi, "/OcorrenciasCsn/Inserir");
                int countSucesso = 0;
                int countErro = 0;

                foreach (OCORRENCIACSN o in lstDados)
                {
                    try
                    {
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                        parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

                        parametros.Add("DATA", o.DATA.ToString("yyyyMMdd HHmmss"));
                        parametros.Add("TITULO", o.TITULO);
                        ModeloObj dia = Util.LstDiaSemana.FirstOrDefault(d => d.Codigo == ((int)o.DATA.DayOfWeek + 1).ToString());
                        if (dia != null)
                            parametros.Add("ID_DIA", dia.IdStrNullSafe());

                        parametros.Add("ID_REGIAO", o.REGIAOSETOR.IdStrNullSafe());
                        parametros.Add("ID_LETRA", o.LETRA.IdStrNullSafe());
                        parametros.Add("ID_TURNO", o.TURNO.IdStrNullSafe());

                        parametros.Add("ID_GERENCIA_GERAL", o.GERENCIAGERAL.IdStrNullSafe());
                        parametros.Add("ID_GERENCIA", o.GERENCIA.IdStrNullSafe());
                        parametros.Add("ID_AREA_EQUIPAMENTO", o.AREAEQUIPAMENTO.IdStrNullSafe());
                        parametros.Add("ID_LOCAL_EQUIPAMENTO", o.LOCALEQUIPAMENTO.IdStrNullSafe());

                        parametros.Add("LOCAL_ANOMALIA", o.LOCALANOMALIA);
                        parametros.Add("DESCRICAO_PRELIMINAR", o.DESCRICAOPRELIMINAR);

                        if (o.ST_ACAOIMEDIATA.HasValue)
                            parametros.Add("ACOES_IMEDIATAS", o.ST_ACAOIMEDIATA.Value.ToIdSimNao());

                        parametros.Add("REMOCAO_SINTOMAS", o.REMOCAOSINTOMAS);
                        parametros.Add("ID_ORIGEM", o.ORIGEMANOMALIA.IdStrNullSafe());
                        parametros.Add("ID_TIPO_ANOMALIA", o.TIPOANOMALIA.IdStrNullSafe());
                        parametros.Add("ID_CLASSIFICACAO_TIPO", o.CLASSIFICACAOTIPO.IdStrNullSafe());
                        parametros.Add("ID_PROBABILIDADE", o.PROBABILIDADE.IdStrNullSafe());
                        parametros.Add("ID_SEVERIDADE", o.SEVERIDADE.IdStrNullSafe());

                        int ab = 0;
                        if (o.PROBABILIDADE.HasValue && o.SEVERIDADE.HasValue)
                        {
                            PROBABILIDADE prob = await App.Banco.BuscarProbabilidadeAsync(o.PROBABILIDADE.Value);
                            SEVERIDADE severidade = await App.Banco.BuscarSeveridadeAsync(o.SEVERIDADE.Value);
                            if (prob != null && severidade != null)
                            {
                                int p = Convert.ToInt32(prob.CD_PROBABILIDADE);
                                int s = Convert.ToInt32(severidade.CD_SEVERIDADE);
                                ab = Util.CalcularProdutoAB(p, s);
                            }
                        }
                        if (ab > 0)
                        {
                            parametros.Add("PRODUTOAB", ab.ToString());
                            ModeloObj m = Util.ResultadoRisco(ab);
                            parametros.Add("ID_RESULTADO", m.IdStrNullSafe());
                        }

                        parametros.Add("ID_REGISTRADO_POR", o.REGISTRADOPOR.IdStrNullSafe());
                        parametros.Add("ID_RELATADO_POR", o.RELATADOPOR.IdStrNullSafe());
                        parametros.Add("ID_SUPERVISOR", o.SUPERVISOR.IdStrNullSafe());
                        parametros.Add("ID_ASSINATURA", o.ASSINATURA.IdStrNullSafe());

                        FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                        using (HttpClient requisicao = new HttpClient())
                        {
                            HttpResponseMessage resposta = await requisicao.PostAsync(url, param);
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            conteudo = conteudo.Replace("\"", string.Empty);

                            if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                await App.Banco.ApagarAsync(o);
                                countSucesso++;
                            }
                            else
                            {
                                o.DS_SYNC = conteudo.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                await App.Banco.InserirOuAlterarAsync(o);
                                countErro++;
                            }
                        }
                        App.Log("Enviada Ocorrência: " + o.ID_OCORRENCIA);
                    }
                    catch (Exception ex)
                    {
                        App.Log("Erro ao enviar Ocorrência: " + ex.Message);
                        countErro++;
                    }
                }

                if (countSucesso > 0 || countErro > 0)
                {
                    string msg = string.Empty;
                    if (countSucesso == 1)
                        msg = "1 ocorrência cadastrada enquanto o dispositivo estava sem conexão foi sincronizada com sucesso!";
                    else if (countSucesso > 1)
                        msg = $"{countSucesso} ocorrências cadastradas enquanto o dispositivo estava sem conexão foram sincronizadas com sucesso!";

                    if (countErro == 1)
                        msg = string.Concat("Erro ao tentar sincronizar 1 ocorrência que foi cadastrada enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);
                    else if (countErro > 1)
                        msg = string.Concat($"Erro ao tentar sincronizar {countErro} ocorrências que foram cadastradas enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);

                    App.Notificar("SDMobile - Relato de Anomalia", msg);
                }

                OffLine.Instancia.SincronizandoOcorrencias = false;
                System.ComponentModel.ProgressChangedEventArgs prog = new ProgressChangedEventArgs(countSucesso + countErro, Textos.Instancia.Ocorrencias);
                this.RegistrosSincronizados?.Invoke(null, prog);
            }

            this.Executando = false;
            App.Log("Final OffLine.SincronizarOcorrenciasCsn");
        }

        #endregion Métodos Ocorrencias Csn


        #region Métodos Opa

        public Task SincronizarOpasAsync()
        {
            return Task.Run(() => { this.SincronizarOpas(); });
        }

        public async void SincronizarOpas()
        {
            App.Log("Início OffLine.SincronizarOpas");

            if (OffLine.Instancia.SincronizandoOpas)
                return;

            if (Util.TemAcessoInternet)
            {
                OffLine.Instancia.SincronizandoInspecoes = true;
                List<OPA> lstDados = await App.Banco.BuscarOpasAsync();
                lstDados = lstDados.OrderBy(o => o.DT_OPA).ToList();

                string url = string.Concat(App.__EnderecoWebApi, "/Opa/Inserir");
                int countSucesso = 0;
                int countErro = 0;

                foreach (OPA o in lstDados)
                {
                    try
                    {
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        parametros.Add("idChaveSessao", UsuarioLogado.Instancia.ID_CHAVE_SESSAO);
                        parametros.Add("login", UsuarioLogado.Instancia.NM_APELIDO);

                        if (!string.IsNullOrEmpty(o.NU_OPA))
                            parametros.Add("NU_OPA", o.NU_OPA);

                        parametros.Add("DT_DATA", o.DT_OPA.ToString("yyyyMMdd HHmmss"));

                        parametros.Add("ID_REGIONAL", o.ID_REGIONAL.ToString());
                        parametros.Add("ID_GERENCIA", o.ID_GERENCIA.ToString());
                        parametros.Add("ID_AREA", o.ID_AREA.ToString());
                        parametros.Add("ID_LOCAL", o.ID_LOCAL.ToString());

                        if (o.ID_ATIVIDADE.HasValue)
                            parametros.Add("ID_ATIVIDADE", o.ID_ATIVIDADE.Value.ToString());

                        if (o.ID_TAREFA.HasValue)
                            parametros.Add("ID_TAREFA", o.ID_TAREFA.Value.ToString());

                        if (o.ID_AVALIADOR.HasValue)
                            parametros.Add("ID_AVALIADOR", o.ID_AVALIADOR.Value.ToString());

                        if (o.ID_TIPO_AVALIADOR.HasValue)
                            parametros.Add("ID_TIPO_AVALIADOR", o.ID_TIPO_AVALIADOR.Value.ToString());


                        List<CampoOpa> camposApi = new List<CampoOpa>();

                        List<CAMPO_OPA> camposSqLite = await App.Banco.BuscarCamposOpaAsync(o.ID_OPA);
                        foreach (CAMPO_OPA campoSqLite in camposSqLite)
                        {
                            CampoOpa campoApi = new CampoOpa();

                            campoApi.IdCampo = campoSqLite.ID_CAMPO;
                            campoApi.Comentario = campoSqLite.DS_COMENTARIO;
                            campoApi.NumeroDNA = campoSqLite.NU_DNA;

                            if (campoSqLite.ID_CONFORME.HasValue)
                                campoApi.IdConforme = campoSqLite.ID_CONFORME.Value.ToString();

                            if (!string.IsNullOrEmpty(campoApi.Comentario) || !string.IsNullOrEmpty(campoApi.NumeroDNA) || !string.IsNullOrEmpty(campoApi.IdConforme))
                            {
                                if (!string.IsNullOrEmpty(campoSqLite.COLUNAS))
                                {
                                    Dictionary<string, string> colunas = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(campoSqLite.COLUNAS);
                                    campoApi.Colunas = colunas;
                                }
                            }

                            camposApi.Add(campoApi);
                        }

                        parametros.Add("CAMPOS", Newtonsoft.Json.JsonConvert.SerializeObject(camposApi));

                        FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

                        using (HttpClient requisicao = new HttpClient())
                        {
                            HttpResponseMessage resposta = await requisicao.PostAsync(url, param);
                            string conteudo = await resposta.Content.ReadAsStringAsync();
                            conteudo = conteudo.Replace("\"", string.Empty);

                            if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string idApi = string.Empty;
                                if (conteudo.Contains("|"))
                                {
                                    string[] valores = conteudo.Split(new string[] { "|" }, StringSplitOptions.None);
                                    if (valores.Length > 2)
                                    {
                                        idApi = valores[0];
                                        o.NU_OPA = valores[1];
                                        for (int j = 2; j < valores.Length; j++)
                                            try { camposSqLite[j - 2].NU_DNA = valores[j]; } catch { }
                                    }
                                }

                                await App.Banco.ApagarAsync(o);
                                countSucesso++;
                            }
                            else
                            {
                                o.DS_SYNC = conteudo.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                await App.Banco.InserirOuAlterarAsync(o);
                                countErro++;
                            }
                        }
                        App.Log("Enviada Opa: " + o.ID_OPA);
                    }
                    catch (Exception ex)
                    {
                        App.Log("Erro ao enviar Opa: " + ex.Message);
                        countErro++;
                    }
                }

                if (countSucesso > 0 || countErro > 0)
                {
                    string msg = string.Empty;
                    if (countSucesso == 1)
                        msg = "1 Observação Positiva Florestal cadastrada enquanto o dispositivo estava sem conexão foi sincronizada com sucesso!";
                    else if (countSucesso > 1)
                        msg = $"{countSucesso} Obeservações Positivas Florestais cadastradas enquanto o dispositivo estava sem conexão foram sincronizadas com sucesso!";

                    if (countErro == 1)
                        msg = string.Concat("Erro ao tentar sincronizar 1 Observação Positiva Florestal que foi cadastrada enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);
                    else if (countErro > 1)
                        msg = string.Concat($"Erro ao tentar sincronizar {countErro} Obeservações Positivas Florestais que foram cadastradas enquanto o dispositivo estava sem conexão!", Environment.NewLine, msg);

                    App.Notificar("SDMobile - OPA", msg);
                }

                OffLine.Instancia.SincronizandoOpas = false;
                System.ComponentModel.ProgressChangedEventArgs prog = new ProgressChangedEventArgs(countSucesso + countErro, Textos.Instancia.Opas);
                this.RegistrosSincronizados?.Invoke(null, prog);
            }

            this.Executando = false;
            App.Log("Final OffLine.SincronizarOpas");
        }

        #endregion Métodos Opa
    }
}
