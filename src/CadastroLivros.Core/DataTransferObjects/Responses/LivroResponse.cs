namespace CadastroLivros.Core.DataTransferObjects.Responses;

public class LivroResponse
{
    public int Codigo { get; set; }
    public string? Titulo { get; set; }
    public string? Editora { get; set; }
    public int Edicao { get; set; }
    public string? AnoPublicacao { get; set; }
    public List<AutorResponse>? Autores { get; set; }
    public List<AssuntoResponse>? Assuntos { get; set; }
    public List<FormaCompraItemResponse>? FormasCompra { get; set; }
}
