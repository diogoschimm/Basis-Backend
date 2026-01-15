using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.Entities;
using CadastroLivros.Infra.Bases;
using CadastroLivros.Infra.DbContexts;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Infra.Repositories;

internal class AssuntoRepository(LivrosDbContext dbContext)
    : RepositoryBase<Assunto, LivrosDbContext>(dbContext), IAssuntoRepository
{
    public async Task<Assunto?> BuscarPorCodigoAsync(int codigo)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Codigo == codigo);
    }

    public async Task<(List<Assunto> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;

        var countQuery = "SELECT COUNT(*) FROM Assunto";
        var totalCount = await _connection.QuerySingleAsync<int>(countQuery);

        var dataQuery = @"
            SELECT Codigo, Descricao 
            FROM Assunto 
            ORDER BY Codigo 
            OFFSET @Offset ROWS 
            FETCH NEXT @PageSize ROWS ONLY";

        var items = (await _connection.QueryAsync<Assunto>(dataQuery, new { Offset = offset, PageSize = pageSize })).ToList();

        return (items, totalCount);
    }
}
