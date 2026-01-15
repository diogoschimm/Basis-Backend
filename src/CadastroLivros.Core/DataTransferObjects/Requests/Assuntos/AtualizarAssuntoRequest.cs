using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Assuntos;

public class AtualizarAssuntoRequest
{
    [Required(ErrorMessage = "O código do assunto é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O código do assunto deve ser maior que zero.")]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "A descrição do assunto é obrigatória.")]
    [MaxLength(40, ErrorMessage = "A descrição do assunto deve ter no máximo 40 caracteres.")]
    public required string Descricao { get; set; }
}

