using Plugin.Media.Abstractions;
using SDMobileXF.Banco.Tabelas;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDMobileXF.Classes
{
    public static class Extensao
    {
        public static string ToIdSimNao(this bool valor)
        {
            string ret = string.Empty;
            if (valor)
                ret = "89106d45-100d-4272-9f6f-2a6f4727929e";
            else
                ret = "c6dddb81-b5be-4145-9093-1fbcff36bffd";
            return ret;
        }

        public static bool? ToBoolSim(this string valor)
        {
            return valor == "89106d45-100d-4272-9f6f-2a6f4727929e";
        }

        public static bool ToBoolOtimo(this string valor)
        {
            return valor == "d297c9f3-b819-4798-8f7f-f08110ecc373";
        }

        public static bool ToBoolBom(this string valor)
        {
            return valor == "fe8e3871-0ea7-4d08-b36f-07cb49858ba3";
        }

        public static bool ToBoolRegular(this string valor)
        {
            return valor == "11b8835c-4522-49d8-88ca-1bdc0e67fea9";
        }

        public static bool ToBoolRuim(this string valor)
        {
            return valor == "aaac4e37-8052-4ec5-b221-214110f83ac4";
        }

        public static bool? ToBoolSimNao(this string valor)
        {
            if (valor == "89106d45-100d-4272-9f6f-2a6f4727929e")
                return true;
            if (valor == "c6dddb81-b5be-4145-9093-1fbcff36bffd")
                return false;

            return null;
        }

        public static string ToNumber(this bool valor)
        {
            string ret = string.Empty;
            if (valor)
                ret = "1";
            else
                ret = "0";
            return ret;
        }

        public static REGIONAL ToRegional(this ModeloObj modelo)                    { return new REGIONAL(modelo); }
        public static GERENCIA ToGerencia(this ModeloObj modelo)                    { return new GERENCIA(modelo); }
        public static AREA ToArea(this ModeloObj modelo)                            { return new AREA(modelo); }
        public static LOCAL ToLocal(this ModeloObj modelo)                          { return new LOCAL(modelo); }
        public static TIPO ToTipo(this ModeloObj modelo)                            { return new TIPO(modelo); }
        public static CLASSIFICACAO ToClassificacao(this ModeloObj modelo)          { return new CLASSIFICACAO(modelo); }
        public static SUBCLASSIFICACAO ToSubClassificacao(this ModeloObj modelo)    { return new SUBCLASSIFICACAO(modelo); }
        public static CATEGORIA ToCategoria(this ModeloObj modelo)                  { return new CATEGORIA(modelo); }
        public static FORNECEDOR ToFornecedor(this ModeloObj modelo)                { return new FORNECEDOR(modelo); }
        public static VINCULO ToVinculo(this ModeloObj modelo)                      { return new VINCULO(modelo); }
        public static SEGURO_INSEGURO ToSeguroInseguro(this ModeloObj modelo)       { return new SEGURO_INSEGURO(modelo); }
        public static COGNITIVO ToCognitivos(this ModeloObj modelo)                 { return new COGNITIVO(modelo); }
        public static FISIOLOGICO ToFisiologicos(this ModeloObj modelo)             { return new FISIOLOGICO(modelo); }
        public static PSICOLOGICO ToPsicologicos(this ModeloObj modelo)             { return new PSICOLOGICO(modelo); }
        public static SOCIAL ToSociais(this ModeloObj modelo)                       { return new SOCIAL(modelo); }
        public static TIPO_INSPECAO ToTipoInspecao(this ModeloObj modelo)           { return new TIPO_INSPECAO(modelo); }
        public static ATIVIDADE_INSPECAO ToAtividadeInspecao(this ModeloObj modelo) { return new ATIVIDADE_INSPECAO(modelo); }
        public static UNIDADE ToUnidade(this ModeloObj modelo)                      { return new UNIDADE(modelo); }
        public static LETRA ToLetra(this ModeloObj modelo)                          { return new LETRA(modelo); }
        public static TURNO ToTurno(this ModeloObj modelo)                          { return new TURNO(modelo); }
        public static GERENCIA_GERAL_CSN ToGerenciaGeralCsn(this ModeloObj modelo)  { return new GERENCIA_GERAL_CSN(modelo); }
        public static GERENCIA_CSN ToGerenciaCsn(this ModeloObj modelo)             { return new GERENCIA_CSN(modelo); }
        public static AREA_EQUIPAMENTO ToAreaEquipamento(this ModeloObj modelo)     { return new AREA_EQUIPAMENTO(modelo); }
        public static LOCAL_EQUIPAMENTO ToLocalEquipamento(this ModeloObj modelo)   { return new LOCAL_EQUIPAMENTO(modelo); }
        public static ORIGEM_ANOMALIA ToOrigemAnomalia(this ModeloObj modelo)       { return new ORIGEM_ANOMALIA(modelo); }
        public static TIPO_ANOMALIA ToTipoAnomalia(this ModeloObj modelo)           { return new TIPO_ANOMALIA(modelo); }
        public static CLASSIFICACAO_TIPO ToClassificacaoTipo(this ModeloObj modelo) { return new CLASSIFICACAO_TIPO(modelo); }
        public static PROBABILIDADE ToProbabilidade(this ModeloObj modelo)          { return new PROBABILIDADE(modelo); }
        public static SEVERIDADE ToSeveridade(this ModeloObj modelo)                { return new SEVERIDADE(modelo); }
        public static TIPO_AVALIADOR ToTipoAvaliador(this ModeloObj modelo)         { return new TIPO_AVALIADOR(modelo); }
        public static TAREFA_OBSERVADA ToTarefaObservada(this ModeloObj modelo)     { return new TAREFA_OBSERVADA(modelo); }

        public static string ToString(this DateTime d)
        {
            return string.Concat(d.ToShortDateString(), " ", d.ToLongTimeString());
        }

        public static string ToAAAAMMDD_HHMINSS(this DateTime d)
        {
            return string.Concat(d.ToString("yyyyMMdd"), "_", d.ToString("HHmmss"));
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static byte[] ToByteArray(this MediaFile foto)
        {
            return foto.GetStream().ToByteArray();
        }

        public static double Clamp(this double self, double min, double max)
        {
            return Math.Min(max, Math.Max(self, min));
        }

        public static bool Marcado(this ModeloObj m)
        {
            if (m != null)
                return m.Marcado;

            return false;
        }

        public static string MarcadoStr(this ModeloObj m)
        {
            if (m != null && m.Marcado)
                return "1";

            return "0";
        }


        public static string ToStringNullSafe(this object s)
        {
            if (s != null)
                return s.ToString();

            return string.Empty;
        }

        public static string IdStrNullSafe(this ModeloObj m)
        {
            if (m != null && m.Id != Guid.Empty)
                return m.Id.ToString();

            return string.Empty;
        }

        public static string IdStrNullSafe(this Guid? id)
        {
            if (id.HasValue)
                return id.Value.ToString();

            return string.Empty;
        }
    }
}
