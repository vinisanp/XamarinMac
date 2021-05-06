using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXF.Classes
{
    public static class UsuarioLogado
    {
        private static Usuario _instancia;

        public static Usuario Instancia
        {
            get { return _instancia; }
            set
            {                
                //Config.SalvarConfiguracao("UsuarioLogado", value);
                _instancia = value;
            }
        }
    }
}
