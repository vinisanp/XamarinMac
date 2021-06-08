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
    public class ViewModelTermoUso : ViewModelBase
    {
        private bool _concordouTermo;
        public ViewModelTermoUso()
        {
            this.ConcordouTermo = false;
        }

        public string TituloTermo
        {
            get { return "TERMOS E CONDIÇÕES DE USO DO APLICATIVO SD2000 ST " + App.__nivelDeProjeto.ToUpper(); }
        }

        public string TextoTermo
        {
            get { return "Estes Termos e Condições de Uso do Aplicativo SD2000 ST " + App.__nivelDeProjeto + " (“Termos”) foram desenvolvidos para que o colaborador empregado da " + App.__nivelDeProjeto + " ou prestador de serviços que preste serviço em uma instalação ou planta da " + App.__nivelDeProjeto + " (“Você” ou “Usuário”) conheça as condições gerais que devem ser observadas durante a utilização do aplicativo, sendo, portanto, obrigatória a sua leitura, compreensão e ciência."; }
        }

        public string TituloAceite
        {
            get { return "1.		ACEITE AOS TERMOS DE USO"; }
        }

        public string TextoAceite        
        { 
            get { return @"Ao acessar pela primeira vez esse aplicativo, Você adere aos Termos, de forma livre, expressa, consciente e informada, declarando que leu e concorda com as condições deste. Ao acessar o aplicativo e aceitar os Termos, Você declara: (i)ser colaborador da " + App.__nivelDeProjeto + " ou prestador de serviço da " + App.__nivelDeProjeto + ", (ii)estar ciente que é vedada a disponibilização de qualquer conteúdo ilícito, contrário à moral e aos bons costumes, ou que possa violar direitos de terceiros ou que prejudique a funcionalidade do aplicativo; e(iii) que toda e qualquer utilização do aplicativo terá apenas finalidades para o objetivo descrito no item 2."; }
        }

        public string TituloObjetivo
        {
            get { return "2.		OBJETIVO"; }
        }

        public string TextoObjetivo
        {
            get { return @"O aplicativo é de adesão voluntária e tem como objetivo o registro de informações relativas à segurança do trabalho de acordo com as ferramentas disponíveis no aplicativo, visando a preservação da vida, o desenvolvimento de comportamentos seguros e avanço na cultura de segurança."; }
        }

        public string TituloAcesso
        {
            get { return "3.		ACESSO E USO DO APLICATIVO"; }
        }

        public string TextoAcesso
        {
            get { return @"Você pode acessar ao aplicativo através do notebook, computador desktop ou de dispositivo móvel. Independentemente do meio de acesso toda as informações incluídas por Você ficarão registradas na mesma base de dados da plataforma do aplicativo."; }
        }
        public string TituloResponsabilidades
        {
            get { return "4.		RESPONSABILIDADES"; }
        }

        public string TextoResponsabilidades
        {
            get { return @"São terminantemente proibidas inclusão de imagens que registrem o comportamento de pessoas e qualquer outra imagem que não tenha como objetivo o previsto no item 2.

A " + App.__nivelDeProjeto + @" não garante a disponibilidade contínua do aplicativo, estando este sujeito a interrupções por falha ou manutenção, podendo ainda ser suspenso ou cancelado.
"; }
        }
        public string TituloAlteracoes
        {
            get { return "5.	  	ALTERAÇÕES"; }
        }

        public string TextoAlteracoes
        {
            get { return @"A " + App.__nivelDeProjeto + @" poderá modificar estes Termos unilateralmente e a qualquer tempo e as atualizações serão informadas no próprio Aplicativo. Ao continuar utilizando o aplicativo, Você concorda automaticamente com os novos Termos, pois a utilização do aplicativo é condicionada a isso. A versão atualizada destes Termos estará sempre disponível no aplicativo."; }
        }

        public string TituloComoFalar
        {
            get { return "6.	  COMO FALAR COM A  " + App.__nivelDeProjeto; }
        }
        public string TextoComoFalar
        {
            get { return @"Caso Você tenha alguma dúvida e/ou precise tratar de qualquer assunto relacionado a estes Termo ou aplicativo, por favor, entrar em com a equipe de Segurança do Trabalho de sua unidade."; }
        }

        public string TituloAceitoTermos
        {
            get { return "Li e concordo"; }
        }

        public virtual bool ConcordouTermo
        {
            get { return this._concordouTermo; }
            set 
            {
                Config.TermoDeUsoApp = value;
                this.DefinirPropriedade(ref this._concordouTermo, value); 
            }
        }
    }
}
