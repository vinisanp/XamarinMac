using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class SNA
    {
        [PrimaryKey]
        public Guid ID_SNA   { get; set; }
        public string NU_SNA { get; set; }

        public DateTime DT_DATA             { get; set; }
        public DateTime? DT_HORARIO_INICIAL { get; set; }
        public DateTime? DT_HORARIO_FINAL   { get; set; }

        public Guid ID_REGIONAL             { get; set; }
        public Guid ID_GERENCIA             { get; set; }
        public Guid ID_AREA                 { get; set; }
        public Guid ID_LOCAL                { get; set; }

        public string DS_TEMA_ABORDADO      { get; set; }

        public string DS_CE_AVALIACAODESCRITIVA { get; set; }
        public bool ST_CE_RUIM                  { get; set; }
        public bool ST_CE_REGULAR               { get; set; }
        public bool ST_CE_BOM                   { get; set; }
        public string NU_DNA_CE                 { get; set; }

        public string DS_CA_AVALIACAODESCRITIVA { get; set; }
        public bool ST_CA_RUIM                  { get; set; }
        public bool ST_CA_REGULAR               { get; set; }
        public bool ST_CA_BOM                   { get; set; }
        public string NU_DNA_CA                 { get; set; }

        public string DS_RAF_AVALIACAODESCRITIVA { get; set; }
        public bool ST_RAF_RUIM                  { get; set; }
        public bool ST_RAF_REGULAR               { get; set; }
        public bool ST_RAF_BOM                   { get; set; }
        public string NU_DNA_RAF                 { get; set; }

        public string DS_QAT_AVALIACAODESCRITIVA { get; set; }
        public bool ST_QAT_RUIM                  { get; set; }
        public bool ST_QAT_REGULAR               { get; set; }
        public bool ST_QAT_BOM                   { get; set; }
        public string NU_DNA_QAT                 { get; set; }

        public string DS_PONTOS_POSITIVOS { get; set; }

        public Guid? ID_REGISTRADOPOR { get; set; }

        public string DS_SYNC { get; set; }

        [Ignore]
        public bool TemErro
        {
            get { return !string.IsNullOrEmpty(this.DS_SYNC); }
        }

        public SNA() { }
    }
}
