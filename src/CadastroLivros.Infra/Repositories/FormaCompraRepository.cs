using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.Entities;
using CadastroLivros.Infra.Bases;
using CadastroLivros.Infra.DbContexts;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Infra.Repositories;

internal class FormaCompraRepository(LivrosDbContext dbContext)
    : RepositoryBase<FormaCompra, LivrosDbContext>(dbContext), IFormaCompraRepository
{
    public async Task<FormaCompra?> BuscarPorCodigoAsync(int codigo)
    {
        return await _dbSet.FirstOrDefaultAsync(fc => fc.Codigo == codigo);
    }

    public async Task<(List<FormaCompra> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;

        var countQuery = "SELECT COUNT(*) FROM FormaCompra";
        var totalCount = await DbConnection.QuerySingleAsync<int>(countQuery);

        var dataQuery = @"
            SELECT Codigo, Descricao 
            FROM FormaCompra 
            ORDER BY Codigo 
            OFFSET @Offset ROWS 
            FETCH NEXT @PageSize ROWS ONLY";

        var items = (await DbConnection.QueryAsync<FormaCompra>(dataQuery, new { Offset = offset, PageSize = pageSize })).ToList();

        return (items, totalCount);
    }
}

