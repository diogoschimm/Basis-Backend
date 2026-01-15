using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.FormasCompra;
using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Helpers.Extensions;
using ErrorOr;

namespace CadastroLivros.Core.Services;

public class FormaCompraService(IFormaCompraRepository formaCompraRepository, IUnitOfWork unitOfWork) : IFormaCompraService
{
    public async Task<ErrorOr<PagedResult<FormaCompraResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1)
            return Error.Validation("FormaCompra.PaginaInvalida", "O número da página deve ser maior que zero");

        if (pageSize < 1 || pageSize > 100)
            return Error.Validation("FormaCompra.TamanhoPaginaInvalido", "O tamanho da página deve estar entre 1 e 100");

        var (items, totalCount) = await formaCompraRepository.BuscarTodosAsync(pageNumber, pageSize);

        var response = new PagedResult<FormaCompraResponse>
        {
            Items = [.. items.Select(fc => fc.ToResponse())],
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return response;
    }

    public async Task<ErrorOr<FormaCompraResponse>> GetAsync(int codigo)
    {
        var formaCompra = await formaCompraRepository.BuscarPorCodigoAsync(codigo);
        if (formaCompra == null)
            return Error.NotFound("FormaCompra.NaoEncontrada", $"Forma de compra com código {codigo} não encontrada");

        return formaCompra.ToResponse();
    }

    public async Task<ErrorOr<FormaCompraResponse>> AdicionarAsync(CriarFormaCompraRequest request)
    {
        var formaCompraExistente = await formaCompraRepository.BuscarPorCodigoAsync(request.Codigo);
        if (formaCompraExistente != null)
            return Error.Conflict("FormaCompra.JaExiste", $"Já existe uma forma de compra com o código {request.Codigo}");

        var formaCompra = new FormaCompra
        {
            Codigo = request.Codigo,
            Descricao = request.Descricao
        };

        await formaCompraRepository.AddAsync(formaCompra);
        await unitOfWork.SaveChangesAsync();

        return formaCompra.ToResponse();
    }

    public async Task<ErrorOr<FormaCompraResponse>> AtualizarAsync(AtualizarFormaCompraRequest request)
    {
        var formaCompraExistente = await formaCompraRepository.BuscarPorCodigoAsync(request.Codigo);
        if (formaCompraExistente == null)
            return Error.NotFound("FormaCompra.NaoEncontrada", $"Forma de compra com código {request.Codigo} não encontrada");

        formaCompraExistente.Descricao = request.Descricao;

        await formaCompraRepository.UpdateAsync(formaCompraExistente);
        await unitOfWork.SaveChangesAsync();

        return formaCompraExistente.ToResponse();
    }

    public async Task<ErrorOr<bool>> RemoverAsync(int codigo)
    {
        var formaCompra = await formaCompraRepository.BuscarPorCodigoAsync(codigo);
        if (formaCompra == null)
            return Error.NotFound("FormaCompra.NaoEncontrada", $"Forma de compra com código {codigo} não encontrada");

        await formaCompraRepository.DeleteAsync(formaCompra);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}

