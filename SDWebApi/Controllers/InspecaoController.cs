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
    public class InspecaoController : ApiController
    {
        // GET: api/SegurancaNaArea
        public IEnumerable<string> Get()
        {
            Contexto contexto = Contexto.Instancia(new ConexaoBanco("BANCO", null, null));

            DateTime data = contexto.DataBanco;

            return new string[] { "Inspeção", data.ToShortDateString() };
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
                Guid idTabela = new Guid("ec4866f9-f918-46a0-933c-5a8da73aec12");
                Guid idColuna = new Guid("c388c0ff-c35b-4680-aa4f-f9da4f39888c");

                string numero = parametros.Get("NU_INSPECAO");

                string participantes = parametros.Get("PARTICIPANTES");
                List<string> lstParticipantes = new List<string>();
                if (!string.IsNullOrEmpty(participantes))
                    lstParticipantes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(participantes);

                string campos = parametros.Get("CAMPOS");
                List<CampoInspecao> lstCampos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CampoInspecao>>(campos);

                if (string.IsNullOrEmpty(numero))
                {
                    numero = Util.GerarSequencia(sessao.Contexto, idTabela, idColuna, idRegistro, DateTime.Today);

                    sql = @" INSERT INTO TBEC4866F9F91846A0933C5A8DA73A
                                  ( CL000000000000ABCD000000000000, CLC388C0FFC35B4680AA4FF9DA4F39, CL000000000000ABCD001100000000,  
                                    CLFC0A5487EF054DD987189FC7488B, CLB902473D1C6B490195C14E304540, 
                                    CL062725DEC7BB4040927156C70452, CL8451E852C2CD4E3AA24FC66E2FF4, 
                                    CL6F32E8F8649E432BA8C4A77DA584, CLFE52A221DC074F328BA558534862, CL63A3BAA7ECD2440392DD9C3F0E53, 
                                    CLD4206822651B4EEE99E940FC3AC1
                                    {0}
                                  ) " +
                        $" VALUES ( '{idRegistro}', '{numero}', " + $" TO_DATE('{parametros.Get("DT_DATA")}', 'YYYYMMDD HH24MISS'), " +
                                 $" '{parametros.Get("ID_REGIONAL")}', '{parametros.Get("ID_GERENCIA")}', " +
                                 $" '{parametros.Get("ID_AREA")}', '{parametros.Get("ID_LOCAL")}', " +
                                 $" '{parametros.Get("ID_FORNECEDOR")}', '{parametros.Get("ID_TIPO")}', '{parametros.Get("ID_ATIVIDADE")}', " +
                                 $" '{parametros.Get("ID_REALIZADOPOR")}' " +
                                 "  {1} ) ";

                    string colunas = string.Empty;
                    string valores = string.Empty;
                    foreach (CampoInspecao c in lstCampos)
                    {
                        if (!string.IsNullOrEmpty(c.NumeroDNA))
                        {
                            colunas += ", " + c.Colunas["CaixaNumerica"];
                            valores += ", " + c.NumeroDNA;
                        }

                        if (!string.IsNullOrEmpty(c.Descricao))
                        {
                            colunas += ", " + c.Colunas["CaixaTexto"];
                            valores += ", '" + c.Descricao.Replace("'", "''") + "' ";
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
                    idRegistro = new Guid(sessao.Contexto.BuscarValorStr($"SELECT CL000000000000ABCD000000000000 FROM TBEC4866F9F91846A0933C5A8DA73A WHERE CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}'"));

                    sql = "UPDATE TBEC4866F9F91846A0933C5A8DA73A " +
                          $"  SET CLC388C0FFC35B4680AA4FF9DA4F39 = '{numero}', " +
                          $"      CL000000000000ABCD001100000000 = TO_DATE('{parametros.Get("DT_DATA")}', 'YYYYMMDD HH24MISS')', " +
                          $"      CLFC0A5487EF054DD987189FC7488B = '{parametros.Get("ID_REGIONAL")}', " +
                          $"      CLB902473D1C6B490195C14E304540 = '{parametros.Get("ID_GERENCIA")}', " +
                          $"      CL062725DEC7BB4040927156C70452 = '{parametros.Get("ID_AREA")}', " +
                          $"      CL8451E852C2CD4E3AA24FC66E2FF4 = '{parametros.Get("ID_LOCAL")}', " +
                          $"      CL6F32E8F8649E432BA8C4A77DA584 = '{parametros.Get("ID_FORNECEDOR")}', " +
                          $"      CLFE52A221DC074F328BA558534862 = '{parametros.Get("ID_TIPO")}', " +
                          $"      CL63A3BAA7ECD2440392DD9C3F0E53 = '{parametros.Get("ID_ATIVIDADE")}', " +
                          $"      CLD4206822651B4EEE99E940FC3AC1 = '{parametros.Get("ID_REALIZADOPOR")}' " + "{0}" +
                         $" WHERE CL000000000000ABCD000000000000 = '{idRegistro}' ";

                    string colunas = string.Empty;
                    foreach (CampoInspecao c in lstCampos)
                    {
                        if (!string.IsNullOrEmpty(c.NumeroDNA))
                            colunas += ", " + c.Colunas["CaixaNumerica"] + " = '" + c.NumeroDNA + "'";

                        if (!string.IsNullOrEmpty(c.Descricao))
                            colunas += ", " + c.Colunas["CaixaTexto"] + " = '" + c.Descricao.Replace("'", "''") + "'";

                        if (!string.IsNullOrEmpty(c.IdConforme))
                            colunas += ", " + c.Colunas["CaixaOpcao"] + " = '" + c.IdConforme + "'";
                    }

                    sql = string.Format(sql, colunas);
                }
                try
                {
                    sessao.Contexto.Executar(sql);

                    sessao.Contexto.Executar($"DELETE TBEC4866F9F91846A0933C5A8DA_MV WHERE ID_REGISTRO = '{idRegistro}'");

                    foreach (string id in lstParticipantes)
                        sessao.Contexto.Executar($"INSERT INTO TBEC4866F9F91846A0933C5A8DA_MV (CL000000000000ABCD000000000000, ID_REGISTRO, NM_TABELA, CL000000000000ABCD001300000000)" +
                                                 $"VALUES (FC_GUID, '{idRegistro}', 'TB831F15D403654FCE84691FE856D2', '{id}')");

                    string colunas = string.Empty;
                    foreach (CampoInspecao c in lstCampos)
                    {
                        if (colunas.Length > 0)
                            colunas += " || '|' || ";
                        if(c.Colunas != null)
                            colunas += c.Colunas["CaixaNumerica"];
                        else
                            colunas += "''";
                    }

                    sql = $"SELECT {colunas} FROM TBEC4866F9F91846A0933C5A8DA73A WHERE CL000000000000ABCD000000000000 = '{idRegistro}' ";

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
        public List<Inspecao> RetornarInspecoes(FormDataCollection parametros)
        {
            List<Inspecao> lst = new List<Inspecao>();

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            string dtInicial = parametros.Get("dtInicial");
            string dtFinal = parametros.Get("dtFinal");
            string idVinculo = parametros.Get("idVinculo");

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            using (RepositorioOcorrenciaAnomalia repositorio = new RepositorioOcorrenciaAnomalia(sessao.Contexto))
            {
                string sql = string.Format(@"SELECT INSP.CL000000000000ABCD000000000000 ID_INSPECAO,
                                                    INSP.CLC388C0FFC35B4680AA4FF9DA4F39 NUMERO, 
                                                    INSP.CL000000000000ABCD001100000000 DATA, 

                                                    TIPO.CL000000000000ABCD000000000000 ID_TIPO,
                                                    TIPO.CL000000000000ABCD000200000000 CD_TIPO,
                                                    TIPO.CL000000000000ABCD000300000000 DS_TIPO,

                                                    ATIV.CL000000000000ABCD000000000000 ID_ATIVIDADE,
                                                    ATIV.CL000000000000ABCD000200000000 CD_ATIVIDADE,
                                                    ATIV.CL000000000000ABCD000300000000 DS_ATIVIDADE

                                               FROM TBEC4866F9F91846A0933C5A8DA73A INSP
                                               LEFT JOIN TBC5E3FF6DDA90496F96B48978A227 TIPO ON TIPO.CL000000000000ABCD000000000000 = INSP.CLFE52A221DC074F328BA558534862
                                               LEFT JOIN TB85F7AFEC399944089B347BB46D55 ATIV ON ATIV.CL000000000000ABCD000000000000 = INSP.CL63A3BAA7ECD2440392DD9C3F0E53
                                              WHERE CLD4206822651B4EEE99E940FC3AC1 = '{0}'
                                                AND CL000000000000ABCD001100000000 >= TO_DATE('{1}', 'dd/MM/yyyy') 
                                                AND CL000000000000ABCD001100000000 < (TO_DATE('{2}', 'dd/MM/yyyy') + 1)                                                
                                              ORDER BY CL000000000000ABCD001100000000 DESC", idVinculo, dtInicial, dtFinal);

                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow linha in dt.Rows)
                    {
                        Inspecao insp = new Inspecao();
                        insp.ID_INSPECAO = linha["ID_INSPECAO"].ToString();
                        insp.NUMERO = linha["NUMERO"].ToString();
                        insp.DATA = Convert.ToDateTime(linha["DATA"]);
                        insp.TIPO = linha.IsNull("ID_TIPO") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                        {
                            Id = new Guid(Convert.ToString(linha["ID_TIPO"])),
                            Codigo = Convert.ToString(linha["CD_TIPO"]),
                            Descricao = Convert.ToString(linha["DS_TIPO"])
                        };
                        insp.ATIVIDADE = linha.IsNull("ID_ATIVIDADE") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                        {
                            Id = new Guid(Convert.ToString(linha["ID_ATIVIDADE"])),
                            Codigo = Convert.ToString(linha["CD_ATIVIDADE"]),
                            Descricao = Convert.ToString(linha["DS_ATIVIDADE"])
                        };

                        lst.Add(insp);
                    }
                }
            }

            return lst;
        }

        [HttpPost]
        public Inspecao Retornar(FormDataCollection parametros)
        {
            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idInspecao = new Guid(parametros.Get("idInspecao"));

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

            string sql = string.Format(@"SELECT INSP.CL000000000000ABCD000000000000 ID_INSPECAO,
                                                INSP.CLC388C0FFC35B4680AA4FF9DA4F39 NUMERO, 
                                                INSP.CL000000000000ABCD001100000000 DATA, 

                                                INSP.CLFC0A5487EF054DD987189FC7488B ID_REGIONAL,
                                                INSP.CLB902473D1C6B490195C14E304540 ID_GERENCIA,
                                                INSP.CL062725DEC7BB4040927156C70452 ID_AREA,
                                                INSP.CL8451E852C2CD4E3AA24FC66E2FF4 ID_LOCAL,
                                                INSP.CLFE52A221DC074F328BA558534862 ID_TIPO,
                                                INSP.CL63A3BAA7ECD2440392DD9C3F0E53 ID_ATIVIDADE,
                                                INSP.CLD4206822651B4EEE99E940FC3AC1 ID_REALIZADOPOR,
                                                INSP.CL6F32E8F8649E432BA8C4A77DA584 ID_FORNECEDOR,

                                                REGI.CL000000000000ABCD000200000000 CD_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000200000000 CD_GERENCIA, 
                                                AREA.CLFD7DF78DB1A446B292F9C76515EC CD_AREA,
                                                LOCAL.CL000000000000ABCD000200000000 CD_LOCAL,
                                                TIPO.CL000000000000ABCD000200000000 CD_TIPO,
                                                ATIV.CL000000000000ABCD000200000000 CD_ATIVIDADE,
                                                REALIZADOPOR.CLA95D0DD6FEFB4A63A7CC460CF494 CD_REALIZADOPOR,
                                                FORN.CLA95D0DD6FEFB4A63A7CC460CF494 CD_FORNECEDOR,

                                                REGI.CL000000000000ABCD000300000000 DS_REGIONAL,                                        
                                                GERE.CL000000000000ABCD000300000000 DS_GERENCIA,   
                                                AREA.CLD79FBD89F9CC4808BEA6E8D732D6 DS_AREA,
                                                LOCAL.CL000000000000ABCD000300000000 DS_LOCAL,
                                                TIPO.CL000000000000ABCD000300000000 DS_TIPO,
                                                ATIV.CL000000000000ABCD000300000000 DS_ATIVIDADE,
                                                CADPESSOAL.CL58284C47272B4729ABF5215F368D DS_REALIZADOPOR,
                                                FORN.CL10914D4F3685436282FBBC16F662 DS_FORNECEDOR

                                           FROM TBEC4866F9F91846A0933C5A8DA73A INSP
                                           LEFT JOIN TB781D63F32AEA4423B070E7EB190C REGI          ON REGI.CL000000000000ABCD000000000000          = INSP.CLFC0A5487EF054DD987189FC7488B
                                           LEFT JOIN TB271A941798BA43378CEE3CDC3A9D GERE          ON GERE.CL000000000000ABCD000000000000          = INSP.CLB902473D1C6B490195C14E304540
                                           LEFT JOIN TB69A8C990799C4B77B4F0D1CE8ECA AREA          ON AREA.CL000000000000ABCD000000000000          = INSP.CL062725DEC7BB4040927156C70452
                                           LEFT JOIN TB6BB39BD086DD4DBCAF41AB4149E2 LOCAL         ON LOCAL.CL000000000000ABCD000000000000         = INSP.CL8451E852C2CD4E3AA24FC66E2FF4
                                           LEFT JOIN TBC5E3FF6DDA90496F96B48978A227 TIPO          ON TIPO.CL000000000000ABCD000000000000          = INSP.CLFE52A221DC074F328BA558534862
                                           LEFT JOIN TB85F7AFEC399944089B347BB46D55 ATIV          ON ATIV.CL000000000000ABCD000000000000          = INSP.CL63A3BAA7ECD2440392DD9C3F0E53
                                           LEFT JOIN TBFB7C40BCD3E2487FBFFD7BC6F98F FORN          ON FORN.CL000000000000ABCD000000000000          = INSP.CL6F32E8F8649E432BA8C4A77DA584
                                           LEFT JOIN TB831F15D403654FCE84691FE856D2 REALIZADOPOR  ON REALIZADOPOR.CL000000000000ABCD000000000000  = INSP.CLD4206822651B4EEE99E940FC3AC1
                                           LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL    ON CADPESSOAL.CL000000000000ABCD000000000000    = REALIZADOPOR.CL81F753A75F3B415CAB2037E094BA
                                          WHERE INSP.CL000000000000ABCD000000000000 = '{0}' ", idInspecao);

            Inspecao insp = null;
            try
            {
                DataTable dt = sessao.Contexto.BuscarTabela(sql);
                if (dt.Rows.Count > 0)
                {
                    insp = new Inspecao();
                    DataRow linha = dt.Rows[0];

                    insp.ID_INSPECAO = linha["ID_INSPECAO"].ToString();
                    insp.NUMERO = linha["NUMERO"].ToString();
                    insp.DATA = Convert.ToDateTime(linha["DATA"]);
                    insp.TIPO = linha.IsNull("ID_TIPO") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_TIPO"])),
                        Codigo = Convert.ToString(linha["CD_TIPO"]),
                        Descricao = Convert.ToString(linha["DS_TIPO"])
                    };
                    insp.ATIVIDADE = linha.IsNull("ID_ATIVIDADE") ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_ATIVIDADE"])),
                        Codigo = Convert.ToString(linha["CD_ATIVIDADE"]),
                        Descricao = Convert.ToString(linha["DS_ATIVIDADE"])
                    };
                    insp.UNIDADE = string.IsNullOrEmpty(Convert.ToString(linha["ID_REGIONAL"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_REGIONAL"])),
                        Codigo = Convert.ToString(linha["CD_REGIONAL"]),
                        Descricao = Convert.ToString(linha["DS_REGIONAL"])
                    };
                    insp.GERENCIA = string.IsNullOrEmpty(Convert.ToString(linha["ID_GERENCIA"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_GERENCIA"])),
                        Codigo = Convert.ToString(linha["CD_GERENCIA"]),
                        Descricao = Convert.ToString(linha["DS_GERENCIA"])
                    };
                    insp.AREA = string.IsNullOrEmpty(Convert.ToString(linha["ID_AREA"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_AREA"])),
                        Codigo = Convert.ToString(linha["CD_AREA"]),
                        Descricao = Convert.ToString(linha["DS_AREA"])
                    };
                    insp.LOCAL = string.IsNullOrEmpty(Convert.ToString(linha["ID_LOCAL"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_LOCAL"])),
                        Codigo = Convert.ToString(linha["CD_LOCAL"]),
                        Descricao = Convert.ToString(linha["DS_LOCAL"])
                    };
                    insp.PARTICIPANTES = new List<SDMobileXFDados.Modelos.ModeloObj>();
                    insp.REALIZADOPOR = string.IsNullOrEmpty(Convert.ToString(linha["ID_REALIZADOPOR"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_REALIZADOPOR"])),
                        Codigo = Convert.ToString(linha["CD_REALIZADOPOR"]),
                        Descricao = Convert.ToString(linha["DS_REALIZADOPOR"])
                    };
                    insp.FORNECEDOR = string.IsNullOrEmpty(Convert.ToString(linha["ID_FORNECEDOR"])) ? null : new SDMobileXFDados.Modelos.ModeloObj()
                    {
                        Id = new Guid(Convert.ToString(linha["ID_FORNECEDOR"])),
                        Codigo = Convert.ToString(linha["CD_FORNECEDOR"]),
                        Descricao = Convert.ToString(linha["DS_FORNECEDOR"])
                    };

                    sql = $"SELECT MV.CL000000000000ABCD001300000000 ID_PARTICIPANTE, " +
                          $"       PARTIC.CLA95D0DD6FEFB4A63A7CC460CF494 CD_PARTICIPANTE, " +
                          $"       CADPESSOAL.CL58284C47272B4729ABF5215F368D DS_PARTICIPANTE " +
                          $"  FROM TBEC4866F9F91846A0933C5A8DA_MV MV" +
                          $"  LEFT JOIN TB831F15D403654FCE84691FE856D2 PARTIC     ON PARTIC.CL000000000000ABCD000000000000 = MV.CL000000000000ABCD001300000000" +
                          $"  LEFT JOIN TB81F753A75F3B415CAB2037E094BA CADPESSOAL ON CADPESSOAL.CL000000000000ABCD000000000000 = PARTIC.CL81F753A75F3B415CAB2037E094BA" +
                          $" WHERE ID_REGISTRO = '{idInspecao}'" +
                          $"   AND CL000000000000ABCD001300000000 IS NOT NULL";
                    dt = sessao.Contexto.BuscarTabela(sql);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow pRow in dt.Rows)
                        {
                            var p = new SDMobileXFDados.Modelos.ModeloObj()
                            {
                                Id = new Guid(Convert.ToString(pRow["ID_PARTICIPANTE"])),
                                Codigo = Convert.ToString(pRow["CD_PARTICIPANTE"]),
                                Descricao = Convert.ToString(pRow["DS_PARTICIPANTE"])
                            };

                            insp.PARTICIPANTES.Add(p);
                        }
                    }

                    insp.CAMPOS = this.Campos(parametros).ToList();

                    sql = "SELECT {0} FROM TBEC4866F9F91846A0933C5A8DA73A INSP WHERE CL000000000000ABCD000000000000 = '{1}'";
                    string colunas = string.Empty;
                    foreach (CampoInspecao campo in insp.CAMPOS)
                    {
                        foreach (KeyValuePair<string, string> col in campo.Colunas)
                        {
                            if (col.Key == "CaixaNumerica" || col.Key == "CaixaTexto" || col.Key == "CaixaOpcao" || col.Key == "Multimidia")
                            {
                                if (colunas.Length > 0)
                                    colunas += ", ";
                                colunas += col.Value;
                            }
                        }
                    }
                    sql = string.Format(sql, colunas, idInspecao); 

                    dt = sessao.Contexto.BuscarTabela(sql);
                    if (dt.Rows.Count > 0)
                    {
                        using (RepositorioMultimidia rep = new RepositorioMultimidia(sessao.Contexto))
                        {
                            linha = dt.Rows[0];
                            foreach (CampoInspecao campo in insp.CAMPOS)
                            {
                                foreach (KeyValuePair<string, string> col in campo.Colunas)
                                {
                                    if (col.Key == "CaixaNumerica" && !linha.IsNull(col.Value))
                                        campo.NumeroDNA = linha[col.Value].ToString();
                                    else if (col.Key == "CaixaTexto" && !linha.IsNull(col.Value))
                                        campo.Descricao = linha[col.Value].ToString();
                                    else if (col.Key == "CaixaOpcao" && !linha.IsNull(col.Value))
                                        campo.IdConforme = linha[col.Value].ToString();
                                    else if (col.Key == "Multimidia" && !linha.IsNull(col.Value))
                                    {
                                        string idFoto = linha[col.Value].ToString();
                                        MULTIMIDIA m = rep.RetornarItem(new Guid(idFoto));
                                        if (m!= null)
                                            campo.Image = m.BI_DOCUMENTO;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return insp;
        }

        [HttpPost]
        public IEnumerable<CampoInspecao> Campos(FormDataCollection parametros)
        {
            List<CampoInspecao> lst = new List<CampoInspecao>();
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
          WHERE CT.id_tela = '4d7d7a88-523a-42d9-8c21-6fdb8484e9ed')
 WHERE POSICAOY > 400 
 ORDER BY POSICAOY, POSICAOX";

            DataTable dt = sessao.Contexto.BuscarTabela(sql);
            if (dt.Rows.Count > 0)
            {
                CampoInspecao cAnt = null;
                foreach (DataRow linha in dt.Rows)
                {
                    string tipoControle = linha["Tipo"].ToString();
                    if (tipoControle == "Legenda")
                    {
                        CampoInspecao c = new CampoInspecao();
                        c.IdCampo = linha["ID_CONTROLE"].ToString();
                        c.Pergunta = linha["Texto"].ToString();
                        if (!char.IsNumber(c.Pergunta[0]))
                            cAnt.Pergunta = string.Concat(cAnt.Pergunta.TrimEnd(), " ", c.Pergunta);
                        else
                        {
                            c.Colunas = new Dictionary<string, string>();
                            lst.Add(c);
                            cAnt = c;
                        }
                    }
                    else
                    {
                        if (!linha.IsNull("NM_COLUNA"))
                            cAnt.Colunas.Add(tipoControle, linha["NM_COLUNA"].ToString());
                    }
                }
            }

            return lst;
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
                    string IdInspecao = httpRequest.Headers.Get("ID_INSPECAO");
                    string nmColuna = httpRequest.Headers.Get("NM_COLUNA");
                    Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");

                    List<string> lstfiles = new List<string>();

                    foreach (string file in httpRequest.Files)
                    {
                        HttpPostedFile postedFile = httpRequest.Files[file];
                        string filePath = Path.Combine(pasta, postedFile.FileName);
                        //postedFile.SaveAs(filePath);
                        lstfiles.Add(filePath);
                        using (RepositorioMultimidia rep = new RepositorioMultimidia(sessao.Contexto))
                        {
                            string nmArquivo = Path.GetFileName(postedFile.FileName);
                            string nmTabela = "TBEC4866F9F91846A0933C5A8DA73A";

                            using (BinaryReader binaryReader = new BinaryReader(postedFile.InputStream))
                            {
                                string biDocumento = Convert.ToBase64String(binaryReader.ReadBytes(postedFile.ContentLength));
                                Guid idFoto = Guid.NewGuid();
                                rep.Incluir(nmTabela, nmColuna, idFoto, biDocumento);
                                sessao.Contexto.Salvar();
                                sessao.Contexto.Executar($"UPDATE TBEC4866F9F91846A0933C5A8DA73A SET {nmColuna} = '{idFoto.ToString()}' WHERE CL000000000000ABCD000000000000 = '{IdInspecao}'");
                            }
                        }
                    }

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

    }
}
