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
    public class ViewModelVisualizarOcorrenciasSQLite : ViewModelBase
    {
        private List<OCORRENCIA> _lista;
        private string _qtdRegistros;
        public Command ExcluirCommand { get; }

        #region Propriedades

        public List<OCORRENCIA> Lista
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


        public ViewModelVisualizarOcorrenciasSQLite(Action<bool, string> retornoSincronizar, Action<OCORRENCIA> retornoConfirmarExcluir)
        {
            this.Titulo = this.Textos.Ocorrencias;
            this.ExcluirCommand = new Command((p) => { this.Excluir(p, retornoConfirmarExcluir); });
        }

        private void Excluir(object parameter, Action<OCORRENCIA> retornoConfirmarExcluir)
        {
            OCORRENCIA o = parameter as OCORRENCIA;
            if(o != null)
                retornoConfirmarExcluir?.Invoke(o);
        }

        public async Task ExclusaoConfirmada(OCORRENCIA o)
        {
            await App.Banco.ApagarAsync(o);
            await this.LoadAsync();
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                List<OCORRENCIA> lstDados = (await App.Banco.BuscarOcorrenciasAsync()).OrderByDescending(o => o.DATA).ToList();
                this.Lista = lstDados;
                this.QtdRegistros = $" ({this.Lista.Count})";
            }
            catch (Exception ex)
            {
                this.Lista = new List<OCORRENCIA>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
