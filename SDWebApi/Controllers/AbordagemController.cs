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
    public class AbordagemController : ApiController
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
                Guid idTabela = new Guid("f4e1fe96-d697-4db4-8121-74d2258a47b7");
                Guid idColuna = new Guid("c388c0ff-c35b-4680-aa4f-f9da4f39888c");

                string numero = parametros.Get("NU_ABORDAGEM");
                string descricao = parametros.Get("DESCRICAO").Replace("'", "''");
                string sql = string.Empty;

                if (string.IsNullOrEmpty(numero))
                {
                    numero = Util.GerarSequencia(sessao.Contexto, idTabela, idColuna, idRegistro, DateTime.Today);

                    sql = @"INSERT INTO TBF4E1FE96D6974DB4812174D2258A (    CL000000000000ABCD000000000000, CLC388C0FFC35B4680AA4FF9DA4F39, CL000000000000ABCD001100000000, 
                                                                                CL9A028F24FC1E448A8B9B4920BADE, CL60727E23076D489085D693B3F656, CLB385850AAF9A4AD1B30AF0BABE9E, 
                                                                                CLE8D2FB41B9E44AF4A02D3A14B9A2, CL5CDDB175B6EC4FF193ACAFEA9B6D, CL000000000000ABCD000300000000,

                                                                                CL5E7110542FF345269D7EB1FFFBB3, CLED5F06A024B34A6C8565889926DB,
                                                                                CL8F7A4680DE224C43A948896FD6F1, CLAC37D53C0E0B4D96900BA09C6289,
                                                                                CL2A00C8A10E2747A99DE33CC33EB8, CL5380925B994B49929E7373A31F64,
                                                                                CL27A2A87514304055928A60E8B7D4, CL8D608570E3FC41328A37483F70A5,

                                                                                CLD172E6FAE63B4AD2AE76690C24F7, CL43E41CB9B8CF4187AD84C277D3BC,
                                                                                CLA74D67236DC54D42A48F0EDE0B78, CLDDC178173B144B7DA21D0C54DCDD,
                                                                                CL63AF72F4170C4210B50B55664424, CL15E3DE414B4D4395AD7F971BB350,
                                                                                CL5D338E69148C4AD7A1AA629BBAC4, CL33C821F1C43645059F7D104FCAF9,
                                                                                CLFC0A5487EF054DD987189FC7488B, CLB3D5CBD657734E8591E45D3F5379,

                                                                                CLE70A4BFEA61947719B5F5D1A01DE, CL91194D9CE9C4400AA4639B61F956,
                                                                                CL3ABC342EE7AB4F6CA6604C3A3529, CLE9A8B9A4D9C14C0A9AEE7B3F89B4,
                                                                                CL76F46269DE2F44FEB20B5F003117, CL7F1E1C38F39B45739507CD6065A3,

                                                                                CLA9866B527A584D7C800923E06BD1, CL9A18C31C9E9E4668ABE1837D8E0C,
                                                                                CLBCDE9494C65F439F9F7ADEA6F440, CL2647049882C9417DB5BDE0829EB8,
                                                                                CL47707576CC9B442F90FDA0FA49D6, CLB7634EAB461E42D68B2A37CE2E88,
                                                                                CL7FD13555B91246FAB9AA882BD1EA, CLAF47A26BADCF4E4A9457F9D7FE4A,

                                                                                CLF3D824AA640D41B8B42B71DE7889, CL8959A65CD28F4A8682167FFF26B5,
                                                                                CL8D3326314B124A13AF1B157C13F9, CL26CAAB88CAE247FFB4520EC73814,
                                                                                CL3C950A657E1A4FD48382C9A4285E, CL8212F8D832394EE2A9FE1B49B930,
                                                                                CLB902473D1C6B490195C14E304540, CL6276F82518294232A21DD4791405,
                                                                                CL062725DEC7BB4040927156C70452, CLE53214B1321D4F44B8E361BF80CE,

                                                                                CL628EB1D6788B4B7C8E480F7210C7, CL537D49B88A92494BBF27C42B8D99 ) " +
                            $" VALUES ('{idRegistro}'                               , '{numero}'                                            , TO_DATE('{parametros.Get("DATA")}', 'YYYYMMDD HH24MISS'), " +
                            $"         '{parametros.Get("ID_FORNECEDOR")}'          , '{parametros.Get("ID_REGIONAL")}'                     , '{parametros.Get("ID_GERENCIA")}', " +
                            $"         '{parametros.Get("ID_AREA")}'                , '{parametros.Get("ID_LOCAL")}'                        , '{parametros.Get("DESCRICAO")}', " +

                            $"         '{parametros.Get("ID_INERENTES_ATIVIDADE")}'                 , {parametros.Get("ST_INERENTES_ATIVIDADE_VER_AGIR")} , " +
                            $"         '{parametros.Get("ID_RELACAO_COM_RISCOS")}'                  , {parametros.Get("ST_RELACAO_COM_RISCOS_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_CONSERVACAO_ADEQUADA")}'                , {parametros.Get("ST_CONSERVACAO_ADEQUADA_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA")}', {parametros.Get("ST_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA_VER_AGIR")},  " +

                            $"         '{parametros.Get("ID_IDENTIFICACAO_RISCOS_MAPEADOS")}', {parametros.Get("ST_IDENTIFICACAO_RISCOS_MAPEADOS_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_MEDIDAS_PREVENCAO")}'            , {parametros.Get("ST_MEDIDAS_PREVENCAO_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_BOAS_CONDICOES_USO")}'           , {parametros.Get("ST_BOAS_CONDICOES_USO_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_UTILIZACAO_CORRETA")}'           , {parametros.Get("ST_UTILIZACAO_CORRETA_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_DESTINADOS_ATIVIDADE")}'         , {parametros.Get("ST_DESTINADOS_ATIVIDADE_VER_AGIR")},  " +

                            $"         '{parametros.Get("ID_LOCAL_LIMPO")}'          , {parametros.Get("ST_LOCAL_LIMPO_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_MATERIAIS_ORGANIZADOS")}', {parametros.Get("ST_MATERIAIS_ORGANIZADOS_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_DESCARTE_RESIDUOS")}'    , {parametros.Get("ST_DESCARTE_RESIDUOS_VER_AGIR")},  " +

                            $"         '{parametros.Get("ID_IDENTIFICACAO_TRATATIVA_RISCOS")}', {parametros.Get("ST_IDENTIFICACAO_TRATATIVA_RISCOS_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_LINHA_FOGO")}'                    , {parametros.Get("ST_LINHA_FOGO_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_POSTURAS_ERGO_ADEQUADAS")}'       , {parametros.Get("ST_POSTURAS_ERGO_ADEQUADAS_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_CONCENTRACAO_TAREFA")}'           , {parametros.Get("ST_CONCENTRACAO_TAREFA_VER_AGIR")},  " +

                            $"         '{parametros.Get("ID_CONHECIMENTO_PROCEDIMENTOS")}', {parametros.Get("ST_CONHECIMENTO_PROCEDIMENTOS_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_CONHECIMENTO_RISCOS")}'       , {parametros.Get("ST_CONHECIMENTO_RISCOS_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_DIREITO_RECUSA")}'            , {parametros.Get("ST_DIREITO_RECUSA_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_ACOES_EMERGENCIA")}'          , {parametros.Get("ST_ACOES_EMERGENCIA_VER_AGIR")},  " +
                            $"         '{parametros.Get("ID_REALIZA_PROCESSOROTINA")}'    , {parametros.Get("ST_REALIZA_PROCESSOROTINA_VER_AGIR")},  " +

                            $"         '{parametros.Get("ID_OBSERVADOR")}', '{parametros.Get("ID_REGISTRADOPOR")}') ";
                }
                else
                {
                    idRegistro = new Guid(sessao.Contexto.BuscarValorStr($"SELECT CL000000000000ABCD000000000000 FROM TBF4E1FE96D6974DB4812174D2258A WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}'"));

                    sql = "UPDATE TBF4E1FE96D6974DB4812174D2258A " +
                            $"SET CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}', " +
                            $"    CL000000000000ABCD001100000000 = TO_DATE('{parametros.Get("DATA")}', 'YYYYMMDD HH24MISS'),  " +
                            $"    CL9A028F24FC1E448A8B9B4920BADE = '{parametros.Get("ID_FORNECEDOR")}', " +
                            $"    CL60727E23076D489085D693B3F656 = '{parametros.Get("ID_REGIONAL")}', " +
                            $"    CLB385850AAF9A4AD1B30AF0BABE9E = '{parametros.Get("ID_GERENCIA")}', " +
                            $"    CLE8D2FB41B9E44AF4A02D3A14B9A2 = '{parametros.Get("ID_AREA")}', " +
                            $"    CL5CDDB175B6EC4FF193ACAFEA9B6D = '{parametros.Get("ID_LOCAL")}', " +
                            $"    CL000000000000ABCD000300000000 = '{parametros.Get("DESCRICAO")}'," +

                            $"    CL5E7110542FF345269D7EB1FFFBB3 = '{parametros.Get("ID_INERENTES_ATIVIDADE")}', " +
                            $"    CLED5F06A024B34A6C8565889926DB = {parametros.Get("ST_INERENTES_ATIVIDADE_VER_AGIR")}, " +
                            $"    CL8F7A4680DE224C43A948896FD6F1 = '{parametros.Get("ID_RELACAO_COM_RISCOS")}', " +
                            $"    CLAC37D53C0E0B4D96900BA09C6289 = {parametros.Get("ST_RELACAO_COM_RISCOS_VER_AGIR")}, " +
                            $"    CL2A00C8A10E2747A99DE33CC33EB8 = '{parametros.Get("ID_CONSERVACAO_ADEQUADA")}', " +
                            $"    CL5380925B994B49929E7373A31F64 = {parametros.Get("ST_CONSERVACAO_ADEQUADA_VER_AGIR")}, " +
                            $"    CL27A2A87514304055928A60E8B7D4 = '{parametros.Get("ID_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA")}', " +
                            $"    CL8D608570E3FC41328A37483F70A5 = {parametros.Get("ST_UTILIZACAO_CORRETA_FIXACAO_DISTANCIA_VER_AGIR")}, " +

                            $"    CLD172E6FAE63B4AD2AE76690C24F7 = '{parametros.Get("ID_IDENTIFICACAO_RISCOS_MAPEADOS")}', " +
                            $"    CL43E41CB9B8CF4187AD84C277D3BC = {parametros.Get("ST_IDENTIFICACAO_RISCOS_MAPEADOS_VER_AGIR")}, " +
                            $"    CLA74D67236DC54D42A48F0EDE0B78 = '{parametros.Get("ID_MEDIDAS_PREVENCAO")}', " +
                            $"    CLDDC178173B144B7DA21D0C54DCDD = {parametros.Get("ST_MEDIDAS_PREVENCAO_VER_AGIR")}, " +
                            $"    CL63AF72F4170C4210B50B55664424 = '{parametros.Get("ID_BOAS_CONDICOES_USO")}', " +
                            $"    CL15E3DE414B4D4395AD7F971BB350 = {parametros.Get("ST_BOAS_CONDICOES_USO_VER_AGIR")}, " +
                            $"    CL5D338E69148C4AD7A1AA629BBAC4 = '{parametros.Get("ID_UTILIZACAO_CORRETA")}', " +
                            $"    CL33C821F1C43645059F7D104FCAF9 = {parametros.Get("ST_UTILIZACAO_CORRETA_VER_AGIR")}, " +
                            $"    CLFC0A5487EF054DD987189FC7488B = '{parametros.Get("ID_DESTINADOS_ATIVIDADE")}', " +
                            $"    CLB3D5CBD657734E8591E45D3F5379 = {parametros.Get("ST_DESTINADOS_ATIVIDADE_VER_AGIR")}," +

                            $"    CLE70A4BFEA61947719B5F5D1A01DE = '{parametros.Get("ID_LOCAL_LIMPO")}', " +
                            $"    CL91194D9CE9C4400AA4639B61F956 = {parametros.Get("ST_LOCAL_LIMPO_VER_AGIR")}, " +
                            $"    CL3ABC342EE7AB4F6CA6604C3A3529 = '{parametros.Get("ID_MATERIAIS_ORGANIZADOS")}', " +
                            $"    CLE9A8B9A4D9C14C0A9AEE7B3F89B4 = {parametros.Get("ST_MATERIAIS_ORGANIZADOS_VER_AGIR")}, " +
                            $"    CL76F46269DE2F44FEB20B5F003117 = '{parametros.Get("ID_DESCARTE_RESIDUOS")}', " +
                            $"    CL7F1E1C38F39B45739507CD6065A3 = {parametros.Get("ST_DESCARTE_RESIDUOS_VER_AGIR")}, " +

                            $"    CLA9866B527A584D7C800923E06BD1 = '{parametros.Get("ID_IDENTIFICACAO_TRATATIVA_RISCOS")}', " +
                            $"    CL9A18C31C9E9E4668ABE1837D8E0C = {parametros.Get("ST_IDENTIFICACAO_TRATATIVA_RISCOS_VER_AGIR")}, " +
                            $"    CLBCDE9494C65F439F9F7ADEA6F440 = '{parametros.Get("ID_LINHA_FOGO")}'," +
                            $"    CL2647049882C9417DB5BDE0829EB8 = {parametros.Get("ST_LINHA_FOGO_VER_AGIR")}, " +
                            $"    CL47707576CC9B442F90FDA0FA49D6 = '{parametros.Get("ID_POSTURAS_ERGO_ADEQUADAS")}', " +
                            $"    CLB7634EAB461E42D68B2A37CE2E88 = {parametros.Get("ST_POSTURAS_ERGO_ADEQUADAS_VER_AGIR")}, " +
                            $"    CL7FD13555B91246FAB9AA882BD1EA = '{parametros.Get("ID_CONCENTRACAO_TAREFA")}', " +
                            $"    CLAF47A26BADCF4E4A9457F9D7FE4A = {parametros.Get("ST_CONCENTRACAO_TAREFA_VER_AGIR")}, " +

                            $"    CLF3D824AA640D41B8B42B71DE7889 = '{parametros.Get("ID_CONHECIMENTO_PROCEDIMENTOS")}', " +
                            $"    CL8959A65CD28F4A8682167FFF26B5 = {parametros.Get("ST_CONHECIMENTO_PROCEDIMENTOS_VER_AGIR")}, " +
                            $"    CL8D3326314B124A13AF1B157C13F9 = '{parametros.Get("ID_CONHECIMENTO_RISCOS")}', " +
                            $"    CL26CAAB88CAE247FFB4520EC73814 = {parametros.Get("ST_CONHECIMENTO_RISCOS_VER_AGIR")}, " +
                            $"    CL3C950A657E1A4FD48382C9A4285E = '{parametros.Get("ID_DIREITO_RECUSA")}', " +
                            $"    CL8212F8D832394EE2A9FE1B49B930 = {parametros.Get("ST_DIREITO_RECUSA_VER_AGIR")}, " +
                            $"    CLB902473D1C6B490195C14E304540 = '{parametros.Get("ID_ACOES_EMERGENCIA")}', " +
                            $"    CL6276F82518294232A21DD4791405 = {parametros.Get("ST_ACOES_EMERGENCIA_VER_AGIR")}, " +
                            $"    CL062725DEC7BB4040927156C70452 = '{parametros.Get("ID_REALIZA_PROCESSOROTINA")}', " +
                            $"    CLE53214B1321D4F44B8E361BF80CE = {parametros.Get("ST_REALIZA_PROCESSOROTINA_VER_AGIR")}, " +

                            $"    CL628EB1D6788B4B7C8E480F7210C7 = '{parametros.Get("ID_OBSERVADOR")}', " +
                            $"    CL537D49B88A92494BBF27C42B8D99 = '{parametros.Get("ID_REGISTRADOPOR")}' " +
                         $" WEHRE CL000000000000ABCD000000000000 = '{idRegistro}'";
                }
                try
                {
                    sessao.Contexto.Executar(sql);

                    sessao.Contexto.Executar($"DELETE TBF4E1FE96D6974DB4812174D22_MV WHERE ID_REGISTRO = '{idRegistro}'");

                    string[] cognitivos = parametros.Get("COGNITIVOS").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in cognitivos)
                        sessao.Contexto.Executar($"INSERT INTO TBF4E1FE96D6974DB4812174D22_MV (CL000000000000ABCD000000000000, ID_REGISTRO, NM_TABELA, CL000000000000ABCD001300000000)" +
                                                    $"VALUES (FC_GUID, '{idRegistro}', 'TB2D73CA163C2A48EBAB5186A84E3F', '{id}')");

                    string[] fisiologicos = parametros.Get("FISIOLOGICOS").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in fisiologicos)
                        sessao.Contexto.Executar($"INSERT INTO TBF4E1FE96D6974DB4812174D22_MV (CL000000000000ABCD000000000000, ID_REGISTRO, NM_TABELA, CLBAAC0265C4984D2084C6BA032E87)" +
                                                    $"VALUES (FC_GUID, '{idRegistro}', 'TBE5472D31841F4B378A8D5213B340', '{id}')");

                    string[] psicologicos = parametros.Get("PSICOLOGICOS").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in psicologicos)
                        sessao.Contexto.Executar($"INSERT INTO TBF4E1FE96D6974DB4812174D22_MV (CL000000000000ABCD000000000000, ID_REGISTRO, NM_TABELA, CLFB5EA87439DD42AEB3EC87C9ED67)" +
                                                    $"VALUES (FC_GUID, '{idRegistro}', 'TBE7573BF58398476E8AAB9E8ACCC7', '{id}')");

                    string[] sociais = parametros.Get("SOCIAIS").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in sociais)
                        sessao.Contexto.Executar($"INSERT INTO TBF4E1FE96D6974DB4812174D22_MV (CL000000000000ABCD000000000000, ID_REGISTRO, NM_TABELA, CL46053660BBF8474DA95D07632855)" +
                                                    $"VALUES (FC_GUID, '{idRegistro}', 'TBAF75B1565235479ABAFB2DFB3511', '{id}')");

                    return Request.CreateResponse(HttpStatusCode.OK, idRegistro + "|" + numero);
                }
                catch (Exception exBanco)
                {
                    erro = exBanco.Message;
                    Util.ExcluirSequencia(sessao.Contexto, idTabela, idRegistro);
                }
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, erro);
        }

        [HttpPost]
        public List<Abordagem> RetornarAbordagens(FormDataCollection parametros)
        {
            List<Abordagem> lst = new List<Abordagem>();

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idVinculo = new Guid(parametros.Get("idVinculo"));
            string dtInicial = parametros.Get("dtInicial");
            string dtFinal = parametros.Get("dtFinal");

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            using (RepositorioOcorrenciaAnomalia repositorio = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
            {
                string sql = string.Format(@"SELECT ORT.CL000000000000ABCD000000000000 ID_ABORDAGEM,
                                                    ORT.CLC388C0FFC35B4680AA4FF9DA4F39 CODIGO, 
                                                    TO_CHAR(ORT.CL000000000000ABCD001100000000, 'DD/MM/YYYY') DATA, 
                                                    ORT.CL000000000000ABCD001100000000 HORA,  
                                                    ORT.CL000000000000ABCD000300000000 DESCRICAO
                                               FROM TBF4E1FE96D6974DB4812174D2258A ORT 
                                              WHERE CL537D49B88A92494BBF27C42B8D99 = '{0}'
                                                AND CL000000000000ABCD001100000000 >= TO_DATE('{1}', 'dd/MM/yyyy') 
                                                AND CL000000000000ABCD001100000000 < (TO_DATE('{2}', 'dd/MM/yyyy') + 1)
                                              ORDER BY CLC388C0FFC35B4680AA4FF9DA4F39 DESC", idVinculo, dtInicial, dtFinal);

                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow linha in dt.Rows)
                    {
                        Abordagem a = new Abordagem();
                        a.ID_ABORDAGEM = linha["ID_ABORDAGEM"].ToString();
                        a.CODIGO = linha["CODIGO"].ToString();
                        a.DATA = Convert.ToDateTime(linha["DATA"]);
                        a.HORA = Convert.ToDateTime(linha["HORA"]).TimeOfDay;
                        a.DESCRICAO = Convert.ToString(linha["DESCRICAO"]);

                        lst.Add(a);
                    }
                }
            }

            return lst;
        }

        [HttpPost]
        public Abordagem RetornarAbordagem(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idAbordagem = new Guid(parametros.Get("idAbordagem"));

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            string sql = string.Format(@"SELECT ORT.CL000000000000ABCD000000000000 ID_ABORDAGEM,
                                                ORT.CLC388C0FFC35B4680AA4FF9DA4F39 CODIGO, 
                                                TO_CHAR(ORT.CL000000000000ABCD001100000000, 'DD/MM/YYYY') DATA, 
                                                ORT.CL000000000000ABCD001100000000 HORA,  
                                                ORT.CL000000000000ABCD000300000000 DESCRICAO,

                                                ORT.CL9A028F24FC1E448A8B9B4920BADE ID_FORNECEDOR,
                                                ORT.CL60727E23076D489085D693B3F656 ID_REGIONAL,
                                                ORT.CLB385850AAF9A4AD1B30AF0BABE9E ID_GERENCIA,
                                                ORT.CLE8D2FB41B9E44AF4A02D3A14B9A2 ID_AREA,
                                                ORT.CL5CDDB175B6EC4FF193ACAFEA9B6D ID_LOCAL,
                                                ORT.CL628EB1D6788B4B7C8E480F7210C7 ID_OBSERVADOR,
                                                ORT.CL537D49B88A92494BBF27C42B8D99 ID_REGISTRADOPOR,

                                                FORNECEDOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_FORNECEDOR,
                                                REGI.CL000000000000ABCD000200000000 CD_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000200000000 CD_GERENCIA,                                        
                                                AREA.CLFD7DF78DB1A446B292F9C76515EC CD_AREA,
                                                LOCAL.CL000000000000ABCD000200000000 CD_LOCAL,  
                                                OBSERVADOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_OBSERVADOR,
                                                REGISTRADOPOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_REGISTRADOPOR,

                                                FORNECEDOR.CL10914D4F3685436282FBBC16F662 DS_FORNECEDOR,
                                                REGI.CL000000000000ABCD000300000000 DS_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000300000000 DS_GERENCIA,                                        
                                                AREA.CLD79FBD89F9CC4808BEA6E8D732D6 DS_AREA,
                                                LOCAL.CL000000000000ABCD000300000000 DS_LOCAL,
                                                CADPESSOAL.CL58284C47272B4729ABF5215F368D DS_OBSERVADOR,
                                                CADPESSOAL1.CL58284C47272B4729ABF5215F368D DS_REGISTRADOPOR,

                                                ORT.CL5E7110542FF345269D7EB1FFFBB3 ID_INERENTES_ATIV, 
                                                ORT.CLED5F06A024B34A6C8565889926DB ST_INERENTES_ATIV_VER_AGIR, 
                                                ORT.CL8F7A4680DE224C43A948896FD6F1 ID_RELACAO_COM_RISCOS, 
                                                ORT.CLAC37D53C0E0B4D96900BA09C6289 ST_RELACAO_COM_RISCOS_VER_AGIR, 
                                                ORT.CL2A00C8A10E2747A99DE33CC33EB8 ID_CONSERVACAO_ADEQ, 
                                                ORT.CL5380925B994B49929E7373A31F64 ST_CONSERVACAO_ADEQ_VER_AGIR, 
                                                ORT.CL27A2A87514304055928A60E8B7D4 ID_UTILIZACAO_CFD, 
                                                ORT.CL8D608570E3FC41328A37483F70A5 ST_UTILIZACAO_CFD_VER_AGIR, 

                                                ORT.CLD172E6FAE63B4AD2AE76690C24F7 ID_IDENTIF_RISCOS_MAP, 
                                                ORT.CL43E41CB9B8CF4187AD84C277D3BC ST_IDENTIF_RISCOS_MAP_VER_AGIR, 
                                                ORT.CLA74D67236DC54D42A48F0EDE0B78 ID_MEDIDAS_PREVENCAO, 
                                                ORT.CLDDC178173B144B7DA21D0C54DCDD ST_MEDIDAS_PREVENCAO_VER_AGIR, 
                                                ORT.CL63AF72F4170C4210B50B55664424 ID_BOAS_CONDICOES_USO, 
                                                ORT.CL15E3DE414B4D4395AD7F971BB350 ST_BOAS_CONDICOES_USO_VER_AGIR, 
                                                ORT.CL5D338E69148C4AD7A1AA629BBAC4 ID_UTILIZACAO_CORRETA, 
                                                ORT.CL33C821F1C43645059F7D104FCAF9 ST_UTILIZACAO_CORRETA_VER_AGIR, 
                                                ORT.CLFC0A5487EF054DD987189FC7488B ID_DESTINADOS_ATIV, 
                                                ORT.CLB3D5CBD657734E8591E45D3F5379 ST_DESTINADOS_ATIV_VER_AGIR, 

                                                ORT.CLE70A4BFEA61947719B5F5D1A01DE ID_LOCAL_LIMPO, 
                                                ORT.CL91194D9CE9C4400AA4639B61F956 ST_LOCAL_LIMPO_VER_AGIR, 
                                                ORT.CL3ABC342EE7AB4F6CA6604C3A3529 ID_MATERIAIS_ORGANIZ, 
                                                ORT.CLE9A8B9A4D9C14C0A9AEE7B3F89B4 ST_MATERIAIS_ORGANIZ_VER_AGIR, 
                                                ORT.CL76F46269DE2F44FEB20B5F003117 ID_DESCARTE_RESIDUOS, 
                                                ORT.CL7F1E1C38F39B45739507CD6065A3 ST_DESCARTE_RESIDUOS_VER_AGIR, 

                                                ORT.CLA9866B527A584D7C800923E06BD1 ID_IDENTIF_TRAT_RIS, 
                                                ORT.CL9A18C31C9E9E4668ABE1837D8E0C ST_IDENTIF_TRAT_RIS_VER_AGIR, 
                                                ORT.CLBCDE9494C65F439F9F7ADEA6F440 ID_LINHA_FOGO, 
                                                ORT.CL2647049882C9417DB5BDE0829EB8 ST_LINHA_FOGO_VER_AGIR, 
                                                ORT.CL47707576CC9B442F90FDA0FA49D6 ID_POSTURAS_ERGO_ADEQ, 
                                                ORT.CLB7634EAB461E42D68B2A37CE2E88 ST_POSTURAS_ERGO_ADEQ_VER_AGIR, 
                                                ORT.CL7FD13555B91246FAB9AA882BD1EA ID_CONCENTRACAO_TAREF, 
                                                ORT.CLAF47A26BADCF4E4A9457F9D7FE4A ST_CONCENTRACAO_TAREF_VER_AGIR, 

                                                ORT.CLF3D824AA640D41B8B42B71DE7889 ID_CONHECIMENTO_PROC, 
                                                ORT.CL8959A65CD28F4A8682167FFF26B5 ST_CONHECIMENTO_PROC_VER_AGIR, 
                                                ORT.CL8D3326314B124A13AF1B157C13F9 ID_CONHECIMENTO_RISC, 
                                                ORT.CL26CAAB88CAE247FFB4520EC73814 ST_CONHECIMENTO_RISC_VER_AGIR, 
                                                ORT.CL3C950A657E1A4FD48382C9A4285E ID_DIREITO_RECUSA, 
                                                ORT.CL8212F8D832394EE2A9FE1B49B930 ST_DIREITO_RECUSA_VER_AGIR, 
                                                ORT.CLB902473D1C6B490195C14E304540 ID_ACOES_EMERGENCIA, 
                                                ORT.CL6276F82518294232A21DD4791405 ST_ACOES_EMERGENCIA_VER_AGIR, 
                                                ORT.CL062725DEC7BB4040927156C70452 ID_REALIZA_PROCES_ROT, 
                                                ORT.CLE53214B1321D4F44B8E361BF80CE ST_REALIZA_PROCES_ROT_VER_AGIR 

                                           FROM TBF4E1FE96D6974DB4812174D2258A ORT 
                                           LEFT JOIN TBFB7C40BCD3E2487FBFFD7BC6F98F FORNECEDOR    ON FORNECEDOR.CL000000000000ABCD000000000000    = ORT.CL9A028F24FC1E448A8B9B4920BADE
                                           LEFT JOIN TB781D63F32AEA4423B070E7EB190C REGI          ON REGI.CL000000000000ABCD000000000000          = ORT.CL60727E23076D489085D693B3F656 
                                           LEFT JOIN TB271A941798BA43378CEE3CDC3A9D GERE          ON GERE.CL000000000000ABCD000000000000          = ORT.CLB385850AAF9A4AD1B30AF0BABE9E 
                                           LEFT JOIN TB69A8C990799C4B77B4F0D1CE8ECA AREA          ON AREA.CL000000000000ABCD000000000000          = ORT.CLE8D2FB41B9E44AF4A02D3A14B9A2 
                                           LEFT JOIN TB6BB39BD086DD4DBCAF41AB4149E2 LOCAL         ON LOCAL.CL000000000000ABCD000000000000         = ORT.CL5CDDB175B6EC4FF193ACAFEA9B6D
                                           LEFT JOIN TB831F15D403654FCE84691FE856D2 OBSERVADOR    ON OBSERVADOR.CL000000000000ABCD000000000000    = ORT.CL628EB1D6788B4B7C8E480F7210C7
                                           LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL    ON CADPESSOAL.CL000000000000ABCD000000000000    = OBSERVADOR.CL81F753A75F3B415CAB2037E094BA 
                                           LEFT JOIN TB831F15D403654FCE84691FE856D2 REGISTRADOPOR ON REGISTRADOPOR.CL000000000000ABCD000000000000 = ORT.CL537D49B88A92494BBF27C42B8D99
                                           LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL1   ON CADPESSOAL1.CL000000000000ABCD000000000000   = REGISTRADOPOR.CL81F753A75F3B415CAB2037E094BA
                                          WHERE ORT.CL000000000000ABCD000000000000 = '{0}' ", idAbordagem);

            Abordagem a = null;
            try
            {
                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    a = new Abordagem();
                    DataRow linha = dt.Rows[0];

                    a.ID_ABORDAGEM = linha["ID_ABORDAGEM"].ToString();
                    a.CODIGO = linha["CODIGO"].ToString();
                    a.DATA = Convert.ToDateTime(linha["DATA"]);
                    a.HORA = Convert.ToDateTime(linha["HORA"]).TimeOfDay;
                    a.DESCRICAO = Convert.ToString(linha["DESCRICAO"]);

                    a.FORNECEDOR = Util.GetModelo(linha, "FORNECEDOR");
                    a.UNIDADE = Util.GetModelo(linha, "REGIONAL");
                    a.GERENCIA = Util.GetModelo(linha, "GERENCIA");
                    a.AREA = Util.GetModelo(linha, "AREA");
                    a.LOCAL = Util.GetModelo(linha, "LOCAL");

                    //A - Equipamento de Proteção Individual e Coletivo
                    a.INERENTES_ATIVIDADE = Util.GetModelo(linha, "INERENTES_ATIV");
                    a.RELACAO_COM_RISCOS = Util.GetModelo(linha, "RELACAO_COM_RISCOS");
                    a.CONSERVACAO_ADEQUADA = Util.GetModelo(linha, "CONSERVACAO_ADEQ");
                    a.UTILIZACAO_CORRETA_FIXACAO_DISTANCIA = Util.GetModelo(linha, "UTILIZACAO_CFD");

                    //B - Máquinas, Veículos, Equipamentos e Ferramentas
                    a.IDENTIFICACAO_RISCOS_MAPEADOS = Util.GetModelo(linha, "IDENTIF_RISCOS_MAP");
                    a.MEDIDAS_PREVENCAO = Util.GetModelo(linha, "MEDIDAS_PREVENCAO");
                    a.BOAS_CONDICOES_USO = Util.GetModelo(linha, "BOAS_CONDICOES_USO");
                    a.UTILIZACAO_CORRETA = Util.GetModelo(linha, "UTILIZACAO_CORRETA");
                    a.DESTINADOS_ATIVIDADE = Util.GetModelo(linha, "DESTINADOS_ATIV");

                    //C - Programa Bom Senso
                    a.LOCAL_LIMPO = Util.GetModelo(linha, "LOCAL_LIMPO");
                    a.MATERIAIS_ORGANIZADOS = Util.GetModelo(linha, "MATERIAIS_ORGANIZ");
                    a.DESCARTE_RESIDUOS = Util.GetModelo(linha, "DESCARTE_RESIDUOS");

                    //D - Acidentes, Incidentes e Desvios
                    a.IDENTIFICACAO_TRATATIVA_RISCOS = Util.GetModelo(linha, "IDENTIF_TRAT_RIS");
                    a.LINHA_FOGO = Util.GetModelo(linha, "LINHA_FOGO");
                    a.POSTURAS_ERGO_ADEQUADAS = Util.GetModelo(linha, "POSTURAS_ERGO_ADEQ");
                    a.CONCENTRACAO_TAREFA = Util.GetModelo(linha, "CONCENTRACAO_TAREF");

                    //E - Planejamento, Procedimento e Instrução
                    a.CONHECIMENTO_PROCEDIMENTOS = Util.GetModelo(linha, "CONHECIMENTO_PROC");
                    a.CONHECIMENTO_RISCOS = Util.GetModelo(linha, "CONHECIMENTO_RISC");
                    a.DIREITO_RECUSA = Util.GetModelo(linha, "DIREITO_RECUSA");
                    a.ACOES_EMERGENCIA = Util.GetModelo(linha, "ACOES_EMERGENCIA");
                    a.REALIZA_PROCESSOROTINA = Util.GetModelo(linha, "REALIZA_PROCES_ROT");

                    a.COGNITIVOS = this.GetLstMv(sessao.Contexto, "CL000000000000ABCD001300000000", "TB2D73CA163C2A48EBAB5186A84E3F", a.ID_ABORDAGEM);
                    a.FISIOLOGICOS = this.GetLstMv(sessao.Contexto, "CLBAAC0265C4984D2084C6BA032E87", "TBE5472D31841F4B378A8D5213B340", a.ID_ABORDAGEM);
                    a.PSICOLOGICOS = this.GetLstMv(sessao.Contexto, "CLFB5EA87439DD42AEB3EC87C9ED67", "TBE7573BF58398476E8AAB9E8ACCC7", a.ID_ABORDAGEM);
                    a.SOCIAIS = this.GetLstMv(sessao.Contexto, "CL46053660BBF8474DA95D07632855", "TBAF75B1565235479ABAFB2DFB3511", a.ID_ABORDAGEM);

                    a.OBSERVADOR = Util.GetModelo(linha, "OBSERVADOR");
                    a.REGISTRADOPOR = Util.GetModelo(linha, "REGISTRADOPOR");
                }
            }
            catch (Exception ex)
            {

            }
            return a;
        }

        private List<SDMobileXFDados.Modelos.ModeloObj> GetLstMv(Contexto c, string coluna, string nmTabela, string idRegistro)
        {
            List<SDMobileXFDados.Modelos.ModeloObj> lst = new List<SDMobileXFDados.Modelos.ModeloObj>();
            string sql = $" SELECT {coluna} FROM TBF4E1FE96D6974DB4812174D22_MV " +
                         $"  WHERE id_registro = '{idRegistro}' AND NM_TABELA = '{nmTabela}'";

            try
            {
                List<object> ids = c.BuscarValores(sql);
                foreach (object id in ids)
                    lst.Add(new SDMobileXFDados.Modelos.ModeloObj() { Id = new Guid(id.ToString()) });
            }
            catch
            {

            }
            return lst;
        }
    }
}
