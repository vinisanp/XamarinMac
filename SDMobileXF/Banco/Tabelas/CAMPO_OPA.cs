using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class CAMPO_OPA
    {
        public Guid ID_OPA { get; set; }
        public string ID_CAMPO { get; set; }
        public Guid? ID_CONFORME { get; set; }
        public string DS_COMENTARIO { get; set; }
        public string NU_DNA { get; set; }
        public string COLUNAS { get; set; }
        public string CAMINHO { get; set; }
        public byte[] BYTES_IMAGEM { get; set; }

        public CAMPO_OPA() { }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_OPA, " - ", this.ID_CAMPO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
