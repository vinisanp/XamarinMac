using SDMobileXF.Classes;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace SDMobileXF.Banco.Tabelas
{
    public class ViewModelModelo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelModelo()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string nomePropriedade = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        public virtual async Task Atualizar()
        {
            this.OnPropertyChanged(string.Empty);
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
