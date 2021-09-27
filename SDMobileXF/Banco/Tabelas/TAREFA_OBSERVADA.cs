using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using SQLite;
using System;

namespace SDMobileXF.Banco.Tabelas
{
    public class TAREFA_OBSERVADA
    {
        [PrimaryKey]
        public Guid ID_TAREFA_OBSERVADA { get; set; }
        public string CD_TAREFA_OBSERVADA { get; set; }
        public string DS_TAREFA_OBSERVADA { get; set; }

        public Guid ID_ATIVIDADE { get; set; }

        [Ignore]
        public string CD_DS_TAREFA_OBSERVADA { get { return string.Concat(this.CD_TAREFA_OBSERVADA.ToStringNullSafe(), " - ", this.DS_TAREFA_OBSERVADA.ToStringNullSafe()); } }

        public TAREFA_OBSERVADA() { }

        public TAREFA_OBSERVADA(ModeloObj modelo)
        {
            this.ID_TAREFA_OBSERVADA = modelo.Id;
            this.CD_TAREFA_OBSERVADA = modelo.Codigo;
            this.DS_TAREFA_OBSERVADA = modelo.Descricao;
            this.ID_ATIVIDADE = modelo.IdPai;
        }

        public ModeloObj ToModeloObj() { return new ModeloObj(this.ID_TAREFA_OBSERVADA, this.CD_TAREFA_OBSERVADA, this.DS_TAREFA_OBSERVADA); }

        public override string ToString()
        {
            try 
            {
                return string.Concat(this.ID_TAREFA_OBSERVADA, " - ", this.CD_DS_TAREFA_OBSERVADA);
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}
