using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Helpers.Extensions;

public static class AssuntoExtensions
{
    public static AssuntoResponse ToResponse(this Assunto assunto)
    {
        return new AssuntoResponse
        {
            Codigo = assunto.Codigo,
            Descricao = assunto.Descricao
        };
    }
}

