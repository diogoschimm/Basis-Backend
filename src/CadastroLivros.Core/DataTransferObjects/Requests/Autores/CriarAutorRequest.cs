using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Autores;

public class CriarAutorRequest
{
    [Required(ErrorMessage = "O nome do autor é obrigatório.")]
    [MaxLength(40, ErrorMessage = "O nome do autor deve ter no máximo 40 caracteres.")]
    public required string Nome { get; set; }
}

