using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados.Modelos
{
    public class RetornoRequest
    {
        public bool Ok { get; set; }
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public string Erro { get; set; }
    }
}
