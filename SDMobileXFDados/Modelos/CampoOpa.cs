using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class CampoOpa
    {
        public string IdCampo { get; set; }

        public string Grupo { get; set; }
        public string Titulo { get; set; }
        public string IdConforme { get; set; }
        public string Comentario { get; set; }
        public string NumeroDNA { get; set; }

        public Dictionary<string, string> Colunas { get; set; }
    }
}
