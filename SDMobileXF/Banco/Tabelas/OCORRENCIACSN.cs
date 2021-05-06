using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class OCORRENCIACSN
    {
        [PrimaryKey]
        public Guid ID_OCORRENCIA { get; set; }
        public string TITULO { get; set; }
        public DateTime DATA { get; set; }

        public Guid? REGIAOSETOR { get; set; }
        public Guid? LETRA { get; set; }
        public Guid? TURNO { get; set; }

        public Guid? GERENCIAGERAL { get; set; }
        public Guid? GERENCIA { get; set; }
        public Guid? AREAEQUIPAMENTO { get; set; }
        public Guid? LOCALEQUIPAMENTO { get; set; }

        public string LOCALANOMALIA { get; set; }
        public string DESCRICAOPRELIMINAR { get; set; }

        public bool? ST_ACAOIMEDIATA { get; set; }
        public string REMOCAOSINTOMAS { get; set; }

        public Guid? ORIGEMANOMALIA { get; set; }
        public Guid? TIPOANOMALIA { get; set; }
        public Guid? CLASSIFICACAOTIPO { get; set; }

        public Guid? PROBABILIDADE { get; set; }
        public Guid? SEVERIDADE { get; set; }

        public Guid? REGISTRADOPOR { get; set; }
        public Guid? RELATADOPOR { get; set; }
        public Guid? SUPERVISOR { get; set; }
        public Guid? ASSINATURA { get; set; }

        public string DS_SYNC { get; set; }

        [Ignore]
        public bool TemErro
        {
            get { return !string.IsNullOrEmpty(this.DS_SYNC); }
        }

        public OCORRENCIACSN() { }
    }
}
