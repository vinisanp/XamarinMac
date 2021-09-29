using System;
using System.Collections.Generic;
using System.Text;

namespace SDMobileXFDados
{
    public class Enumerados
    {
        public enum Tabela
        {
            RelatoDeAnomalia = 0,
            UnidadeRegional = 1,
            Gerencia = 2,
            Area = 3,
            Local = 4,
            Tipo = 5,
            Classificacao = 6,
            SubClassificacao = 7,
            Fornecedor = 8,
            RespostasSimNao = 9,
            CadastroBasico = 10,
            Vinculo = 11,
            Empresa = 12,
            Unidade = 13,
            Categoria = 14,
            RespostasSeguroInseguroNa = 15,
            AtivadoresComportamentoCOGNITIVOS = 16,
            AtivadoresComportamentoFISIOLOGICOS = 17,
            AtivadoresComportamentoPSICOLOGICOS = 18,
            AtivadoresComportamentoSOCIAIS = 19,
            TipoInspecao = 20,
            AtividadeInspecao = 21,
            Letra = 22,
            TurnoAnomalia = 23,
            GerenciaGeralCsn = 24,
            GerenciaCsn = 25,
            AreaEquipamento = 26,
            LocalEquipamento = 27,
            OrigemAnomalia = 28,
            TipoAnomalia = 29,
            ClassificacaoTipo = 30,
            Probabilidade = 31,
            Severidade = 32,
            TarefaObservada = 33,
            TipoAvaliador = 34
        }

        public enum OrigemDados
        {
            Api = 0,
            SQLite = 1
        }

        public enum StatusLogin
        {
            LoginAutorizado = 0,
            LoginNaoAutorizado = 1,
            UsuarioInativo = 2
        }
    }
}
