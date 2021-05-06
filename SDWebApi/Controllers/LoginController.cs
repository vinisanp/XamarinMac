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
    public class LoginController : ApiController
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
            string nuMatricula = string.Empty;

            Sessao sessao = Sessao.Instancia(idChaveSessao, "SUZANO");
            SD.Dados.Modelos.USUARIO usuarioOrigem = null;
            bool papelMestre = false;
            bool papelSSO = false;
            bool temAcessoAbordagem = false;
            bool temAcessoSNA = false;
            bool temAcessoInspecoes = false;
            bool temAcessoOPA = false;


            if (senha.Length == 8 && Regex.IsMatch(senha, "^[0-9]+$"))//se tem 8 digitos e so numero verificar se a senha é a data de nascimento
            {
                using (RepositorioCadastroBasico repCadastroBasico = new RepositorioCadastroBasico(sessao.Contexto))
                {
                    CADASTRO_BASICO cad = repCadastroBasico.RetornarItemPorCPF(login);
                    if (cad != null)
                    {
                        if (cad.DT_NASCIMENTO.HasValue && cad.DT_NASCIMENTO.Value.ToString("ddMMyyyy") == senha)
                        {
                            usuarioOrigem = new SD.Dados.Modelos.USUARIO();
                            usuarioOrigem.ID_USUARIO = cad.ID_CADASTRO;
                            usuarioOrigem.NM_USUARIO = cad.NM_CADASTRO;
                            usuarioOrigem.NU_CPF = login;

                            VINCULO_CONTRATUAL vinc = cad.LISTA_VINCULOS.OrderByDescending(v => v.DT_ADMISSAO).FirstOrDefault();
                            if (vinc.DT_DEMISSAO != null)
                                usuarioOrigem.DT_DESATIVACAO = vinc.DT_DEMISSAO;
                            idVinculo = vinc.ID_VINCULO;
                            nuMatricula = vinc.NU_MATRICULA;

                            using (RepositorioUsuario repositorioUsuario = new RepositorioUsuario(sessao.Contexto))
                            {
                                repositorioUsuario.VerificarUsuarioNoCadastroProfissionais(ref usuarioOrigem, login, login);
                            }
                        }
                    }
                }
            }            

            if (usuarioOrigem == null)
            {
                bool loginRedeValido = false;                
                PARAMETROS_SEGURANCA parametrosSeguranca = null;
                using (RepositorioParametrosSeguranca repositorioParametrosSeguranca = new RepositorioParametrosSeguranca(sessao.Contexto))
                {
                    parametrosSeguranca = repositorioParametrosSeguranca.RetornarItem();
                    if (parametrosSeguranca.ST_AUTENTICA_REDE)
                        loginRedeValido = sessao.AutenticarAD(login, parametrosSeguranca.NM_DOMINIOS, senha);
                }
                
                using (RepositorioUsuario repositorioUsuario = new RepositorioUsuario(sessao.Contexto))
                {
                    string hash = Gema.Criptografia.CodigoHash(senha);                    
                    usuarioOrigem = repositorioUsuario.RetornarItemPorLoginSenha(login, hash);

                    if (loginRedeValido) //metodos liberados somente se a autenticação for pela rede
                    {
                        if (usuarioOrigem == null)
                            usuarioOrigem = repositorioUsuario.RetornarItemPorLogin(login);

                        if (usuarioOrigem == null)
                            usuarioOrigem = repositorioUsuario.RetornarUsuarioDoVinculo(login);
                    }

                   //Verificar se a chave de rede está no cadastro de profissionais
                    repositorioUsuario.VerificarUsuarioNoCadastroProfissionais(ref usuarioOrigem, login, 
                    usuarioOrigem != null && usuarioOrigem.CADASTRO_BASICO != null ? usuarioOrigem.CADASTRO_BASICO.CPF : string.Empty);

                    if (usuarioOrigem != null && usuarioOrigem.CADASTRO_BASICO != null)
                    {                        
                        VINCULO_CONTRATUAL vinculo = usuarioOrigem.CADASTRO_BASICO.LISTA_VINCULOS.OrderByDescending(v => v.DT_ADMISSAO).FirstOrDefault();
                        idVinculo = vinculo.ID_VINCULO;
                        nuMatricula = vinculo.NU_MATRICULA;
                    }
                }

            }

            if (usuarioOrigem != null)
            {
                string perfisIN = string.Empty;
                using (RepositorioPerfil repositorioPerfil = new RepositorioPerfil(sessao.Contexto))
                {
                    List<PERFIL> perfis = repositorioPerfil.RetornarPerfisUtilizados(usuarioOrigem.ID_USUARIO, true).ToList();
                    repositorioPerfil.RetornarPerfisPorCadastroProfissional(usuarioOrigem, ref perfis);
                    papelMestre = perfis.Any(p => p.TP_PAPEL == 11);
                    temAcessoOPA = temAcessoInspecoes = temAcessoSNA = temAcessoAbordagem = perfis.Any(p => p.TP_PAPEL == 8) || perfis.Any(p => p.TP_PAPEL == 10);
                }

                SDMobileXFDados.Modelos.Usuario usuarioApp = new SDMobileXFDados.Modelos.Usuario()
                {
                    ID_USUARIO = usuarioOrigem.ID_USUARIO,
                    NM_USUARIO = usuarioOrigem.NM_USUARIO,
                    NM_APELIDO = usuarioOrigem.NM_APELIDO,
                    NU_CPF = usuarioOrigem.NU_CPF,
                    DT_DESATIVACAO = usuarioOrigem.DT_DESATIVACAO,
                    ID_VINCULO = idVinculo,
                    NU_MATRICULA = nuMatricula,
                    PAPEL_MESTRE = papelMestre,
                    PAPEL_SSO = papelSSO,
                    HS_SENHA = usuarioOrigem.HS_SENHA,
                    TEM_ACESSO_ABORDAGEM = temAcessoAbordagem,
                    TEM_ACESSO_SNA = temAcessoSNA,
                    TEM_ACESSO_INSPECOES = temAcessoInspecoes,
                    TEM_ACESSO_OPA = temAcessoOPA

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
    }
}
