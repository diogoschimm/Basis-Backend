using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Contracts.Repositories;

public interface IAutorRepository : IRepositoryBase<Autor>
{
    Task<Autor?> BuscarPorCodigoAsync(int codigo);
    Task<(List<Autor> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize);
}
