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
    public class ViewModelVisualizarOpasSQLite : ViewModelBase
    {
        private List<OPA> _lista;
        private string _qtdRegistros;
        public Command ExcluirCommand { get; }

        #region Propriedades

        public List<OPA> Lista
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


        public ViewModelVisualizarOpasSQLite(Action<bool, string> retornoSincronizar, Action<OPA> retornoConfirmarExcluir)
        {
            this.Titulo = this.Textos.Opas;
            this.ExcluirCommand = new Command((p) => { this.Excluir(p, retornoConfirmarExcluir); });
        }

        private void Excluir(object parameter, Action<OPA> retornoConfirmarExcluir)
        {
            OPA o = parameter as OPA;
            if(o != null)
                retornoConfirmarExcluir?.Invoke(o);
        }

        public async Task ExclusaoConfirmada(OPA a)
        {
            await App.Banco.ApagarAsync(a);
            await this.LoadAsync();
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                List<OPA> lstDados = (await App.Banco.BuscarOpasAsync()).OrderByDescending(o => o.DT_OPA).ToList();
                foreach (OPA o in lstDados)
                {
                    if (o.ID_ATIVIDADE.HasValue)
                    {
                        ATIVIDADE_INSPECAO ativ = await App.Banco.BuscarAtividadeInspecaoAsync(o.ID_ATIVIDADE.Value);
                        if (ativ != null)
                            o.DS_OPA = ativ.DS_ATIVIDADE_INSPECAO;
                    }

                    if (o.ID_TAREFA.HasValue)
                    {
                        TAREFA_OBSERVADA t = await App.Banco.BuscarTarefaObservadaAsync(o.ID_TAREFA.Value);
                        if (t != null)
                            o.DS_OPA = string.Concat(o.DS_OPA, " - ", t.DS_TAREFA_OBSERVADA);
                    }
                }

                this.Lista = lstDados;
                this.QtdRegistros = $" ({this.Lista.Count})";
            }
            catch (Exception ex)
            {
                this.Lista = new List<OPA>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
