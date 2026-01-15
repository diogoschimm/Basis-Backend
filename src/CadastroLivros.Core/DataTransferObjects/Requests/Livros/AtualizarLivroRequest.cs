using System.ComponentModel.DataAnnotations;
using CadastroLivros.Core.Helpers.Validations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Livros;

public class AtualizarLivroRequest
{
    [Required(ErrorMessage = "O código do livro é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O código do livro deve ser maior que zero.")]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "O título do livro é obrigatório.")]
    [MaxLength(40, ErrorMessage = "O título do livro deve ter no máximo 40 caracteres.")]
    public required string Titulo { get; set; }

    [Required(ErrorMessage = "A editora do livro é obrigatória.")]
    [MaxLength(40, ErrorMessage = "A editora do livro deve ter no máximo 40 caracteres.")]
    public required string Editora { get; set; }

    [Required(ErrorMessage = "A edição do livro é obrigatória.")]
    [Range(1, 1000, ErrorMessage = "A edição do livro deve estar entre 1 e 1000.")]
    public int Edicao { get; set; }

    [Required(ErrorMessage = "O ano de publicação é obrigatório.")]
    [MaxLength(4, ErrorMessage = "O ano de publicação deve ter no máximo 4 caracteres.")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "O ano de publicação deve ter exatamente 4 dígitos.")]
    [AnoPublicacao]
    public required string AnoPublicacao { get; set; }
}

