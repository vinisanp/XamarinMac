using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class INSPECAO
    {
        [PrimaryKey]
        public Guid ID_INSPECAO   { get; set; }
        public string NU_INSPECAO { get; set; }

        public DateTime DT_DATA   { get; set; }

        public Guid ID_REGIONAL   { get; set; }
        public Guid ID_GERENCIA   { get; set; }
        public Guid ID_AREA       { get; set; }
        public Guid ID_LOCAL      { get; set; }
        public Guid ID_FORNECEDOR { get; set; }

        public Guid? ID_TIPO      { get; set; }
        [Ignore]
        public string DS_TIPO { get; set; }

        public Guid? ID_ATIVIDADE { get; set; }
        [Ignore]
        public string DS_ATIVIDADE { get; set; }

        public string PARTICIPANTES { get; set; }

        public Guid? ID_REALIZADOPOR { get; set; }

        public string DS_SYNC { get; set; }

        [Ignore]
        public bool TemErro
        {
            get { return !string.IsNullOrEmpty(this.DS_SYNC); }
        }

        public INSPECAO() { }
    }
}
