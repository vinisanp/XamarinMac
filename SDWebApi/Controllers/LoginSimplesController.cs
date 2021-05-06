using SD.Dados.Repositorio;
using SD.Dados.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using SD.Dados;
using SDWebApi.Models;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Data;

namespace SDWebApi.Controllers
{
    public class LoginSimplesController : ApiController
    {
        // GET: api/Login
        public IEnumerable<string> Get()
        {
            Contexto contexto = Contexto.Instancia(new ConexaoBanco("BANCO", null, null));

            DateTime data = contexto.DataBanco;

            return new string[] { "Login", data.ToShortDateString() };
        }

        // GET: api/Login/5 
        [HttpPost]
        public SDMobileXFDados.Modelos.Usuario RetornarLoginUsuario(FormDataCollection parametros)
        {
            string login = parametros.Get("login");
            string senha = parametros.Get("senha");

            Guid idChaveSessao = new Guid(parametros.Get("idChaveSessao"));
            Guid idVinculo = Guid.Empty;
            string cpf = string.Empty;
            string nuMatricula = string.Empty;

            string nmBanco = "CSN";
            if(parametros.Get("NM_BANCO") != null)
                nmBanco = parametros.Get("NM_BANCO").ToString();

            Sessao sessao = Sessao.Instancia(idChaveSessao, nmBanco);
            SD.Dados.Modelos.USUARIO usuarioOrigem = null;

            bool loginRedeValido = false;
            PARAMETROS_SEGURANCA parametrosSeguranca = null;
            using (RepositorioParametrosSeguranca repositorioParametrosSeguranca = new RepositorioParametrosSeguranca(sessao.Contexto))
            {
                parametrosSeguranca = repositorioParametrosSeguranca.RetornarItem();
                if (parametrosSeguranca.ST_AUTENTICA_REDE && login != "sdgt")
                    loginRedeValido = sessao.AutenticarAD(login, parametrosSeguranca.NM_DOMINIOS, senha);
                else
                    loginRedeValido = true;
            }

            if (loginRedeValido)
            {
                using (RepositorioUsuario repositorioUsuario = new RepositorioUsuario(sessao.Contexto))
                {
                    string hash = Gema.Criptografia.CodigoHash(senha);
                    usuarioOrigem = repositorioUsuario.RetornarItemPorLoginSenha(login, hash);

                    if (usuarioOrigem == null)
                        usuarioOrigem = repositorioUsuario.RetornarItemPorLogin(login);

                    if (usuarioOrigem != null && usuarioOrigem.CADASTRO_BASICO != null)
                        cpf = usuarioOrigem.CADASTRO_BASICO.CPF;

                    //Verificar se a chave de rede está no cadastro de profissionais
                    repositorioUsuario.VerificarUsuarioNoCadastroProfissionais(ref usuarioOrigem, login, cpf);

                    if (usuarioOrigem == null && parametrosSeguranca.BuscarLoginDoVinculo)
                        usuarioOrigem = repositorioUsuario.RetornarUsuarioDoVinculo(login);

                    if (usuarioOrigem != null)
                    {
                        if (usuarioOrigem.CADASTRO_BASICO != null && usuarioOrigem.CADASTRO_BASICO.LISTA_VINCULOS != null)
                        {
                            VINCULO_CONTRATUAL vinculo = usuarioOrigem.CADASTRO_BASICO.LISTA_VINCULOS.OrderByDescending(v => v.DT_ADMISSAO).FirstOrDefault();
                            idVinculo = vinculo.ID_VINCULO;
                            nuMatricula = vinculo.NU_MATRICULA;
                        }

                        using (RepositorioPerfil repositorioPerfil = new RepositorioPerfil(sessao.Contexto))
                        {
                            IEnumerable<PERFIL> perfis = repositorioPerfil.RetornarPerfisUtilizados(usuarioOrigem.ID_USUARIO, true);
                            IEnumerable<Guid> idsPerfis = perfis.Select<PERFIL, Guid>(c => c.ID_PERFIL);
                            using (RepositorioAplicativo repositorioAplicativo = new RepositorioAplicativo(sessao.Contexto))
                            {
                                IEnumerable<APLICATIVO> lstAplicativos = repositorioAplicativo.RetornarItensPorPerfil(idsPerfis.ToList(), false, false, false);
                                string app = parametros.Get("SG_APLICATIVO").ToString();
                                if (!lstAplicativos.Any(a => a.SG_CATEGORIA.Trim().ToUpper() == app.ToUpper()))
                                    usuarioOrigem = null;
                            }
                        }
                    }

                    if (!parametrosSeguranca.ST_AUTENTICA_REDE && usuarioOrigem.HS_SENHA != hash)
                        usuarioOrigem = null;
                }

                if (usuarioOrigem != null)
                {

                    SDMobileXFDados.Modelos.Usuario usuarioApp = new SDMobileXFDados.Modelos.Usuario()
                    {
                        ID_USUARIO = usuarioOrigem.ID_USUARIO,
                        NM_USUARIO = usuarioOrigem.NM_USUARIO,
                        NM_APELIDO = usuarioOrigem.NM_APELIDO,
                        NU_CPF = usuarioOrigem.NU_CPF,
                        DT_DESATIVACAO = usuarioOrigem.DT_DESATIVACAO,
                        ID_VINCULO = idVinculo,
                        NU_MATRICULA = nuMatricula,
                        HS_SENHA = usuarioOrigem.HS_SENHA

                    };
                    foreach (REGISTRO_PROFISSIONAL regProf in usuarioOrigem.LISTA_REGISTRO_PROFISSIONAL)
                        usuarioApp.IDS_REGISTRO_PROFISSIONAL += $"'{regProf.ID_REGISTRO}',";

                    if (!string.IsNullOrEmpty(usuarioApp.IDS_REGISTRO_PROFISSIONAL))
                    {
                        usuarioApp.IDS_REGISTRO_PROFISSIONAL = usuarioApp.IDS_REGISTRO_PROFISSIONAL.Remove(usuarioApp.IDS_REGISTRO_PROFISSIONAL.LastIndexOf(","));

                        using (RepositorioPesquisa pesq = new RepositorioPesquisa(sessao.Contexto))
                        {
                            System.Data.DataTable dtRegionais = pesq.GetDados(@"SELECT ID_REGISTRO FROM TB781D63F32AEA4423B070E7EB1_MV WHERE CLBAAC0265C4984D2084C6BA032E87 IN (" +
                                usuarioApp.IDS_REGISTRO_PROFISSIONAL + ") AND ROWNUM = 1");
                            usuarioApp.PAPEL_SSO = dtRegionais.Rows.Count > 0;
                        }
                    }

                    return usuarioApp;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
