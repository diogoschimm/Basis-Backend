using CadastroLivros.Core.DataTransferObjects.Requests.Autores;
using CadastroLivros.Core.DataTransferObjects.Responses;
using ErrorOr;

namespace CadastroLivros.Core.Contracts.Services;

public interface IAutorService
{
    Task<ErrorOr<PagedResult<AutorResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10);
    Task<ErrorOr<AutorResponse>> GetAsync(int codigo);
    Task<ErrorOr<AutorResponse>> AdicionarAsync(CriarAutorRequest request);
    Task<ErrorOr<AutorResponse>> AtualizarAsync(AtualizarAutorRequest request);
    Task<ErrorOr<bool>> RemoverAsync(int codigo);
}
