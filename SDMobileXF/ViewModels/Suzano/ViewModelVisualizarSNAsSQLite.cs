using SDMobileXF.Banco.Tabelas;
using SDMobileXF.Classes;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SDMobileXF.ViewModels
{
    public class ViewModelVisualizarSNAsSQLite : ViewModelBase
    {
        private List<SNA> _lista;
        private string _qtdRegistros;
        public Command ExcluirCommand { get; }

        #region Propriedades

        public List<SNA> Lista
        {
            get
            {
                return this._lista;
            }
            set
            {
                this.DefinirPropriedade(ref this._lista, value);
            }
        }

        public string QtdRegistros
        {
            get { return this._qtdRegistros; }
            set { this.DefinirPropriedade(ref this._qtdRegistros, value); }

        }
        
        #endregion Propriedades


        public ViewModelVisualizarSNAsSQLite(Action<bool, string> retornoSincronizar, Action<SNA> retornoConfirmarExcluir)
        {
            this.Titulo = this.Textos.Abordagens;
            this.ExcluirCommand = new Command((p) => { this.Excluir(p, retornoConfirmarExcluir); });
        }

        private void Excluir(object parameter, Action<SNA> retornoConfirmarExcluir)
        {
            SNA a = parameter as SNA;
            if(a != null)
                retornoConfirmarExcluir?.Invoke(a);
        }

        public async Task ExclusaoConfirmada(SNA a)
        {
            await App.Banco.ApagarAsync(a);
            await this.LoadAsync();
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                List<SNA> lstDados = (await App.Banco.BuscarSNAsAsync()).OrderByDescending(o => o.DT_DATA).ToList();
                this.Lista = lstDados;
                this.QtdRegistros = $" ({this.Lista.Count})";
            }
            catch (Exception ex)
            {
                this.Lista = new List<SNA>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
