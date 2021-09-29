using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class GrupoOpa
    {
        public string Nome { get; set; }

        public string ColunaConforme { get; set; }
        public int TotalConforme { get; set; }

        public string ColunaNaoConforme { get; set; }
        public int TotalNaoConforme { get; set; }

        public string ColunaNA { get; set; }
        public int TotalNA { get; set; }

        public string ColunaPontuacao { get; set; }
        public double Pontuacao { get; set; }

        public List<CampoOpa> Campos { get; set; }

		public GrupoOpa()
		{
			this.Campos            = new List<CampoOpa>();
            this.ColunaConforme    = string.Empty;
            this.TotalConforme     = 0;
            this.ColunaNaoConforme = string.Empty;
            this.TotalNaoConforme  = 0;
            this.ColunaNA          = string.Empty;
            this.TotalNA           = 0;
            this.ColunaPontuacao   = string.Empty;
            this.Pontuacao         = 0;
        }
	}
}
