using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
    public class CampoInspecaoVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ModeloObj _resposta;
        private List<ModeloObj> _respostasDaPergunta;
        private byte[] _image;
        private ImageSource _imageSource;
        private string _descricao;
        private bool _outrosCamposVisiveis;
        private string _numeroDNA;

        public string IdCampo { get; set; }
        public Dictionary<string, string> Colunas { get; set; }
        public string Pergunta { get; set; }
        public string TituloNumeroDNA { get; set; }
        public string TituloFoto { get; set; }
        public string TituloDescricaoSituacao { get; set; }

        public string Descricao
        {
            get { return this._descricao; }
            set { this.DefinirPropriedade(ref this._descricao, value); }
        }
        public bool OutrosCamposVisiveis
        {
            get { return this._outrosCamposVisiveis; }
            set { this.DefinirPropriedade(ref this._outrosCamposVisiveis, value); }
        }

        public string NumeroDNA
        {
            get { return this._numeroDNA; }
            set { this.DefinirPropriedade(ref this._numeroDNA, value); }
        }

        public ImageSource ImageSource
        {
            get { return this._imageSource; }
            set { this.DefinirPropriedade(ref this._imageSource, value); }
        }

        public byte[] Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;

                try
                {
                    if (value == null || value.Length == 0)
                        this.ImageSource = null;
                    else
                        this.ImageSource = ImageSource.FromStream(() => new MemoryStream(value));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    this.ImageSource = null;
                }
            }
        }

        public string CaminhoImagem { get; set; }

        public List<ModeloObj> RespostasDaPergunta
        {
            get { return this._respostasDaPergunta; }
            set { this.DefinirPropriedade(ref this._respostasDaPergunta, value); }
        }

        public ModeloObj Resposta
        {
            get { return this._resposta; }
            set 
            { 
                this.DefinirPropriedade(ref this._resposta, value);
                this.OutrosCamposVisiveis = (value != null);
            }
        }

        public CampoInspecaoVm()
        {
        }

        public CampoInspecaoVm(CampoInspecao c, List<ModeloObj> lstRespostas)
        {
            this.IdCampo = c.IdCampo;
            this.Colunas = c.Colunas;

            this.Pergunta = c.Pergunta;
            this.Descricao = c.Descricao;
            this.NumeroDNA = c.NumeroDNA;

            this.TituloDescricaoSituacao = Textos.Instancia.DescricaoSituacao;
            this.TituloFoto = Textos.Instancia.Foto;
            this.TituloNumeroDNA = Textos.Instancia.NumeroDNA;
            this.RespostasDaPergunta = lstRespostas;
            this.Resposta = lstRespostas.FirstOrDefault(r => r.Id.ToStringNullSafe() == c.IdConforme);

            this.Image = c.Image;
        }

        public CampoInspecao ToCampoInspecao()
        {
            CampoInspecao c = new CampoInspecao();

            c.IdCampo = this.IdCampo;
            
            c.Pergunta   = this.Pergunta;
            c.Descricao  = this.Descricao;
            c.NumeroDNA  = this.NumeroDNA;

            if (this.Resposta != null)
                c.IdConforme = this.Resposta.IdStrNullSafe();

            if (!string.IsNullOrEmpty(c.Descricao) || !string.IsNullOrEmpty(c.NumeroDNA) || !string.IsNullOrEmpty(c.IdConforme))
                c.Colunas = this.Colunas;

            return c;
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
    }
}
