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
    public class OpaController : ApiController
    {
        // GET: api/SegurancaNaArea
        public IEnumerable<string> Get()
        {
            Contexto contexto = Contexto.Instancia(new ConexaoBanco("BANCO", null, null));

            DateTime data = contexto.DataBanco;

            return new string[] { "Opa", data.ToShortDateString() };
        }

        [HttpPost]
        public HttpResponseMessage Inserir(FormDataCollection parametros)
        {
            string erro = string.Empty;
            string sql = string.Empty;
            try
            {
                Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
                string login = parametros.Get("login");
                Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

                Util.IniciarSessao(sessao, login);

                Guid idRegistro = Guid.NewGuid();
                Guid idTabela = new Guid("32488541-cd1c-485b-b1e4-2b4f9d912290");
                Guid idColuna = new Guid("c388c0ff-c35b-4680-aa4f-f9da4f39888c");

                string numero = parametros.Get("NU_OPA");

                string campos = parametros.Get("CAMPOS");
                List<CampoOpa> lstCampos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CampoOpa>>(campos);

                if (string.IsNullOrEmpty(numero))
                {
                    numero = Util.GerarSequencia(sessao.Contexto, idTabela, idColuna, idRegistro, DateTime.Today);

                    sql = @" INSERT INTO TB32488541CD1C485BB1E42B4F9D91
                                  ( CL000000000000ABCD000000000000, CLC388C0FFC35B4680AA4FF9DA4F39, CL000000000000ABCD001100000000,  
                                    CL5E7110542FF345269D7EB1FFFBB3, CL361D9FD735BF4B68AF69544EFF66, 
                                    CLD172E6FAE63B4AD2AE76690C24F7, CLA74D67236DC54D42A48F0EDE0B78, 
                                    CL914C1709C69C481D894EEE44E498, CLE8D2FB41B9E44AF4A02D3A14B9A2, 
                                    CL63AF72F4170C4210B50B55664424, CL5D338E69148C4AD7A1AA629BBAC4,
                                    CL000000000000ABCD003300000000 {0}
                                  ) " +
                        $" VALUES ( '{idRegistro}', '{numero}', " + $" TO_DATE('{parametros.Get("DT_DATA")}', 'YYYYMMDD HH24MISS'), " +
                                 $" '{parametros.Get("ID_REGIONAL")}', '{parametros.Get("ID_GERENCIA")}', " +
                                 $" '{parametros.Get("ID_AREA")}', '{parametros.Get("ID_LOCAL")}', " +
                                 $" '{parametros.Get("ID_ATIVIDADE")}', '{parametros.Get("ID_TAREFA")}', " +
                                 $" '{parametros.Get("ID_AVALIADOR")}', '{parametros.Get("ID_TIPO_AVALIADOR")}', " +
                                 "  1 {1} ) ";

                    string colunas = string.Empty;
                    string valores = string.Empty;
                    foreach (CampoOpa c in lstCampos)
                    {
                        if (!string.IsNullOrEmpty(c.NumeroDNA))
                        {
                            colunas += ", " + c.Colunas["CaixaNumerica"];
                            valores += ", " + c.NumeroDNA;
                        }

                        if (!string.IsNullOrEmpty(c.Comentario))
                        {
                            colunas += ", " + c.Colunas["CaixaTexto"];
                            valores += ", '" + c.Comentario.Replace("'", "''") + "' ";
                        }

                        if (!string.IsNullOrEmpty(c.IdConforme))
                        {
                            colunas += ", " + c.Colunas["CaixaOpcao"];
                            valores += ", '" + c.IdConforme + "' ";
                        }
                    }

                    sql = string.Format(sql, colunas, valores);
                }
                else
                {
                    idRegistro = new Guid(sessao.Contexto.BuscarValorStr($"SELECT CL000000000000ABCD000000000000 FROM TB32488541CD1C485BB1E42B4F9D91 WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}'"));

                    sql = "UPDATE TB32488541CD1C485BB1E42B4F9D91 " +
                          $"  SET CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}', " +
                          $"      CL000000000000ABCD001100000000 = TO_DATE('{parametros.Get("DT_DATA")}', 'YYYYMMDD HH24MISS')', " +
                          $"      CL5E7110542FF345269D7EB1FFFBB3 = '{parametros.Get("ID_REGIONAL")}', " +
                          $"      CL361D9FD735BF4B68AF69544EFF66 = '{parametros.Get("ID_GERENCIA")}', " +
                          $"      CLD172E6FAE63B4AD2AE76690C24F7 = '{parametros.Get("ID_AREA")}', " +
                          $"      CLA74D67236DC54D42A48F0EDE0B78 = '{parametros.Get("ID_LOCAL")}', " +
                          $"      CL914C1709C69C481D894EEE44E498 = '{parametros.Get("ID_ATIVIDADE")}', " +
                          $"      CLE8D2FB41B9E44AF4A02D3A14B9A2 = '{parametros.Get("ID_TAREFA")}', " +
                          $"      CL63AF72F4170C4210B50B55664424 = '{parametros.Get("ID_AVALIADOR")}', " +
                          $"      CL5D338E69148C4AD7A1AA629BBAC4 = '{parametros.Get("ID_TIPO_AVALIADOR")}' " + "{0}" +
                         $" WHERE CL000000000000ABCD000000000000 = '{idRegistro}' ";

                    string colunas = string.Empty;
                    foreach (CampoOpa c in lstCampos)
                    {
                        if (!string.IsNullOrEmpty(c.NumeroDNA))
                            colunas += ", " + c.Colunas["CaixaNumerica"] + " = '" + c.NumeroDNA + "'";

                        if (!string.IsNullOrEmpty(c.Comentario))
                            colunas += ", " + c.Colunas["CaixaTexto"] + " = '" + c.Comentario.Replace("'", "''") + "'";

                        if (!string.IsNullOrEmpty(c.IdConforme))
                            colunas += ", " + c.Colunas["CaixaOpcao"] + " = '" + c.IdConforme + "'";
                    }

                    sql = string.Format(sql, colunas);
                }
                try
                {
                    sessao.Contexto.Executar(sql);

                    string colunas = string.Empty;
                    foreach (CampoOpa c in lstCampos)
                    {
                        if (colunas.Length > 0)
                            colunas += " || '|' || ";
                        if(c.Colunas != null)
                            colunas += c.Colunas["CaixaNumerica"];
                        else
                            colunas += "''";
                    }

                    sql = $"SELECT {colunas} FROM TB32488541CD1C485BB1E42B4F9D91 WHERE CL000000000000ABCD000000000000 = '{idRegistro}' ";

                    string numeros = sessao.Contexto.BuscarValorStr(sql);
                    return Request.CreateResponse(HttpStatusCode.OK, idRegistro + "|" + numero + "|" + numeros);
                }
                catch (Exception exBanco)
                {
                    erro = exBanco.Message;
                    Util.ExcluirSequencia(sessao.Contexto, idTabela, idRegistro);
                }
            }
            catch (Exception ex) { erro = ex.Message; }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, erro);
        }

        [HttpPost]
        public List<Opa> RetornarOpas(FormDataCollection parametros)
        {
            List<Opa> lst = new List<Opa>();

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            string dtInicial = parametros.Get("dtInicial");
            string dtFinal = parametros.Get("dtFinal");

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            using (RepositorioOcorrenciaAnomalia repositorio = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
            {
                string sql = string.Format(@"SELECT OPA.CL000000000000ABCD000000000000 ID_OPA,
                                                    OPA.CLC388C0FFC35B4680AA4FF9DA4F39 NUMERO, 
                                                    OPA.CL000000000000ABCD001100000000 DATA,

                                                    ATIV.CL000000000000ABCD000000000000 ID_ATIVIDADE,
                                                    ATIV.CL000000000000ABCD000200000000 CD_ATIVIDADE,
                                                    ATIV.CL000000000000ABCD000300000000 DS_ATIVIDADE,

                                                    TARE.CL000000000000ABCD000000000000 ID_TAREFA,
                                                    TARE.CL000000000000ABCD000200000000 CD_TAREFA,
                                                    TARE.CL000000000000ABCD000300000000 DS_TAREFA

                                               FROM TB32488541CD1C485BB1E42B4F9D91 OPA
                                               LEFT JOIN TB85F7AFEC399944089B347BB46D55 ATIV ON ATIV.CL000000000000ABCD000000000000 = OPA.CL914C1709C69C481D894EEE44E498
                                               LEFT JOIN TBB760FCEDDBFE4965BE358DC6335C TARE ON TARE.CL000000000000ABCD000000000000 = OPA.CLE8D2FB41B9E44AF4A02D3A14B9A2

                                              WHERE CL000000000000ABCD001100000000 >= TO_DATE('{0}', 'dd/MM/yyyy') 
                                                AND CL000000000000ABCD001100000000 < (TO_DATE('{1}', 'dd/MM/yyyy') + 1)                                                
                                              ORDER BY CL000000000000ABCD001100000000 DESC", dtInicial, dtFinal);

                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow linha in dt.Rows)
                    {
                        Opa opa = new Opa();
                        opa.ID_OPA = linha["ID_OPA"].ToString();
                        opa.NUMERO = linha["NUMERO"].ToString();
                        opa.DATA = Convert.ToDateTime(linha["DATA"]);
                        opa.ATIVIDADE = linha.IsNull("ID_ATIVIDADE") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                        {
                            Id = new Guid(Convert.ToString(linha["ID_ATIVIDADE"])),
                            Codigo = Convert.ToString(linha["CD_ATIVIDADE"]),
                            Descricao = Convert.ToString(linha["DS_ATIVIDADE"])
                        };
                        opa.TAREFA_OBSERVADA = linha.IsNull("ID_TAREFA") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                        {
                            Id = new Guid(Convert.ToString(linha["ID_TAREFA"])),
                            Codigo = Convert.ToString(linha["CD_TAREFA"]),
                            Descricao = Convert.ToString(linha["DS_TAREFA"])
                        };

                        lst.Add(opa);
                    }
                }
            }

            return lst;
        }

        [HttpPost]
        public Opa Retornar(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idOpa = new Guid(parametros.Get("idOpa"));

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            string sql = string.Format(@"SELECT OPA.CL000000000000ABCD000000000000 ID_OPA,
                                                OPA.CLC388C0FFC35B4680AA4FF9DA4F39 NUMERO, 
                                                OPA.CL000000000000ABCD001100000000 DATA, 

                                                OPA.CL5E7110542FF345269D7EB1FFFBB3 ID_REGIONAL,
                                                OPA.CL361D9FD735BF4B68AF69544EFF66 ID_GERENCIA,
                                                OPA.CLD172E6FAE63B4AD2AE76690C24F7 ID_AREA,
                                                OPA.CLA74D67236DC54D42A48F0EDE0B78 ID_LOCAL,
                                                OPA.CL914C1709C69C481D894EEE44E498 ID_ATIVIDADE,
                                                OPA.CLE8D2FB41B9E44AF4A02D3A14B9A2 ID_TAREFA,
                                                OPA.CL63AF72F4170C4210B50B55664424 ID_AVALIADOR,
                                                OPA.CL5D338E69148C4AD7A1AA629BBAC4 ID_TIPO_AVALIADOR,

                                                REGI.CL000000000000ABCD000200000000 CD_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000200000000 CD_GERENCIA, 
                                                AREA.CLFD7DF78DB1A446B292F9C76515EC CD_AREA,
                                                LOCAL.CL000000000000ABCD000200000000 CD_LOCAL,
                                                ATIV.CL000000000000ABCD000200000000 CD_ATIVIDADE,
                                                TARE.CL000000000000ABCD000200000000 CD_TAREFA,
                                                AVALIADOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_AVALIADOR,
                                                TIPO_AVAL.CL000000000000ABCD000200000000 CD_TIPO_AVALIADOR,

                                                REGI.CL000000000000ABCD000300000000 DS_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000300000000 DS_GERENCIA,   
                                                AREA.CLD79FBD89F9CC4808BEA6E8D732D6 DS_AREA,
                                                LOCAL.CL000000000000ABCD000300000000 DS_LOCAL,
                                                ATIV.CL000000000000ABCD000300000000 DS_ATIVIDADE,
                                                TARE.CL000000000000ABCD000300000000 DS_TAREFA,
                                                CADPESSOAL.CL58284C47272B4729ABF5215F368D DS_AVALIADOR,
                                                TIPO_AVAL.CL000000000000ABCD000300000000 DS_TIPO_AVALIADOR

                                           FROM TB32488541CD1C485BB1E42B4F9D91 OPA

                                           LEFT JOIN TB781D63F32AEA4423B070E7EB190C REGI          ON REGI.CL000000000000ABCD000000000000          = OPA.CL5E7110542FF345269D7EB1FFFBB3
                                           LEFT JOIN TB271A941798BA43378CEE3CDC3A9D GERE          ON GERE.CL000000000000ABCD000000000000          = OPA.CL361D9FD735BF4B68AF69544EFF66
                                           LEFT JOIN TB69A8C990799C4B77B4F0D1CE8ECA AREA          ON AREA.CL000000000000ABCD000000000000          = OPA.CLD172E6FAE63B4AD2AE76690C24F7
                                           LEFT JOIN TB6BB39BD086DD4DBCAF41AB4149E2 LOCAL         ON LOCAL.CL000000000000ABCD000000000000         = OPA.CLA74D67236DC54D42A48F0EDE0B78

                                           LEFT JOIN TB85F7AFEC399944089B347BB46D55 ATIV          ON ATIV.CL000000000000ABCD000000000000          = OPA.CL914C1709C69C481D894EEE44E498
                                           LEFT JOIN TBB760FCEDDBFE4965BE358DC6335C TARE          ON TARE.CL000000000000ABCD000000000000          = OPA.CLE8D2FB41B9E44AF4A02D3A14B9A2
                                           LEFT JOIN TB831F15D403654FCE84691FE856D2 AVALIADOR     ON AVALIADOR.CL000000000000ABCD000000000000     = OPA.CL63AF72F4170C4210B50B55664424
                                           LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL    ON CADPESSOAL.CL000000000000ABCD000000000000    = AVALIADOR.CL81F753A75F3B415CAB2037E094BA
                                           LEFT JOIN TBF62704CC120D41D9B6C30C007_TR TIPO_AVAL     ON TIPO_AVAL.CL000000000000ABCD000000000000     = OPA.CL5D338E69148C4AD7A1AA629BBAC4
                                                                                                 AND TIPO_AVAL.ID_IDIOMA_VERSAO                   = '00000000-0000-0000-0000-000000000000'

                                          WHERE OPA.CL000000000000ABCD000000000000 = '{0}' ", idOpa);

            Opa opa = null;
            try
            {
                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    opa = new Opa();
                    DataRow linha = dt.Rows[0];

                    opa.ID_OPA = linha["ID_OPA"].ToString();
                    opa.NUMERO = linha["NUMERO"].ToString();
                    opa.DATA = Convert.ToDateTime(linha["DATA"]);
                    opa.UNIDADE = string.IsNullOrEmpty(Convert.ToString(linha["ID_REGIONAL"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_REGIONAL"])),
                        Codigo = Convert.ToString(linha["CD_REGIONAL"]),
                        Descricao = Convert.ToString(linha["DS_REGIONAL"])
                    };
                    opa.GERENCIA = string.IsNullOrEmpty(Convert.ToString(linha["ID_GERENCIA"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_GERENCIA"])),
                        Codigo = Convert.ToString(linha["CD_GERENCIA"]),
                        Descricao = Convert.ToString(linha["DS_GERENCIA"])
                    };
                    opa.AREA = string.IsNullOrEmpty(Convert.ToString(linha["ID_AREA"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_AREA"])),
                        Codigo = Convert.ToString(linha["CD_AREA"]),
                        Descricao = Convert.ToString(linha["DS_AREA"])
                    };
                    opa.LOCAL = string.IsNullOrEmpty(Convert.ToString(linha["ID_LOCAL"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_LOCAL"])),
                        Codigo = Convert.ToString(linha["CD_LOCAL"]),
                        Descricao = Convert.ToString(linha["DS_LOCAL"])
                    };
                    opa.ATIVIDADE = linha.IsNull("ID_ATIVIDADE") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_ATIVIDADE"])),
                        Codigo = Convert.ToString(linha["CD_ATIVIDADE"]),
                        Descricao = Convert.ToString(linha["DS_ATIVIDADE"])
                    };
                    opa.TAREFA_OBSERVADA = linha.IsNull("ID_TAREFA") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_TAREFA"])),
                        Codigo = Convert.ToString(linha["CD_TAREFA"]),
                        Descricao = Convert.ToString(linha["DS_TAREFA"])
                    };
                    opa.AVALIADOR = string.IsNullOrEmpty(Convert.ToString(linha["ID_AVALIADOR"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_AVALIADOR"])),
                        Codigo = Convert.ToString(linha["CD_AVALIADOR"]),
                        Descricao = Convert.ToString(linha["DS_AVALIADOR"])
                    };
                    opa.TIPO_AVALIADOR = linha.IsNull("ID_TIPO_AVALIADOR") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_TIPO_AVALIADOR"])),
                        Codigo = Convert.ToString(linha["CD_TIPO_AVALIADOR"]),
                        Descricao = Convert.ToString(linha["DS_TIPO_AVALIADOR"])
                    };

                    opa.CAMPOS = this.Campos(parametros).ToList();

                    sql = "SELECT {0} FROM TB32488541CD1C485BB1E42B4F9D91 OPA WHERE CL000000000000ABCD000000000000 = '{1}'";
                    string colunas = string.Empty;
                    foreach (CampoOpa campo in opa.CAMPOS)
                    {
                        foreach (KeyValuePair<string, string> col in campo.Colunas)
                        {
                            if (col.Key == "CaixaNumerica" || col.Key == "CaixaTexto" || col.Key == "CaixaOpcao")
                            {
                                if (colunas.Length > 0)
                                    colunas += ", ";
                                colunas += col.Value;
                            }
                        }
                    }
                    sql = string.Format(sql, colunas, idOpa); 

                    dt = sessao.Contexto.BuscarTabela(sql);
                    if (dt.Rows.Count > 0)
                    {
                        using (RepositorioMultimidia rep = new RepositorioMultimidia(sessao.Contexto))
                        {
                            linha = dt.Rows[0];
                            foreach (CampoOpa campo in opa.CAMPOS)
                            {
                                foreach (KeyValuePair<string, string> col in campo.Colunas)
                                {
                                    if (col.Key == "CaixaNumerica" && !linha.IsNull(col.Value))
                                        campo.NumeroDNA = linha[col.Value].ToString();
                                    else if (col.Key == "CaixaTexto" && !linha.IsNull(col.Value))
                                        campo.Comentario = linha[col.Value].ToString();
                                    else if (col.Key == "CaixaOpcao" && !linha.IsNull(col.Value))
                                        campo.IdConforme = linha[col.Value].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return opa;
        }

        [HttpPost]
        public IEnumerable<CampoOpa> Campos(FormDataCollection parametros)
        {
            List<CampoOpa> lst = new List<CampoOpa>();
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            string sql = @"
SELECT * 
  FROM ( SELECT To_Number(EXTRACTVALUE(XMLTYPE(CT.XM_ATRIBUTOS_XML),'/Atributos/@PosicaoX')) POSICAOX,
                To_Number(EXTRACTVALUE(XMLTYPE(CT.XM_ATRIBUTOS_XML),'/Atributos/@PosicaoY')) POSICAOY,   
                EXTRACTVALUE(XMLTYPE(CT.XM_ATRIBUTOS_XML),'/Atributos/@Texto') Texto,
                REPLACE(EXTRACTVALUE(XMLTYPE(CT.XM_ATRIBUTOS_XML),'/Atributos/@Tipo'), 'Gema.Parametrizacao.Portinari.Toolbox.Controle.', '') Tipo,
                CT.ID_CONTROLE, CT.ID_TABELA, C.NM_COLUNA, C.DS_TITULO
           FROM CONTROLE_TELA CT  
           left JOIN VW_COLUNAS C  ON CT.ID_TABELA = C.ID_TABELA
                                  AND CT.ID_COLUNA = C.ID_COLUNA
          WHERE CT.id_tela = '924ae8ce-125f-4b0e-8d68-2af84798a56c')
 WHERE POSICAOY > 290 
 ORDER BY POSICAOY, POSICAOX";

            DataTable dt = sessao.Contexto.BuscarTabela(sql);
            if (dt.Rows.Count > 0)
            {
                CampoOpa c = null;
                string grupo = string.Empty;
                foreach (DataRow linha in dt.Rows)
                {
                    string tipoControle = linha["Tipo"].ToString();
                    if (tipoControle == "Legenda")
                    {
                        c = null;
                        grupo = linha["Texto"].ToString();
                    }
                    else
                    {
                        if (tipoControle == "CaixaOpcao")
                        {
                            c = new CampoOpa();
                            c.Grupo = grupo;
                            c.IdCampo = linha["ID_CONTROLE"].ToString();
                            c.Titulo = linha["Texto"].ToString();
                            c.Colunas = new Dictionary<string, string>();
                            lst.Add(c);
                        }
                        if (!linha.IsNull("NM_COLUNA") && c != null)
                            c.Colunas.Add(tipoControle, linha["NM_COLUNA"].ToString());
                    }
                }
            }

            return lst;
        }
    }
}
