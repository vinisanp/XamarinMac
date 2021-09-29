using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class ModeloOpa
    {
        public string ColunaNota { get; set; }
        public string Nota { get; set; }
        public string ColunaClassificacao { get; set; }
        public string Classificacao { get; set; }

        public List<GrupoOpa> Grupos { get; set; }

        public ModeloOpa()
		{
            this.Grupos = new List<GrupoOpa>();
            this.ColunaNota = string.Empty;
            this.Nota = string.Empty;
            this.ColunaClassificacao = string.Empty;
            this.Classificacao = string.Empty;
        }
    }
}
