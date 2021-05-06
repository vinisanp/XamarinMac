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
    public class ViewModelVisualizarInspecoesSQLite : ViewModelBase
    {
        private List<INSPECAO> _lista;
        private string _qtdRegistros;
        public Command ExcluirCommand { get; }

        #region Propriedades

        public List<INSPECAO> Lista
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


        public ViewModelVisualizarInspecoesSQLite(Action<bool, string> retornoSincronizar, Action<INSPECAO> retornoConfirmarExcluir)
        {
            this.Titulo = this.Textos.Inspecoes;
            this.ExcluirCommand = new Command((p) => { this.Excluir(p, retornoConfirmarExcluir); });
        }

        private void Excluir(object parameter, Action<INSPECAO> retornoConfirmarExcluir)
        {
            INSPECAO i = parameter as INSPECAO;
            if(i != null)
                retornoConfirmarExcluir?.Invoke(i);
        }

        public async Task ExclusaoConfirmada(INSPECAO a)
        {
            await App.Banco.ApagarAsync(a);
            await this.LoadAsync();
        }

        public override async Task LoadAsync()
        {
            this.Ocupado = true;
            try
            {
                List<INSPECAO> lstDados = (await App.Banco.BuscarInspecoesAsync()).OrderByDescending(o => o.DT_DATA).ToList();
                foreach (INSPECAO i in lstDados)
                {
                    if (i.ID_TIPO.HasValue)
                    {
                        TIPO_INSPECAO tipo = await App.Banco.BuscarTipoInspecaoAsync(i.ID_TIPO.Value);
                        if (tipo != null)
                            i.DS_TIPO = tipo.CD_DS_TIPO_INSPECAO;
                    }

                    if (i.ID_ATIVIDADE.HasValue)
                    {
                        ATIVIDADE_INSPECAO ativ = await App.Banco.BuscarAtividadeInspecaoAsync(i.ID_ATIVIDADE.Value);
                        if (ativ != null)
                            i.DS_ATIVIDADE = ativ.CD_DS_ATIVIDADE_INSPECAO;
                    }
                }
                this.Lista = lstDados;
                this.QtdRegistros = $" ({this.Lista.Count})";
            }
            catch (Exception ex)
            {
                this.Lista = new List<INSPECAO>();
                Debug.WriteLine(ex.Message);
            }
            this.Ocupado = false;
        }
    }
}
