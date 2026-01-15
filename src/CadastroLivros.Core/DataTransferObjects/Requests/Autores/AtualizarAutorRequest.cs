using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Autores;

public class AtualizarAutorRequest
{
    [Required(ErrorMessage = "O código do autor é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O código do autor deve ser maior que zero.")]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "O nome do autor é obrigatório.")]
    [MaxLength(40, ErrorMessage = "O nome do autor deve ter no máximo 40 caracteres.")]
    public required string Nome { get; set; }
}

