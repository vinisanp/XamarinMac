using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
    public class Util
    {
        private static List<ModeloObj> _lstOtimoBomRegulaRuim;
        private static List<ModeloObj> _lstDiaSemana;
        private static List<ModeloObj> _lstClassificacaoRisco;

        public static List<ModeloObj> LstOtimoBomRegulaRuim
        {
            get
            {
                if(_lstOtimoBomRegulaRuim == null)
                {
                    _lstOtimoBomRegulaRuim = new List<ModeloObj>();
                    _lstOtimoBomRegulaRuim.Add(new ModeloObj(new Guid("fe8e3871-0ea7-4d08-b36f-07cb49858ba3"), "B", "Bom"));
                    _lstOtimoBomRegulaRuim.Add(new ModeloObj(new Guid("11b8835c-4522-49d8-88ca-1bdc0e67fea9"), "M", "Regular"));
                    _lstOtimoBomRegulaRuim.Add(new ModeloObj(new Guid("aaac4e37-8052-4ec5-b221-214110f83ac4"), "R", "Ruim"));
                    _lstOtimoBomRegulaRuim.Add(new ModeloObj(new Guid("d297c9f3-b819-4798-8f7f-f08110ecc373"), "O", "Ótimo"));
                }

                return _lstOtimoBomRegulaRuim;
            }
        }

        public static List<ModeloObj> LstDiaSemana
        {
            get
            {
                if (_lstDiaSemana == null)
                {
                    _lstDiaSemana = new List<ModeloObj>();
                    _lstDiaSemana.Add(new ModeloObj(new Guid("f001207e-7885-40cb-9ab6-07cbc9f6ea06"), "1", "Domingo"));
                    _lstDiaSemana.Add(new ModeloObj(new Guid("89436c6b-9334-4f7b-97bb-7013d89c9811"), "2", "Segunda-feira"));
                    _lstDiaSemana.Add(new ModeloObj(new Guid("108bb408-63be-4817-88ff-7c81882a2dfd"), "3", "Terça-feira"));
                    _lstDiaSemana.Add(new ModeloObj(new Guid("53d66bf8-fd16-43d2-a0ea-5db553508e37"), "4", "Quarta-feira"));
                    _lstDiaSemana.Add(new ModeloObj(new Guid("67dc8049-6a39-40eb-966e-a47cc48ac3c6"), "5", "Quinta-feira"));
                    _lstDiaSemana.Add(new ModeloObj(new Guid("641c2e6d-f531-4941-8095-5a8b83d5dea3"), "6", "Sexta-feira"));
                    _lstDiaSemana.Add(new ModeloObj(new Guid("c1c2ba62-1448-4e6e-8873-18197f93a228"), "7", "Sábado"));
                }

                return _lstDiaSemana;
            }
        }

        public static List<ModeloObj> LstClassificacaoRisco
        {
            get
            {
                if (_lstClassificacaoRisco == null)
                {
                    _lstClassificacaoRisco = new List<ModeloObj>();

                    _lstClassificacaoRisco.Add(new ModeloObj(new Guid("63c31316-1385-402e-8a4c-f15ae1b3ad04"), "CRÍTICO", "RISCO CRÍTICO"));
                    _lstClassificacaoRisco.Add(new ModeloObj(new Guid("bdbebb08-d71f-4a5d-816f-ce78059114e6"), "SUBSTANCIAL", "RISCO SUBSTANCIAL"));
                    _lstClassificacaoRisco.Add(new ModeloObj(new Guid("d67fbd5e-8291-41a3-8659-be4909d8ef46"), "MODERADO", "RISCO MODERADO"));
                    _lstClassificacaoRisco.Add(new ModeloObj(new Guid("db81b76a-8aca-4dc6-98cd-388bb41208d4"), "DESPREZIVEL", "RISCO DESPREZIVEL"));
                    _lstClassificacaoRisco.Add(new ModeloObj(new Guid("e9af53d2-a420-4729-8858-7bd6a31e6f33"), "ACEITÁVEL", "RISCO ACEITÁVEL"));
                }

                return _lstClassificacaoRisco;
            }
        }

        public static string LocalImagem(string imagem)
        {
            string img = string.Empty;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    img = imagem;
                    break;
                case Device.UWP:
                    img = "Assets\\" + imagem;
                    break;
                default:
                    break;
            }
            return img;
        }

        public static Thickness Padding
        {
            get
            {
                double top;
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        top = 20;
                        break;
                    case Device.Android:
                    case Device.UWP:
                    default:
                        top = 10;
                        break;
                }
                return new Thickness(10, top, 10, 10);
            }
        }

        public static bool TemAcessoInternet
        {
            get
            {
                return Connectivity.NetworkAccess == NetworkAccess.Internet;
            }
        }

        public static bool Debug
        {
            get
            {
                return Debugger.IsAttached;
            }
        }

        public static bool UWP
        {
            get
            {
                return Device.RuntimePlatform == Device.UWP;
            }
        }

        public static string CodigoHash2(string s)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(s);
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1Managed();
            byte[] res = sha.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < res.Length; i++)
                sb.Append(res[i].ToString());
            return sb.ToString();
        }

        public static string CodigoHash(string s)
        {
            System.Security.Cryptography.HashAlgorithm sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            char[] charArray = s.ToCharArray();
            byte[] byteArray = new byte[charArray.Length];
            byte[] byteHash;
            string texto = "";
            for (int i = 0; i < charArray.Length; i++)
                byteArray[i] = Convert.ToByte(charArray[i]);
            byteHash = sha.ComputeHash(byteArray);
            foreach (byte bit in byteHash)
                texto = texto + bit.ToString();
            return texto;
        }

        public static double AlturaLabel(NamedSize tamBase)
        {            
            double tam = Device.GetNamedSize(tamBase, typeof(Xamarin.Forms.Label));
            if (Device.RuntimePlatform == Device.iOS)
                tam += 5;
            if (Config.TamanhoFonte == "Pequeno")
                tam -= 5;
            else if (Config.TamanhoFonte == "Grande")
                tam += 4;

            if (Device.RuntimePlatform == Device.UWP)
                tam *= .7;

            return tam;
        }

        public static string GetIdRuimRegularBom(bool bom, bool regular, bool ruim)
        {
            ModeloObj modelo = null;

            if (bom)
                modelo = Util.LstOtimoBomRegulaRuim.FirstOrDefault(m => m.Codigo == "B");
            else if (regular)
                modelo = Util.LstOtimoBomRegulaRuim.FirstOrDefault(m => m.Codigo == "M");
            else if (ruim)
                modelo = Util.LstOtimoBomRegulaRuim.FirstOrDefault(m => m.Codigo == "R");

            if (modelo != null)
                return modelo.Id.ToString();

            return string.Empty;
        }

        public static int CalcularProdutoAB(int p, int s)
        {
            int ab = 0;
            if (p == 1)
            {
                if (s == 1) ab = 1;
                if (s == 2) ab = 3;
                if (s == 3) ab = 6;
                if (s == 4) ab = 10;
            }
            if (p == 2)
            {
                if (s == 1) ab = 2;
                if (s == 2) ab = 5;
                if (s == 3) ab = 9;
                if (s == 4) ab = 13;
            }
            if (p == 3)
            {
                if (s == 1) ab = 4;
                if (s == 2) ab = 8;
                if (s == 3) ab = 12;
                if (s == 4) ab = 15;
            }
            if (p == 4)
            {
                if (s == 1) ab = 7;
                if (s == 2) ab = 11;
                if (s == 3) ab = 14;
                if (s == 4) ab = 16;
            }
            return ab;
        }

        public static ModeloObj ResultadoRisco(int ab)
        {
            ModeloObj m = null;
            if (ab == 1)
                m = Util.LstClassificacaoRisco.FirstOrDefault(c => c.Id == new Guid("db81b76a-8aca-4dc6-98cd-388bb41208d4"));
            else if (ab >= 2 && ab <= 3)
                m = Util.LstClassificacaoRisco.FirstOrDefault(c => c.Id == new Guid("e9af53d2-a420-4729-8858-7bd6a31e6f33"));
            else if (ab >= 4 && ab <= 7)
                m = Util.LstClassificacaoRisco.FirstOrDefault(c => c.Id == new Guid("d67fbd5e-8291-41a3-8659-be4909d8ef46"));
            else if (ab >= 8 && ab <= 11)
                m = Util.LstClassificacaoRisco.FirstOrDefault(c => c.Id == new Guid("bdbebb08-d71f-4a5d-816f-ce78059114e6"));
            else if (ab >= 12)
                m = Util.LstClassificacaoRisco.FirstOrDefault(c => c.Id == new Guid("63c31316-1385-402e-8a4c-f15ae1b3ad04"));

            return m;
        }
    }
}
