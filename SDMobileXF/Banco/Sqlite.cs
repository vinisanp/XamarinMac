using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SDMobileXF.Banco.Tabelas;
using SDMobileXFDados;
using SDMobileXFDados.Modelos;
using SQLite;
using Xamarin.Forms;

namespace SDMobileXF.Banco
{
    public class Sqlite
    {
        protected readonly SQLiteAsyncConnection _conn;

        public string Caminho { get; }

        public IEnumerable<TableMapping> Mapeamento
        {
            get
            {
                return this._conn.TableMappings;
            }
        }

        public SQLiteAsyncConnection Connection => this._conn;

        public Sqlite(string caminho)
        {
            try
            {
                this.Caminho = caminho;
                this._conn = new SQLiteAsyncConnection(this.Caminho, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                this._conn.ExecuteScalarAsync<string>("PRAGMA journal_mode = WAL");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public Task<int> ApagarAsync<T>() { return this._conn.DeleteAllAsync<T>(); }

        public Task<int> AlterarAsync(IEnumerable lst) { return this._conn.UpdateAllAsync(lst, true); }

        public Task<int> InserirAsync(IEnumerable lst) { return this._conn.InsertAllAsync(lst, true); }
    }
}

