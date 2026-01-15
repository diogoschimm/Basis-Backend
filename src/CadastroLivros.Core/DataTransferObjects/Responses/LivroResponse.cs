namespace CadastroLivros.Core.DataTransferObjects.Responses;

public class LivroResponse
{
    public int Codigo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Editora { get; set; } = string.Empty;
    public int Edicao { get; set; }
    public string AnoPublicacao { get; set; } = string.Empty;
}
