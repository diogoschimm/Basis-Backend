using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Assuntos;

public class CriarAssuntoRequest
{
    [Required(ErrorMessage = "A descrição do assunto é obrigatória.")]
    [MaxLength(40, ErrorMessage = "A descrição do assunto deve ter no máximo 40 caracteres.")]
    public required string Descricao { get; set; }
}

