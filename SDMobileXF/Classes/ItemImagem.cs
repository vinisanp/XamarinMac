using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
    public class ItemImagem
    {
        private byte[] _image;
        private string _caminho;

        public Guid IdImagem { get; set; }

        public string Nome { get; private set; }
        
        public string Caminho 
        {
            get
            {
                return this._caminho;
            }
            set
            {
                this._caminho = value;
                if (!string.IsNullOrEmpty(value))
                    this.Nome = Path.GetFileName(value);
                else
                    this.Nome = null;
            }
        }
        
        public DateTime? Data { get; set; }

        public ImageSource ImageSource { get; private set; }

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

        public ItemImagem()
        {
            this.Data = DateTime.Now;
        }
    }
}
