using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class CAMPO_INSPECAO 
    {
        public Guid ID_INSPECAO { get; set; }
        public string ID_CAMPO { get; set; }
        public Guid? ID_CONFORME { get; set; }
        public string DS_SITUACAO { get; set; }
        public string NU_DNA { get; set; }
        public string COLUNAS { get; set; }
        public string CAMINHO { get; set; }
        public byte[] BYTES_IMAGEM { get; set; }

        public CAMPO_INSPECAO() { }

        public override string ToString()
        {
            try
            {
                return string.Concat(this.ID_INSPECAO, " - ", this.ID_CAMPO);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
