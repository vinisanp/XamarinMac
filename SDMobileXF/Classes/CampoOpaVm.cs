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
    public class CampoOpaVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ModeloObj _resposta;
        private List<ModeloObj> _respostasDaPergunta;
        private string _comentario;
        private bool _dsVisivel;
        private bool _dnaVisivel;
        private string _numeroDNA;
        private GrupoOpaVm _pai;
        private bool _emEdicao = false;
        private bool _podeAtualizarPai = true;
        private bool _comentarioVisivel = true;
        private byte[] _image;
        private ImageSource _imageSource;

        public virtual bool ComentarioVisivel
        {
            get { return this._comentarioVisivel; }
            set { this.DefinirPropriedade(ref this._comentarioVisivel, value); }
        }
        public virtual bool EmEdicao
        {
            get { return this._emEdicao; }
            set { this.DefinirPropriedade(ref this._emEdicao, value); }
        }
        public string CorTituloPicker { get; set; }
        public string IdCampo { get; set; }
        public Dictionary<string, string> Colunas { get; set; }
        public string Titulo { get; set; }
        public string TituloNumeroDNA { get; set; }
        public string TituloComentarios { get; set; }
        public string TituloFoto { get; set; }

        public string Comentario
        {
            get { return this._comentario; }
            set { this.DefinirPropriedade(ref this._comentario, value); }
        }
        public bool DnaVisivel
        {
            get { return this._dnaVisivel; }
            set { this.DefinirPropriedade(ref this._dnaVisivel, value); }
        }

        public string NumeroDNA
        {
            get { return this._numeroDNA; }
            set { this.DefinirPropriedade(ref this._numeroDNA, value); }
        }

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
                if (this.DefinirPropriedade(ref this._resposta, value))
                {
                    this.DnaVisivel = (value != null && value.Id == new Guid("8196e394-50de-453d-8f40-a386ee068dd9"));
                    if (this._pai != null && this._podeAtualizarPai)
                        this._pai.AtualizarGrupo();
                }
            }
        }

        public void DefinirRespostaSemAtualizarGrupo(string id)
        {
            this._podeAtualizarPai = false;
            if (!string.IsNullOrEmpty(id))
                this.Resposta = this.RespostasDaPergunta.FirstOrDefault(r => r.Id == new Guid(id));
            else
                this.Resposta = null;
            this._podeAtualizarPai = true;
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


        public CampoOpaVm()
        {
        }

        public CampoOpaVm(CampoOpa c, List<ModeloObj> lstRespostas, GrupoOpaVm grupo)
        {
            this._pai = grupo;

            this.IdCampo = c.IdCampo;
            this.Colunas = c.Colunas;

            this.Titulo = c.Titulo;
            this.Comentario = c.Comentario;
            this.NumeroDNA = c.NumeroDNA;

            this.TituloComentarios = Textos.Instancia.Comentarios;
            this.TituloNumeroDNA = Textos.Instancia.NumeroDNA;
            this.RespostasDaPergunta = lstRespostas;
            this.TituloFoto = Textos.Instancia.Foto;
            this.Image = c.Image;

            if (!string.IsNullOrEmpty(c.IdConforme))
                this.Resposta = lstRespostas.FirstOrDefault(r => r.Id.ToStringNullSafe() == c.IdConforme);
        }

        public void ToCampoOpa(CampoOpa c)
        {
            c.IdCampo   = this.IdCampo;
            c.Titulo    = this.Titulo;
            c.Comentario = this.Comentario;
            c.NumeroDNA = this.NumeroDNA;

            if (this.Resposta != null)
                c.IdConforme = this.Resposta.IdStrNullSafe();

            if (!string.IsNullOrEmpty(c.Comentario) || !string.IsNullOrEmpty(c.NumeroDNA) || !string.IsNullOrEmpty(c.IdConforme))
                c.Colunas = this.Colunas;
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
