using SD.Dados;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Web;

namespace SDWebApi.Models
{
    public class Sessao
    {
        public Contexto Contexto { get; private set; }
        public Sessao(Guid idChaveSessao, string nmBanco)
        {
            this.Contexto = Contexto.Instancia(new ConexaoBanco(nmBanco, idChaveSessao, Guid.Empty));
        }

        public static Sessao Instancia(Guid idChaveSessao, string nmBanco)
        {
            return new Sessao(idChaveSessao, nmBanco);
        }

		public bool AutenticarAD(string nome, string dominio, string senha)
		{
			bool ret = false;

			if (!string.IsNullOrEmpty(dominio))
			{
				byte[] bytes = Encoding.Default.GetBytes(senha);

				PrincipalContext principalContext = null;
				if (!dominio.Contains(";"))
				{
					try
					{
						principalContext = new PrincipalContext(ContextType.Domain, dominio);
						ret = principalContext.ValidateCredentials(nome, senha);
					}
					catch
					{ }
				}
				else
				{
					string[] dominios = dominio.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string d in dominios)
					{
						try
						{
							principalContext = new PrincipalContext(ContextType.Domain, d);
							ret = principalContext.ValidateCredentials(nome, senha);
							if (ret)
								break;
						}
						catch
						{ }
					}
				}
			}			
			return ret;
		}
	}
}