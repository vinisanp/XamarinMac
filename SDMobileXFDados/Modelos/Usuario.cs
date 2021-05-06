using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class Usuario
    {
        public Guid ID_USUARIO { get; set; }
        public string NM_USUARIO { get; set; }
        public DateTime? DT_DESATIVACAO { get; set; }
        public string NU_CPF { get; set; }

        public Guid ID_VINCULO { get; set; }
        public string NU_MATRICULA { get; set; }
        public string HS_SENHA { get; set; }

        public string ID_CHAVE_SESSAO { get; set; }

        public string IDS_REGISTRO_PROFISSIONAL{ get; set; }

        public bool PAPEL_MESTRE { get; set; }

        public bool PAPEL_SSO { get; set; }
        public string NM_APELIDO { get; set; }
        public bool TEM_ACESSO_ABORDAGEM { get; set; }
        public bool TEM_ACESSO_SNA { get; set; }
        public bool TEM_ACESSO_INSPECOES { get; set; }
        public bool TEM_ACESSO_OPA { get; set; }
    }
}
