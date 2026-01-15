namespace CadastroLivros.Core.Entities.Ternarias;

public class LivroFormaCompra
{
    public required int LivroCodigo { get; set; }
    public required int FormaCompraCodigo { get; set; }
    public required decimal ValorCompra { get; set; }
}