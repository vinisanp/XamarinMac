using SDMobileXF.ViewModels;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
	public class GrupoOpaVm : INotifyPropertyChanged
	{
        private ObservableCollection<CampoOpaVm> _campos;
        private int _countC = 0;
        private int _countNC = 0;
        private int _countNA = 0;
        private double _media = 0;
        private List<CampoOpa> _camposModelo;
        private List<ModeloObj> _lstConformeNaoConforme;
        private bool _emEdicao = false;
		private ViewModelOPA _pai;
        private bool _exibirGrupo = true;

		public event PropertyChangedEventHandler PropertyChanged;
        public Command CarregarCamposCommand { get; }
        public Textos Textos => Textos.Instancia;
		public string Titulo { get; set; }
        public string CorTituloPicker { get; set; }

        public bool EmEdicao
        {
            get { return this._emEdicao; }
            set
            {
                if (this.Campos != null)
                    foreach (CampoOpaVm campo in this.Campos)
                        campo.EmEdicao = this.EmEdicao;
                this.DefinirPropriedade(ref this._emEdicao, value);
            }
        }

        public bool ExibirGrupo
        {
            get { return this._exibirGrupo; }
            set { this.DefinirPropriedade(ref this._exibirGrupo, value); }
        }

        public ObservableCollection<CampoOpaVm> Campos
        {
            get { return this._campos; }
            set { this.DefinirPropriedade(ref this._campos, value); }
        }
        public int CountC
        {
            get { return this._countC; }
            set { this.DefinirPropriedade(ref this._countC, value); }
        }
        public int CountNC
        {
            get { return this._countNC; }
            set { this.DefinirPropriedade(ref this._countNC, value); }
        }
        public int CountNA
        {
            get { return this._countNA; }
            set { this.DefinirPropriedade(ref this._countNA, value); }
        }
        public double Media
        {
            get { return this._media; }
            set { this.DefinirPropriedade(ref this._media, value); }
        }

        public bool Preenchido
        {
            get
            {
                bool ret = true;

                if (this.Campos != null && this.Campos.Count > 0)
                {
                    foreach (CampoOpaVm c in this.Campos)
                        if (c.Resposta == null)
                        {
                            ret = false;
                            break;
                        }
                        else if(c.Resposta != null && c.Resposta.Id == new Guid("8196e394-50de-453d-8f40-a386ee068dd9") && string.IsNullOrEmpty(c.Comentario))
                        {
                            ret = false;
                            break;
                        }
                }
                else
                    ret = false;

                return ret;
            }
        }

        public GrupoOpaVm()
        {
            this.CarregarCamposCommand = new Command(() => { this.CarregarCampos(); });
        }

        public GrupoOpaVm(string titulo, List<CampoOpa> campos, List<ModeloObj> lstConformeNaoConforme, ViewModelOPA vm)
        {
            this._pai = vm;

            this.Titulo = titulo;
            this._campos = new ObservableCollection<CampoOpaVm>();
            this._camposModelo = campos;
            this._lstConformeNaoConforme = lstConformeNaoConforme;
            this.CarregarCamposCommand = new Command(() => { this.CarregarCampos(); });
        }
        public void DefinirCampos(List<CampoOpa> campos)
        {
            this._camposModelo = campos;
        }

        public void CarregarCampos()
        {
            if (this.Campos.Count == 0)
                foreach (CampoOpa c in this._camposModelo)
                    this.Campos.Add(new CampoOpaVm(c, this._lstConformeNaoConforme, this) { CorTituloPicker = this.CorTituloPicker, EmEdicao = this.EmEdicao });
            else
                this.ExibirGrupo = !this.ExibirGrupo;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string nomePropriedade = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        protected bool DefinirPropriedade<T>(ref T variavel, T valor, [CallerMemberName] string nomePropriedade = null)
        {
            if (EqualityComparer<T>.Default.Equals(variavel, valor))
                return false;

            variavel = valor;
            this.OnPropertyChanged(nomePropriedade);
            return true;
        }

        public void Calcular()
        {
            int c = 0;
            int nc = 0;
            int na = 0;

            List<CampoOpaVm> campos = this.Campos.ToList();
            if (campos == null || campos.Count == 0)
                campos = this._camposModelo.Select(campo => new CampoOpaVm(campo, this._lstConformeNaoConforme, null)).ToList();

            foreach (CampoOpaVm campo in campos)
            {
                if (campo.Resposta != null)
                {
                    if (campo.Resposta.Id == new Guid("9e0fd48b-3811-44d6-bcbf-fa6577c8211d"))
                        c++;
                    else if (campo.Resposta.Id == new Guid("8196e394-50de-453d-8f40-a386ee068dd9"))
                        nc++;
                    else if (campo.Resposta.Id == new Guid("2f837df0-d09d-46a6-8d66-5667f76c3a57"))
                        na++;
                }
            }
            this.CountC = c;
            this.CountNC = nc;
            this.CountNA = na;

            int t = c + nc;
            if (t > 0)
                this.Media = ((double)c * 100) / (double)t;
            else if (na > 0 && na == campos.Count)
                this.Media = 100;
            else
                this.Media = 0;
        }

        public void AtualizarGrupo()
        {
            this.Calcular();

            if (this._pai != null)
                this._pai.AtualizarOpa();
        }
    }
}
