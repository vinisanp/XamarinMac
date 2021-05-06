using SD.Dados;
using SD.Dados.Modelos;
using SD.Dados.Repositorio;
using SDWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SDWebApi.Controllers
{
    public class Util
    {
        public static void ExcluirSequencia(Contexto contexto, Guid idTabela, Guid idRegistro)
        {
            string nmTabela = Gema.Utils.RetornarNomeObjeto(Gema.Enumerados.TipoObjeto.Tabela, idTabela);

            using (RepositorioSequenciaTabela repositorio = new RepositorioSequenciaTabela(contexto))
            {
                repositorio.ExcluirSequenciaTabela(nmTabela, idRegistro);
            }
        }

        public static string GerarSequencia(Contexto contexto, Guid idTabela, Guid idColuna, Guid idRegistro, DateTime data)
        {
            string nmTabela = Gema.Utils.RetornarNomeObjeto(Gema.Enumerados.TipoObjeto.Tabela, idTabela);
            string nmColuna = Gema.Utils.RetornarNomeObjeto(Gema.Enumerados.TipoObjeto.Coluna, idColuna);

            SEQUENCIA_TABELA_BASE sequenciaTabelaBase = null;
            using (RepositorioSequenciaTabelaBase repositorio = new RepositorioSequenciaTabelaBase(contexto))
            {
                sequenciaTabelaBase = repositorio.RetornarItem(idTabela);
            }

            if (sequenciaTabelaBase != null)
            {
                using (RepositorioSequenciaTabela repositorio = new RepositorioSequenciaTabela(contexto))
                {
                    int nuDigitos = sequenciaTabelaBase.NU_DIGITOS;
                    if (nuDigitos == 0)
                        nuDigitos = 4;

                    SEQUENCIA_TABELA sequenciaTabela = repositorio.GerarSequenciaTabela(nmTabela, nmColuna, null, idRegistro, null, nuDigitos);

                    if (sequenciaTabelaBase.Sequencia != TipoSequencia.CodigoLivre)
                    {
                        string seq = sequenciaTabela.NU_SEQUENCIA.ToString().PadLeft(nuDigitos, '0');

                        if (sequenciaTabelaBase.Sequencia == TipoSequencia.SequencialAnoMes)
                            seq = string.Concat(data.Year.ToString(), data.Month.ToString().PadLeft(2, '0'), seq);
                        else if (sequenciaTabelaBase.Sequencia == TipoSequencia.SequencialAnoMesDia)
                            seq = string.Concat(data.Year.ToString(), data.Month.ToString().PadLeft(2, '0'), data.Day.ToString().PadLeft(2, '0'), seq);

                        return seq;
                    }
                }
            }

            return null;
        }

        public static void IniciarSessao(Sessao sessao, string login)
        {
            try
            {
                string loginRede = System.Environment.UserName.Trim();
                string dominioRede = System.Environment.UserDomainName.Trim();
                string maquinaRede = System.Environment.MachineName.Trim();
                string nomeAplicativo = Assembly.GetExecutingAssembly().GetName().Name;
                Guid idChaveSessao = sessao.Contexto.ConexaoComBanco.IdChaveSessao.Value;

                using (RepositorioSessaoUsuario repositorioSessaoUsuario = new RepositorioSessaoUsuario(sessao.Contexto))
                {
                    SESSAO_USUARIO sessaoUsuario = repositorioSessaoUsuario.RetornarItem(idChaveSessao);
                    if (sessaoUsuario == null)
                    {
                        repositorioSessaoUsuario.IniciarSessao(idChaveSessao, nomeAplicativo, login, loginRede, String.Format("Domínio: {0}, Máquina: {1}", dominioRede, maquinaRede));
                    }
                }
            }
            catch { }
        }

        public static List<SDMobileXFDados.Modelos.ModeloObj> BuscarTabela(Sessao sessao, string tab, string colId, string colCod, string colDesc, bool ativos)
        {
            List<SDMobileXFDados.Modelos.ModeloObj> lst = new List<SDMobileXFDados.Modelos.ModeloObj>();
            using (RepositorioPesquisa rep = new RepositorioPesquisa(sessao.Contexto))
            {
                string sql = "SELECT {0} Id, {1} Codigo, {2} Descricao FROM {3} {4}";
                string filtro = string.Empty;
                if (ativos)
                    filtro = "WHERE CL000000000000ABCD003300000000 = 1";

                    sql = string.Format(sql, colId, colCod, colDesc, tab, filtro);
                lst = rep.GetDados<SDMobileXFDados.Modelos.ModeloObj>(sql).ToList();
            }

            return lst;
        }

        public static SDMobileXFDados.Modelos.ModeloObj GetModelo(DataRow linha, string coluna)
        {
            if (!linha.IsNull("ID_" + coluna))
            {
                SDMobileXFDados.Modelos.ModeloObj m = new SDMobileXFDados.Modelos.ModeloObj();
                m.Id = new Guid(Convert.ToString(linha["ID_" + coluna]));

                if (linha.Table.Columns.Contains("ST_" + coluna) && !linha.IsNull("ST_" + coluna))
                    m.Marcado = Convert.ToBoolean(linha["ST_" + coluna]);

                if (linha.Table.Columns.Contains("ST_" + coluna + "_VER_AGIR") && !linha.IsNull("ST_" + coluna + "_VER_AGIR"))
                    m.Marcado = Convert.ToBoolean(linha["ST_" + coluna + "_VER_AGIR"]);

                if (linha.Table.Columns.Contains("CD_" + coluna) && !linha.IsNull("CD_" + coluna))
                    m.Codigo = Convert.ToString(linha["CD_" + coluna]);

                if (linha.Table.Columns.Contains("DS_" + coluna) && !linha.IsNull("DS_" + coluna))
                    m.Descricao = Convert.ToString(linha["DS_" + coluna]);

                return m;
            }

            return null;
        }
    }
}