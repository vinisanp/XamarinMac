using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class IMAGEM_RELATO
    {
        [PrimaryKey]
        public Guid ID_IMAGEM { get; set; }
        public Guid ID_OCORRENCIA { get; set; }
        public DateTime DATA { get; set; }
        public string CAMINHO { get; set; }
        public byte[] BYTES_IMAGEM { get; set; }
    }
}
