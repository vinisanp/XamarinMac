using SDMobileXF.Classes;
using SDMobileXF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SDMobileXF.ViewModels
{
    public class ViewModelSobre : ViewModelBase
    {
        public string Texto
        {
            get
            {
                string texto = string.Empty;

                switch(Globalizacao.CodigoIdiomaAtual)
                {
                    case "en":
                        texto =  @"With a solid experience in software development and proven success in providing definitive management services for Occupational Health, Workplace Safety and Environment, Glauco Tecnologia encourages the success of companies and organizations through the use of intelligent technology and the creativity of people.
With a business unit in Rio de Janeiro, Salvador, São Paulo and Natal, we provide the SD2000 software and a service portfolio that includes process consulting, operational and technical advice; support and training.";
                        break;
                    case "es":
                        texto = @"Con una sólida experiencia en el desarrollo de software y un éxito comprobado en la prestación de servicios de gestión definitiva para la salud ocupacional, la seguridad en el trabajo y el medio ambiente, Glauco Tecnologia fomenta el éxito de las empresas y organizaciones mediante el uso de tecnología inteligente y la creatividad de personas
Con una unidad de negocios en Río de Janeiro, Salvador, São Paulo y Natal, proporcionamos el software SD2000 y una cartera de servicios que incluye consultoría de procesos, asesoramiento operativo y técnico; Apoyo y formación.";
                        break;
                    case "fr":
                        texto = @"Avec une solide expérience dans le développement de logiciels et un succès avéré dans la fourniture de services de gestion définitive pour la santé au travail, la sécurité au travail et l'environnement, Glauco Tecnologia encourage le succès des entreprises et des organisations grâce à l'utilisation de technologies intelligentes et la créativité de les gens.
Avec une unité commerciale à Rio de Janeiro, Salvador, São Paulo et Natal, nous fournissons le logiciel SD2000 et un portefeuille de services qui comprend des conseils en processus, des conseils opérationnels et techniques; soutien et formation.";
                        break;
                    default:
                        texto = @"Com uma sólida experiência no desenvolvimento de Software e comprovado sucesso no fornecimento de serviços de gestão definitiva de Saúde Ocupacional, Segurança do Trabalho e Meio Ambiente, a Glauco Tecnologia estimula o sucesso de empresas e organizações por meio do uso de tecnologia inteligente e da criatividade das pessoas.
Com unidade de negócios no Rio de Janeiro, Salvador, São Paulo e Natal, disponibilizamos o software SD2000 e um portfólio de serviços que engloba consultoria de processos, assessoria operacional e técnica; suporte e treinamentos.";
                        break;
                }

                return texto;
            }
        }

        public ViewModelSobre()
        {
            this.Titulo = this.Textos.Informacoes;
        }
    }
}
