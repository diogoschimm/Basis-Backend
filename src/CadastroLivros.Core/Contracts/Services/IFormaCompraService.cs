using CadastroLivros.Core.DataTransferObjects.Requests.FormasCompra;
using CadastroLivros.Core.DataTransferObjects.Responses;
using ErrorOr;

namespace CadastroLivros.Core.Contracts.Services;

public interface IFormaCompraService
{
    Task<ErrorOr<PagedResult<FormaCompraResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10);
    Task<ErrorOr<FormaCompraResponse>> GetAsync(int codigo);
    Task<ErrorOr<FormaCompraResponse>> AdicionarAsync(CriarFormaCompraRequest request);
    Task<ErrorOr<FormaCompraResponse>> AtualizarAsync(AtualizarFormaCompraRequest request);
    Task<ErrorOr<bool>> RemoverAsync(int codigo);
}

