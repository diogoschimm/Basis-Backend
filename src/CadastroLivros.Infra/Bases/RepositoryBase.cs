using CadastroLivros.Core.Bases;
using CadastroLivros.Core.Contracts.Bases;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CadastroLivros.Infra.Bases;

public abstract class RepositoryBase<TEntity, TContext>: IRepositoryBase<TEntity>
    where TEntity : Entity
    where TContext : DbContext
{
    protected readonly TContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly DbConnection _connection;

    public RepositoryBase(TContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
        _connection = _dbContext.Database.GetDbConnection();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return Task.FromResult(entity);
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return Task.FromResult(true);
    }
}
