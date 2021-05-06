using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class OPA
    {
        [PrimaryKey]
        public Guid ID_OPA        { get; set; }
        public string NU_OPA      { get; set; }
        public string DS_OPA      { get; set; }
        public DateTime DT_OPA    { get; set; }

        public Guid ID_REGIONAL   { get; set; }
        public Guid ID_GERENCIA   { get; set; }
        public Guid ID_AREA       { get; set; }
        public Guid ID_LOCAL      { get; set; }
        public Guid? ID_ATIVIDADE { get; set; }
        public Guid? ID_TAREFA    { get; set; }
        public Guid? ID_AVALIADOR { get; set; }
        public Guid? ID_TIPO_AVALIADOR { get; set; }

        public string DS_SYNC { get; set; }

        [Ignore]
        public bool TemErro
        {
            get { return !string.IsNullOrEmpty(this.DS_SYNC); }
        }

        public OPA() { }
    }
}
