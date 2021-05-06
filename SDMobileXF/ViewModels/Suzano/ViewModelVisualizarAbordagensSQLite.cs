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
    public class ViewModelVisualizarAbordagensSQLite : ViewModelBase
    {
        private List<ABORDAGEM_COMPORTAMENTAL> _lista;
        private string _qtdRegistros;
        public Command ExcluirCommand { get; }

        #region Propriedades

        public List<ABORDAGEM_COMPORTAMENTAL> Lista
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


        public ViewModelVisualizarAbordagensSQLite(Action<bool, string> retornoSincronizar, Action<ABORDAGEM_COMPORTAMENTAL> retornoConfirmarExcluir)
        {
            this.Titulo = this.Textos.Abordagens;
            this.ExcluirCommand = new Command((p) => { this.Excluir(p, retornoConfirmarExcluir); });
        }

        private void Excluir(object parameter, Action<ABORDAGEM_COMPORTAMENTAL> retornoConfirmarExcluir)
        {
            ABORDAGEM_COMPORTAMENTAL a = parameter as ABORDAGEM_COMPORTAMENTAL;
            if(a != null)
                retornoConfirmarExcluir?.Invoke(a);
        }

        public async Task ExclusaoConfirmada(ABORDAGEM_COMPORTAMENTAL a)
        {
            await App.Banco.ApagarAsync(a);
            await this.LoadAsync();
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                List<ABORDAGEM_COMPORTAMENTAL> lstDados = (await App.Banco.BuscarAbordagensAsync()).OrderByDescending(o => o.DATA).ToList();
                this.Lista = lstDados;
                this.QtdRegistros = $" ({this.Lista.Count})";
            }
            catch (Exception ex)
            {
                this.Lista = new List<ABORDAGEM_COMPORTAMENTAL>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
