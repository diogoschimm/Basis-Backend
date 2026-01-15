using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Helpers.Extensions;

public static class AutorExtensions
{
    public static AutorResponse ToResponse(this Autor autor)
    {
        return new AutorResponse
        {
            Codigo = autor.Codigo,
            Nome = autor.Nome
        };
    }
}

