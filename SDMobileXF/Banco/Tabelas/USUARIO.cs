using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class USUARIO
    {
        [PrimaryKey]
        public Guid ID_USUARIO { get; set; }
        public string NM_APELIDO { get; set; }
        public string NM_USUARIO { get; set; }
        public DateTime? DT_DESATIVACAO { get; set; }
        public string NU_CPF { get; set; }
        public string HS_SENHA { get; set; }
        public Guid ID_VINCULO { get; set; }
        public string NU_MATRICULA { get; set; }
        public string IDS_REGISTRO_PROFISSIONAL { get; set; }
        public bool PAPEL_MESTRE { get; set; }

        public bool PAPEL_SSO { get; set; }
        public bool TEM_ACESSO_ABORDAGEM { get; set; }
        public bool TEM_ACESSO_SNA { get; set; }
        public bool TEM_ACESSO_INSPECOES { get; set; }
        public bool TEM_ACESSO_OPA { get; set; }

        public USUARIO() { }

        public USUARIO(Usuario usuario)
        {
            this.ID_USUARIO = usuario.ID_USUARIO;
            this.NM_APELIDO = usuario.NM_APELIDO;
            this.NM_USUARIO = usuario.NM_USUARIO;
            this.DT_DESATIVACAO = usuario.DT_DESATIVACAO;
            this.NU_CPF = usuario.NU_CPF;
            this.HS_SENHA = usuario.HS_SENHA;
            this.ID_VINCULO = usuario.ID_VINCULO;
            this.NU_MATRICULA = usuario.NU_MATRICULA;
            this.IDS_REGISTRO_PROFISSIONAL = usuario.IDS_REGISTRO_PROFISSIONAL;
            this.PAPEL_MESTRE = usuario.PAPEL_MESTRE;
            this.PAPEL_SSO = usuario.PAPEL_SSO;
            this.TEM_ACESSO_ABORDAGEM = usuario.TEM_ACESSO_ABORDAGEM;
            this.TEM_ACESSO_SNA = usuario.TEM_ACESSO_SNA;
            this.TEM_ACESSO_INSPECOES = usuario.TEM_ACESSO_INSPECOES;
            this.TEM_ACESSO_OPA = usuario.TEM_ACESSO_OPA;
        }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_USUARIO, " - ", this.NM_APELIDO, " - ", this.NM_USUARIO, " - ", this.ID_VINCULO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
