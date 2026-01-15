using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;
using CadastroLivros.Infra.Bases;
using CadastroLivros.Infra.DbContexts;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Infra.Repositories;

internal class LivroRepository(LivrosDbContext dbContext) 
    : RepositoryBase<Livro, LivrosDbContext>(dbContext), ILivroRepository
{
    public async Task<Livro?> BuscarPorCodigoAsync(int codigo)
    {
        return await _dbSet.FirstOrDefaultAsync(l => l.Codigo == codigo);
    }

    public async Task<(List<Livro> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;

        var countQuery = "SELECT COUNT(*) FROM Livro";
        var totalCount = await _connection.QuerySingleAsync<int>(countQuery);

        var dataQuery = @"
            SELECT Codigo, Titulo, Editora, Edicao, AnoPublicacao 
            FROM Livro 
            ORDER BY Codigo 
            OFFSET @Offset ROWS 
            FETCH NEXT @PageSize ROWS ONLY";

        var items = (await _connection.QueryAsync<Livro>(dataQuery, new { Offset = offset, PageSize = pageSize })).ToList();

        return (items, totalCount);
    }
}
