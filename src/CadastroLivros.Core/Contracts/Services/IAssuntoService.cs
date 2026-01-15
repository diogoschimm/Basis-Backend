using CadastroLivros.Core.DataTransferObjects.Requests.Assuntos;
using CadastroLivros.Core.DataTransferObjects.Responses;
using ErrorOr;

namespace CadastroLivros.Core.Contracts.Services;

public interface IAssuntoService
{
    Task<ErrorOr<PagedResult<AssuntoResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10);
    Task<ErrorOr<AssuntoResponse>> GetAsync(int codigo);
    Task<ErrorOr<AssuntoResponse>> AdicionarAsync(CriarAssuntoRequest request);
    Task<ErrorOr<AssuntoResponse>> AtualizarAsync(AtualizarAssuntoRequest request);
    Task<ErrorOr<bool>> RemoverAsync(int codigo);
}