using SDMobileXF.Banco;
using SDMobileXF.Classes;
using SDMobileXF.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SDMobileXF
{
    public partial class App : Xamarin.Forms.Application
    {
        //public static string __nivelDeProjeto = "CSN";
        public static string __nivelDeProjeto = "Suzano";

        //public static string __EnderecoWebApi = "http://desenvolvimento.portalglauco.com.br/sdwebapi/api";
        //public static string __EnderecoWebApi = "http://localhost/sdwebapi/api";

        //QA Suzano
        //public static string __EnderecoWebApi = "https://sdweb-qa.suzanonet.com.br/api/api";
        //Produção Suzano
        public static string __EnderecoWebApi = "https://sdweb.suzanonet.com.br/api/api";

        private static SqliteSD _banco;
        private static INotificationManager _notificationManager;
        public static List<string> LstLog { get; set; }

        public static SqliteSD Banco
        {
            get
            {
                if (_banco == null)
                {
                    string caminho = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SDMobileXF.db3");
                    _banco = new SqliteSD(caminho);
                }

                return _banco;
            }
        }

        public static bool Sincronizado { get; set; }

        public static void Log(string texto)
        {
            string t = DateTime.Now.ToString("hh:mm:ss.fff") + " - " + texto;
            LstLog.Add(t);
            Debug.WriteLine(t);
        }

        public App()
        {
            LstLog = new List<string>();
            App.Log("App Iniciado");

            InitializeComponent();

            Device.SetFlags(new string[] { "RadioButton_Experimental", "CarouselView_Experimental" });

            Globalizacao.CodigoIdiomaAtual = Config.Idioma;
            this.AplicarEstilo();

            Plugin.Media.CrossMedia.Current.Initialize();

            Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new LoginPage());
            Xamarin.Forms.Application.Current.MainPage.SetDynamicResource(Xamarin.Forms.VisualElement.StyleProperty, "EstiloNavigationPage");
        }

        protected override void OnStart()
        {
            App.Log("App OnStart");
            App.Banco.CriarTabelasSeNaoExistir();
            Connectivity.ConnectivityChanged += this.ConnectivityChanged;

            App._notificationManager = DependencyService.Get<INotificationManager>();
            if (App._notificationManager != null)
            {
                App._notificationManager.NotificationReceived += (sender, eventArgs) =>
                {
                    NotificationEventArgs evtData = (NotificationEventArgs)eventArgs;
                    this.NotificacaoClicada(evtData.Title, evtData.Message);
                };
            }
        }

        private void NotificacaoClicada(string title, string message)
        {
            if (App._notificationManager != null)
                App._notificationManager.Clear();
        }

        public static void Notificar(string titulo, string mensagem)
        {
            if (App._notificationManager != null)
                App._notificationManager.ScheduleNotification(titulo, mensagem);
        }

        private async void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            App.Log("ConnectivityChanged: " + e.NetworkAccess.ToString());
            if (e.NetworkAccess == NetworkAccess.Internet && UsuarioLogado.Instancia != null)
            {
                App.Log("Verificando registros cadastrados no modo offline");
                if (App.__nivelDeProjeto == "Suzano")
                {
                    if (!OffLine.Instancia.SincronizandoOcorrencias)
                        await OffLine.Instancia.SincronizarOcorrenciasAsync();
                    if (!OffLine.Instancia.SincronizandoAbordagens)
                        await OffLine.Instancia.SincronizarAbordagensAsync();
                    if (!OffLine.Instancia.SincronizandoSnas)
                        await OffLine.Instancia.SincronizarSNAsAsync();
                    if (!OffLine.Instancia.SincronizandoInspecoes)
                        await OffLine.Instancia.SincronizarInspecoesAsync();
                    if (!OffLine.Instancia.SincronizandoOpas)
                        await OffLine.Instancia.SincronizarOpasAsync();
                }
                else
                {
                    if (!OffLine.Instancia.SincronizandoOcorrencias)
                        await OffLine.Instancia.SincronizarOcorrenciasCsnAsync();
                }
            }
        }

        public void AplicarEstilo()
        {            
            this.AjustarTamanho("EstiloLabelEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelMenuEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelSubTituloEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelEscuroGrande", NamedSize.Large);            
            this.AjustarTamanho("EstiloLabelCampoEscuro", NamedSize.Medium); 
            this.AjustarTamanho("EstiloPickerEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloEntryEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloEditorEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloTimePickerEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloDatePickerEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloSearchBarEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloListViewEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelDataVisOcorrenciaEscuro", NamedSize.Medium);
            this.AjustarTamanho("EstiloStackLayoutSubTituloEscuro", NamedSize.Medium);
            
            this.AjustarTamanho("EstiloLabelClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelMenuClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelSubTituloClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelClaroGrande", NamedSize.Large);            
            this.AjustarTamanho("EstiloLabelCampoClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloPickerClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloEntryClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloEditorClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloTimePickerClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloDatePickerClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloSearchBarClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloListViewClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelDataVisOcorrenciaClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloStackLayoutSubTituloClaro", NamedSize.Medium);
            this.AjustarTamanho("EstiloLabelBoasVindas", NamedSize.Medium);

            this.AjustarTamanho("EstiloBtn", NamedSize.Medium);

            this.Resources["EstiloLabel"] = null;
            this.Resources["EstiloLabelMenu"] = null;
            this.Resources["EstiloLabelSubTitulo"] = null;            
            this.Resources["EstiloLabelGrande"] = null;            
            this.Resources["EstiloLabelCampo"] = null;
            this.Resources["EstiloPage"] = null;
            this.Resources["EstiloNavigationPage"] = null;
            this.Resources["EstiloPicker"] = null;
            this.Resources["EstiloFrame"] = null;
            this.Resources["EstiloEntry"] = null;
            this.Resources["EstiloEditor"] = null;
            this.Resources["EstiloRadioButton"] = null;
            this.Resources["EstiloCheckBox"] = null;
            this.Resources["EstiloFramePage"] = null;
            this.Resources["EstiloLinha"] = null;
            this.Resources["EstiloTimePicker"] = null;
            this.Resources["EstiloDatePicker"] = null;
            this.Resources["EstiloTituloStackLayout"] = null;
            this.Resources["EstiloSearchBar"] = null;
            this.Resources["EstiloListView"] = null;
            this.Resources["EstiloLabelDataVisOcorrencia"] = null;
            this.Resources["EstiloStackLayoutSubTitulo"] = null;
            this.Resources["EstiloTabPage"] = null;

            //forcar update ao mudar de tamanho de fonte
            var estilo = this.Resources["EstiloBtn"];
            this.Resources["EstiloBtn"] = null;
            this.Resources["EstiloBtn"] = estilo;

            estilo = this.Resources["EstiloLabelBoasVindas"];
            this.Resources["EstiloLabelBoasVindas"] = null;
            this.Resources["EstiloLabelBoasVindas"] = estilo;

            if (Config.Estilo == "Claro")
            {
                this.Resources["EstiloLabelMenu"] = this.Resources["EstiloLabelMenuClaro"];
                this.Resources["EstiloLabel"] = this.Resources["EstiloLabelClaro"];
                this.Resources["EstiloLabelGrande"] = this.Resources["EstiloLabelClaroGrande"];                
                this.Resources["EstiloLabelCampo"] = this.Resources["EstiloLabelCampoClaro"];
                this.Resources["EstiloPage"] = this.Resources["EstiloPageClaro"];
                this.Resources["EstiloNavigationPage"] = this.Resources["EstiloNavigationPageClaro"];
                this.Resources["EstiloPicker"] = this.Resources["EstiloPickerClaro"];
                this.Resources["EstiloFrame"] = this.Resources["EstiloFrameClaro"];
                this.Resources["EstiloEntry"] = this.Resources["EstiloEntryClaro"];
                this.Resources["EstiloEditor"] = this.Resources["EstiloEditorClaro"];
                this.Resources["EstiloRadioButton"] = this.Resources["EstiloRadioButtonClaro"];
                this.Resources["EstiloCheckBox"] = this.Resources["EstiloCheckBoxClaro"];
                this.Resources["EstiloFramePage"] = this.Resources["EstiloFramePageClaro"];
                this.Resources["EstiloLinha"] = this.Resources["EstiloLinhaClaro"];
                this.Resources["EstiloTimePicker"] = this.Resources["EstiloTimePickerClaro"];
                this.Resources["EstiloDatePicker"] = this.Resources["EstiloDatePickerClaro"];
                this.Resources["EstiloTituloStackLayout"] = this.Resources["EstiloTituloStackLayoutClaro"];
                this.Resources["EstiloLabelSubTitulo"] = this.Resources["EstiloLabelSubTituloClaro"];
                this.Resources["EstiloSearchBar"] = this.Resources["EstiloSearchBarClaro"];
                this.Resources["EstiloListView"] = this.Resources["EstiloListViewClaro"];
                this.Resources["EstiloLabelDataVisOcorrencia"] = this.Resources["EstiloLabelDataVisOcorrenciaClaro"];
                this.Resources["EstiloStackLayoutSubTitulo"] = this.Resources["EstiloStackLayoutSubTituloClaro"];
                this.Resources["EstiloTabPage"] = this.Resources["EstiloTabPageClaro"];
            }
            else
            {
                this.Resources["EstiloLabelMenu"] = this.Resources["EstiloLabelMenuEscuro"];
                this.Resources["EstiloLabel"] = this.Resources["EstiloLabelEscuro"];
                this.Resources["EstiloLabelGrande"] = this.Resources["EstiloLabelEscuroGrande"];                
                this.Resources["EstiloLabelCampo"] = this.Resources["EstiloLabelCampoEscuro"];
                this.Resources["EstiloPage"] = this.Resources["EstiloPageEscuro"];
                this.Resources["EstiloNavigationPage"] = this.Resources["EstiloNavigationPageEscuro"];
                this.Resources["EstiloPicker"] = this.Resources["EstiloPickerEscuro"];
                this.Resources["EstiloFrame"] = this.Resources["EstiloFrameEscuro"];
                this.Resources["EstiloEntry"] = this.Resources["EstiloEntryEscuro"];
                this.Resources["EstiloEditor"] = this.Resources["EstiloEditorEscuro"];
                this.Resources["EstiloRadioButton"] = this.Resources["EstiloRadioButtonEscuro"];
                this.Resources["EstiloCheckBox"] = this.Resources["EstiloCheckBoxEscuro"];
                this.Resources["EstiloFramePage"] = this.Resources["EstiloFramePageEscuro"];
                this.Resources["EstiloLinha"] = this.Resources["EstiloLinhaEscuro"];
                this.Resources["EstiloTimePicker"] = this.Resources["EstiloTimePickerEscuro"];
                this.Resources["EstiloDatePicker"] = this.Resources["EstiloDatePickerEscuro"];
                this.Resources["EstiloTituloStackLayout"] = this.Resources["EstiloTituloStackLayoutEscuro"];
                this.Resources["EstiloLabelSubTitulo"] = this.Resources["EstiloLabelSubTituloEscuro"];
                this.Resources["EstiloSearchBar"] = this.Resources["EstiloSearchBarEscuro"];
                this.Resources["EstiloListView"] = this.Resources["EstiloListViewEscuro"];
                this.Resources["EstiloLabelDataVisOcorrencia"] = this.Resources["EstiloLabelDataVisOcorrenciaEscuro"];
                this.Resources["EstiloStackLayoutSubTitulo"] = this.Resources["EstiloStackLayoutSubTituloEscuro"];
                this.Resources["EstiloTabPage"] = this.Resources["EstiloTabPageEscuro"];
            }
        }

        private void AjustarTamanho(string nome, NamedSize tamBase)
        {
            foreach (Setter s in ((Style)this.Resources[nome]).Setters)
                if (s.Property.PropertyName == "FontSize")
                    s.Value = Util.AlturaLabel(tamBase);
        }

        protected override void OnSleep()
        {
            App.Log("App OnSleep");
        }

        protected override void OnResume()
        {
            App.Log("App OnResume");
        }
    }
}
