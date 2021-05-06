namespace SDMobileXF.Classes
{
    public class Idioma
    {
        public Idioma(string codigo, string descricao, string imagem)
        {
            this.Codigo = codigo;
            this.Descricao = descricao;
            this.Imagem = imagem;
        }

        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }
}
