using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Livros;

public class FormaCompraItemRequest
{
    [Required(ErrorMessage = "O código da forma de compra é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O código da forma de compra deve ser maior que zero.")]
    public int FormaCompraCodigo { get; set; }

    [Required(ErrorMessage = "O valor da compra é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor da compra deve ser maior que zero.")]
    public decimal ValorCompra { get; set; }
}

