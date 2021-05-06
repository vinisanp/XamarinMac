using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
    public class Config
    {
        private static string _estilo;
        private static string _tamFonte;
        private static string _idioma;
        private static bool? _lembrarMe;
        private static bool? _termoDeUsoApp;

        public static string Estilo
        {
            get
            {
                if (string.IsNullOrEmpty(_estilo))
                {
                    object obj = Config.CarregarConfiguracao("EstiloSelecionado");
                    if (obj != null)
                        _estilo = obj.ToString();
                    else
                        _estilo = "Claro";
                }
                return _estilo;
            }
            set
            {
                _estilo = value;
                Config.SalvarConfiguracao("EstiloSelecionado", value);
            }
        }

        public static string TamanhoFonte
        {
            get
            {
                //Small, Medium, Large
                if (string.IsNullOrEmpty(_tamFonte))
                {
                    object obj = Config.CarregarConfiguracao("TamanhoFonte");
                    if (obj != null)
                        _tamFonte = obj.ToString();
                    else
                        _tamFonte = "Pequeno";
                }
                return _tamFonte;
            }
            set
            {
                _tamFonte = value;
                Config.SalvarConfiguracao("TamanhoFonte", value);
            }
        }

        public static string Idioma
        {
            get
            {
                if (string.IsNullOrEmpty(_idioma))
                {
                    object obj = Config.CarregarConfiguracao("IdiomaSelecionado");
                    if (obj != null)
                        _idioma = obj.ToString();
                    else
                        _idioma = "pt";
                }
                return _idioma;
            }
            set
            {
                _idioma = value;
                Config.SalvarConfiguracao("IdiomaSelecionado", value);
            }
        }

        public static bool LembrarMe
        {
            get
            {
                if (!_lembrarMe.HasValue)
                {
                    object obj = Config.CarregarConfiguracao("LembrarMe");
                    if (obj != null)
                        _lembrarMe = Convert.ToBoolean(obj);
                    else
                        _lembrarMe = false;
                }
                return _lembrarMe.Value;
            }
            set
            {
                _lembrarMe = value;
                Config.SalvarConfiguracao("LembrarMe", value);
            }
        }

        public static bool TermoDeUsoApp
        {
            get
            {
                if (!_termoDeUsoApp.HasValue)
                {
                    object obj = Config.CarregarConfiguracao("TermoDeUsoApp");
                    if (obj != null)
                        _termoDeUsoApp = Convert.ToBoolean(obj);
                    else
                        _termoDeUsoApp = false;
                }
                return _termoDeUsoApp.Value;
            }
            set
            {
                _termoDeUsoApp = value;
                Config.SalvarConfiguracao("TermoDeUsoApp", value);
            }
        }
            

        public static bool Existe(string nome)
        {
            return Application.Current.Properties.ContainsKey(nome);
        }

        public static object CarregarConfiguracao(string nome)
        {
            object obj = null;
            if (Application.Current.Properties.TryGetValue(nome, out obj))
                return obj;

            return null;
        }

        public static void SalvarConfiguracao(string nome, object valor)
        {
            if (valor == null)
                Application.Current.Properties.Remove(nome);
            else
            {
                if (!Application.Current.Properties.ContainsKey(nome))
                    Application.Current.Properties.Add(nome, valor);
                else
                    Application.Current.Properties[nome] = valor;
            }

            Application.Current.SavePropertiesAsync();
        }
    }
}
