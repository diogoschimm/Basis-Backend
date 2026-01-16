using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Autores;
using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Helpers.Extensions;
using ErrorOr;

namespace CadastroLivros.Core.Services;

public class AutorService(IAutorRepository autorRepository, IUnitOfWork unitOfWork) : IAutorService
{
    public async Task<ErrorOr<PagedResult<AutorResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1)
            return Error.Validation("Autor.PaginaInvalida", "O número da página deve ser maior que zero");

        if (pageSize < 1 || pageSize > 10000)
            return Error.Validation("Autor.TamanhoPaginaInvalido", "O tamanho da página deve estar entre 1 e 10000");

        var (items, totalCount) = await autorRepository.BuscarTodosAsync(pageNumber, pageSize);

        var response = new PagedResult<AutorResponse>
        {
            Items = items.Select(a => a.ToResponse()).ToList(),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return response;
    }

    public async Task<ErrorOr<AutorResponse>> GetAsync(int codigo)
    {
        var autor = await autorRepository.BuscarPorCodigoAsync(codigo);
        if (autor == null)
            return Error.NotFound("Autor.NaoEncontrado", $"Autor com código {codigo} não encontrado");

        return autor.ToResponse();
    }

    public async Task<ErrorOr<AutorResponse>> AdicionarAsync(CriarAutorRequest request)
    {
        var autorExistente = await autorRepository.BuscarPorCodigoAsync(request.Codigo);
        if (autorExistente != null)
            return Error.Conflict("Autor.JaExiste", $"Já existe um autor com o código {request.Codigo}");

        var autor = new Autor
        {
            Codigo = request.Codigo,
            Nome = request.Nome
        };

        await autorRepository.AddAsync(autor);
        await unitOfWork.SaveChangesAsync();
        return autor.ToResponse();
    }

    public async Task<ErrorOr<AutorResponse>> AtualizarAsync(AtualizarAutorRequest request)
    {
        var autorExistente = await autorRepository.BuscarPorCodigoAsync(request.Codigo);
        if (autorExistente == null)
            return Error.NotFound("Autor.NaoEncontrado", $"Autor com código {request.Codigo} não encontrado");

        autorExistente.Nome = request.Nome;

        await autorRepository.UpdateAsync(autorExistente);
        await unitOfWork.SaveChangesAsync();
        return autorExistente.ToResponse();
    }

    public async Task<ErrorOr<bool>> RemoverAsync(int codigo)
    {
        var autor = await autorRepository.BuscarPorCodigoAsync(codigo);
        if (autor == null)
            return Error.NotFound("Autor.NaoEncontrado", $"Autor com código {codigo} não encontrado");

        await autorRepository.DeleteAsync(autor);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}

