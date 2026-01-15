using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Livros;

public class AdicionarFormasCompraRequest
{
    [Required(ErrorMessage = "O código do livro é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O código do livro deve ser maior que zero.")]
    public int LivroCodigo { get; set; }

    [Required(ErrorMessage = "A lista de formas de compra é obrigatória.")]
    [MinLength(1, ErrorMessage = "É necessário informar pelo menos uma forma de compra.")]
    public required List<FormaCompraItemRequest> FormasCompra { get; set; }
}

