using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Assuntos;
using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Helpers.Extensions;
using ErrorOr;

namespace CadastroLivros.Core.Services;

public class AssuntoService(IAssuntoRepository assuntoRepository, IUnitOfWork unitOfWork) : IAssuntoService
{
    public async Task<ErrorOr<PagedResult<AssuntoResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1)
            return Error.Validation("Assunto.PaginaInvalida", "O número da página deve ser maior que zero");

        if (pageSize < 1 || pageSize > 10000)
            return Error.Validation("Assunto.TamanhoPaginaInvalido", "O tamanho da página deve estar entre 1 e 10000");

        var (items, totalCount) = await assuntoRepository.BuscarTodosAsync(pageNumber, pageSize);

        var response = new PagedResult<AssuntoResponse>
        {
            Items = [.. items.Select(a => a.ToResponse())],
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return response;
    }

    public async Task<ErrorOr<AssuntoResponse>> GetAsync(int codigo)
    {
        var assunto = await assuntoRepository.BuscarPorCodigoAsync(codigo);
        if (assunto == null)
            return Error.NotFound("Assunto.NaoEncontrado", $"Assunto com código {codigo} não encontrado");

        return assunto.ToResponse();
    }

    public async Task<ErrorOr<AssuntoResponse>> AdicionarAsync(CriarAssuntoRequest request)
    {
        var assunto = new Assunto
        {
            Descricao = request.Descricao
        };

        await assuntoRepository.AddAsync(assunto);
        await unitOfWork.SaveChangesAsync();

        return assunto.ToResponse();
    }

    public async Task<ErrorOr<AssuntoResponse>> AtualizarAsync(AtualizarAssuntoRequest request)
    {
        var assuntoExistente = await assuntoRepository.BuscarPorCodigoAsync(request.Codigo);
        if (assuntoExistente == null)
            return Error.NotFound("Assunto.NaoEncontrado", $"Assunto com código {request.Codigo} não encontrado");

        assuntoExistente.Descricao = request.Descricao;

        await assuntoRepository.UpdateAsync(assuntoExistente);
        await unitOfWork.SaveChangesAsync();

        return assuntoExistente.ToResponse();
    }

    public async Task<ErrorOr<bool>> RemoverAsync(int codigo)
    {
        var assunto = await assuntoRepository.BuscarPorCodigoAsync(codigo);
        if (assunto == null)
            return Error.NotFound("Assunto.NaoEncontrado", $"Assunto com código {codigo} não encontrado");

        await assuntoRepository.DeleteAsync(assunto);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}

