using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class CampoInspecao
    {
        public string IdCampo { get; set; }

        public string Pergunta { get; set; }
        public string IdConforme { get; set; }
        public string Descricao { get; set; }
        public string NumeroDNA { get; set; }
        public byte[] Image { get; set; }

        public Dictionary<string, string> Colunas { get; set; }
    }
}
