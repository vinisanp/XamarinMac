using SD.Dados;
using SD.Dados.Repositorio;
using SD.Dados.Repositorio.OcorrenciaAnomalia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using SDWebApi.Models;
using SD.Dados.Modelos;
using System.Data;
using SDMobileXFDados.Modelos;
using System.Web;
using System.IO;
using System.Reflection;

namespace SDWebApi.Controllers
{
    public class OcorrenciasCsnController : ApiController
    {
        // GET: api/Ocorrencia
        public IEnumerable<string> Get()
        {
            Contexto contexto = Contexto.Instancia(new ConexaoBanco("BANCO", null, null));

            RepositorioOcorrenciaAnomalia ocorrencia = new RepositorioOcorrenciaAnomalia(contexto);
            string teste = ocorrencia.NomeTabela;
            DateTime data = contexto.DataBanco;

            return new string[] { teste, data.ToShortDateString() };
        }

        [HttpPost]
        public HttpResponseMessage Inserir(FormDataCollection parametros)
        {
            string erro = string.Empty;
            try
            {
                Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
                string login = parametros.Get("login");
                Sessao sessao = Sessao.Instancia(idChaveSessao, "CSN");

                Util.IniciarSessao(sessao, login);

                Guid idRegistro = Guid.NewGuid();
                Guid idTabela = new Guid("5824888c-9ed4-419f-90eb-bea77f717ec8");
                Guid idColuna = new Guid("c388c0ff-c35b-4680-aa4f-f9da4f39888c");

                using (RepositorioOcorrenciaAnomalia rep = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
                {
                    string numero = parametros.Get("NU_OCORRENCIA");
                    string titulo = parametros.Get("TITULO").Replace("'", "''");
                    string sql = string.Empty;

                    if (string.IsNullOrEmpty(numero))
                    {
                        numero = Util.GerarSequencia(sessao.Contexto, idTabela, idColuna, idRegistro, DateTime.Today);

                        sql = @"INSERT INTO TB5824888C9ED4419F90EBBEA77F71 
                                       (CL000000000000ABCD000000000000, CLC388C0FFC35B4680AA4FF9DA4F39, CL000000000000ABCD001100000000,
                                        CL000000000000ABCD000300000000, CL2A00C8A10E2747A99DE33CC33EB8, CL000000000000ABCD003500000000,
                                        CL0AADD227F8ED44E2BF581494F7EC, CL833FA07877624A8EAA02C4046FBC, CL9ABEA49EEE4A42858CBFCFC2FC4A,
                                        CLB385850AAF9A4AD1B30AF0BABE9E, CL537D49B88A92494BBF27C42B8D99, CL10EA2CCBA69E427ABD738A161A09, 
                                        CL012A022F71BD4EECAB2AB92C8152, CL1BA3CCE96716406A8CC8F5D5C4A8, CL361D9FD735BF4B68AF69544EFF66,
                                        CLDDAF84BE3F634F9EBB4A853FCF6F, CL67AC7170211B48A4840791037367, CLCF4E7C94AC2F42278AB8A96A872B, 
                                        CLB6966C30E07D4DE58552E5CC4351, CL797D8B85A36C4C99BE1AA6E70728, CL52A0FE3406DA4595A069F9E5A682, 
                                        CLA570AAF03BE74FDFA5485E409AF4, CLD69A7C64D86F45C2A64EE9B6C40C, CLB2A1EDEBC0444A19AA2FA2E73FE5,
                                        CL7EE5A052A86F45B983A5AD831585, CL16912FEB57524DF7A324B5CA1E1C, CLD172E6FAE63B4AD2AE76690C24F7, 
                                        CLCB425A1BE63C4C5BB928F5910374) " +
                             $" VALUES ('{idRegistro}'                              , '{numero}'                                , TO_DATE('{parametros.Get("DATA")}', 'YYYYMMDD HH24MISS'), " +
                             $"         '{titulo}'                                  , '{parametros.Get("ID_DIA")}'              , '{parametros.Get("ID_REGIAO")}', " +
                             $"         '{parametros.Get("ID_LETRA")}'              , '{parametros.Get("ID_TURNO")}'            , '{parametros.Get("ID_GERENCIA_GERAL")}', " +
                             $"         '{parametros.Get("ID_GERENCIA")}'           , '{parametros.Get("ID_AREA_EQUIPAMENTO")}' , '{parametros.Get("ID_LOCAL_EQUIPAMENTO")}', " +
                             $"         '{parametros.Get("LOCAL_ANOMALIA")}'        , '{parametros.Get("DESCRICAO_PRELIMINAR")}', '{parametros.Get("ACOES_IMEDIATAS")}', " +
                             $"         '{parametros.Get("REMOCAO_SINTOMAS")}'      , '{parametros.Get("ID_ORIGEM")}'           , '{parametros.Get("ID_TIPO_ANOMALIA")}', " +
                             $"         '{parametros.Get("ID_CLASSIFICACAO_TIPO")}' , '{parametros.Get("ID_PROBABILIDADE")}'    , '{parametros.Get("ID_SEVERIDADE")}', " +
                             $"         '{parametros.Get("PRODUTOAB")}'             , '{parametros.Get("ID_RESULTADO")}'        , '{parametros.Get("ID_REGISTRADO_POR")}', " +
                             $"         '{parametros.Get("ID_RELATADO_POR")}'       , '{parametros.Get("ID_SUPERVISOR")}'       , '{parametros.Get("ID_ASSINATURA")}', " +
                             $"         1) ";
                    }
                    else
                    {
                        idRegistro = new Guid(sessao.Contexto.BuscarValorStr($"SELECT CL000000000000ABCD000000000000 FROM TB5824888C9ED4419F90EBBEA77F71 WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}'"));

                        sql = " UPDATE TB5824888C9ED4419F90EBBEA77F71 " + 
                                $" SET CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}', " +
                                    $" CL000000000000ABCD001100000000 = TO_DATE('{parametros.Get("DATA")}', 'YYYYMMDD HH24MISS'), " +
                                    $" CL000000000000ABCD000300000000 = '{titulo}', " +
                                    $" CL2A00C8A10E2747A99DE33CC33EB8 = '{parametros.Get("ID_DIA")}', " +
                                    $" CL000000000000ABCD003500000000 = '{parametros.Get("ID_REGIAO")}', " +
                                    $" CL0AADD227F8ED44E2BF581494F7EC = '{parametros.Get("ID_LETRA")}', " +
                                    $" CL833FA07877624A8EAA02C4046FBC = '{parametros.Get("ID_TURNO")}', " +
                                    $" CL9ABEA49EEE4A42858CBFCFC2FC4A = '{parametros.Get("ID_GERENCIA_GERAL")}', " +
                                    $" CLB385850AAF9A4AD1B30AF0BABE9E = '{parametros.Get("ID_GERENCIA")}', " +
                                    $" CL537D49B88A92494BBF27C42B8D99 = '{parametros.Get("ID_AREA_EQUIPAMENTO")}', " +
                                    $" CL10EA2CCBA69E427ABD738A161A09 = '{parametros.Get("ID_LOCAL_EQUIPAMENTO")}', " +
                                    $" CL012A022F71BD4EECAB2AB92C8152 = '{parametros.Get("LOCAL_ANOMALIA")}', " +
                                    $" CL1BA3CCE96716406A8CC8F5D5C4A8 = '{parametros.Get("DESCRICAO_PRELIMINAR")}', " +
                                    $" CL361D9FD735BF4B68AF69544EFF66 = '{parametros.Get("ACOES_IMEDIATAS")}', " +
                                    $" CLDDAF84BE3F634F9EBB4A853FCF6F = '{parametros.Get("REMOCAO_SINTOMAS")}', " +
                                    $" CL67AC7170211B48A4840791037367 = '{parametros.Get("ID_ORIGEM")}', " +
                                    $" CLCF4E7C94AC2F42278AB8A96A872B = '{parametros.Get("ID_TIPO_ANOMALIA")}', " +
                                    $" CLB6966C30E07D4DE58552E5CC4351 = '{parametros.Get("ID_CLASSIFICACAO_TIPO")}', " +
                                    $" CL797D8B85A36C4C99BE1AA6E70728 = '{parametros.Get("ID_PROBABILIDADE")}', " +
                                    $" CL52A0FE3406DA4595A069F9E5A682 = '{parametros.Get("ID_SEVERIDADE")}', " +
                                    $" CLA570AAF03BE74FDFA5485E409AF4 = '{parametros.Get("PRODUTOAB")}', " +
                                    $" CLD69A7C64D86F45C2A64EE9B6C40C = '{parametros.Get("ID_RESULTADO")}', " +
                                    $" CLB2A1EDEBC0444A19AA2FA2E73FE5 = '{parametros.Get("ID_REGISTRADO_POR")}', " +
                                    $" CL7EE5A052A86F45B983A5AD831585 = '{parametros.Get("ID_RELATADO_POR")}', " +
                                    $" CL16912FEB57524DF7A324B5CA1E1C = '{parametros.Get("ID_SUPERVISOR")}', " +
                                    $" CLD172E6FAE63B4AD2AE76690C24F7 = '{parametros.Get("ID_ASSINATURA")}', " +
                              $" WEHRE CL000000000000ABCD000000000000 = '{idRegistro}'";
                    }
                    try
                    {
                        sessao.Contexto.Executar(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, idRegistro + "|" + numero);
                    }
                    catch (Exception exBanco)
                    {
                        erro = exBanco.Message;
                        Util.ExcluirSequencia(sessao.Contexto, idTabela, idRegistro);
                    }
                }
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, erro);
        }

        [HttpPost]
        public List<SDMobileXFDados.Modelos.OcorrenciaCsn> RetornarOcorrencias(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid? idVinculo = null;
            
            if(parametros.Get("idVinculo") != null)
                idVinculo = new Guid(parametros.Get("idVinculo"));
            string dtInicial = parametros.Get("dtInicial");
            string dtFinal = parametros.Get("dtFinal");

            Sessao sessao = Sessao.Instancia(idChaveSessao, "CSN");

            using (RepositorioOcorrenciaAnomalia repositorio = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
            {
                string sql = string.Format(@"SELECT CL000000000000ABCD000000000000 ID_OCORRENCIA,
                                                    CLC388C0FFC35B4680AA4FF9DA4F39 CODIGO, 
                                                    CL000000000000ABCD001100000000 DATA, 
                                                    CL000000000000ABCD000300000000 TITULO,                                                   
                                                    FC_SITUACAO_FOLLOWUP2(  OCOR.CL000000000000ABCD000000000000, 
                                                                            (SELECT COUNT(*) FROM TB1951B7A0ED8D41CF875779C7EF22 PLANO 
                                                                              WHERE PLANO.CL000000000000ABCD001000000000 = OCOR.CL000000000000ABCD000000000000), 
                                                                            OCOR.CL062725DEC7BB4040927156C70452, 
                                                                            NULL, 
                                                                            OCOR.CL361D9FD735BF4B68AF69544EFF66, 
                                                                            OCOR.CLB2240E5153A84520A047D7B7C8E8, 
                                                                            OCOR.CL63AF72F4170C4210B50B55664424) STATUS                                                 
                                               FROM TB5824888C9ED4419F90EBBEA77F71 OCOR 
                                              WHERE CL000000000000ABCD001100000000 >= TO_DATE('{0}', 'dd/MM/yyyy') 
                                                AND CL000000000000ABCD001100000000 < (TO_DATE('{1}', 'dd/MM/yyyy') + 1)", dtInicial, dtFinal);
                //if(idVinculo.HasValue)
                //    sql += string.Format(" AND (CLB2A1EDEBC0444A19AA2FA2E73FE5 = '{0}' OR CL7EE5A052A86F45B983A5AD831585 = '{0}' OR CL16912FEB57524DF7A324B5CA1E1C = '{0}' OR CLD172E6FAE63B4AD2AE76690C24F7 = '{0}')", idVinculo);

                sql += " ORDER BY CL000000000000ABCD001100000000 DESC";

                List<OcorrenciaCsn> lst = repositorio.GetDados<SDMobileXFDados.Modelos.OcorrenciaCsn>(sql).ToList();
                return lst;
            }
        }

        [HttpPost]
        public SDMobileXFDados.Modelos.OcorrenciaCsn RetornarOcorrencia(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idOcorrencia = new Guid(parametros.Get("idOcorrencia"));

            Sessao sessao = Sessao.Instancia(idChaveSessao, "CSN");

            string sql = string.Format(@"   SELECT OCORRENCIA.CL000000000000ABCD000000000000 ID_OCORRENCIA,
                                                   OCORRENCIA.CL000000000000ABCD001100000000 DATA,
                                                   OCORRENCIA.CLC388C0FFC35B4680AA4FF9DA4F39 NUMERO,
                                                   OCORRENCIA.CL000000000000ABCD000300000000 TITULO,
                                                   REGIAO.CL000000000000ABCD000000000000 ID_REGIAO,
                                                   REGIAO.CLC388C0FFC35B4680AA4FF9DA4F39 CD_REGIAO,
                                                   REGIAO.CL95B9121DDCD54FC7B399A6BFBCF3 DS_REGIAO,
                                                   DIA.CL000000000000ABCD000000000000 ID_DIA,
                                                   DIA.CL000000000000ABCD000200000000 CD_DIA,
                                                   DIA.CL000000000000ABCD000300000000 DS_DIA,
                                                   LETRA.CL000000000000ABCD000000000000 ID_LETRA,
                                                   LETRA.CL000000000000ABCD000200000000 CD_LETRA,
                                                   LETRA.CL000000000000ABCD000300000000 DS_LETRA,
                                                   TURNO.CL000000000000ABCD000000000000 ID_TURNO,
                                                   TURNO.CL000000000000ABCD000200000000 CD_TURNO,
                                                   TURNO.CL000000000000ABCD000300000000 DS_TURNO,
                                                   GERENCIA_GERAL.CL000000000000ABCD000000000000 ID_GERENCIA_GERAL,
                                                   GERENCIA_GERAL.CL000000000000ABCD000200000000 CD_GERENCIA_GERAL,
                                                   GERENCIA_GERAL.CL000000000000ABCD000300000000 DS_GERENCIA_GERAL,
                                                   GERENCIA.CL000000000000ABCD000000000000 ID_GERENCIA,
                                                   GERENCIA.CL000000000000ABCD000200000000 CD_GERENCIA,
                                                   GERENCIA.CL000000000000ABCD000300000000 DS_GERENCIA,
                                                   AREA.CL000000000000ABCD000000000000 ID_AREA,
                                                   AREA.CL000000000000ABCD000200000000 CD_AREA,
                                                   AREA.CL000000000000ABCD000300000000 DS_AREA,
                                                   LOCAL.CL000000000000ABCD000000000000 ID_LOCAL,
                                                   LOCAL.CL000000000000ABCD000200000000 CD_LOCAL,
                                                   LOCAL.CL000000000000ABCD000300000000 DS_LOCAL,
                                                   CL012A022F71BD4EECAB2AB92C8152 DS_LOCAL_ANOMALIA,
                                                   CL1BA3CCE96716406A8CC8F5D5C4A8 DS_PRELIMINAR,
                                                   ACOES_IMEDATAS.CL000000000000ABCD000000000000 ID_ACOES_IMEDATAS,
                                                   ACOES_IMEDATAS.CL000000000000ABCD000200000000 CD_ACOES_IMEDATAS,
                                                   ACOES_IMEDATAS.CL000000000000ABCD000300000000 DS_ACOES_IMEDATAS,
                                                   CLDDAF84BE3F634F9EBB4A853FCF6F DS_REMOCAO_SINTOMAS,
                                                   TIPO.CL000000000000ABCD000000000000 ID_TIPO,
                                                   TIPO.CL000000000000ABCD000200000000 CD_TIPO,
                                                   TIPO.CL000000000000ABCD000300000000 DS_TIPO,
                                                   CLASSIFICACAO.CL000000000000ABCD000000000000 ID_CLASSIFICACAO,
                                                   CLASSIFICACAO.CL000000000000ABCD000200000000 CD_CLASSIFICACAO,
                                                   CLASSIFICACAO.CL000000000000ABCD000300000000 DS_CLASSIFICACAO,
                                                   ORIGEM.CL000000000000ABCD000000000000 ID_ORIGEM,
                                                   ORIGEM.CL000000000000ABCD000200000000 CD_ORIGEM,
                                                   ORIGEM.CL000000000000ABCD000300000000 DS_ORIGEM,
                                                   REGISTRADO.CL000000000000ABCD000000000000 ID_REGISTRADO,
                                                   REGISTRADO.CLA95D0DD6FEFB4A63A7CC460CF494 CD_REGISTRADO,
                                                   (SELECT NM_NOME FROM VW_ISH_CADASTRO WHERE ID_CADASTRO = REGISTRADO.CL81F753A75F3B415CAB2037E094BA) DS_REGISTRADO,
                                                   RELATADO.CL000000000000ABCD000000000000 ID_RELATADO,
                                                   RELATADO.CLA95D0DD6FEFB4A63A7CC460CF494 CD_RELATADO,
                                                   (SELECT NM_NOME FROM VW_ISH_CADASTRO WHERE ID_CADASTRO = RELATADO.CL81F753A75F3B415CAB2037E094BA) DS_RELATADO,
                                                   SUPERVISOR.CL000000000000ABCD000000000000 ID_SUPERVISOR,
                                                   SUPERVISOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_SUPERVISOR,
                                                   (SELECT NM_NOME FROM VW_ISH_CADASTRO WHERE ID_CADASTRO = SUPERVISOR.CL81F753A75F3B415CAB2037E094BA) DS_SUPERVISOR,
                                                   ASSINATURA.CL000000000000ABCD000000000000 ID_ASSINATURA,
                                                   ASSINATURA.CLA95D0DD6FEFB4A63A7CC460CF494 CD_ASSINATURA,
                                                   (SELECT NM_NOME FROM VW_ISH_CADASTRO WHERE ID_CADASTRO = ASSINATURA.CL81F753A75F3B415CAB2037E094BA) DS_ASSINATURA,
                                                   PROBABILIDADE.CL000000000000ABCD000000000000 ID_PROBABILIDADE,
                                                   PROBABILIDADE.CL000000000000ABCD000200000000 CD_PROBABILIDADE,
                                                   PROBABILIDADE.CL000000000000ABCD000300000000 DS_PROBABILIDADE,
                                                   SEVERIDADE.CL000000000000ABCD000000000000 ID_SEVERIDADE,
                                                   SEVERIDADE.CL000000000000ABCD000200000000 CD_SEVERIDADE,
                                                   SEVERIDADE.CL000000000000ABCD000300000000 DS_SEVERIDADE,
                                                   OCORRENCIA.CLA570AAF03BE74FDFA5485E409AF4 PRODUTOAB,
                                                   RESULTADO.CL000000000000ABCD000000000000 ID_RESULTADO,
                                                   RESULTADO.CLD71E82DA3FE94E6993408F732A72 CD_RESULTADO,
                                                   RESULTADO.CL000000000000ABCD000300000000 DS_RESULTADO
                                              FROM TB5824888C9ED4419F90EBBEA77F71 OCORRENCIA
                                              LEFT JOIN TB000000000000ABCD003500000000 REGIAO         ON REGIAO.CL000000000000ABCD000000000000         = OCORRENCIA.CL000000000000ABCD003500000000
                                              LEFT JOIN TB4B9B5BD2AEDC410DB62113227_TR DIA            ON DIA.CL000000000000ABCD000000000000            = OCORRENCIA.CL2A00C8A10E2747A99DE33CC33EB8
                                                                                                     AND ID_IDIOMA_VERSAO                              = '00000000-0000-0000-0000-000000000000'
                                              LEFT JOIN TB8B8D4EB2C92D4E9CAB0F630C2_TR LETRA          ON LETRA.CL000000000000ABCD000000000000          = OCORRENCIA.CL0AADD227F8ED44E2BF581494F7EC
                                                                                                     AND LETRA.ID_IDIOMA_VERSAO                        = '00000000-0000-0000-0000-000000000000'
                                              LEFT JOIN TB6832734F630E4F9FA2787E38B_TR TURNO          ON TURNO.CL000000000000ABCD000000000000          = OCORRENCIA.CL833FA07877624A8EAA02C4046FBC
                                                                                                     AND TURNO.ID_IDIOMA_VERSAO                        = '00000000-0000-0000-0000-000000000000'
                                              LEFT JOIN TB344C2F79D7524A46A1139C29BF6E GERENCIA_GERAL ON GERENCIA_GERAL.CL000000000000ABCD000000000000 = OCORRENCIA.CL9ABEA49EEE4A42858CBFCFC2FC4A
                                              LEFT JOIN TB271A941798BA43378CEE3CDC3A9D GERENCIA       ON GERENCIA.CL000000000000ABCD000000000000       = OCORRENCIA.CLB385850AAF9A4AD1B30AF0BABE9E
                                              LEFT JOIN TBD508F13F060A428396E469536B07 AREA           ON AREA.CL000000000000ABCD000000000000           = OCORRENCIA.CL537D49B88A92494BBF27C42B8D99
                                              LEFT JOIN TB0A03C0FE46084274A955A59A276C LOCAL          ON LOCAL.CL000000000000ABCD000000000000          = OCORRENCIA.CL10EA2CCBA69E427ABD738A161A09
                                              LEFT JOIN TB1F8DD1AB8654454F81D9709B4_TR ACOES_IMEDATAS ON ACOES_IMEDATAS.CL000000000000ABCD000000000000 = OCORRENCIA.CL361D9FD735BF4B68AF69544EFF66
                                                                                                     AND ACOES_IMEDATAS.ID_IDIOMA_VERSAO               = '00000000-0000-0000-0000-000000000000'
                                              LEFT JOIN TBD9C48E7D86D448B5BA6720355DB9 TIPO           ON TIPO.CL000000000000ABCD000000000000           = OCORRENCIA.CLCF4E7C94AC2F42278AB8A96A872B
                                              LEFT JOIN TB814E21AD96124F69AF959B12DF5F CLASSIFICACAO  ON CLASSIFICACAO.CL000000000000ABCD000000000000  = OCORRENCIA.CLB6966C30E07D4DE58552E5CC4351
                                              LEFT JOIN TBE281D0F9B469467495F62BC67F39 ORIGEM         ON ORIGEM.CL000000000000ABCD000000000000         = OCORRENCIA.CL67AC7170211B48A4840791037367
                                              LEFT JOIN TB831F15D403654FCE84691FE856D2 REGISTRADO     ON REGISTRADO.CL000000000000ABCD000000000000     = OCORRENCIA.CLB2A1EDEBC0444A19AA2FA2E73FE5
                                              LEFT JOIN TB831F15D403654FCE84691FE856D2 RELATADO       ON RELATADO.CL000000000000ABCD000000000000       = OCORRENCIA.CL7EE5A052A86F45B983A5AD831585
                                              LEFT JOIN TB831F15D403654FCE84691FE856D2 SUPERVISOR     ON SUPERVISOR.CL000000000000ABCD000000000000     = OCORRENCIA.CL16912FEB57524DF7A324B5CA1E1C
                                              LEFT JOIN TB831F15D403654FCE84691FE856D2 ASSINATURA     ON ASSINATURA.CL000000000000ABCD000000000000     = OCORRENCIA.CLD172E6FAE63B4AD2AE76690C24F7
                                              LEFT JOIN TB21779E51EC44445F9A8F7E42E_TR PROBABILIDADE  ON PROBABILIDADE.CL000000000000ABCD000000000000  = OCORRENCIA.CL797D8B85A36C4C99BE1AA6E70728
                                                                                                     AND PROBABILIDADE.ID_IDIOMA_VERSAO                = '00000000-0000-0000-0000-000000000000'
                                              LEFT JOIN TB42C30BDA4C22450E8D1234088_TR SEVERIDADE     ON SEVERIDADE.CL000000000000ABCD000000000000     = OCORRENCIA.CL52A0FE3406DA4595A069F9E5A682
                                                                                                     AND SEVERIDADE.ID_IDIOMA_VERSAO                   = '00000000-0000-0000-0000-000000000000'
                                              LEFT JOIN TB83CC1F005E9144F28086DB731_TR RESULTADO      ON RESULTADO.CL000000000000ABCD000000000000      = OCORRENCIA.CLD69A7C64D86F45C2A64EE9B6C40C
                                                                                                     AND RESULTADO.ID_IDIOMA_VERSAO                    = '00000000-0000-0000-0000-000000000000'
                                             WHERE OCORRENCIA.CL000000000000ABCD000000000000 = '{0}'", idOcorrencia);

            DataTable dt = sessao.Contexto.BuscarTabela(sql);
            SDMobileXFDados.Modelos.OcorrenciaCsn ocorrencia = null;
            if (dt.Rows.Count > 0)
            {
                DataRow linha = dt.Rows[0];

                ocorrencia = new SDMobileXFDados.Modelos.OcorrenciaCsn()
                {
                    ID_OCORRENCIA = linha["ID_OCORRENCIA"].ToString(),
                    CODIGO = linha["NUMERO"].ToString(),
                    DATA = Convert.ToDateTime(linha["DATA"]),
                    TITULO = linha.IsNull("TITULO") ? null : linha["TITULO"].ToString(),

                    REGIAOSETOR = Util.GetModelo(linha, "REGIAO"),
                    DIA = Util.GetModelo(linha, "DIA"),
                    LETRA = Util.GetModelo(linha, "LETRA"),
                    TURNO = Util.GetModelo(linha, "TURNO"),

                    GERENCIAGERAL = Util.GetModelo(linha, "GERENCIA_GERAL"),
                    GERENCIA = Util.GetModelo(linha, "GERENCIA"),
                    AREAEQUIPAMENTO = Util.GetModelo(linha, "AREA"),
                    LOCALEQUIPAMENTO = Util.GetModelo(linha, "LOCAL"),
                    
                    LOCALANOMALIA = linha.IsNull("DS_LOCAL_ANOMALIA") ? null : linha["DS_LOCAL_ANOMALIA"].ToString(),
                    DESCRICAOPRELIMINAR = linha.IsNull("DS_PRELIMINAR") ? null : linha["DS_PRELIMINAR"].ToString(),
                    ACAOIMEDIATA = Util.GetModelo(linha, "ACOES_IMEDATAS"),
                    REMOCAOSINTOMAS = linha.IsNull("DS_REMOCAO_SINTOMAS") ? null : linha["DS_REMOCAO_SINTOMAS"].ToString(),
                    
                    TIPOANOMALIA = Util.GetModelo(linha, "TIPO"),
                    CLASSIFICACAOTIPO = Util.GetModelo(linha, "CLASSIFICACAO"),
                    ORIGEMANOMALIA = Util.GetModelo(linha, "ORIGEM"),
                    
                    REGISTRADOPOR = Util.GetModelo(linha, "REGISTRADO"),
                    RELATADOPOR = Util.GetModelo(linha, "RELATADO"),
                    SUPERVISOR = Util.GetModelo(linha, "SUPERVISOR"),
                    ASSINATURA = Util.GetModelo(linha, "ASSINATURA"),
                    
                    PROBABILIDADE = Util.GetModelo(linha, "PROBABILIDADE"),
                    SEVERIDADE = Util.GetModelo(linha, "SEVERIDADE"),
                    PRODUTOAB = linha.IsNull("PRODUTOAB") ? null : linha["PRODUTOAB"].ToString(),
                    RESULTADOCLASIFICACAO = Util.GetModelo(linha, "RESULTADO")
                };

            }
            return ocorrencia;
        }

        // POST: api/Ocorrencia
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Ocorrencia/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Ocorrencia/5
        public void Delete(int id)
        {
        }
    }
}
