using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.DataTransferObjects.Requests.Livros;

public class AdicionarAssuntosRequest
{
    [Required(ErrorMessage = "O código do livro é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O código do livro deve ser maior que zero.")]
    public int LivroCodigo { get; set; }

    [Required(ErrorMessage = "A lista de códigos de assuntos é obrigatória.")]
    [MinLength(1, ErrorMessage = "É necessário informar pelo menos um assunto.")]
    public required List<int> AssuntosCodigos { get; set; }
}

