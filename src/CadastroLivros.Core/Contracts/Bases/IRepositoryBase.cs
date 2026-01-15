using CadastroLivros.Core.Bases;

namespace CadastroLivros.Core.Contracts.Bases;

public interface IRepositoryBase<TEntity> where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
}
