using Gema;
using SD.Dados;
using SD.Dados.Repositorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace SDWebApi.Controllers
{
    public class TabelaController : ApiController
    {
        [HttpPost]
        public List<SDMobileXFDados.Modelos.ModeloObj> RetornarDados(FormDataCollection parametros)
        {
            SDMobileXFDados.Enumerados.Tabela tabela = (SDMobileXFDados.Enumerados.Tabela)Enum.Parse(typeof(SDMobileXFDados.Enumerados.Tabela), parametros.Get("tabela"));

            bool papelsso = false;
            bool count = false;
            if (!string.IsNullOrEmpty(parametros.Get("papelsso")))
                papelsso = Convert.ToBoolean(parametros.Get("papelsso"));

            if (!string.IsNullOrEmpty(parametros.Get("count")))
                count = Convert.ToBoolean(parametros.Get("count"));

            string sql = this.GetSql(tabela, parametros.Get("idRegistro"), parametros.Get("idRegistroPai"), parametros.Get("filtro"), count, papelsso);

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(idChaveSessao, "SUZANO");

            List<SDMobileXFDados.Modelos.ModeloObj> lista;
            using (RepositorioPesquisa rep = new RepositorioPesquisa(sessao.Contexto))
            {
                lista = rep.GetDados<SDMobileXFDados.Modelos.ModeloObj>(sql).ToList();
            }
            return lista;
        }

        /// <summary>
        /// Verifica se o registro filho é unico e retorna caso seja verdadeiro. Utilizado para fazer cargas rapidas das informações de registro filho.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public SDMobileXFDados.Modelos.ModeloObj RetornarSeUnicoFilho(FormDataCollection parametros)
        {
            SDMobileXFDados.Enumerados.Tabela tabela = (SDMobileXFDados.Enumerados.Tabela)Enum.Parse(typeof(SDMobileXFDados.Enumerados.Tabela), parametros.Get("tabela"));
            string sql = this.GetSql(tabela, null, parametros.Get("idRegistroPai"), null);

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(idChaveSessao, "SUZANO");

            List<SDMobileXFDados.Modelos.ModeloObj> lista;
            using (RepositorioPesquisa rep = new RepositorioPesquisa(sessao.Contexto))
            {
                lista = rep.GetDados<SDMobileXFDados.Modelos.ModeloObj>(sql).ToList();
                if (lista.Count == 1)
                    return lista[0];
                else
                    return null;
            }
        }

        [HttpPost]
        public int RetornarQuantidade(FormDataCollection parametros)
        {
            SDMobileXFDados.Enumerados.Tabela tabela = (SDMobileXFDados.Enumerados.Tabela)Enum.Parse(typeof(SDMobileXFDados.Enumerados.Tabela), parametros.Get("tabela"));
            string sql = this.GetSql(tabela, parametros.Get("idRegistro"), parametros.Get("idRegistroPai"), parametros.Get("filtro"), true);

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(idChaveSessao, "SUZANO");

            object quantidade = sessao.Contexto.BuscarValor(sql);
            return Convert.ToInt32(quantidade);
        }

        private string GetSql(SDMobileXFDados.Enumerados.Tabela tabela, string idRegistro, string idRegistroPai, string filtro, bool count = false, bool papelsso = false)
        {
            string sql = string.Empty;
            string where = string.Empty;

            if (tabela == SDMobileXFDados.Enumerados.Tabela.UnidadeRegional)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao
                          FROM TB781D63F32AEA4423B070E7EB190C REGI ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                {
                    sql += string.Format(" WHERE (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND  CL000000000000ABCD003300000000 = 1 " + " ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Gerencia)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao, CL5E7110542FF345269D7EB1FFFBB3 IdPai
                          FROM TB271A941798BA43378CEE3CDC3A9D GERE ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format("WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(idRegistroPai))
                {
                    sql += string.Format("WHERE CL5E7110542FF345269D7EB1FFFBB3 = '{0}'", idRegistroPai);
                    if (!string.IsNullOrEmpty(filtro))
                        sql += string.Format(" AND (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Area)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CLFD7DF78DB1A446B292F9C76515EC Codigo, CLD79FBD89F9CC4808BEA6E8D732D6 Descricao, CL5E7110542FF345269D7EB1FFFBB3 IdPai
                          FROM TB69A8C990799C4B77B4F0D1CE8ECA AREA ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format("WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(idRegistroPai))
                {
                    sql += string.Format("WHERE CL5E7110542FF345269D7EB1FFFBB3 = '{0}'", idRegistroPai);
                    if (!string.IsNullOrEmpty(filtro))
                        sql += string.Format(" AND (UPPER(CLFD7DF78DB1A446B292F9C76515EC) LIKE '%{0}%' OR UPPER(CLD79FBD89F9CC4808BEA6E8D732D6) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CLFD7DF78DB1A446B292F9C76515EC, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CLFD7DF78DB1A446B292F9C76515EC, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Local)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao, CL5E7110542FF345269D7EB1FFFBB3 IdPai
                          FROM TB6BB39BD086DD4DBCAF41AB4149E2 LOCAL ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(idRegistroPai))
                {
                    sql += string.Format("WHERE CL5E7110542FF345269D7EB1FFFBB3 = '{0}'", idRegistroPai);
                    if (!string.IsNullOrEmpty(filtro))
                        sql += string.Format(" AND (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Tipo)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao
                          FROM TB40A18BA0B6F04B3ABD7A82AB8E05 TIPO ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                {
                    sql += string.Format(" WHERE (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Classificacao)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao
                          FROM TB5E5BE4B0C71E4F3B97E471EFBA69 CLASSIF ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else
                {
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        sql += string.Format(" WHERE (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                        sql += " AND CL000000000000ABCD003300000000 = 1";
                    }
                    else
                        sql += " WHERE CL000000000000ABCD003300000000 = 1";
                    if (!papelsso)
                        sql += " AND CL000000000000ABCD000200000000 IN ('DO', 'DQ', 'OC', 'BS', 'IN')";
                    sql += " ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.SubClassificacao)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao, CL5E7110542FF345269D7EB1FFFBB3 IdPai
                          FROM TB7DDBD552E69D4721B68BE65F4735 SUBCLASSIF ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(idRegistroPai))
                {
                    sql += string.Format(" WHERE CL5E7110542FF345269D7EB1FFFBB3 = '{0}'", idRegistroPai);
                    if (!string.IsNullOrEmpty(filtro))
                        sql += string.Format(" AND (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";

            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Categoria)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao, CL361D9FD735BF4B68AF69544EFF66 IdPai
                          FROM TB5710DEF79A934512910FDD238152 CATEGORIA ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(idRegistroPai))
                {
                    sql += string.Format(" WHERE CL361D9FD735BF4B68AF69544EFF66 = '{0}'", idRegistroPai);
                    if (!string.IsNullOrEmpty(filtro))
                        sql += string.Format(" AND (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Fornecedor)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CLA95D0DD6FEFB4A63A7CC460CF494 Codigo, CL10914D4F3685436282FBBC16F662 Descricao
                          FROM TBFB7C40BCD3E2487FBFFD7BC6F98F FORNECEDOR ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                {
                    sql += string.Format(" WHERE (UPPER(CLA95D0DD6FEFB4A63A7CC460CF494) LIKE '%{0}%' OR UPPER(CL10914D4F3685436282FBBC16F662) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CLA95D0DD6FEFB4A63A7CC460CF494, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CLA95D0DD6FEFB4A63A7CC460CF494, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Vinculo)
            {
                sql = @"SELECT ID_VINCULO Id, NU_MATRICULA Codigo, NM_NOME Descricao,
                               CASE WHEN dt_demissao IS NULL OR Trunc(SYSDATE) <= Trunc(dt_demissao) THEN 1 ELSE 0 END Marcado
                          FROM VW_ISH_VINCULO V
                          JOIN VW_ISH_CADASTRO C ON C.ID_CADASTRO = V.ID_CAD_PESSOAL";
                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE ID_VINCULO = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                    sql += string.Format(" WHERE (UPPER(NU_MATRICULA) LIKE '%{0}%' OR UPPER(NM_NOME) LIKE '%{0}%')", filtro.ToUpper());
                sql += " ORDER BY LPad(NU_MATRICULA, 20, '0'), NM_NOME";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.RespostasSeguroInseguroNa ||
                    tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoCOGNITIVOS ||
                    tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoFISIOLOGICOS ||
                    tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoPSICOLOGICOS ||
                    tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoSOCIAIS)
            {
                string sqlColecao = "SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao FROM {0} " +
                                    " WHERE ID_IDIOMA_VERSAO = '00000000-0000-0000-0000-000000000000' ORDER BY CL000000000000ABCD000200000000";
                if (tabela == SDMobileXFDados.Enumerados.Tabela.RespostasSeguroInseguroNa)
                    sql = string.Format(sqlColecao, "TBDEEC158C1F06449D971E07E88_TR");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoCOGNITIVOS)
                    sql = string.Format(sqlColecao, "TB2D73CA163C2A48EBAB5186A84_TR");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoFISIOLOGICOS)
                    sql = string.Format(sqlColecao, "TBE5472D31841F4B378A8D5213B_TR");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoPSICOLOGICOS)
                    sql = string.Format(sqlColecao, "TBE7573BF58398476E8AAB9E8AC_TR");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.AtivadoresComportamentoSOCIAIS)
                    sql = string.Format(sqlColecao, "TBAF75B1565235479ABAFB2DFB3_TR");
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.TipoInspecao ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.AtividadeInspecao ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.TarefaObservada)
            {
                sql = "SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao " +
                      "  FROM {0} ";
                if (tabela == SDMobileXFDados.Enumerados.Tabela.TipoInspecao)
                    sql = string.Format(sql, "TBC5E3FF6DDA90496F96B48978A227");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.AtividadeInspecao)
                    sql = string.Format(sql, "TB85F7AFEC399944089B347BB46D55");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.TarefaObservada)
                    sql = string.Format(sql, "TBB760FCEDDBFE4965BE358DC6335C");
                
                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                {
                    sql += string.Format(" WHERE (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";

            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.TipoAvaliador)
            {
                sql = "SELECT TB.CL000000000000ABCD000000000000 Id, TR.CL000000000000ABCD000200000000 Codigo, TR.CL000000000000ABCD000300000000 Descricao " +
                      "  FROM TBF62704CC120D41D9B6C30C00783D TB " +
                      "  LEFT JOIN TBF62704CC120D41D9B6C30C007_TR TR ON TR.CL000000000000ABCD000000000000 = TB.CL000000000000ABCD000000000000 " +
                      " WHERE ID_IDIOMA_VERSAO = '00000000-0000-0000-0000-000000000000' ";

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" AND TB.CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                {
                    sql += string.Format(" AND (UPPER(TR.CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(TR.CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND TB.CL000000000000ABCD003300000000 = 1 ORDER BY LPad(TR.CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                    sql += " AND TB.CL000000000000ABCD003300000000 = 1 ORDER BY LPad(TR.CL000000000000ABCD000200000000, 20, '0')";
            }

            if (count)
            {
                int pos = sql.IndexOf("FROM ");
                sql = "SELECT Count(*) " + sql.Substring(pos, sql.Length - pos);
            }

            return sql;
        }

        // GET: api/Tabela/Buscar/TBB0324B78CADB48B5B522BF6CEE9B/CL000000000000ABCD000300000000,"CL000000000000ABCD000000000000 = 'a6d8d868-cdea-4563-afcb-1c903c341a5f'"
        // GET: api/Tabela/Buscar/VW_ISH_VINCULO/ID_VINCULO, NU_MATRICULA/NU_MATRICULA LIKE '%11%'
        [HttpGet]
        public IEnumerable<object> Buscar(string idChaveSessao, string tabela, string colunas, string filtro)
        {
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(new Guid(idChaveSessao), "SUZANO");

            if (string.IsNullOrEmpty(colunas))
                colunas = "*";

            string sql = $"SELECT {colunas} FROM {tabela}";

            if (!string.IsNullOrEmpty(filtro))
                sql = string.Concat(sql, " WHERE ", filtro);

            DataTable dt = sessao.Contexto.BuscarTabela(sql);
            foreach (DataRow linha in dt.Rows)
            {
                dynamic expandoObj = new ExpandoObject();
                IDictionary<string, object> expandoDict = expandoObj as IDictionary<string, object>;

                foreach (DataColumn col in dt.Columns)
                {
                    if (linha.IsNull(col))
                        expandoDict.Add(col.ColumnName, string.Empty);
                    else
                        expandoDict.Add(col.ColumnName, linha[col].ToString());
                }
                yield return expandoObj;
            }
        }

        [HttpPut]
        public HttpResponseMessage Inserir(string idChaveSessao, string tabela, string colunas, string valores)
        {
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(new Guid(idChaveSessao), "SUZANO");

            if (string.IsNullOrEmpty(colunas))
                colunas = "*";

            string sql = $"INSERT INTO {tabela} ({colunas}) VALUES ({valores})";

            try
            {
                sessao.Contexto.Executar(sql);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage InserirPost(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            string tabela = parametros.Get("tabela");
            string colunas = parametros.Get("colunas");
            string valores = parametros.Get("valores");
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(idChaveSessao, "SUZANO");

            if (string.IsNullOrEmpty(colunas))
                colunas = "*";

            string sql = $"INSERT INTO {tabela} ({colunas}) VALUES ({valores})";

            try
            {
                sessao.Contexto.Executar(sql);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Apagar(string idChaveSessao, string tabela, string criterio)
        {
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(new Guid(idChaveSessao), "SUZANO");

            string sql = $"DELETE {tabela} WHERE {criterio}";

            try
            {
                sessao.Contexto.Executar(sql);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
