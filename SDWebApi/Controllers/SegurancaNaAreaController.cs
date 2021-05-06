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
    public class SegurancaNaAreaController : ApiController
    {
        // GET: api/SegurancaNaArea
        public IEnumerable<string> Get()
        {
            Contexto contexto = Contexto.Instancia(new ConexaoBanco("BANCO", null, null));

            DateTime data = contexto.DataBanco;

            return new string[] { "SegurancaNaArea", data.ToShortDateString() };
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
                Guid idTabela = new Guid("351509a5-3252-4dd4-b312-39ac83f70c7d");
                Guid idColuna = new Guid("c388c0ff-c35b-4680-aa4f-f9da4f39888c");

                string numero = parametros.Get("NU_SNA");
                string descricao = parametros.Get("DS_TEMA_ABORDADO").Replace("'", "''");
                string sql = string.Empty;

                if (string.IsNullOrEmpty(numero))
                {
                    numero = Util.GerarSequencia(sessao.Contexto, idTabela, idColuna, idRegistro, DateTime.Today);

                    sql = @" INSERT INTO TB351509A532524DD4B31239AC83F7
                                           ( CL000000000000ABCD000000000000, CLC388C0FFC35B4680AA4FF9DA4F39, 
                                             CL000000000000ABCD001100000000, CL7AD2C587708B43139C4E76839FFE, 

                                             CL5E7110542FF345269D7EB1FFFBB3, CL361D9FD735BF4B68AF69544EFF66, 
                                             CLD172E6FAE63B4AD2AE76690C24F7, CLA74D67236DC54D42A48F0EDE0B78, 
                                             CL0A9CE9C62F054223A3C787873BD1,
             
                                             CL914C1709C69C481D894EEE44E498, CLBCE1791AFCD5490B9B4DBA2BF938, CL0CA8EB5B997046FB964B79B9EB51, 
                                             CLE8D2FB41B9E44AF4A02D3A14B9A2, CL19B11EC844CC4020A87CF79D6105, CL61C5662B3F074ECBBBC0F40E295A, 
                                             CL63AF72F4170C4210B50B55664424, CLCAA0D8457A4E45EC8819518ECCF0, CL527582A4AFE9442B8E2C2D9D8793, 
                                             CL5D338E69148C4AD7A1AA629BBAC4, CLD7005E3F60A9497F84E912C8527F, CLCE94FF6AECE3491F842950B9C8FA, 

                                             CL2C2DC51613774F5495FD93DFC005, 
                                             CL5CDDB175B6EC4FF193ACAFEA9B6D) " +
                             $" VALUES ( '{idRegistro}', '{numero}', " +
                                      $" TO_DATE('{parametros.Get("DT_DATA")}', 'YYYYMMDD HH24MISS'),  TO_DATE('{parametros.Get("DT_HORARIO_INICIAL")}', 'YYYYMMDD HH24MISS'), " +

                                      $" '{parametros.Get("ID_REGIONAL")}', '{parametros.Get("ID_GERENCIA")}', " +
                                      $" '{parametros.Get("ID_AREA")}', '{parametros.Get("ID_LOCAL")}', " +
                                      $" '{parametros.Get("DS_TEMA_ABORDADO")}', " +

                                      $" '{parametros.Get("ID_CE")}', '{parametros.Get("DS_CE_AVALIACAODESCRITIVA")}', '{parametros.Get("NU_DNA_CE")}', " +
                                      $" '{parametros.Get("ID_CA")}', '{parametros.Get("DS_CA_AVALIACAODESCRITIVA")}', '{parametros.Get("NU_DNA_CA")}', " +
                                      $" '{parametros.Get("ID_RAF")}', '{parametros.Get("DS_RAF_AVALIACAODESCRITIVA")}', '{parametros.Get("NU_DNA_RAF")}', " +
                                      $" '{parametros.Get("ID_QAT")}', '{parametros.Get("DS_QAT_AVALIACAODESCRITIVA")}', '{parametros.Get("NU_DNA_QAT")}', " +

                                      $" '{parametros.Get("DS_PONTOS_POSITIVOS")}', " +
                                      $" '{parametros.Get("ID_REGISTRADOPOR")}') ";
                }
                else
                {
                    idRegistro = new Guid(sessao.Contexto.BuscarValorStr($"SELECT CL000000000000ABCD000000000000 FROM TB351509A532524DD4B31239AC83F7 WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}'"));

                    sql = "UPDATE TB351509A532524DD4B31239AC83F7 " +
                          $"  SET CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}', " +
                          $"      CL000000000000ABCD001100000000 = TO_DATE('{parametros.Get("DT_DATA")}', 'YYYYMMDD HH24MISS')', " +
                          $"      CL7AD2C587708B43139C4E76839FFE = TO_DATE('{parametros.Get("DT_HORARIO_INICIAL")}', 'YYYYMMDD HH24MISS'), " +
                          $"      CL5E7110542FF345269D7EB1FFFBB3 = '{parametros.Get("ID_REGIONAL")}', " +
                          $"      CL361D9FD735BF4B68AF69544EFF66 = '{parametros.Get("ID_GERENCIA")}', " +
                          $"      CLD172E6FAE63B4AD2AE76690C24F7 = '{parametros.Get("ID_AREA")}', " +
                          $"      CLA74D67236DC54D42A48F0EDE0B78 = '{parametros.Get("ID_LOCAL")}', " +
                          $"      CL0A9CE9C62F054223A3C787873BD1 = '{parametros.Get("DS_TEMA_ABORDADO")}', " +
                          $"      CL914C1709C69C481D894EEE44E498 = '{parametros.Get("ID_CE")}', " +
                          $"      CLBCE1791AFCD5490B9B4DBA2BF938 = '{parametros.Get("DS_CE_AVALIACAODESCRITIVA")}', " +
                          $"      CL0CA8EB5B997046FB964B79B9EB51 = '{parametros.Get("NU_DNA_CE")}', " +
                          $"      CLE8D2FB41B9E44AF4A02D3A14B9A2 = '{parametros.Get("ID_CA")}', " +
                          $"      CL19B11EC844CC4020A87CF79D6105 = '{parametros.Get("DS_CA_AVALIACAODESCRITIVA")}', " +
                          $"      CL61C5662B3F074ECBBBC0F40E295A = '{parametros.Get("NU_DNA_CA")}', " +
                          $"      CL63AF72F4170C4210B50B55664424 = '{parametros.Get("ID_RAF")}', " +
                          $"      CLCAA0D8457A4E45EC8819518ECCF0 = '{parametros.Get("DS_RAF_AVALIACAODESCRITIVA")}', " +
                          $"      CL527582A4AFE9442B8E2C2D9D8793 = '{parametros.Get("NU_DNA_RAF")}', " +
                          $"      CL5D338E69148C4AD7A1AA629BBAC4 = '{parametros.Get("ID_QAT")}', " +
                          $"      CLD7005E3F60A9497F84E912C8527F = '{parametros.Get("DS_QAT_AVALIACAODESCRITIVA")}', " +
                          $"      CLCE94FF6AECE3491F842950B9C8FA = '{parametros.Get("NU_DNA_QAT")}', " +
                          $"      CL2C2DC51613774F5495FD93DFC005 = '{parametros.Get("DS_PONTOS_POSITIVOS")}', " +
                          $"      CL5CDDB175B6EC4FF193ACAFEA9B6D = '{parametros.Get("ID_REGISTRADOPOR")}' " +
                         $" WHERE CL000000000000ABCD000000000000 = '{idRegistro}' ";
                }
                try
                {
                    sessao.Contexto.Executar(sql);

                    sql =   $"SELECT CL0CA8EB5B997046FB964B79B9EB51 || '|' ||" +
                            $"       CL61C5662B3F074ECBBBC0F40E295A || '|' ||" +
                            $"       CL527582A4AFE9442B8E2C2D9D8793 || '|' ||" +
                            $"       CLCE94FF6AECE3491F842950B9C8FA" +
                            $"  FROM TB351509A532524DD4B31239AC83F7 WHERE CL000000000000ABCD000000000000 = '{idRegistro}' ";

                    string numeros = sessao.Contexto.BuscarValorStr(sql);
                    return Request.CreateResponse(HttpStatusCode.OK, idRegistro + "|" + numero + "|" + numeros);
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
        public List<Sna> RetornarSNAs(FormDataCollection parametros)
        {
            List<Sna> lst = new List<Sna>();

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));            
            string dtInicial = parametros.Get("dtInicial");
            string dtFinal = parametros.Get("dtFinal");
            string idVinculo = parametros.Get("idVinculo");

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            using (RepositorioOcorrenciaAnomalia repositorio = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
            {
                string sql = string.Format(@"SELECT SNA.CL000000000000ABCD000000000000 ID_SNA,
                                                    SNA.CLC388C0FFC35B4680AA4FF9DA4F39 CODIGO, 
                                                    SNA.CL000000000000ABCD001100000000 DATA, 
                                                    SNA.CL000000000000ABCD001100000000 DT_HORARIO_INICIAL, 
                                                    SNA.CL0A9CE9C62F054223A3C787873BD1 DS_TEMA_ABORDADO
                                               FROM TB351509A532524DD4B31239AC83F7 SNA 
                                              WHERE CL5CDDB175B6EC4FF193ACAFEA9B6D = '{0}'
                                                AND CL000000000000ABCD001100000000 >= TO_DATE('{1}', 'dd/MM/yyyy') 
                                                AND CL000000000000ABCD001100000000 < (TO_DATE('{2}', 'dd/MM/yyyy') + 1)                                                
                                              ORDER BY CL000000000000ABCD001100000000 DESC", idVinculo, dtInicial, dtFinal);

                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow linha in dt.Rows)
                    {
                        Sna s = new Sna();
                        s.ID_SNA = linha["ID_SNA"].ToString();
                        s.CODIGO = linha["CODIGO"].ToString();
                        s.DATA = Convert.ToDateTime(linha["DATA"]);
                        s.HORA = Convert.ToDateTime(linha["DT_HORARIO_INICIAL"]).TimeOfDay;
                        s.DS_TEMA_ABORDADO = Convert.ToString(linha["DS_TEMA_ABORDADO"]);

                        lst.Add(s);
                    }
                }
            }

            return lst;
        }

        [HttpPost]
        public Sna RetornarSNA(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idSNA = new Guid(parametros.Get("idSNA"));

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            string sql = string.Format(@"    SELECT SNA.CL000000000000ABCD000000000000 ID_SNA,
                                                    SNA.CLC388C0FFC35B4680AA4FF9DA4F39 CODIGO, 
                                                    SNA.CL000000000000ABCD001100000000 DATA, 
                                                    SNA.CL000000000000ABCD001100000000 DT_HORARIO_INICIAL, 
                                                    SNA.CL5E7110542FF345269D7EB1FFFBB3 ID_REGIONAL,
                                                    SNA.CL361D9FD735BF4B68AF69544EFF66 ID_GERENCIA,
                                                    SNA.CLD172E6FAE63B4AD2AE76690C24F7 ID_AREA,
                                                    SNA.CLA74D67236DC54D42A48F0EDE0B78 ID_LOCAL,
                                                    REGI.CL000000000000ABCD000200000000 CD_REGIONAL,                                        
                                                    GERE.CL000000000000ABCD000200000000 CD_GERENCIA,                                        
                                                    AREA.CLFD7DF78DB1A446B292F9C76515EC CD_AREA,
                                                    LOCAL.CL000000000000ABCD000200000000 CD_LOCAL,
                                                    REGI.CL000000000000ABCD000300000000 DS_REGIONAL,                                        
                                                    GERE.CL000000000000ABCD000300000000 DS_GERENCIA,                                        
                                                    AREA.CLD79FBD89F9CC4808BEA6E8D732D6 DS_AREA,
                                                    LOCAL.CL000000000000ABCD000300000000 DS_LOCAL,
                                                    SNA.CL0A9CE9C62F054223A3C787873BD1 DS_TEMA_ABORDADO,
                                                    SNA.CL914C1709C69C481D894EEE44E498 ID_CE,
                                                    SNA.CLBCE1791AFCD5490B9B4DBA2BF938 DS_CE_AVALIACAODESCRITIVA,
                                                    SNA.CL0CA8EB5B997046FB964B79B9EB51 NU_DNA_CE,
                                                    SNA.CLE8D2FB41B9E44AF4A02D3A14B9A2 ID_CA,
                                                    SNA.CL19B11EC844CC4020A87CF79D6105 DS_CA_AVALIACAODESCRITIVA,
                                                    SNA.CL61C5662B3F074ECBBBC0F40E295A NU_DNA_CA,
                                                    SNA.CL63AF72F4170C4210B50B55664424 ID_RAF,
                                                    SNA.CLCAA0D8457A4E45EC8819518ECCF0 DS_RAF_AVALIACAODESCRITIVA,
                                                    SNA.CL527582A4AFE9442B8E2C2D9D8793 NU_DNA_RAF,
                                                    SNA.CL5D338E69148C4AD7A1AA629BBAC4 ID_QAT,
                                                    SNA.CLD7005E3F60A9497F84E912C8527F DS_QAT_AVALIACAODESCRITIVA,
                                                    SNA.CLCE94FF6AECE3491F842950B9C8FA NU_DNA_QAT,
                                                    SNA.CL2C2DC51613774F5495FD93DFC005 DS_PONTOS_POSITIVOS,
                                                    SNA.CL5CDDB175B6EC4FF193ACAFEA9B6D ID_REGISTRADOPOR,
                                                    REGISTRADOPOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_REGISTRADOPOR,
                                                    CADPESSOAL.CL58284C47272B4729ABF5215F368D DS_REGISTRADOPOR
                                               FROM TB351509A532524DD4B31239AC83F7 SNA 
                                               LEFT JOIN TB781D63F32AEA4423B070E7EB190C REGI          ON REGI.CL000000000000ABCD000000000000          = SNA.CL5E7110542FF345269D7EB1FFFBB3
                                               LEFT JOIN TB271A941798BA43378CEE3CDC3A9D GERE          ON GERE.CL000000000000ABCD000000000000          = SNA.CL361D9FD735BF4B68AF69544EFF66
                                               LEFT JOIN TB69A8C990799C4B77B4F0D1CE8ECA AREA          ON AREA.CL000000000000ABCD000000000000          = SNA.CLD172E6FAE63B4AD2AE76690C24F7
                                               LEFT JOIN TB6BB39BD086DD4DBCAF41AB4149E2 LOCAL         ON LOCAL.CL000000000000ABCD000000000000         = SNA.CLA74D67236DC54D42A48F0EDE0B78
                                               LEFT JOIN TB831F15D403654FCE84691FE856D2 REGISTRADOPOR ON REGISTRADOPOR.CL000000000000ABCD000000000000 = SNA.CL5CDDB175B6EC4FF193ACAFEA9B6D
                                               LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL    ON CADPESSOAL.CL000000000000ABCD000000000000    = REGISTRADOPOR.CL81F753A75F3B415CAB2037E094BA
                                              WHERE SNA.CL000000000000ABCD000000000000 = '{0}'", idSNA);

            Sna s = null;
            try
            {
                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    s = new Sna();
                    DataRow linha = dt.Rows[0];

                    s.ID_SNA = linha["ID_SNA"].ToString();
                    s.CODIGO = linha["CODIGO"].ToString();
                    s.DATA = Convert.ToDateTime(linha["DATA"]);
                    s.HORA = Convert.ToDateTime(linha["DT_HORARIO_INICIAL"]).TimeOfDay;                    
                    s.UNIDADE = linha.IsNull("ID_REGIONAL") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_REGIONAL"])),
                        Codigo = Convert.ToString(linha["CD_REGIONAL"]),
                        Descricao = Convert.ToString(linha["DS_REGIONAL"])
                    };
                    s.GERENCIA = linha.IsNull("ID_GERENCIA") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_GERENCIA"])),
                        Codigo = Convert.ToString(linha["CD_GERENCIA"]),
                        Descricao = Convert.ToString(linha["DS_GERENCIA"])
                    };
                    s.AREA = linha.IsNull("ID_AREA") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_AREA"])),
                        Codigo = Convert.ToString(linha["CD_AREA"]),
                        Descricao = Convert.ToString(linha["DS_AREA"])
                    };
                    s.LOCAL = linha.IsNull("ID_LOCAL") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_LOCAL"])),
                        Codigo = Convert.ToString(linha["CD_LOCAL"]),
                        Descricao = Convert.ToString(linha["DS_LOCAL"])
                    };
                    s.DS_TEMA_ABORDADO = Convert.ToString(linha["DS_TEMA_ABORDADO"]);
                    s.ID_CE = Convert.ToString(linha["ID_CE"]);
                    s.DS_CE_AVALIACAODESCRITIVA = Convert.ToString(linha["DS_CE_AVALIACAODESCRITIVA"]);
                    s.NU_DNA_CE = Convert.ToString(linha["NU_DNA_CE"]);
                    s.ID_CA = Convert.ToString(linha["ID_CA"]);
                    s.DS_CA_AVALIACAODESCRITIVA = Convert.ToString(linha["DS_CA_AVALIACAODESCRITIVA"]);
                    s.NU_DNA_CA = Convert.ToString(linha["NU_DNA_CA"]);
                    s.ID_RAF = Convert.ToString(linha["ID_RAF"]);
                    s.DS_RAF_AVALIACAODESCRITIVA = Convert.ToString(linha["DS_RAF_AVALIACAODESCRITIVA"]);
                    s.NU_DNA_RAF = Convert.ToString(linha["NU_DNA_RAF"]);
                    s.ID_RAF = Convert.ToString(linha["ID_RAF"]);
                    s.DS_RAF_AVALIACAODESCRITIVA = Convert.ToString(linha["DS_RAF_AVALIACAODESCRITIVA"]);
                    s.NU_DNA_RAF = Convert.ToString(linha["NU_DNA_RAF"]);
                    s.ID_QAT = Convert.ToString(linha["ID_QAT"]);
                    s.DS_QAT_AVALIACAODESCRITIVA = Convert.ToString(linha["DS_QAT_AVALIACAODESCRITIVA"]);
                    s.NU_DNA_QAT = Convert.ToString(linha["NU_DNA_QAT"]);
                    s.DS_PONTOS_POSITIVOS = Convert.ToString(linha["DS_PONTOS_POSITIVOS"]);
                    s.REGISTRADOPOR = linha.IsNull("ID_REGISTRADOPOR") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_REGISTRADOPOR"])),
                        Codigo = Convert.ToString(linha["CD_REGISTRADOPOR"]),
                        Descricao = Convert.ToString(linha["DS_REGISTRADOPOR"])
                    };
                }
            }
            catch (Exception ex)
            {

            }
            return s;
        }
    }
}
