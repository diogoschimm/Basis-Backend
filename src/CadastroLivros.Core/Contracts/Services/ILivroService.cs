using CadastroLivros.Core.DataTransferObjects.Requests.Livros;
using CadastroLivros.Core.DataTransferObjects.Responses;
using ErrorOr;

namespace CadastroLivros.Core.Contracts.Services;

public interface ILivroService
{
    Task<ErrorOr<PagedResult<LivroResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10);
    Task<ErrorOr<LivroResponse>> GetAsync(int codigo);
    Task<ErrorOr<LivroResponse>> AdicionarAsync(CriarLivroRequest request);
    Task<ErrorOr<LivroResponse>> AtualizarAsync(AtualizarLivroRequest request);
    Task<ErrorOr<bool>> RemoverAsync(int codigo);
    Task<ErrorOr<LivroResponse>> AdicionarAutoresAsync(AdicionarAutoresRequest request);
    Task<ErrorOr<LivroResponse>> AdicionarAssuntosAsync(AdicionarAssuntosRequest request);
    Task<ErrorOr<LivroResponse>> RemoverAutoresAsync(RemoverAutoresRequest request);
    Task<ErrorOr<LivroResponse>> RemoverAssuntosAsync(RemoverAssuntosRequest request);
}
