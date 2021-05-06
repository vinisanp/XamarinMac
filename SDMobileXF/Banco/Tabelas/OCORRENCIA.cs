using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class OCORRENCIA
    {
        [PrimaryKey]
        public Guid ID_OCORRENCIA { get; set; }
        public DateTime DATA { get; set; }
        public Guid ID_REGIONAL { get; set; }
        public Guid ID_GERENCIA { get; set; }
        public Guid ID_AREA { get; set; }
        public Guid ID_LOCAL { get; set; }
        public string DESCRICAO { get; set; }
        public Guid ID_TIPO { get; set; }
        public Guid ID_CLASSIFICACAO { get; set; }
        public Guid ID_SUBCLASSIFICACAO { get; set; }
        public Guid? ID_CATEGORIA { get; set; }
        public Guid? ID_FORNECEDOR { get; set; }
        public bool? ST_ACAOIMEDIATA { get; set; }
        public string DS_ACAOIMEDIATA { get; set; }
        public bool ST_NAOQUEROIDENTIFICAR { get; set; }
        public Guid? ID_COMUNICADOPOR { get; set; }
        public Guid? ID_REGISTRADOPOR { get; set; }

        public string DS_SYNC { get; set; }

        [Ignore]
        public bool TemErro
        {
            get { return !string.IsNullOrEmpty(this.DS_SYNC); }
        }

        public OCORRENCIA() { }
    }
}
