using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDMobileXF.Banco.Tabelas;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using SQLite;
using Xamarin.Forms;

namespace SDMobileXF.Banco
{
    public class SqliteSD : Sqlite
    {
        public SqliteSD(string caminho) : base(caminho)
        {
            this.GerarCodigo("TIPO_AVALIADOR", "TAREFA_OBSERVADA");

        }

        public void CriarTabelasSeNaoExistir()
        {
            try
            {
                this._conn.CreateTableAsync<Usuario>();
                this._conn.CreateIndexAsync<Usuario>(u => u.ID_USUARIO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            
            try 
            {
                this._conn.CreateTableAsync<REGIONAL>();
                this._conn.CreateIndexAsync<REGIONAL>(m => m.ID_REGIONAL, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<GERENCIA>();
                this._conn.CreateIndexAsync<GERENCIA>(m => m.ID_GERENCIA, true);
                this._conn.CreateIndexAsync<GERENCIA>(m => m.ID_REGIONAL);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<AREA>();
                this._conn.CreateIndexAsync<AREA>(m => m.ID_AREA, true);
                this._conn.CreateIndexAsync<AREA>(m => m.ID_GERENCIA);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<LOCAL>();
                this._conn.CreateIndexAsync<LOCAL>(m => m.ID_LOCAL, true);
                this._conn.CreateIndexAsync<LOCAL>(m => m.ID_AREA);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<TIPO>();
                this._conn.CreateIndexAsync<TIPO>(m => m.ID_TIPO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<CLASSIFICACAO>();
                this._conn.CreateIndexAsync<CLASSIFICACAO>(m => m.ID_CLASSIFICACAO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<SUBCLASSIFICACAO>();
                this._conn.CreateIndexAsync<SUBCLASSIFICACAO>(m => m.ID_SUBCLASSIFICACAO, true);
                this._conn.CreateIndexAsync<SUBCLASSIFICACAO>(m => m.ID_CLASSIFICACAO);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<CATEGORIA>();
                this._conn.CreateIndexAsync<CATEGORIA>(m => m.ID_CATEGORIA, true);
                this._conn.CreateIndexAsync<CATEGORIA>(m => m.ID_SUBCLASSIFICACAO);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<FORNECEDOR>();
                this._conn.CreateIndexAsync<FORNECEDOR>(m => m.ID_FORNECEDOR, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<INFO>();
                this._conn.CreateIndexAsync<INFO>(i => i.NM_TABELA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<VINCULO>();
                this._conn.CreateIndexAsync<VINCULO>(v => v.ID_VINCULO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<OCORRENCIA>();
                this._conn.CreateIndexAsync<OCORRENCIA>(o => o.ID_OCORRENCIA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<IMAGEM_RELATO>();
                this._conn.CreateIndexAsync<IMAGEM_RELATO>(i => i.ID_OCORRENCIA, false);
                this._conn.CreateIndexAsync<IMAGEM_RELATO>(i => i.ID_IMAGEM, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<SEGURO_INSEGURO>();
                this._conn.CreateIndexAsync<SEGURO_INSEGURO>(m => m.ID_SEGURO_INSEGURO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<COGNITIVO>();
                this._conn.CreateIndexAsync<COGNITIVO>(m => m.ID_COGNITIVO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<FISIOLOGICO>();
                this._conn.CreateIndexAsync<FISIOLOGICO>(m => m.ID_FISIOLOGICO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<PSICOLOGICO>();
                this._conn.CreateIndexAsync<PSICOLOGICO>(m => m.ID_PSICOLOGICO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<SOCIAL>();
                this._conn.CreateIndexAsync<SOCIAL>(m => m.ID_SOCIAL, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<ABORDAGEM_COMPORTAMENTAL>();
                this._conn.CreateIndexAsync<ABORDAGEM_COMPORTAMENTAL>(a => a.ID_ABORDAGEM, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<SNA>();
                this._conn.CreateIndexAsync<SNA>(a => a.ID_SNA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<TIPO_INSPECAO>();
                this._conn.CreateIndexAsync<TIPO_INSPECAO>(m => m.ID_TIPO_INSPECAO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<ATIVIDADE_INSPECAO>();
                this._conn.CreateIndexAsync<ATIVIDADE_INSPECAO>(m => m.ID_ATIVIDADE_INSPECAO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<INSPECAO>();
                this._conn.CreateIndexAsync<INSPECAO>(m => m.ID_INSPECAO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<CAMPO_INSPECAO>();
                this._conn.CreateIndexAsync<CAMPO_INSPECAO>(m => m.ID_INSPECAO, false);
                this._conn.CreateIndexAsync<CAMPO_INSPECAO>(m => m.ID_CAMPO, false);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<UNIDADE>();
                this._conn.CreateIndexAsync<UNIDADE>(m => m.ID_UNIDADE, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<LETRA>();
                this._conn.CreateIndexAsync<LETRA>(m => m.ID_LETRA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<TURNO>();
                this._conn.CreateIndexAsync<TURNO>(m => m.ID_TURNO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }


            try
            {
                this._conn.CreateTableAsync<GERENCIA_GERAL_CSN>();
                this._conn.CreateIndexAsync<GERENCIA_GERAL_CSN>(m => m.ID_GERENCIA_GERAL_CSN, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<GERENCIA_CSN>();
                this._conn.CreateIndexAsync<GERENCIA_CSN>(m => m.ID_GERENCIA_CSN, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<AREA_EQUIPAMENTO>();
                this._conn.CreateIndexAsync<AREA_EQUIPAMENTO>(m => m.ID_AREA_EQUIPAMENTO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<LOCAL_EQUIPAMENTO>();
                this._conn.CreateIndexAsync<LOCAL_EQUIPAMENTO>(m => m.ID_LOCAL_EQUIPAMENTO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<ORIGEM_ANOMALIA>();
                this._conn.CreateIndexAsync<ORIGEM_ANOMALIA>(m => m.ID_ORIGEM_ANOMALIA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<TIPO_ANOMALIA>();
                this._conn.CreateIndexAsync<TIPO_ANOMALIA>(m => m.ID_TIPO_ANOMALIA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<CLASSIFICACAO_TIPO>();
                this._conn.CreateIndexAsync<CLASSIFICACAO_TIPO>(m => m.ID_CLASSIFICACAO_TIPO, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<PROBABILIDADE>();
                this._conn.CreateIndexAsync<PROBABILIDADE>(m => m.ID_PROBABILIDADE, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<SEVERIDADE>();
                this._conn.CreateIndexAsync<SEVERIDADE>(m => m.ID_SEVERIDADE, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<OCORRENCIACSN>();
                this._conn.CreateIndexAsync<OCORRENCIACSN>(m => m.ID_OCORRENCIA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<OPA>();
                this._conn.CreateIndexAsync<OPA>(m => m.ID_OPA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<CAMPO_OPA>();
                this._conn.CreateIndexAsync<CAMPO_OPA>(m => m.ID_OPA, false);
                this._conn.CreateIndexAsync<CAMPO_OPA>(m => m.ID_CAMPO, false);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<TIPO_AVALIADOR>();
                this._conn.CreateIndexAsync<TIPO_AVALIADOR>(m => m.ID_TIPO_AVALIADOR, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            try
            {
                this._conn.CreateTableAsync<TAREFA_OBSERVADA>();
                this._conn.CreateIndexAsync<TAREFA_OBSERVADA>(m => m.ID_TAREFA_OBSERVADA, true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

        }

        private void GerarCodigo(params string[] tabelas)
        {
            string createTable = string.Empty;
            string operacoes = string.Empty;
            string buscarIds = string.Empty;
            string viewModelPesquisa = string.Empty;
            string adicionarNasListas = string.Empty;
            string modeloToSQLITE = string.Empty;

            foreach (string t in tabelas)
            {
                string n = string.Empty;
                for (int i = 0; i < t.Length; i++)
                {
                    char c = t[i];
                    if (i == 0)
                        n += c.ToString().ToUpper();
                    else if (c != '_')
                    {
                        if (t[i - 1] == '_')
                            n += c.ToString().ToUpper();
                        else
                            n += c.ToString().ToLower();
                    }
                }

                try
                {
                    createTable += @"
            try
            {
                this._conn.CreateTableAsync<" + t + @">();
                this._conn.CreateIndexAsync<" + t + @">(m => m.ID_" + t + @", true);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
";
                    operacoes += @"

        #region " + n + @"

        public Task<List<" + t + @">> Buscar" + n + @"sAsync() { return this._conn.Table<" + t + @">().ToListAsync(); }

        public Task<List<" + t + @">> Buscar" + n + @"sAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<" + t + @">().ToListAsync();
            else
                return this._conn.Table<" + t + @">().Where(t => t.CD_" + t + @".ToUpper().Contains(filtro.ToUpper()) || t.DS_" + t + @".ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<" + t + @"> Buscar" + n + @"Async(Guid id) { return this._conn.FindAsync<" + t + @">(id); }

        public Task<int> AlterarAsync(" + t + @" modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(" + t + @" modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(" + t + @" modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(" + t + @" modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion " + n;
                    buscarIds += @"
                else if (tabela == Enumerados.Tabela." + n + @")
                    sql = string.Format(sql, """ + t + @""");";
                    viewModelPesquisa += @"
else if (this.Tabela == Enumerados.Tabela." + n + @")
            {
                List<" + t + @"> aux = await App.Banco.Buscar" + n + @"Async(textoBusca);
                lst = aux.Select(i => i.ToModeloObj()).ToList();
            }";
                    adicionarNasListas += @"
else if (tabela == Enumerados.Tabela." + n + @") obj = m.To" + n + @"();";
                    modeloToSQLITE += @"
        public static " + t + @" To" + n + @"(this ModeloObj modelo) { return new " + t + @"(modelo); }";

                }
                catch (Exception ex)
                {

                }
            } 
        }

        public void DroparTabelas()
        {
            foreach (TableMapping map in this.Mapeamento)
            {
                try
                {
                    if (map.TableName != "USUARIO" &&
                        map.TableName != "OCORRENCIA" &&
                        map.TableName != "IMAGEM_RELATO" &&
                        map.TableName != "ABORDAGEM_COMPORTAMENTAL" &&
                        map.TableName != "INSPECAO" &&
                        map.TableName != "OCORRENCIACSN" &&
                        map.TableName != "SNA" &&
                        map.TableName != "OPA")
                    {
                        App.Log("Dropando " + map.TableName);
                        this._conn.DropTableAsync(map).Wait();
                    }
                }
                catch (Exception ex)
                {
                    App.Log("Dropando " + map.TableName + " - " + ex.Message);
                }
            }
        }

        public Task<HashSet<Guid>> BuscarIdsAsync(Enumerados.Tabela tabela)
        {
            HashSet<Guid> hashLst = new HashSet<Guid>();
            try
            {
                string sql = "SELECT ID_{0} FROM {0}";
                if (tabela == Enumerados.Tabela.UnidadeRegional)
                    sql = string.Format(sql, "REGIONAL");
                else if (tabela == Enumerados.Tabela.Gerencia)
                    sql = string.Format(sql, "GERENCIA");
                else if (tabela == Enumerados.Tabela.Area)
                    sql = string.Format(sql, "AREA");
                else if (tabela == Enumerados.Tabela.Local)
                    sql = string.Format(sql, "LOCAL");
                else if (tabela == Enumerados.Tabela.Tipo)
                    sql = string.Format(sql, "TIPO");
                else if (tabela == Enumerados.Tabela.Classificacao)
                    sql = string.Format(sql, "CLASSIFICACAO");
                else if (tabela == Enumerados.Tabela.SubClassificacao)
                    sql = string.Format(sql, "SUBCLASSIFICACAO");
                else if (tabela == Enumerados.Tabela.Categoria)
                    sql = string.Format(sql, "CATEGORIA");
                else if (tabela == Enumerados.Tabela.Fornecedor)
                    sql = string.Format(sql, "FORNECEDOR");
                else if (tabela == Enumerados.Tabela.Vinculo)
                    sql = string.Format(sql, "VINCULO");
                else if (tabela == Enumerados.Tabela.RespostasSeguroInseguroNa)
                    sql = string.Format(sql, "SEGURO_INSEGURO");
                else if (tabela == Enumerados.Tabela.AtivadoresComportamentoCOGNITIVOS)
                    sql = string.Format(sql, "COGNITIVO");
                else if (tabela == Enumerados.Tabela.AtivadoresComportamentoFISIOLOGICOS)
                    sql = string.Format(sql, "FISIOLOGICO");
                else if (tabela == Enumerados.Tabela.AtivadoresComportamentoPSICOLOGICOS)
                    sql = string.Format(sql, "PSICOLOGICO");
                else if (tabela == Enumerados.Tabela.AtivadoresComportamentoSOCIAIS)
                    sql = string.Format(sql, "SOCIAL");
                else if (tabela == Enumerados.Tabela.TipoInspecao)
                    sql = string.Format(sql, "TIPO_INSPECAO");
                else if (tabela == Enumerados.Tabela.AtividadeInspecao)
                    sql = string.Format(sql, "ATIVIDADE_INSPECAO");
                else if (tabela == Enumerados.Tabela.Unidade)
                    sql = string.Format(sql, "UNIDADE");
                else if (tabela == Enumerados.Tabela.Letra)
                    sql = string.Format(sql, "LETRA");
                else if (tabela == Enumerados.Tabela.TurnoAnomalia)
                    sql = string.Format(sql, "TURNO");
                else if (tabela == Enumerados.Tabela.GerenciaGeralCsn)
                    sql = string.Format(sql, "GERENCIA_GERAL_CSN");
                else if (tabela == Enumerados.Tabela.GerenciaCsn)
                    sql = string.Format(sql, "GERENCIA_CSN");
                else if (tabela == Enumerados.Tabela.AreaEquipamento)
                    sql = string.Format(sql, "AREA_EQUIPAMENTO");
                else if (tabela == Enumerados.Tabela.LocalEquipamento)
                    sql = string.Format(sql, "LOCAL_EQUIPAMENTO");
                else if (tabela == Enumerados.Tabela.OrigemAnomalia)
                    sql = string.Format(sql, "ORIGEM_ANOMALIA");
                else if (tabela == Enumerados.Tabela.TipoAnomalia)
                    sql = string.Format(sql, "TIPO_ANOMALIA");
                else if (tabela == Enumerados.Tabela.ClassificacaoTipo)
                    sql = string.Format(sql, "CLASSIFICACAO_TIPO");
                else if (tabela == Enumerados.Tabela.Probabilidade)
                    sql = string.Format(sql, "PROBABILIDADE");
                else if (tabela == Enumerados.Tabela.Severidade)
                    sql = string.Format(sql, "SEVERIDADE");
                else if (tabela == Enumerados.Tabela.TipoAvaliador)
                    sql = string.Format(sql, "TIPO_AVALIADOR");
                else if (tabela == Enumerados.Tabela.TarefaObservada)
                    sql = string.Format(sql, "TAREFA_OBSERVADA");
                else
                {

                }

                List<Guid> lst = this._conn.QueryScalarsAsync<Guid>(sql).Result;
                hashLst = new HashSet<Guid>(lst);
            }
            catch { }

            return Task.FromResult(hashLst);
        }

        public async Task<int> QuantidadeAsync(Enumerados.Tabela tabela)
        {
            int quant = -1;
            try
            {
                string sql = "SELECT Count(*) FROM {0}";
                if (tabela == Enumerados.Tabela.UnidadeRegional)
                    sql = string.Format(sql, "REGIONAL");
                else if (tabela == Enumerados.Tabela.Gerencia)
                    sql = string.Format(sql, "GERENCIA");
                else if (tabela == Enumerados.Tabela.Area)
                    sql = string.Format(sql, "AREA");
                else if (tabela == Enumerados.Tabela.Local)
                    sql = string.Format(sql, "LOCAL");
                else if (tabela == Enumerados.Tabela.Tipo)
                    sql = string.Format(sql, "TIPO");
                else if (tabela == Enumerados.Tabela.Classificacao)
                    sql = string.Format(sql, "CLASSIFICACAO");
                else if (tabela == Enumerados.Tabela.SubClassificacao)
                    sql = string.Format(sql, "SUBCLASSIFICACAO");
                else if (tabela == Enumerados.Tabela.Categoria)
                    sql = string.Format(sql, "CATEGORIA");
                else if (tabela == Enumerados.Tabela.Fornecedor)
                    sql = string.Format(sql, "FORNECEDOR");
                else if (tabela == Enumerados.Tabela.Vinculo)
                    sql = string.Format(sql, "VINCULO");

                quant = (await this._conn.QueryScalarsAsync<int>(sql)).FirstOrDefault();
            }
            catch { }

            return quant;
        }


        #region Usuario

        public Task<List<USUARIO>> BuscarUsuariosAsync() { return this._conn.Table<USUARIO>().ToListAsync(); }

        public Task<USUARIO> BuscarUsuarioAsync(Guid id) { return this._conn.FindAsync<USUARIO>(id); }

        public Task<List<USUARIO>> BuscarUsuariosPorLoginAsync(string login)
        {
            return this._conn.Table<USUARIO>().Where(u => u.NM_APELIDO.ToUpper().Contains(login.ToUpper())).ToListAsync();
        }

        public Task<int> AlterarAsync(SDMobileXFDados.Modelos.Usuario modelo) { return this._conn.UpdateAsync(new USUARIO(modelo)); }

        public Task<int> InserirAsync(SDMobileXFDados.Modelos.Usuario modelo) { return this._conn.InsertAsync(new USUARIO(modelo)); }

        public Task<int> ApagarAsync(SDMobileXFDados.Modelos.Usuario modelo) { return this._conn.DeleteAsync(new USUARIO(modelo)); }

        public Task<int> InserirOuAlterarAsync(SDMobileXFDados.Modelos.Usuario modelo) { return this._conn.InsertOrReplaceAsync(new USUARIO(modelo)); }

        #endregion Usuario


        #region Info

        public Task<List<INFO>> BuscarInfosAsync() { return this._conn.Table<INFO>().ToListAsync(); }

        public Task<INFO> BuscarInfoAsync(string nm) { return this._conn.FindAsync<INFO>(nm); }

        public Task<int> AlterarAsync(INFO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(INFO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(INFO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(INFO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Info


        #region Regional

        public Task<List<REGIONAL>> BuscarRegionaisAsync() { return this._conn.Table<REGIONAL>().ToListAsync(); }

        public Task<List<REGIONAL>> BuscarRegionaisAsync(string filtro) 
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<REGIONAL>().ToListAsync();
            else
                return this._conn.Table<REGIONAL>().Where(r => r.CD_REGIONAL.ToUpper().Contains(filtro.ToUpper()) || r.DS_REGIONAL.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<REGIONAL> BuscarRegionalAsync(Guid id) { return this._conn.FindAsync<REGIONAL>(id); }

        public Task<int> AlterarAsync(REGIONAL modelo) { return this._conn.UpdateAsync(modelo); }
        
        public Task<int> InserirAsync(REGIONAL modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(REGIONAL modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(REGIONAL modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Regional


        #region Gerencia

        public Task<List<GERENCIA>> BuscarGerenciasAsync() { return this._conn.Table<GERENCIA>().ToListAsync(); }

        public Task<List<GERENCIA>> BuscarGerenciasAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<GERENCIA>().Where(g => g.ID_REGIONAL == idPai).ToListAsync();
            else
                return this._conn.Table<GERENCIA>().Where(g => g.ID_REGIONAL == idPai && (g.CD_GERENCIA.ToUpper().Contains(filtro.ToUpper()) || g.DS_GERENCIA.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<GERENCIA> BuscarGerenciaAsync(Guid id) { return this._conn.FindAsync<GERENCIA>(id); }

        public Task<int> AlterarAsync(GERENCIA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(GERENCIA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(GERENCIA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(GERENCIA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Gerencia


        #region Area

        public Task<List<AREA>> BuscarAreasAsync() { return this._conn.Table<AREA>().ToListAsync(); }

        public Task<List<AREA>> BuscarAreasAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<AREA>().Where(a => a.ID_GERENCIA == idPai).ToListAsync();
            else
                return this._conn.Table<AREA>().Where(a => a.ID_GERENCIA == idPai && (a.CD_AREA.ToUpper().Contains(filtro.ToUpper()) || a.DS_AREA.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<AREA> BuscarAreaAsync(Guid id) { return this._conn.FindAsync<AREA>(id); }

        public Task<int> AlterarAsync(AREA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(AREA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(AREA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(AREA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Area


        #region Local

        public Task<List<LOCAL>> BuscarLocaisAsync() { return this._conn.Table<LOCAL>().ToListAsync(); }

        public Task<List<LOCAL>> BuscarLocaisAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<LOCAL>().Where(l => l.ID_AREA == idPai).ToListAsync();
            else
                return this._conn.Table<LOCAL>().Where(l => l.ID_AREA == idPai && (l.CD_LOCAL.ToUpper().Contains(filtro.ToUpper()) || l.DS_LOCAL.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<LOCAL> BuscarLocalAsync(Guid id) { return this._conn.FindAsync<LOCAL>(id); }

        public Task<int> AlterarAsync(LOCAL modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(LOCAL modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(LOCAL modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(LOCAL modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Local


        #region Tipo

        public Task<List<TIPO>> BuscarTiposAsync() { return this._conn.Table<TIPO>().ToListAsync(); }

        public Task<List<TIPO>> BuscarTiposAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<TIPO>().ToListAsync();
            else
                return this._conn.Table<TIPO>().Where(t => t.CD_TIPO.ToUpper().Contains(filtro.ToUpper()) || t.DS_TIPO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<TIPO> BuscarTipoAsync(Guid id) { return this._conn.FindAsync<TIPO>(id); }

        public Task<int> AlterarAsync(TIPO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(TIPO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(TIPO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(TIPO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Tipo


        #region Classificação

        public Task<List<CLASSIFICACAO>> BuscarClassificacoesAsync() { return this._conn.Table<CLASSIFICACAO>().ToListAsync(); }

        public Task<List<CLASSIFICACAO>> BuscarClassificacoesAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<CLASSIFICACAO>().ToListAsync();
            else
                return this._conn.Table<CLASSIFICACAO>().Where(c => c.CD_CLASSIFICACAO.ToUpper().Contains(filtro.ToUpper()) || c.DS_CLASSIFICACAO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<CLASSIFICACAO> BuscarClassificacaoAsync(Guid id) { return this._conn.FindAsync<CLASSIFICACAO>(id); }

        public Task<int> AlterarAsync(CLASSIFICACAO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(CLASSIFICACAO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(CLASSIFICACAO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(CLASSIFICACAO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Classificação


        #region SubClassificação

        public Task<List<SUBCLASSIFICACAO>> BuscarSubClassificacoesAsync() { return this._conn.Table<SUBCLASSIFICACAO>().ToListAsync(); }

        public Task<List<SUBCLASSIFICACAO>> BuscarSubClassificacoesAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<SUBCLASSIFICACAO>().Where(s => s.ID_CLASSIFICACAO == idPai).ToListAsync();
            else
                return this._conn.Table<SUBCLASSIFICACAO>().Where(s => s.ID_CLASSIFICACAO == idPai && (s.CD_SUBCLASSIFICACAO.ToUpper().Contains(filtro.ToUpper()) || s.DS_SUBCLASSIFICACAO.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<SUBCLASSIFICACAO> BuscarSubClassificacaoAsync(Guid id) { return this._conn.FindAsync<SUBCLASSIFICACAO>(id); }

        public Task<int> AlterarAsync(SUBCLASSIFICACAO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(SUBCLASSIFICACAO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(SUBCLASSIFICACAO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(SUBCLASSIFICACAO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion SubClassificação


        #region Categoria

        public Task<List<CATEGORIA>> BuscarCategoriasAsync() { return this._conn.Table<CATEGORIA>().ToListAsync(); }

        public Task<List<CATEGORIA>> BuscarCategoriasAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<CATEGORIA>().Where(c => c.ID_SUBCLASSIFICACAO == idPai).ToListAsync();
            else
                return this._conn.Table<CATEGORIA>().Where(c => c.ID_SUBCLASSIFICACAO == idPai && (c.CD_CATEGORIA.ToUpper().Contains(filtro.ToUpper()) || c.DS_CATEGORIA.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<CATEGORIA> BuscarCategoriaAsync(Guid id) { return this._conn.FindAsync<CATEGORIA>(id); }

        public Task<int> AlterarAsync(CATEGORIA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(CATEGORIA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(CATEGORIA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(CATEGORIA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Categoria


        #region Fornecedor

        public Task<List<FORNECEDOR>> BuscarFornecedoresAsync() { return this._conn.Table<FORNECEDOR>().ToListAsync(); }

        public Task<List<FORNECEDOR>> BuscarFornecedoresAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<FORNECEDOR>().ToListAsync();
            else
                return this._conn.Table<FORNECEDOR>().Where(f => f.CD_FORNECEDOR.ToUpper().Contains(filtro.ToUpper()) || f.DS_FORNECEDOR.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<FORNECEDOR> BuscarFornecedorAsync(Guid id) { return this._conn.FindAsync<FORNECEDOR>(id); }

        public Task<int> AlterarAsync(FORNECEDOR modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(FORNECEDOR modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(FORNECEDOR modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(FORNECEDOR modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Fornecedor


        #region Vinculo

        public Task<List<VINCULO>> BuscarVinculosAsync() { return this._conn.Table<VINCULO>().ToListAsync(); }

        public Task<List<VINCULO>> BuscarVinculosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<VINCULO>().ToListAsync();
            else
                return this._conn.Table<VINCULO>().Where(v => v.NU_MATRICULA.ToUpper().Contains(filtro.ToUpper()) || v.NM_NOME.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadeVinculosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<VINCULO>().CountAsync();
            else
                return this._conn.Table<VINCULO>().CountAsync(v => v.NU_MATRICULA.ToUpper().Contains(filtro.ToUpper()) || v.NM_NOME.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<VINCULO> BuscarVinculoAsync(Guid id) { return this._conn.FindAsync<VINCULO>(id); }

        public Task<int> AlterarAsync(VINCULO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(VINCULO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(VINCULO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(VINCULO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Vinculo


        #region Ocorrência

        public Task<List<OCORRENCIA>> BuscarOcorrenciasAsync() { return this._conn.Table<OCORRENCIA>().ToListAsync(); }

        public Task<int> QuantidadeOcorrenciasAsync() { return this._conn.Table<OCORRENCIA>().CountAsync(); }

        public Task<OCORRENCIA> BuscarOcorrenciaAsync(Guid id) { return this._conn.FindAsync<OCORRENCIA>(id); }

        public Task<int> AlterarAsync(OCORRENCIA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(OCORRENCIA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(OCORRENCIA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(OCORRENCIA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Ocorrência


        #region Imagens Ocorrência

        public Task<List<IMAGEM_RELATO>> BuscarImagensAsync() { return this._conn.Table<IMAGEM_RELATO>().ToListAsync(); }

        public Task<List<IMAGEM_RELATO>> BuscarImagensAsync(Guid idOcorrencia) 
        {
            string sql = $"SELECT * FROM IMAGEM_RELATO WHERE ID_OCORRENCIA = '{idOcorrencia}'";
            return this._conn.QueryAsync<IMAGEM_RELATO>(sql); 
        }

        public Task<int> AlterarAsync(IMAGEM_RELATO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(IMAGEM_RELATO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(IMAGEM_RELATO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(IMAGEM_RELATO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Imagens Ocorrência


        #region Seguro Inseguro

        public Task<List<SEGURO_INSEGURO>> BuscarSeguroInsegurosAsync() { return this._conn.Table<SEGURO_INSEGURO>().ToListAsync(); }

        public Task<List<SEGURO_INSEGURO>> BuscarSeguroInsegurosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<SEGURO_INSEGURO>().ToListAsync();
            else
                return this._conn.Table<SEGURO_INSEGURO>().Where(v => v.CD_SEGURO_INSEGURO.ToUpper().Contains(filtro.ToUpper()) || v.DS_SEGURO_INSEGURO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadeSeguroInsegurosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<SEGURO_INSEGURO>().CountAsync();
            else
                return this._conn.Table<SEGURO_INSEGURO>().CountAsync(v => v.CD_SEGURO_INSEGURO.ToUpper().Contains(filtro.ToUpper()) || v.DS_SEGURO_INSEGURO.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<SEGURO_INSEGURO> BuscarSeguroInseguroAsync(Guid id) { return this._conn.FindAsync<SEGURO_INSEGURO>(id); }

        public Task<int> AlterarAsync(SEGURO_INSEGURO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(SEGURO_INSEGURO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(SEGURO_INSEGURO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(SEGURO_INSEGURO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Seguro Inseguro


        #region Cognitivos

        public Task<List<COGNITIVO>> BuscarCognitivosAsync() { return this._conn.Table<COGNITIVO>().ToListAsync(); }

        public Task<List<COGNITIVO>> BuscarCognitivosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<COGNITIVO>().ToListAsync();
            else
                return this._conn.Table<COGNITIVO>().Where(v => v.CD_COGNITIVO.ToUpper().Contains(filtro.ToUpper()) || v.DS_COGNITIVO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadeCognitivosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<COGNITIVO>().CountAsync();
            else
                return this._conn.Table<COGNITIVO>().CountAsync(v => v.CD_COGNITIVO.ToUpper().Contains(filtro.ToUpper()) || v.DS_COGNITIVO.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<COGNITIVO> BuscarCognitivoAsync(Guid id) { return this._conn.FindAsync<COGNITIVO>(id); }

        public Task<int> AlterarAsync(COGNITIVO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(COGNITIVO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(COGNITIVO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(COGNITIVO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Cognitivos


        #region Fisiológicos

        public Task<List<FISIOLOGICO>> BuscarFisiologicosAsync() { return this._conn.Table<FISIOLOGICO>().ToListAsync(); }

        public Task<List<FISIOLOGICO>> BuscarFisiologicosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<FISIOLOGICO>().ToListAsync();
            else
                return this._conn.Table<FISIOLOGICO>().Where(v => v.CD_FISIOLOGICO.ToUpper().Contains(filtro.ToUpper()) || v.DS_FISIOLOGICO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadeFisiologicosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<FISIOLOGICO>().CountAsync();
            else
                return this._conn.Table<FISIOLOGICO>().CountAsync(v => v.CD_FISIOLOGICO.ToUpper().Contains(filtro.ToUpper()) || v.DS_FISIOLOGICO.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<FISIOLOGICO> BuscarFisiologicoAsync(Guid id) { return this._conn.FindAsync<FISIOLOGICO>(id); }

        public Task<int> AlterarAsync(FISIOLOGICO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(FISIOLOGICO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(FISIOLOGICO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(FISIOLOGICO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Fisiológicos


        #region Psicológicos

        public Task<List<PSICOLOGICO>> BuscarPsicologicosAsync() { return this._conn.Table<PSICOLOGICO>().ToListAsync(); }

        public Task<List<PSICOLOGICO>> BuscarPsicologicosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<PSICOLOGICO>().ToListAsync();
            else
                return this._conn.Table<PSICOLOGICO>().Where(v => v.CD_PSICOLOGICO.ToUpper().Contains(filtro.ToUpper()) || v.DS_PSICOLOGICO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadePsicologicosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<PSICOLOGICO>().CountAsync();
            else
                return this._conn.Table<PSICOLOGICO>().CountAsync(v => v.CD_PSICOLOGICO.ToUpper().Contains(filtro.ToUpper()) || v.DS_PSICOLOGICO.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<PSICOLOGICO> BuscarPsicologicoAsync(Guid id) { return this._conn.FindAsync<PSICOLOGICO>(id); }

        public Task<int> AlterarAsync(PSICOLOGICO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(PSICOLOGICO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(PSICOLOGICO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(PSICOLOGICO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Psicológicos


        #region Sociais

        public Task<List<SOCIAL>> BuscarSociaisAsync() { return this._conn.Table<SOCIAL>().ToListAsync(); }

        public Task<List<SOCIAL>> BuscarSociaisAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<SOCIAL>().ToListAsync();
            else
                return this._conn.Table<SOCIAL>().Where(v => v.CD_SOCIAL.ToUpper().Contains(filtro.ToUpper()) || v.DS_SOCIAL.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadeSociaisAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<SOCIAL>().CountAsync();
            else
                return this._conn.Table<SOCIAL>().CountAsync(v => v.CD_SOCIAL.ToUpper().Contains(filtro.ToUpper()) || v.DS_SOCIAL.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<SOCIAL> BuscarSocialAsync(Guid id) { return this._conn.FindAsync<SOCIAL>(id); }

        public Task<int> AlterarAsync(SOCIAL modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(SOCIAL modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(SOCIAL modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(SOCIAL modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Psicológicos


        #region Abordagem Comportamental

        public Task<List<ABORDAGEM_COMPORTAMENTAL>> BuscarAbordagensAsync() { return this._conn.Table<ABORDAGEM_COMPORTAMENTAL>().ToListAsync(); }

        public Task<int> QuantidadeAbordagensAsync() { return this._conn.Table<ABORDAGEM_COMPORTAMENTAL>().CountAsync(); }

        public Task<ABORDAGEM_COMPORTAMENTAL> BuscarAbordagemAsync(Guid id) { return this._conn.FindAsync<ABORDAGEM_COMPORTAMENTAL>(id); }

        public Task<int> AlterarAsync(ABORDAGEM_COMPORTAMENTAL modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(ABORDAGEM_COMPORTAMENTAL modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(ABORDAGEM_COMPORTAMENTAL modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(ABORDAGEM_COMPORTAMENTAL modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Abordagem Comportamental


        #region Segurança Na Área

        public Task<List<SNA>> BuscarSNAsAsync() { return this._conn.Table<SNA>().ToListAsync(); }

        public Task<int> QuantidadeSNAsAsync() { return this._conn.Table<SNA>().CountAsync(); }

        public Task<SNA> BuscarSNAAsync(Guid id) { return this._conn.FindAsync<SNA>(id); }

        public Task<int> AlterarAsync(SNA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(SNA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(SNA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(SNA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Segurança Na Área


        #region Tipo Inspeção

        public Task<List<TIPO_INSPECAO>> BuscarTipoInspecoesAsync() { return this._conn.Table<TIPO_INSPECAO>().ToListAsync(); }

        public Task<List<TIPO_INSPECAO>> BuscarTipoInspecoesAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<TIPO_INSPECAO>().ToListAsync();
            else
                return this._conn.Table<TIPO_INSPECAO>().Where(v => v.CD_TIPO_INSPECAO.ToUpper().Contains(filtro.ToUpper()) || v.DS_TIPO_INSPECAO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadeTipoInspecaoAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<TIPO_INSPECAO>().CountAsync();
            else
                return this._conn.Table<TIPO_INSPECAO>().CountAsync(v => v.CD_TIPO_INSPECAO.ToUpper().Contains(filtro.ToUpper()) || v.DS_TIPO_INSPECAO.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<TIPO_INSPECAO> BuscarTipoInspecaoAsync(Guid id) { return this._conn.FindAsync<TIPO_INSPECAO>(id); }

        public Task<int> AlterarAsync(TIPO_INSPECAO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(TIPO_INSPECAO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(TIPO_INSPECAO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(TIPO_INSPECAO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Tipo Inspeção


        #region Atividade Inspeção

        public Task<List<ATIVIDADE_INSPECAO>> BuscarAtividadeInspecoesAsync() { return this._conn.Table<ATIVIDADE_INSPECAO>().ToListAsync(); }

        public Task<List<ATIVIDADE_INSPECAO>> BuscarAtividadeInspecoesAsync(Guid idUnidadeRegional, string filtro)
        {            
            REGIONAL regional = this._conn.Table<REGIONAL>().FirstOrDefaultAsync(r => r.ID_REGIONAL == idUnidadeRegional).Result;
            
            if (regional == null)
                return this._conn.Table<ATIVIDADE_INSPECAO>().ToListAsync();
            else if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<ATIVIDADE_INSPECAO>().Where(v => v.ID_SEGMENTO == regional.ID_SEGMENTO).ToListAsync();
            else
                return this._conn.Table<ATIVIDADE_INSPECAO>().Where(v => v.ID_SEGMENTO == regional.ID_SEGMENTO && v.CD_ATIVIDADE_INSPECAO.ToUpper().Contains(filtro.ToUpper()) || v.DS_ATIVIDADE_INSPECAO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<int> QuantidadeAtividadeInspecaoAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<ATIVIDADE_INSPECAO>().CountAsync();
            else
                return this._conn.Table<ATIVIDADE_INSPECAO>().CountAsync(v => v.CD_ATIVIDADE_INSPECAO.ToUpper().Contains(filtro.ToUpper()) || v.DS_ATIVIDADE_INSPECAO.ToUpper().Contains(filtro.ToUpper()));
        }

        public Task<ATIVIDADE_INSPECAO> BuscarAtividadeInspecaoAsync(Guid id) { return this._conn.FindAsync<ATIVIDADE_INSPECAO>(id); }

        public Task<int> AlterarAsync(ATIVIDADE_INSPECAO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(ATIVIDADE_INSPECAO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(ATIVIDADE_INSPECAO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(ATIVIDADE_INSPECAO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Tipo Inspeção


        #region Inspeção

        public Task<List<INSPECAO>> BuscarInspecoesAsync() { return this._conn.Table<INSPECAO>().ToListAsync(); }

        public Task<int> QuantidadeInspecoesAsync() { return this._conn.Table<INSPECAO>().CountAsync(); }

        public Task<INSPECAO> BuscarInspecaoAsync(Guid id) { return this._conn.FindAsync<INSPECAO>(id); }

        public Task<int> AlterarAsync(INSPECAO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(INSPECAO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(INSPECAO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(INSPECAO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Inspeção


        #region Campo Inspeção

        public Task<List<CAMPO_INSPECAO>> BuscarCamposInspecaoAsync() { return this._conn.Table<CAMPO_INSPECAO>().ToListAsync(); }

        public Task<List<CAMPO_INSPECAO>> BuscarCamposInspecaoAsync(Guid idOcorrencia)
        {
            string sql = $"SELECT * FROM CAMPO_INSPECAO WHERE ID_INSPECAO = '{idOcorrencia}'";
            return this._conn.QueryAsync<CAMPO_INSPECAO>(sql);
        }

        public Task<int> AlterarAsync(CAMPO_INSPECAO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(CAMPO_INSPECAO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(CAMPO_INSPECAO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(CAMPO_INSPECAO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Campo Inspeção


        #region Ocorrência Csn

        public Task<List<OCORRENCIACSN>> BuscarOcorrenciasCsnAsync() { return this._conn.Table<OCORRENCIACSN>().ToListAsync(); }

        public Task<int> QuantidadeOcorrenciasCsnAsync() { return this._conn.Table<OCORRENCIACSN>().CountAsync(); }

        public Task<OCORRENCIACSN> BuscarOcorrenciaCsnAsync(Guid id) { return this._conn.FindAsync<OCORRENCIACSN>(id); }

        public Task<int> AlterarAsync(OCORRENCIACSN modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(OCORRENCIACSN modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(OCORRENCIACSN modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(OCORRENCIACSN modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Ocorrência Csn


        #region Unidade

        public Task<List<UNIDADE>> BuscarUnidadesAsync() { return this._conn.Table<UNIDADE>().ToListAsync(); }

        public Task<List<UNIDADE>> BuscarUnidadesAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<UNIDADE>().ToListAsync();
            else
                return this._conn.Table<UNIDADE>().Where(t => t.CD_UNIDADE.ToUpper().Contains(filtro.ToUpper()) || t.DS_UNIDADE.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<UNIDADE> BuscarUnidadeAsync(Guid id) { return this._conn.FindAsync<UNIDADE>(id); }

        public Task<int> AlterarAsync(UNIDADE modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(UNIDADE modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(UNIDADE modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(UNIDADE modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Unidade


        #region Letra

        public Task<List<LETRA>> BuscarLetrasAsync() { return this._conn.Table<LETRA>().ToListAsync(); }

        public Task<List<LETRA>> BuscarLetrasAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<LETRA>().ToListAsync();
            else
                return this._conn.Table<LETRA>().Where(t => t.CD_LETRA.ToUpper().Contains(filtro.ToUpper()) || t.DS_LETRA.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<LETRA> BuscarLetraAsync(Guid id) { return this._conn.FindAsync<LETRA>(id); }

        public Task<int> AlterarAsync(LETRA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(LETRA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(LETRA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(LETRA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Letra


        #region Turno

        public Task<List<TURNO>> BuscarTurnosAsync() { return this._conn.Table<TURNO>().ToListAsync(); }

        public Task<List<TURNO>> BuscarTurnosAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<TURNO>().ToListAsync();
            else
                return this._conn.Table<TURNO>().Where(t => t.CD_TURNO.ToUpper().Contains(filtro.ToUpper()) || t.DS_TURNO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<TURNO> BuscarTurnoAsync(Guid id) { return this._conn.FindAsync<TURNO>(id); }

        public Task<int> AlterarAsync(TURNO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(TURNO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(TURNO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(TURNO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Turno


        #region GerenciaGeralCsn

        public Task<List<GERENCIA_GERAL_CSN>> BuscarGerenciasGeraisCsnAsync() { return this._conn.Table<GERENCIA_GERAL_CSN>().ToListAsync(); }

        public Task<List<GERENCIA_GERAL_CSN>> BuscarGerenciasGeraisCsnAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<GERENCIA_GERAL_CSN>().ToListAsync();
            else
                return this._conn.Table<GERENCIA_GERAL_CSN>().Where(t => t.CD_GERENCIA_GERAL_CSN.ToUpper().Contains(filtro.ToUpper()) || t.DS_GERENCIA_GERAL_CSN.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<GERENCIA_GERAL_CSN> BuscarGerenciaGeralCsnAsync(Guid id) { return this._conn.FindAsync<GERENCIA_GERAL_CSN>(id); }

        public Task<int> AlterarAsync(GERENCIA_GERAL_CSN modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(GERENCIA_GERAL_CSN modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(GERENCIA_GERAL_CSN modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(GERENCIA_GERAL_CSN modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion GerenciaGeralCsn


        #region GerenciaCsn

        public Task<List<GERENCIA_CSN>> BuscarGerenciasCsnAsync() { return this._conn.Table<GERENCIA_CSN>().ToListAsync(); }

        public Task<List<GERENCIA_CSN>> BuscarGerenciasCsnAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<GERENCIA_CSN>().Where(t => t.ID_GERENCIA_GERAL_CSN == idPai).ToListAsync();
            else
                return this._conn.Table<GERENCIA_CSN>().Where(t => t.ID_GERENCIA_GERAL_CSN == idPai && (t.CD_GERENCIA_CSN.ToUpper().Contains(filtro.ToUpper()) || t.DS_GERENCIA_CSN.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<GERENCIA_CSN> BuscarGerenciaCsnAsync(Guid id) { return this._conn.FindAsync<GERENCIA_CSN>(id); }

        public Task<int> AlterarAsync(GERENCIA_CSN modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(GERENCIA_CSN modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(GERENCIA_CSN modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(GERENCIA_CSN modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion GerenciaCsn


        #region AreaEquipamento

        public Task<List<AREA_EQUIPAMENTO>> BuscarAreaEquipamentosAsync() { return this._conn.Table<AREA_EQUIPAMENTO>().ToListAsync(); }

        public Task<List<AREA_EQUIPAMENTO>> BuscarAreaEquipamentosAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<AREA_EQUIPAMENTO>().Where(t => t.ID_GERENCIA_CSN == idPai).ToListAsync();
            else
                return this._conn.Table<AREA_EQUIPAMENTO>().Where(t => t.ID_GERENCIA_CSN == idPai && (t.CD_AREA_EQUIPAMENTO.ToUpper().Contains(filtro.ToUpper()) || t.DS_AREA_EQUIPAMENTO.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<AREA_EQUIPAMENTO> BuscarAreaEquipamentoAsync(Guid id) { return this._conn.FindAsync<AREA_EQUIPAMENTO>(id); }

        public Task<int> AlterarAsync(AREA_EQUIPAMENTO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(AREA_EQUIPAMENTO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(AREA_EQUIPAMENTO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(AREA_EQUIPAMENTO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion AreaEquipamento


        #region LocalEquipamento

        public Task<List<LOCAL_EQUIPAMENTO>> BuscarLocalEquipamentosAsync() { return this._conn.Table<LOCAL_EQUIPAMENTO>().ToListAsync(); }

        public Task<List<LOCAL_EQUIPAMENTO>> BuscarLocalEquipamentosAsync(Guid idPai, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<LOCAL_EQUIPAMENTO>().Where(t => t.ID_AREA_EQUIPAMENTO == idPai).ToListAsync();
            else
                return this._conn.Table<LOCAL_EQUIPAMENTO>().Where(t => t.ID_AREA_EQUIPAMENTO == idPai && (t.CD_LOCAL_EQUIPAMENTO.ToUpper().Contains(filtro.ToUpper()) || t.DS_LOCAL_EQUIPAMENTO.ToUpper().Contains(filtro.ToUpper()))).ToListAsync();
        }

        public Task<LOCAL_EQUIPAMENTO> BuscarLocalEquipamentoAsync(Guid id) { return this._conn.FindAsync<LOCAL_EQUIPAMENTO>(id); }

        public Task<int> AlterarAsync(LOCAL_EQUIPAMENTO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(LOCAL_EQUIPAMENTO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(LOCAL_EQUIPAMENTO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(LOCAL_EQUIPAMENTO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion LocalEquipamento


        #region OrigemAnomalia

        public Task<List<ORIGEM_ANOMALIA>> BuscarOrigemAnomaliasAsync() { return this._conn.Table<ORIGEM_ANOMALIA>().ToListAsync(); }

        public Task<List<ORIGEM_ANOMALIA>> BuscarOrigemAnomaliasAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<ORIGEM_ANOMALIA>().ToListAsync();
            else
                return this._conn.Table<ORIGEM_ANOMALIA>().Where(t => t.CD_ORIGEM_ANOMALIA.ToUpper().Contains(filtro.ToUpper()) || t.DS_ORIGEM_ANOMALIA.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<ORIGEM_ANOMALIA> BuscarOrigemAnomaliaAsync(Guid id) { return this._conn.FindAsync<ORIGEM_ANOMALIA>(id); }

        public Task<int> AlterarAsync(ORIGEM_ANOMALIA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(ORIGEM_ANOMALIA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(ORIGEM_ANOMALIA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(ORIGEM_ANOMALIA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion OrigemAnomalia


        #region TipoAnomalia

        public Task<List<TIPO_ANOMALIA>> BuscarTipoAnomaliasAsync() { return this._conn.Table<TIPO_ANOMALIA>().ToListAsync(); }

        public Task<List<TIPO_ANOMALIA>> BuscarTipoAnomaliasAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<TIPO_ANOMALIA>().ToListAsync();
            else
                return this._conn.Table<TIPO_ANOMALIA>().Where(t => t.CD_TIPO_ANOMALIA.ToUpper().Contains(filtro.ToUpper()) || t.DS_TIPO_ANOMALIA.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<TIPO_ANOMALIA> BuscarTipoAnomaliaAsync(Guid id) { return this._conn.FindAsync<TIPO_ANOMALIA>(id); }

        public Task<int> AlterarAsync(TIPO_ANOMALIA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(TIPO_ANOMALIA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(TIPO_ANOMALIA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(TIPO_ANOMALIA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion TipoAnomalia


        #region ClassificacaoTipo

        public Task<List<CLASSIFICACAO_TIPO>> BuscarClassificacaoTiposAsync() { return this._conn.Table<CLASSIFICACAO_TIPO>().ToListAsync(); }

        public Task<List<CLASSIFICACAO_TIPO>> BuscarClassificacaoTiposAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<CLASSIFICACAO_TIPO>().ToListAsync();
            else
                return this._conn.Table<CLASSIFICACAO_TIPO>().Where(t => t.CD_CLASSIFICACAO_TIPO.ToUpper().Contains(filtro.ToUpper()) || t.DS_CLASSIFICACAO_TIPO.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<CLASSIFICACAO_TIPO> BuscarClassificacaoTipoAsync(Guid id) { return this._conn.FindAsync<CLASSIFICACAO_TIPO>(id); }

        public Task<int> AlterarAsync(CLASSIFICACAO_TIPO modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(CLASSIFICACAO_TIPO modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(CLASSIFICACAO_TIPO modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(CLASSIFICACAO_TIPO modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion ClassificacaoTipo


        #region Probabilidade

        public Task<List<PROBABILIDADE>> BuscarProbabilidadesAsync() { return this._conn.Table<PROBABILIDADE>().ToListAsync(); }

        public Task<List<PROBABILIDADE>> BuscarProbabilidadesAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<PROBABILIDADE>().ToListAsync();
            else
                return this._conn.Table<PROBABILIDADE>().Where(t => t.CD_PROBABILIDADE.ToUpper().Contains(filtro.ToUpper()) || t.DS_PROBABILIDADE.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<PROBABILIDADE> BuscarProbabilidadeAsync(Guid id) { return this._conn.FindAsync<PROBABILIDADE>(id); }

        public Task<int> AlterarAsync(PROBABILIDADE modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(PROBABILIDADE modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(PROBABILIDADE modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(PROBABILIDADE modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Probabilidade


        #region Severidade

        public Task<List<SEVERIDADE>> BuscarSeveridadesAsync() { return this._conn.Table<SEVERIDADE>().ToListAsync(); }

        public Task<List<SEVERIDADE>> BuscarSeveridadesAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<SEVERIDADE>().ToListAsync();
            else
                return this._conn.Table<SEVERIDADE>().Where(t => t.CD_SEVERIDADE.ToUpper().Contains(filtro.ToUpper()) || t.DS_SEVERIDADE.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<SEVERIDADE> BuscarSeveridadeAsync(Guid id) { return this._conn.FindAsync<SEVERIDADE>(id); }

        public Task<int> AlterarAsync(SEVERIDADE modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(SEVERIDADE modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(SEVERIDADE modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(SEVERIDADE modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Severidade


        #region CampoOpa

        public Task<List<CAMPO_OPA>> BuscarCamposOpaAsync() { return this._conn.Table<CAMPO_OPA>().ToListAsync(); }

        public Task<List<CAMPO_OPA>> BuscarCamposOpaAsync(Guid idOpa)
        {
            string sql = $"SELECT * FROM CAMPO_OPA WHERE ID_OPA = '{idOpa}'";
            return this._conn.QueryAsync<CAMPO_OPA>(sql);
        }

        public Task<CAMPO_OPA> BuscarCampoOpaAsync(Guid id) { return this._conn.FindAsync<CAMPO_OPA>(id); }

        public Task<int> AlterarAsync(CAMPO_OPA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(CAMPO_OPA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(CAMPO_OPA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(CAMPO_OPA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion CampoOpa

        #region Opa

        public Task<List<OPA>> BuscarOpasAsync() { return this._conn.Table<OPA>().ToListAsync(); }
        public Task<int> QuantidadeOpasAsync() { return this._conn.Table<OPA>().CountAsync(); }

        public Task<OPA> BuscarOpaAsync(Guid id) { return this._conn.FindAsync<OPA>(id); }

        public Task<int> AlterarAsync(OPA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(OPA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(OPA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(OPA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion Opa


        #region TipoAvaliador

        public Task<List<TIPO_AVALIADOR>> BuscarTipoAvaliadorsAsync() { return this._conn.Table<TIPO_AVALIADOR>().ToListAsync(); }

        public Task<List<TIPO_AVALIADOR>> BuscarTiposAvaliadorAsync(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<TIPO_AVALIADOR>().ToListAsync();
            else
                return this._conn.Table<TIPO_AVALIADOR>().Where(t => t.CD_TIPO_AVALIADOR.ToUpper().Contains(filtro.ToUpper()) || t.DS_TIPO_AVALIADOR.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<TIPO_AVALIADOR> BuscarTipoAvaliadorAsync(Guid id) { return this._conn.FindAsync<TIPO_AVALIADOR>(id); }

        public Task<int> AlterarAsync(TIPO_AVALIADOR modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(TIPO_AVALIADOR modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(TIPO_AVALIADOR modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(TIPO_AVALIADOR modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion TipoAvaliador

        #region TarefaObservada

        public Task<List<TAREFA_OBSERVADA>> BuscarTarefaObservadasAsync() { return this._conn.Table<TAREFA_OBSERVADA>().ToListAsync(); }

        public Task<List<TAREFA_OBSERVADA>> BuscarTarefasObservadasAsync(Guid IdAtividade, string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return this._conn.Table<TAREFA_OBSERVADA>().Where(t => t.ID_ATIVIDADE == IdAtividade).ToListAsync();
            else
                return this._conn.Table<TAREFA_OBSERVADA>().Where(t => t.ID_ATIVIDADE == IdAtividade && 
                    t.CD_TAREFA_OBSERVADA.ToUpper().Contains(filtro.ToUpper()) || t.DS_TAREFA_OBSERVADA.ToUpper().Contains(filtro.ToUpper())).ToListAsync();
        }

        public Task<TAREFA_OBSERVADA> BuscarTarefaObservadaAsync(Guid id) { return this._conn.FindAsync<TAREFA_OBSERVADA>(id); }

        public Task<int> AlterarAsync(TAREFA_OBSERVADA modelo) { return this._conn.UpdateAsync(modelo); }

        public Task<int> InserirAsync(TAREFA_OBSERVADA modelo) { return this._conn.InsertAsync(modelo); }

        public Task<int> ApagarAsync(TAREFA_OBSERVADA modelo) { return this._conn.DeleteAsync(modelo); }

        public Task<int> InserirOuAlterarAsync(TAREFA_OBSERVADA modelo) { return this._conn.InsertOrReplaceAsync(modelo); }

        #endregion TarefaObservada
    }
}

