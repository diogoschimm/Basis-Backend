using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Entities;

namespace CadastroLivros.Core.Contracts.Repositories;

public interface ILivroRepository : IRepositoryBase<Livro>
{
    Task<Livro?> BuscarPorCodigoAsync(int codigo);
    Task<(List<Livro> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize);
}