using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Helpers.Extensions;

public static class LivroExtensions
{
    public static LivroResponse ToResponse(this Livro livro)
    {
        return new LivroResponse
        {
            Codigo = livro.Codigo,
            Titulo = livro.Titulo,
            Editora = livro.Editora,
            Edicao = livro.Edicao,
            AnoPublicacao = livro.AnoPublicacao
        };
    }
}

