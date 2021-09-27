using Newtonsoft.Json;
using SDMobileXF.Banco.Tabelas;
using SDMobileXF.Classes;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SDMobileXF.ViewModels
{
    public class ViewModelLogin : ViewModelBase
    {
        private string _login;
        private string _senha;
        private bool _lembrarMe;

        public Command LoginCommand { get; }

        public string Login
        {
            get { return this._login; }
            set 
            { 
                this.DefinirPropriedade(ref this._login, value);
                this.LoginCommand.ChangeCanExecute();
            }
        }

        public string Senha
        {
            get { return this._senha; }
            set 
            { 
                this.DefinirPropriedade(ref this._senha, value);
                this.LoginCommand.ChangeCanExecute();
            }
        }

        public bool LembrarMe
        {
            get { return this._lembrarMe; }
            set 
            {
                this.DefinirPropriedade(ref this._lembrarMe, value);
                if(!this.Ocupado)
                    Config.LembrarMe = value;
            }
        }

        public override bool Ocupado
        {
            get { return this._ocupado; }
            set 
            {
                this.DefinirPropriedade(ref this._ocupado, value);
                this.LoginCommand.ChangeCanExecute();
            }
        }

        public ViewModelLogin(Action<Enumerados.StatusLogin> retornoLogin)
        {
            this.LoginCommand = new Command(
                execute: () => { this.Validar(retornoLogin); },
                canExecute: () => { return !string.IsNullOrEmpty(this.Login) && !string.IsNullOrEmpty(this.Senha) && !this.Ocupado; });

            this.Ocupado = true;
        }

        private async void Validar(Action<Enumerados.StatusLogin> retornoLogin)
        {
            this.Ocupado = true;
            Enumerados.StatusLogin status;

            if (Util.TemAcessoInternet)
                status = await this.ValidarOnLine();
            else
                status = await this.ValidarOffLine();

            retornoLogin(status);
            this.Ocupado = false;
        }

        private async Task<Enumerados.StatusLogin> ValidarOnLine()
        {
            App.Log("Início Login online");
            Enumerados.StatusLogin loginAutorizado = Enumerados.StatusLogin.LoginNaoAutorizado;
            try
            {
                string idChaveSessao = Guid.NewGuid().ToString();
                
                string url = string.Concat(App.__EnderecoWebApi, "/Login/RetornarLoginUsuario");
                if (App.__nivelDeProjeto == "CSN")
                    url = string.Concat(App.__EnderecoWebApi, "/LoginSimples/RetornarLoginUsuario");

                FormUrlEncodedContent param = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("login", this.Login),
                    new KeyValuePair<string,string>("senha", this.Senha),
                    new KeyValuePair<string,string>("idChaveSessao", idChaveSessao),
                    new KeyValuePair<string,string>("NM_BANCO", App.__nivelDeProjeto),
                    new KeyValuePair<string,string>("SG_APLICATIVO", "ST")
                });

                HttpClient requisicao = new HttpClient();
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string conteudo = await resposta.Content.ReadAsStringAsync();
                    Usuario usuario = JsonConvert.DeserializeObject<SDMobileXFDados.Modelos.Usuario>(conteudo);

                    bool loginCpf = this.Login.Length == 11 && System.Text.RegularExpressions.Regex.IsMatch(this.Login, "^[0-9]+$") &&
                        this.Senha.Length == 8 && System.Text.RegularExpressions.Regex.IsMatch(this.Senha, "^[0-9]+$");

                    if (usuario != null && ((!loginCpf && usuario.HS_SENHA != null) || loginCpf)) //temporario ate resolver o serviço. usuario vem do cadastro de profissionais mas sem senha
                    {
                        if (usuario.DT_DESATIVACAO == null || usuario.DT_DESATIVACAO > DateTime.Today)
                        {
                            usuario.ID_CHAVE_SESSAO = idChaveSessao;
                            Config.SalvarConfiguracao("UltimoLogin", this.Login);
                            Config.SalvarConfiguracao("UltimaSenha", this.Senha);

                            usuario.NM_APELIDO = this.Login;
                            usuario.HS_SENHA = Util.CodigoHash2(this.Senha);

                            UsuarioLogado.Instancia = usuario;                            

                            List<USUARIO> usuarios = await App.Banco.BuscarUsuariosPorLoginAsync(this.Login);
                            int ret = 0;
                            if (usuarios.Count == 0)
                                ret = await App.Banco.InserirAsync(UsuarioLogado.Instancia);
                            else
                                ret = await App.Banco.AlterarAsync(UsuarioLogado.Instancia);


                            loginAutorizado = Enumerados.StatusLogin.LoginAutorizado;
                        }
                        else
                            loginAutorizado = Enumerados.StatusLogin.UsuarioInativo;
                    }
                    else
                        UsuarioLogado.Instancia = null;
                }
            }
            catch (Exception ex)
            {
                UsuarioLogado.Instancia = null;
                Debug.WriteLine(ex.Message);
            }
            App.Log("Fim Login online: " + this.Login);
            return loginAutorizado;
        }

        private async Task<Enumerados.StatusLogin> ValidarOffLine()
        {
            App.Log("Fim Login offline");
            Enumerados.StatusLogin status = Enumerados.StatusLogin.LoginNaoAutorizado;
            try
            {
                List<USUARIO> usuarios = await App.Banco.BuscarUsuariosPorLoginAsync(this.Login);
                foreach (USUARIO u in usuarios)
                {
                    if (u.HS_SENHA == Util.CodigoHash2(this.Senha))
                    {
                        UsuarioLogado.Instancia = new Usuario() 
                        {
                            ID_USUARIO = u.ID_USUARIO, 
                            ID_VINCULO = u.ID_VINCULO,
                            DT_DESATIVACAO = u.DT_DESATIVACAO,
                            IDS_REGISTRO_PROFISSIONAL = u.IDS_REGISTRO_PROFISSIONAL,
                            PAPEL_MESTRE = u.PAPEL_MESTRE,
                            PAPEL_SSO    = u.PAPEL_SSO,
                            HS_SENHA = u.HS_SENHA,
                            ID_CHAVE_SESSAO = Guid.NewGuid().ToString(),
                            NM_APELIDO = u.NM_APELIDO,
                            NM_USUARIO = u.NM_USUARIO,
                            NU_CPF = u.NU_CPF,
                            NU_MATRICULA = u.NU_MATRICULA,
                            TEM_ACESSO_ABORDAGEM = u.TEM_ACESSO_ABORDAGEM,
                            TEM_ACESSO_SNA  = u.TEM_ACESSO_SNA,
                            TEM_ACESSO_INSPECOES = u.TEM_ACESSO_INSPECOES,
                            TEM_ACESSO_OPA = u.TEM_ACESSO_OPA
                        };

                        Config.SalvarConfiguracao("UltimoLogin", this.Login);
                        Config.SalvarConfiguracao("UltimaSenha", this.Senha);

                        status = Enumerados.StatusLogin.LoginAutorizado;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                UsuarioLogado.Instancia = null;
                Debug.WriteLine(ex.Message);
            }
            App.Log("Fim Login offline: " + this.Login);
            return status;
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                this.LembrarMe = Config.LembrarMe;
                if (this.LembrarMe)
                {
                    this.Login = Config.CarregarConfiguracao("UltimoLogin")?.ToString();
                    this.Senha = Config.CarregarConfiguracao("UltimaSenha")?.ToString();
                    if (System.Diagnostics.Debugger.IsAttached && string.IsNullOrEmpty(this.Senha))
                        this.Senha = "Topline01*";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
