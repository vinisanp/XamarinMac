using SDMobileXF.Banco.Tabelas;
using SDMobileXF.Classes;
using SDMobileXF.Views;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SDMobileXF.ViewModels
{
    public class ViewModelConfig : ViewModelBase
    {
        private ModeloObj _estiloSelecionado;
        private List<ModeloObj> _estilos;
        private readonly Action<ModeloObj> _retornoEstiloSelecionado;

        private ModeloObj _tamanhoSelecionado;
        private List<ModeloObj> _tamanhosFonte;
        private readonly Action<ModeloObj> _retornoTamanhoSelecionado;

        private bool _habilitado;
        private List<Idioma> _idiomas;
        private string _bandeira;
        private Idioma _idiomaSelecionado;
        private string _infoSincronizacao;
        private string _textoLimparDados;
        private string _textoSincronizar;
        private string _textoIdioma;
        private string _textoTamanhoDaFonte;
        private string _textoEstilo;
        private string _textoSincronizacao;
        private readonly Action<Idioma> _retornoIdiomaSelecionado;

        public Command SyncCommand { get; }

        public Command LimparDadosCommand { get; }

        public string Log { get { return string.Join(Environment.NewLine, App.LstLog.ToArray()); } }

        public List<ModeloObj> Estilos
        {
            get { return this._estilos; }
            set { this.DefinirPropriedade(ref this._estilos, value); }
        }

        public ModeloObj EstiloSelecionado
        {
            get
            {
                return this._estiloSelecionado;
            }
            set
            {
                this.DefinirPropriedade(ref this._estiloSelecionado, value);
                if (!this.Ocupado)
                    this._retornoEstiloSelecionado?.Invoke(value);
            }
        }

        public List<ModeloObj> TamanhosFonte
        {
            get { return this._tamanhosFonte; }
            set { this.DefinirPropriedade(ref this._tamanhosFonte, value); }
        }

        public ModeloObj TamanhoFonteSelecionado
        {
            get
            {
                return this._tamanhoSelecionado;
            }
            set
            {
                this.DefinirPropriedade(ref this._tamanhoSelecionado, value);
                if (!this.Ocupado)
                    this._retornoTamanhoSelecionado?.Invoke(value);
            }
        }

        public string Bandeira
        {
            get { return this._bandeira; }
            set { this.DefinirPropriedade(ref this._bandeira, value); }
        }

        public List<Idioma> Idiomas
        {
            get { return this._idiomas; }
            set { this.DefinirPropriedade(ref this._idiomas, value); }
        }

        public string TextoEstilo
        {
            get { return this._textoEstilo; }
            set { this.DefinirPropriedade(ref this._textoEstilo, value); }
        }
        public string TextoTamanhoDaFonte
        {
            get { return this._textoTamanhoDaFonte; }
            set { this.DefinirPropriedade(ref this._textoTamanhoDaFonte, value); }
        }
        public string TextoIdioma
        {
            get { return this._textoIdioma; }
            set { this.DefinirPropriedade(ref this._textoIdioma, value); }
        }
        public string TextoLimparDados
        {
            get { return this._textoLimparDados; }
            set { this.DefinirPropriedade(ref this._textoLimparDados, value); }
        }
        public string TextoSincronizar
        {
            get { return this._textoSincronizar; }
            set { this.DefinirPropriedade(ref this._textoSincronizar, value); }
        }
        public string TextoSincronizacao
        {
            get { return this._textoSincronizacao; }
            set { this.DefinirPropriedade(ref this._textoSincronizacao, value); }
        }

        public Idioma IdiomaSelecionado
        {
            get
            {
                return this._idiomaSelecionado;
            }
            set
            {
                this.DefinirPropriedade(ref this._idiomaSelecionado, value);
                Config.Idioma = value.Codigo;
                Globalizacao.CodigoIdiomaAtual = value.Codigo;
                
                if (value == null)
                    this.Bandeira = string.Empty;
                else
                    this.Bandeira = value.Imagem;

                this.InfoSincronizacao = this.DadosBanco();

                this.TextoEstilo = this.Textos.Estilo;
                this.TextoIdioma = this.Textos.Idioma;
                this.TextoLimparDados = this.Textos.LimparDados;
                this.TextoSincronizar = this.Textos.Sincronizar;
                this.TextoSincronizacao = this.Textos.Sincronizacao;
                this.TextoTamanhoDaFonte = this.Textos.TamanhoDaFonte;

                List<ModeloObj> estilos = new List<ModeloObj>();
                estilos.Add(new ModeloObj(Guid.Empty, "Claro", this.Textos.Claro));
                estilos.Add(new ModeloObj(Guid.Empty, "Escuro", this.Textos.Escuro));
                this.Estilos = estilos;
                this.EstiloSelecionado = estilos.FirstOrDefault(e => e.Codigo == Config.Estilo);

                List<ModeloObj> tamanhos = new List<ModeloObj>();
                tamanhos.Add(new ModeloObj(Guid.Empty, "Pequeno", this.Textos.Pequeno));
                tamanhos.Add(new ModeloObj(Guid.Empty, "Medio", this.Textos.Medio));
                tamanhos.Add(new ModeloObj(Guid.Empty, "Grande", this.Textos.Grande));
                this.TamanhosFonte = tamanhos;
                this.TamanhoFonteSelecionado = tamanhos.FirstOrDefault(e => e.Codigo == Config.TamanhoFonte);

                //if (!this.Ocupado)
                //    this._retornoIdiomaSelecionado?.Invoke(value);
            }
        }

        public string InfoSincronizacao
        {
            get { return this._infoSincronizacao; }
            set { this.DefinirPropriedade(ref this._infoSincronizacao, value); }
        }

        public bool Habilitado
        {
            get { return this._habilitado; }
            set
            {
                bool updateCommands = this._habilitado != value;
                this.DefinirPropriedade(ref this._habilitado, value);

                if (updateCommands)
                {
                    this.SyncCommand.ChangeCanExecute();
                    this.LimparDadosCommand.ChangeCanExecute();
                }
            }
        }

        public ViewModelConfig(Action<ModeloObj> estiloSelecionado, Action<ModeloObj> tamanhoSelecionado, Action<Idioma> idiomaSelecionado)
        {
            this.SyncCommand = new Command(
                execute: () => { this.Sincronizar(); },
                canExecute: () => { return this.Habilitado; });

            this.LimparDadosCommand = new Command(
                execute: () => { this.LimparDados(); },
                canExecute: () => { return this.Habilitado; });

            this.Titulo = this.Textos.Configuracoes;

            this._retornoEstiloSelecionado = estiloSelecionado;
            this._retornoTamanhoSelecionado = tamanhoSelecionado;
            this._retornoIdiomaSelecionado = idiomaSelecionado;

            this.Ocupado = true;
        }

        private async void Sincronizar()
        {
            if(App.__nivelDeProjeto == "CSN")
                await OffLine.Instancia.SincronizarTabelasBaseSuzanoAsync();
            else if (App.__nivelDeProjeto == "Suzano")
                await OffLine.Instancia.SincronizarTabelasBaseCsnAsync();
            this.IniciarTemporizador();
        }

        private void IniciarTemporizador()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), () =>
            {
                try
                {
                    if (OffLine.Instancia.Percentual >= 10000 || !OffLine.Instancia.Executando)
                    {
                        if (this.Habilitado == false)
                        {
                            this.InfoSincronizacao = this.DadosBanco();
                            this.Habilitado = true;
                            this.OnPropertyChanged("Log");
                        }
                    }
                    else
                    {
                        this.InfoSincronizacao = string.Concat(this.Textos.AtualizandoTabelasProgresso, OffLine.Instancia.Percentual.ToString("f2"), "%");
                        this.Habilitado = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                return OffLine.Instancia.Executando;
            });
        }

        private async void LimparDados()
        {
            await OffLine.Instancia.LimparAsync();
            this.InfoSincronizacao = this.Textos.UltimaAtualizacaoTabelas;
            this.OnPropertyChanged("Log");
        }

        private string DadosBanco()
        {
            string msg = string.Empty;
            List<INFO> infos = App.Banco.BuscarInfosAsync().Result;
            if (infos != null && infos.Count > 0)
            {
                DateTime dt = infos.Max(i => i.DT_LAST_UPDATE);
                
                msg = string.Concat(this.Textos.UltimaAtualizacaoTabelas, 
                                    dt.ToString(Globalizacao.FormatoData), 
                                    " ", 
                                    dt.ToString(Globalizacao.FormatoHora));

                foreach (INFO item in infos)
                {
                    if (item.NM_TABELA == "kbytes")
                        msg = string.Concat(msg, Environment.NewLine, this.Textos.KbytesBaixados, item.NU_QUANT);
                    else if (item.NM_TABELA == "registros")
                        msg = string.Concat(msg, Environment.NewLine, this.Textos.TotalRegistrosBaixados, item.NU_QUANT);
                }
            }
            return msg;
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            this.Habilitado = true;       
            try
            {
                List<ModeloObj> estilos = new List<ModeloObj>();
                estilos.Add(new ModeloObj(Guid.Empty, "Claro", this.Textos.Claro));
                estilos.Add(new ModeloObj(Guid.Empty, "Escuro", this.Textos.Escuro));
                this.Estilos = estilos;
                this.EstiloSelecionado = estilos.FirstOrDefault(e => e.Codigo == Config.Estilo);

                List<ModeloObj> tamanhos = new List<ModeloObj>();
                tamanhos.Add(new ModeloObj(Guid.Empty, "Pequeno", this.Textos.Pequeno));
                tamanhos.Add(new ModeloObj(Guid.Empty, "Medio", this.Textos.Medio));
                tamanhos.Add(new ModeloObj(Guid.Empty, "Grande", this.Textos.Grande));
                this.TamanhosFonte = tamanhos;
                this.TamanhoFonteSelecionado = tamanhos.FirstOrDefault(e => e.Codigo == Config.TamanhoFonte);

                this.Idiomas = Globalizacao.Idiomas.OrderBy(c => c.Descricao).ToList();
                this.IdiomaSelecionado = Globalizacao.IdiomaAtual;

                this.TextoEstilo = this.Textos.Estilo;
                this.TextoIdioma = this.Textos.Idioma;
                this.TextoLimparDados = this.Textos.LimparDados;
                this.TextoSincronizar = this.Textos.Sincronizar;
                this.TextoSincronizacao = this.Textos.Sincronizacao;
                this.TextoTamanhoDaFonte = this.Textos.TamanhoDaFonte;

                this.IniciarTemporizador();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            this.Ocupado = false;
        }
    }
}
