using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Contracts.Repositories;

public interface IFormaCompraRepository : IRepositoryBase<FormaCompra>
{
    Task<FormaCompra?> BuscarPorCodigoAsync(int codigo);
    Task<(List<FormaCompra> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize);
}

