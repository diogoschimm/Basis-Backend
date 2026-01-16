using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.FormasCompra;

public class CriarFormaCompraRequest
{
    [Required(ErrorMessage = "A descrição da forma de compra é obrigatória.")]
    [MaxLength(40, ErrorMessage = "A descrição da forma de compra deve ter no máximo 40 caracteres.")]
    public required string Descricao { get; set; }
}

