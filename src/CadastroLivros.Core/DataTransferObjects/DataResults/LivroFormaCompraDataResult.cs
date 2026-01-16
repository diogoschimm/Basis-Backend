namespace CadastroLivros.Core.DataTransferObjects.DataResults;

public class LivroFormaCompraDataResult
{
    public int FormaCompraCodigo { get; set; }
    public decimal ValorCompra { get; set; }
    public string FormaCompraDescricao { get; set; } = string.Empty;
}

