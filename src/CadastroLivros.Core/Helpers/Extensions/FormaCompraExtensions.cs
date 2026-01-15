using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Helpers.Extensions;

public static class FormaCompraExtensions
{
    public static FormaCompraResponse ToResponse(this FormaCompra formaCompra)
    {
        return new FormaCompraResponse
        {
            Codigo = formaCompra.Codigo,
            Descricao = formaCompra.Descricao
        };
    }
}

