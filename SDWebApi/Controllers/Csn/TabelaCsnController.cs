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
    public class TabelaCsnController : ApiController
    {
        [HttpPost]
        public List<SDMobileXFDados.Modelos.ModeloObj> RetornarDados(FormDataCollection parametros)
        {
            SDMobileXFDados.Enumerados.Tabela tabela = (SDMobileXFDados.Enumerados.Tabela)Enum.Parse(typeof(SDMobileXFDados.Enumerados.Tabela), parametros.Get("tabela"));

            bool count = false;

            if (!string.IsNullOrEmpty(parametros.Get("count")))
                count = Convert.ToBoolean(parametros.Get("count"));

            string sql = this.GetSql(tabela, parametros.Get("idRegistro"), parametros.Get("idRegistroPai"), parametros.Get("filtro"), count);

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(idChaveSessao, "CSN");

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
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(idChaveSessao, "CSN");

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
            SDWebApi.Models.Sessao sessao = SDWebApi.Models.Sessao.Instancia(idChaveSessao, "CSN");

            object quantidade = sessao.Contexto.BuscarValor(sql);
            return Convert.ToInt32(quantidade);
        }

        private string GetSql(SDMobileXFDados.Enumerados.Tabela tabela, string idRegistro, string idRegistroPai, string filtro, bool count = false)
        {
            string sql = string.Empty;
            string where = string.Empty;

            if (tabela == SDMobileXFDados.Enumerados.Tabela.Vinculo)
            {
                sql = @"SELECT ID_VINCULO Id, NU_MATRICULA Codigo, NM_NOME Descricao,
                               CASE WHEN dt_demissao IS NULL OR Trunc(SYSDATE) <= Trunc(dt_demissao) THEN 1 ELSE 0 END Marcado
                          FROM VW_ISH_VINCULO V
                          JOIN VW_ISH_CADASTRO C ON C.ID_CADASTRO = V.ID_CAD_PESSOAL";
                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" AND ID_VINCULO = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                    sql += string.Format(" AND (UPPER(NU_MATRICULA) LIKE '%{0}%' OR UPPER(NM_NOME) LIKE '%{0}%')", filtro.ToUpper());
                sql += " ORDER BY LPad(NU_MATRICULA, 20, '0'), NM_NOME";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Unidade)
            {
                sql = @"SELECT ID_UNIDADE Id, CD_UNIDADE Codigo, NM_RAZAO_SOCIAL Descricao
                          FROM VW_ISH_UNIDADE V WHERE ST_ATIVADO = 1 ";
                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" AND ID_UNIDADE = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                    sql += string.Format(" AND (UPPER(NM_RAZAO_SOCIAL) LIKE '%{0}%' OR UPPER(CD_UNIDADE) LIKE '%{0}%') ", filtro.ToUpper());
                sql += " ORDER BY LPad(CD_UNIDADE, 20, '0'), NM_RAZAO_SOCIAL";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Letra || tabela == SDMobileXFDados.Enumerados.Tabela.TurnoAnomalia)
            {
                string sqlColecao = "SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao FROM {0} " +
                                    " WHERE ID_IDIOMA_VERSAO = '00000000-0000-0000-0000-000000000000' ";
                if (tabela == SDMobileXFDados.Enumerados.Tabela.Letra)
                    sql = string.Format(sqlColecao, "TB8B8D4EB2C92D4E9CAB0F630C2_TR");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.TurnoAnomalia)
                    sql = string.Format(sqlColecao, "TB6832734F630E4F9FA2787E38B_TR");

                if (!string.IsNullOrEmpty(idRegistro))
                    sql += string.Format(" WHERE CL000000000000ABCD000000000000 = '{0}'", idRegistro);
                else if (!string.IsNullOrEmpty(filtro))
                    sql += string.Format(" WHERE (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
           
                sql += " ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.GerenciaGeralCsn)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao
                          FROM TB344C2F79D7524A46A1139C29BF6E G ";

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
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.GerenciaCsn ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.AreaEquipamento ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.LocalEquipamento ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.OrigemAnomalia ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.TipoAnomalia ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.ClassificacaoTipo)
            {
                sql = @"SELECT CL000000000000ABCD000000000000 Id, CL000000000000ABCD000200000000 Codigo, CL000000000000ABCD000300000000 Descricao";
                      

                string nmTabela = string.Empty;
                string colPai = string.Empty;

                if (tabela == SDMobileXFDados.Enumerados.Tabela.GerenciaCsn)
                {
                    nmTabela = "TB271A941798BA43378CEE3CDC3A9D";
                    colPai = "CL2A00C8A10E2747A99DE33CC33EB8";
                }
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.AreaEquipamento)
                {
                    nmTabela = "TBD508F13F060A428396E469536B07";
                    colPai = "CL2A00C8A10E2747A99DE33CC33EB8";
                }
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.LocalEquipamento)
                {
                    nmTabela = "TB0A03C0FE46084274A955A59A276C";
                    colPai = "CL5E7110542FF345269D7EB1FFFBB3";
                }
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.OrigemAnomalia)
                {
                    nmTabela = "TBE281D0F9B469467495F62BC67F39";
                }
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.TipoAnomalia)
                {
                    nmTabela = "TBD9C48E7D86D448B5BA6720355DB9";
                }
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.ClassificacaoTipo)
                {
                    nmTabela = "TB814E21AD96124F69AF959B12DF5F";
                    colPai = "CL8F7A4680DE224C43A948896FD6F1";
                }

                if (!string.IsNullOrEmpty(colPai))
                    sql += ", " + colPai + " IdPai";
                sql += " FROM {0} T ";

                if (!string.IsNullOrEmpty(idRegistro))
                {
                    sql += " WHERE CL000000000000ABCD000000000000 = '{1}'";
                    sql += string.Format(sql, nmTabela, idRegistro);
                }
                else if (!string.IsNullOrEmpty(idRegistroPai))
                {
                    sql += " WHERE {1} = '{2}'";
                    sql = string.Format(sql, nmTabela, colPai, idRegistroPai);

                    if (!string.IsNullOrEmpty(filtro))
                        sql += string.Format(" AND (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%')", filtro.ToUpper());
                    sql += " AND CL000000000000ABCD003300000000 = 1 ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
                else
                {
                    sql = string.Format(sql, nmTabela);
                    sql += " WHERE CL000000000000ABCD003300000000 = 1 ";
                    if (!string.IsNullOrEmpty(filtro))
                        sql += string.Format(" AND (UPPER(CL000000000000ABCD000200000000) LIKE '%{0}%' OR UPPER(CL000000000000ABCD000300000000) LIKE '%{0}%') ", filtro.ToUpper());
                    sql += " ORDER BY LPad(CL000000000000ABCD000200000000, 20, '0')";
                }
            }
            else if (tabela == SDMobileXFDados.Enumerados.Tabela.Probabilidade ||
                     tabela == SDMobileXFDados.Enumerados.Tabela.Severidade)
            {
                sql = "SELECT TB.CL000000000000ABCD000000000000 Id, TR.CL000000000000ABCD000200000000 Codigo, TR.CL000000000000ABCD000300000000 Descricao," +
                      "       TB.CL6F2D362573B24478915D4CDEBD8D ValorInt " +
                      "  FROM {0} TB, {1} TR" + 
                      " WHERE TB.CL000000000000ABCD000000000000 = TR.CL000000000000ABCD000000000000 " +
                      "   AND ID_IDIOMA_VERSAO = '00000000-0000-0000-0000-000000000000' " +
                      " ORDER BY CL000000000000ABCD000200000000";
                if (tabela == SDMobileXFDados.Enumerados.Tabela.Probabilidade)
                    sql = string.Format(sql, "TB21779E51EC44445F9A8F7E42E216", "TB21779E51EC44445F9A8F7E42E_TR");
                else if (tabela == SDMobileXFDados.Enumerados.Tabela.Severidade)
                    sql = string.Format(sql, "TB42C30BDA4C22450E8D1234088F2C", "TB42C30BDA4C22450E8D1234088_TR");
            }

            if (count)
            {
                int pos = sql.IndexOf("FROM ");
                sql = "SELECT Count(*) " + sql.Substring(pos, sql.Length - pos);
            }

            return sql;
        }
    }
}
