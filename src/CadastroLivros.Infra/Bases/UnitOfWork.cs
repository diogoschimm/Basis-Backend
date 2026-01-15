using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Infra.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace CadastroLivros.Infra.Bases;

public class UnitOfWork(LivrosDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        GC.SuppressFinalize(this);
    }
}

