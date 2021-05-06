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
    public class OcorrenciasController : ApiController
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
                Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

                Util.IniciarSessao(sessao, login);

                Guid idRegistro = Guid.NewGuid();
                Guid idTabela = new Guid("5824888c-9ed4-419f-90eb-bea77f717ec8");
                Guid idColuna = new Guid("c388c0ff-c35b-4680-aa4f-f9da4f39888c");

                using (RepositorioOcorrenciaAnomalia rep = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
                {
                    string numero = parametros.Get("NU_OCORRENCIA");
                    string descricao = parametros.Get("DESCRICAO").Replace("'", "''");
                    string sql = string.Empty;

                    if (string.IsNullOrEmpty(numero))
                    {
                        numero = Util.GerarSequencia(sessao.Contexto, idTabela, idColuna, idRegistro, DateTime.Today);

                        sql = @"INSERT INTO TB5824888C9ED4419F90EBBEA77F71 (  CL000000000000ABCD000000000000, CLC388C0FFC35B4680AA4FF9DA4F39, CL000000000000ABCD001100000000, 
                                                                                CL60727E23076D489085D693B3F656, CL747EE3D6B76D42259D9B248BC634, CLE8D2FB41B9E44AF4A02D3A14B9A2, 
                                                                                CL5CDDB175B6EC4FF193ACAFEA9B6D, CL1BA3CCE96716406A8CC8F5D5C4A8, CLBBBD21D7C00A4908BBF90A1D2E04, 
                                                                                CL63AF72F4170C4210B50B55664424, CL93D7CEADB7B84208BA44D204F82E, CLE2A5D31F697041BBA66417F21456, 
                                                                                CL871D3C3461C9405399EB2D2ECB38, CL361D9FD735BF4B68AF69544EFF66, CLEA1A305E37624CD0A829C659F318, 
                                                                                CLA608C8DFD8674761A9001BB2F46F, CL7EE5A052A86F45B983A5AD831585, CLB2A1EDEBC0444A19AA2FA2E73FE5,
                                                                                CLCB425A1BE63C4C5BB928F5910374) " +
                                $" VALUES ('{idRegistro}'                                , '{numero}'                               , TO_DATE('{parametros.Get("DATA")}', 'YYYYMMDD HH24MISS'), " +
                                $"         '{parametros.Get("ID_REGIONAL")}'             , '{parametros.Get("ID_GERENCIA")}'        , '{parametros.Get("ID_AREA")}', " +
                                $"         '{parametros.Get("ID_LOCAL")}'                , '{descricao}'                            , '{parametros.Get("ID_TIPO")}', " +
                                $"         '{parametros.Get("ID_CLASSIFICACAO")}'        , '{parametros.Get("ID_SUBCLASSIFICACAO")}', '{parametros.Get("ID_CATEGORIA")}', " +
                                $"         '{parametros.Get("ID_FORNECEDOR")}'           , '{parametros.Get("ACOES_IMEDIATAS")}'    , '{parametros.Get("DS_ACOES_IMEDIATAS")}', " +
                                $"         '{parametros.Get("NAO_QUERO_ME_IDENTIFICAR")}', '{parametros.Get("ID_COMUNICADO_POR")}'  , '{parametros.Get("ID_REGISTRADO_POR")}', 1) ";
                    }
                    else
                    {
                        idRegistro = new Guid(sessao.Contexto.BuscarValorStr($"SELECT CL000000000000ABCD000000000000 FROM TB5824888C9ED4419F90EBBEA77F71 WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}'"));

                        sql = " UPDATE TB5824888C9ED4419F90EBBEA77F71 " + 
                                $" SET CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}', " +
                                    $" CL000000000000ABCD001100000000 = TO_DATE('{parametros.Get("DATA")}', 'YYYYMMDD HH24MISS'), " +
                                    $" CL60727E23076D489085D693B3F656 = '{parametros.Get("ID_REGIONAL")}', " +
                                    $" CL747EE3D6B76D42259D9B248BC634 = '{parametros.Get("ID_GERENCIA")}', " +
                                    $" CLE8D2FB41B9E44AF4A02D3A14B9A2 = '{parametros.Get("ID_AREA")}', " +
                                    $" CL5CDDB175B6EC4FF193ACAFEA9B6D = '{parametros.Get("ID_LOCAL")}', " +
                                    $" CL1BA3CCE96716406A8CC8F5D5C4A8 = '{descricao}', " +
                                    $" CLBBBD21D7C00A4908BBF90A1D2E04 = '{parametros.Get("ID_TIPO")}', " +
                                    $" CL63AF72F4170C4210B50B55664424 = '{parametros.Get("ID_CLASSIFICACAO")}', " +
                                    $" CL93D7CEADB7B84208BA44D204F82E = '{parametros.Get("ID_SUBCLASSIFICACAO")}', " +
                                    $" CLE2A5D31F697041BBA66417F21456 = '{parametros.Get("ID_CATEGORIA")}', " +
                                    $" CL871D3C3461C9405399EB2D2ECB38 = '{parametros.Get("ID_FORNECEDOR")}', " +
                                    $" CL361D9FD735BF4B68AF69544EFF66 = '{parametros.Get("ACOES_IMEDIATAS")}', " +
                                    $" CLEA1A305E37624CD0A829C659F318 = '{parametros.Get("DS_ACOES_IMEDIATAS")}', " +
                                    $" CLA608C8DFD8674761A9001BB2F46F = '{parametros.Get("NAO_QUERO_ME_IDENTIFICAR")}', " +
                                    $" CL7EE5A052A86F45B983A5AD831585 = '{parametros.Get("ID_COMUNICADO_POR")}', " +
                                    $" CLB2A1EDEBC0444A19AA2FA2E73FE5 = '{parametros.Get("ID_REGISTRADO_POR")}'" +
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
        public List<SDMobileXFDados.Modelos.Ocorrencia> RetornarOcorrencias(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idVinculo = new Guid(parametros.Get("idVinculo"));
            string dtInicial = parametros.Get("dtInicial");
            string dtFinal = parametros.Get("dtFinal");
            string idsRegProfissional = parametros.Get("idsRegProfissional");
            bool papelMestre = Convert.ToBoolean(parametros.Get("papelMestre"));

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            using (RepositorioOcorrenciaAnomalia repositorio = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
            {                
                string sql = string.Format(@"SELECT OCOR.CL000000000000ABCD000000000000 ID_OCORRENCIA,
                                                    CLC388C0FFC35B4680AA4FF9DA4F39 CODIGO, 
                                                    TO_CHAR(CL000000000000ABCD001100000000, 'DD/MM/YYYY') DATA, 
                                                    CL1BA3CCE96716406A8CC8F5D5C4A8 DESCRICAO,                                            
                                                    REGI.CL000000000000ABCD000300000000 DS_REGIONAL,                                        
                                                    GERE.CL000000000000ABCD000300000000 DS_GERENCIA,                                        
                                                    AREA.CLD79FBD89F9CC4808BEA6E8D732D6 DS_AREA,                                            
                                                    FC_SITUACAO_FOLLOWUP2(  OCOR.CL000000000000ABCD000000000000, 
                                                                            (SELECT COUNT(*) FROM TB1951B7A0ED8D41CF875779C7EF22 PLANO 
                                                                              WHERE PLANO.CL000000000000ABCD001000000000 = OCOR.CL000000000000ABCD000000000000), 
                                                                            OCOR.CL062725DEC7BB4040927156C70452, 
                                                                            NULL, 
                                                                            OCOR.CL361D9FD735BF4B68AF69544EFF66, 
                                                                            OCOR.CLB2240E5153A84520A047D7B7C8E8, 
                                                                            OCOR.CL63AF72F4170C4210B50B55664424) STATUS                                                 
                                               FROM TB5824888C9ED4419F90EBBEA77F71 OCOR 
                                               LEFT JOIN TB781D63F32AEA4423B070E7EB190C REGI ON OCOR.CL60727E23076D489085D693B3F656 = REGI.CL000000000000ABCD000000000000
                                               LEFT JOIN TB271A941798BA43378CEE3CDC3A9D GERE ON OCOR.CL747EE3D6B76D42259D9B248BC634 = GERE.CL000000000000ABCD000000000000
                                               LEFT JOIN TB69A8C990799C4B77B4F0D1CE8ECA AREA ON OCOR.CLE8D2FB41B9E44AF4A02D3A14B9A2 = AREA.CL000000000000ABCD000000000000 
                                              WHERE CL000000000000ABCD001100000000 >= TO_DATE('{0}', 'dd/MM/yyyy') 
                                                AND CL000000000000ABCD001100000000 < (TO_DATE('{1}', 'dd/MM/yyyy') + 1)", dtInicial, dtFinal);
                if (!papelMestre)
                {
                    string sqlRegProfissional = string.Empty;
                    if (!string.IsNullOrEmpty(idsRegProfissional)) //Regionais e areas
                    {
                        sqlRegProfissional += string.Format(@"OR OCOR.CL60727E23076D489085D693B3F656 IN
                                                (SELECT ID_REGISTRO FROM TB781D63F32AEA4423B070E7EB1_MV WHERE CLBAAC0265C4984D2084C6BA032E87 IN ({0}))
                                                          OR OCOR.CL60727E23076D489085D693B3F656 IN
                                                (SELECT ID_REGISTRO FROM TB69A8C990799C4B77B4F0D1CE8_MV WHERE CLE8D2FB41B9E44AF4A02D3A14B9A2 IN ({0}))", idsRegProfissional);
                    }
                    sql += string.Format(" AND (CL7EE5A052A86F45B983A5AD831585 = '{0}' OR CLB2A1EDEBC0444A19AA2FA2E73FE5 = '{0}' {1})", 
                        idVinculo, sqlRegProfissional);
                    
                }
                sql += " ORDER BY CL000000000000ABCD001100000000 DESC";

                return repositorio.GetDados<SDMobileXFDados.Modelos.Ocorrencia>(sql).ToList();
            }
        }

        [HttpPost]
        public SDMobileXFDados.Modelos.Ocorrencia RetornarOcorrencia(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idOcorrencia = new Guid(parametros.Get("idOcorrencia"));

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            string sql = string.Format(@"SELECT OCOR.CL000000000000ABCD000000000000 ID_OCORRENCIA,
                                                CLC388C0FFC35B4680AA4FF9DA4F39 CODIGO, 
                                                TO_CHAR(CL000000000000ABCD001100000000, 'DD/MM/YYYY') DATA, 
                                                CL000000000000ABCD001100000000 HORA,                                            
                                                OCOR.CL60727E23076D489085D693B3F656 ID_REGIONAL,
                                                OCOR.CL747EE3D6B76D42259D9B248BC634 ID_GERENCIA,
                                                OCOR.CLE8D2FB41B9E44AF4A02D3A14B9A2 ID_AREA,
                                                OCOR.CL5CDDB175B6EC4FF193ACAFEA9B6D ID_LOCAL,
                                                OCOR.CLBBBD21D7C00A4908BBF90A1D2E04 ID_TIPO,
                                                OCOR.CL63AF72F4170C4210B50B55664424 ID_CLASSIFICACAO,
                                                OCOR.CL93D7CEADB7B84208BA44D204F82E ID_SUBCLASSIFICACAO,
                                                OCOR.CLE2A5D31F697041BBA66417F21456 ID_CATEGORIA,
                                                OCOR.CL871D3C3461C9405399EB2D2ECB38 ID_FORNECEDOR,
                                                OCOR.CL7EE5A052A86F45B983A5AD831585 ID_COMUNICADOPOR,
                                                OCOR.CLB2A1EDEBC0444A19AA2FA2E73FE5 ID_REGISTRADOPOR,
                                                REGI.CL000000000000ABCD000200000000 CD_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000200000000 CD_GERENCIA,                                        
                                                AREA.CLFD7DF78DB1A446B292F9C76515EC CD_AREA,
                                                LOCAL.CL000000000000ABCD000200000000 CD_LOCAL,
                                                TIPO.CL000000000000ABCD000200000000 CD_TIPO,
                                                CLASS.CL000000000000ABCD000200000000 CD_CLASSIFICACAO,
                                                SUBCLASS.CL000000000000ABCD000200000000 CD_SUBCLASSIFICACAO,
                                                CATEGORIA.CL000000000000ABCD000200000000 CD_CATEGORIA,
                                                FORNECEDOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_FORNECEDOR,
                                                COMUNICADOPOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_COMUNICADOPOR,
                                                REGISTRADOPOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_REGISTRADOPOR,
                                                REGI.CL000000000000ABCD000300000000 DS_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000300000000 DS_GERENCIA,                                        
                                                AREA.CLD79FBD89F9CC4808BEA6E8D732D6 DS_AREA,
                                                LOCAL.CL000000000000ABCD000300000000 DS_LOCAL,
                                                TIPO.CL000000000000ABCD000300000000 DS_TIPO,
                                                CLASS.CL000000000000ABCD000300000000 DS_CLASSIFICACAO,
                                                SUBCLASS.CL000000000000ABCD000300000000 DS_SUBCLASSIFICACAO,
                                                CATEGORIA.CL000000000000ABCD000300000000 DS_CATEGORIA,
                                                FORNECEDOR.CL10914D4F3685436282FBBC16F662 DS_FORNECEDOR,
                                                CADPESSOAL.CL58284C47272B4729ABF5215F368D DS_COMUNICADOPOR,
                                                CADPESSOAL1.CL58284C47272B4729ABF5215F368D DS_REGISTRADOPOR,
                                                OCOR.CL1BA3CCE96716406A8CC8F5D5C4A8 DESCRICAO,
                                                OCOR.CL062725DEC7BB4040927156C70452 ACEITE,                                            
                                                FC_SITUACAO_FOLLOWUP2(OCOR.CL000000000000ABCD000000000000, (SELECT COUNT(*) FROM TB1951B7A0ED8D41CF875779C7EF22 PLANO WHERE 
                                                PLANO.CL000000000000ABCD001000000000 = OCOR.CL000000000000ABCD000000000000), OCOR.CL062725DEC7BB4040927156C70452, NULL, 
                                                OCOR.CL361D9FD735BF4B68AF69544EFF66, OCOR.CLB2240E5153A84520A047D7B7C8E8, 
                                                OCOR.CL63AF72F4170C4210B50B55664424) STATUS,
                                                OCOR.CL361D9FD735BF4B68AF69544EFF66 ST_ACAOIMEDIATA,
                                                OCOR.CLEA1A305E37624CD0A829C659F318 DS_ACAOIMEDIATA,
                                                OCOR.CLB2240E5153A84520A047D7B7C8E8 ST_ACAOIMEDIATAEFICAZ,
                                                OCOR.CLA608C8DFD8674761A9001BB2F46F ST_NAOQUEROIDENTIFICAR
                                           FROM TB5824888C9ED4419F90EBBEA77F71 OCOR 
                                           LEFT JOIN TB781D63F32AEA4423B070E7EB190C REGI          ON OCOR.CL60727E23076D489085D693B3F656 = REGI.CL000000000000ABCD000000000000
                                           LEFT JOIN TB271A941798BA43378CEE3CDC3A9D GERE          ON OCOR.CL747EE3D6B76D42259D9B248BC634 = GERE.CL000000000000ABCD000000000000
                                           LEFT JOIN TB69A8C990799C4B77B4F0D1CE8ECA AREA          ON OCOR.CLE8D2FB41B9E44AF4A02D3A14B9A2 = AREA.CL000000000000ABCD000000000000
                                           LEFT JOIN TB6BB39BD086DD4DBCAF41AB4149E2 LOCAL         ON LOCAL.CL000000000000ABCD000000000000 = OCOR.CL5CDDB175B6EC4FF193ACAFEA9B6D
                                           LEFT JOIN TB40A18BA0B6F04B3ABD7A82AB8E05 TIPO          ON TIPO.CL000000000000ABCD000000000000 = OCOR.CLBBBD21D7C00A4908BBF90A1D2E04
                                           LEFT JOIN TB5E5BE4B0C71E4F3B97E471EFBA69 CLASS         ON  CLASS.CL000000000000ABCD000000000000 = OCOR.CL63AF72F4170C4210B50B55664424
                                           LEFT JOIN TB7DDBD552E69D4721B68BE65F4735 SUBCLASS      ON SUBCLASS.CL000000000000ABCD000000000000 = OCOR.CL93D7CEADB7B84208BA44D204F82E
                                           LEFT JOIN TB5710DEF79A934512910FDD238152 CATEGORIA     ON CATEGORIA.CL000000000000ABCD000000000000 = OCOR.CLE2A5D31F697041BBA66417F21456
                                           LEFT JOIN TBFB7C40BCD3E2487FBFFD7BC6F98F FORNECEDOR    ON FORNECEDOR.CL000000000000ABCD000000000000 = OCOR.CL871D3C3461C9405399EB2D2ECB38
                                           LEFT JOIN TB831F15D403654FCE84691FE856D2 COMUNICADOPOR ON COMUNICADOPOR.CL000000000000ABCD000000000000 = OCOR.CL7EE5A052A86F45B983A5AD831585
                                           LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL    ON COMUNICADOPOR.CL81F753A75F3B415CAB2037E094BA = CADPESSOAL.CL000000000000ABCD000000000000                                            
                                           LEFT JOIN TB831F15D403654FCE84691FE856D2 REGISTRADOPOR ON REGISTRADOPOR.CL000000000000ABCD000000000000 = OCOR.CLB2A1EDEBC0444A19AA2FA2E73FE5
                                           LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL1   ON REGISTRADOPOR.CL81F753A75F3B415CAB2037E094BA = CADPESSOAL1.CL000000000000ABCD000000000000
                                          WHERE OCOR.CL000000000000ABCD000000000000 = '{0}'", idOcorrencia);

            DataTable dt = sessao.Contexto.BuscarTabela(sql);
            SDMobileXFDados.Modelos.Ocorrencia ocorrencia = null;
            if (dt.Rows.Count > 0)
            {
                DataRow linha = dt.Rows[0];

                ocorrencia = new SDMobileXFDados.Modelos.Ocorrencia()
                {
                    ID_OCORRENCIA = linha["ID_OCORRENCIA"].ToString(),
                    CODIGO = linha["CODIGO"].ToString(),
                    DATA = Convert.ToDateTime(linha["DATA"]),
                    HORA = Convert.ToDateTime(linha["HORA"]).TimeOfDay,
                    UNIDADEREGIONAL = linha.IsNull("ID_REGIONAL") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_REGIONAL"])),
                        Codigo = Convert.ToString(linha["CD_REGIONAL"]),
                        Descricao = Convert.ToString(linha["DS_REGIONAL"])
                    },
                    GERENCIA = linha.IsNull("ID_GERENCIA") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_GERENCIA"])),
                        Codigo = Convert.ToString(linha["CD_GERENCIA"]),
                        Descricao = Convert.ToString(linha["DS_GERENCIA"])
                    },
                    AREA = linha.IsNull("ID_AREA") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_AREA"])),
                        Codigo = Convert.ToString(linha["CD_AREA"]),
                        Descricao = Convert.ToString(linha["DS_AREA"])
                    },
                    LOCAL = linha.IsNull("ID_LOCAL") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_LOCAL"])),
                        Codigo = Convert.ToString(linha["CD_LOCAL"]),
                        Descricao = Convert.ToString(linha["DS_LOCAL"])
                    },
                    DESCRICAO = Convert.ToString(linha["DESCRICAO"]),
                    TIPO = linha.IsNull("ID_TIPO") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_TIPO"])),
                        Codigo = Convert.ToString(linha["CD_TIPO"]),
                        Descricao = Convert.ToString(linha["DS_TIPO"])
                    },
                    CLASSIFICACAO = linha.IsNull("ID_CLASSIFICACAO") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_CLASSIFICACAO"])),
                        Codigo = Convert.ToString(linha["CD_CLASSIFICACAO"]),
                        Descricao = Convert.ToString(linha["DS_CLASSIFICACAO"])
                    },
                    SUBCLASSIFICACAO = linha.IsNull("ID_SUBCLASSIFICACAO") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_SUBCLASSIFICACAO"])),
                        Codigo = Convert.ToString(linha["CD_SUBCLASSIFICACAO"]),
                        Descricao = Convert.ToString(linha["DS_SUBCLASSIFICACAO"])
                    },
                    CATEGORIA = linha.IsNull("ID_CATEGORIA") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_CATEGORIA"])),
                        Codigo = Convert.ToString(linha["CD_CATEGORIA"]),
                        Descricao = Convert.ToString(linha["DS_CATEGORIA"])
                    },
                    FORNECEDOR = linha.IsNull("ID_FORNECEDOR") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_FORNECEDOR"])),
                        Codigo = Convert.ToString(linha["CD_FORNECEDOR"]),
                        Descricao = Convert.ToString(linha["DS_FORNECEDOR"])
                    },
                    ACAOIMEDIATA = linha["ST_ACAOIMEDIATA"] == null ? string.Empty : Convert.ToString(linha["ST_ACAOIMEDIATA"]),
                    DESCRICAOACOESIMEDIATAS = Convert.ToString(linha["DS_ACAOIMEDIATA"]),
                    NAOQUEROMEIDENTIFICAR = linha["ST_NAOQUEROIDENTIFICAR"] == System.DBNull.Value ? false : Convert.ToBoolean(linha["ST_NAOQUEROIDENTIFICAR"]),
                    COMUNICADOPOR = linha.IsNull("ID_COMUNICADOPOR") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_COMUNICADOPOR"])),
                        Codigo = Convert.ToString(linha["CD_COMUNICADOPOR"]),
                        Descricao = Convert.ToString(linha["DS_COMUNICADOPOR"])
                    },
                    REGISTRADOPOR = linha.IsNull("ID_REGISTRADOPOR") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_REGISTRADOPOR"])),
                        Codigo = Convert.ToString(linha["CD_REGISTRADOPOR"]),
                        Descricao = Convert.ToString(linha["DS_REGISTRADOPOR"])
                    }
                };

                ocorrencia.ANEXOS = new List<DocAnexo>();
                using (RepositorioDocAnexo repAnexos = new RepositorioDocAnexo(sessao.Contexto))
                {
                    IEnumerable<DOC_ANEXO> anexos = repAnexos.RetornarItensPorIdRegistro("TB5824888C9ED4419F90EBBEA77F71", "CL7477547A7A82406C8C10CA08C1DA", new Guid(ocorrencia.ID_OCORRENCIA));
                    if(anexos != null)
                    {
                        foreach (DOC_ANEXO item in anexos)
                            if(item.DS_TIPO_ANEXO.ToUpper().Equals(".JPG") || item.DS_TIPO_ANEXO.ToUpper().Equals(".PNG"))
                                ocorrencia.ANEXOS.Add(new DocAnexo() { NM_ANEXO = item.NM_ANEXO, DS_TIPO_ANEXO = item.DS_TIPO_ANEXO, BI_ANEXO = item.BI_ANEXO });
                    }
                }

            }
            return ocorrencia;
        }

        [HttpPost]
        public HttpResponseMessage UploadImagem()
        {
            HttpResponseMessage result = null;
            string pasta = HttpContext.Current.Server.MapPath("~/App_Data/");
            try
            {
                HttpRequest httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {

                    Guid idChaveSessao = new Guid(httpRequest.Headers.Get("idChaveSessao"));
                    string IdOcocrrencia = httpRequest.Headers.Get("ID_OCORRENCIA");
                    Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

                    List<string> lstfiles = new List<string>();

                    foreach (string file in httpRequest.Files)
                    {
                        HttpPostedFile postedFile = httpRequest.Files[file];
                        using (RepositorioDocAnexo repAnexo = new RepositorioDocAnexo(sessao.Contexto))
                        {
                            string filePath = Path.Combine(pasta, postedFile.FileName);
                            //postedFile.SaveAs(filePath);
                            lstfiles.Add(filePath);

                            string nmArquivo = Path.GetFileName(postedFile.FileName);

                            string nmTabela = "TB5824888C9ED4419F90EBBEA77F71";
                            string nmColuna = "CL7477547A7A82406C8C10CA08C1DA";

                            IEnumerable<DOC_ANEXO> anexosAtuais = repAnexo.RetornarItensPorIdRegistroSemLOB(nmTabela, nmColuna, new Guid(IdOcocrrencia));
                            DOC_ANEXO anexo = anexosAtuais.FirstOrDefault(a => a.NM_ANEXO == nmArquivo);
                            if (anexo != null)
                                sessao.Contexto.Executar($"DELETE DOC_ANEXO " +
                                                         $" WHERE NM_TABELA = '{nmTabela}' " +
                                                         $"   AND NM_COLUNA = '{nmColuna}' " +
                                                         $"   AND ID_REGISTRO = '{IdOcocrrencia}' " +
                                                         $"   AND CL000000000000ABCD000000000000 = '{anexo.CL000000000000ABCD000000000000}'");

                            using (BinaryReader binaryReader = new BinaryReader(postedFile.InputStream))
                            {
                                string biDocumento = Convert.ToBase64String(binaryReader.ReadBytes(postedFile.ContentLength));
                                repAnexo.Incluir(nmTabela, nmColuna, new Guid(IdOcocrrencia), nmArquivo, biDocumento);
                            }
                        }
                    }

                    sessao.Contexto.Salvar();

                    result = Request.CreateResponse(HttpStatusCode.Created, lstfiles);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return result;
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
