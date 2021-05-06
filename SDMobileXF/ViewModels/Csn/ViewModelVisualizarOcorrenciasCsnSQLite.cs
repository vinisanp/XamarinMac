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
    public class ViewModelVisualizarOcorrenciasCsnSQLite : ViewModelBase
    {
        private List<OCORRENCIACSN> _lista;
        private string _qtdRegistros;
        public Command ExcluirCommand { get; }

        #region Propriedades

        public List<OCORRENCIACSN> Lista
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


        public ViewModelVisualizarOcorrenciasCsnSQLite(Action<bool, string> retornoSincronizar, Action<OCORRENCIACSN> retornoConfirmarExcluir)
        {
            this.Titulo = this.Textos.Ocorrencias;
            this.ExcluirCommand = new Command((p) => { this.Excluir(p, retornoConfirmarExcluir); });
        }

        private void Excluir(object parameter, Action<OCORRENCIACSN> retornoConfirmarExcluir)
        {
            OCORRENCIACSN o = parameter as OCORRENCIACSN;
            if(o != null)
                retornoConfirmarExcluir?.Invoke(o);
        }

        public async Task ExclusaoConfirmada(OCORRENCIACSN o)
        {
            await App.Banco.ApagarAsync(o);
            await this.LoadAsync();
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                List<OCORRENCIACSN> lstDados = (await App.Banco.BuscarOcorrenciasCsnAsync()).OrderByDescending(o => o.DATA).ToList();
                this.Lista = lstDados;
                this.QtdRegistros = $" ({this.Lista.Count})";
            }
            catch (Exception ex)
            {
                this.Lista = new List<OCORRENCIACSN>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
