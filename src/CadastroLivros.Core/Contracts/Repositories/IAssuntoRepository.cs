using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Contracts.Repositories;

public interface IAssuntoRepository : IRepositoryBase<Assunto>
{
    Task<Assunto?> BuscarPorCodigoAsync(int codigo);
    Task<(List<Assunto> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize);
}
