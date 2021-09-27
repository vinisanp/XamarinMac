using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SDMobileXF.ViewModels
{
    public static class Utils
    {
        public static async Task<object> RetornarValorAsync(string idChaveSessao, string sql)
        {
            string url = string.Concat(App.__EnderecoWebApi, "/Tabela/RetornarValor");

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("idChaveSessao", idChaveSessao);
            parametros.Add("sql", sql);

            FormUrlEncodedContent param = new FormUrlEncodedContent(parametros.ToArray());

            using (HttpClient requisicao = new HttpClient())
            {
                HttpResponseMessage resposta = await requisicao.PostAsync(url, param);

                if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string conteudo = await resposta.Content.ReadAsStringAsync();
                    if (conteudo == "null")
                        return string.Empty;
                    else
                    {
                        conteudo = conteudo.Replace("\"", string.Empty);
                        return conteudo;
                    }
                }
                else
                    return string.Empty;
            }
        }
    }
}
