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

    // Métodos para LivroAutor
    public async Task<List<int>> BuscarAutoresCodigosAsync(int livroCodigo)
    {
        return await _dbContext.LivroAutor
            .Where(la => la.LivroCodigo == livroCodigo)
            .Select(la => la.AutorCodigo)
            .ToListAsync();
    }

    public async Task AdicionarAutoresAsync(List<LivroAutor> livroAutores)
    {
        await _dbContext.LivroAutor.AddRangeAsync(livroAutores);
    }

    public async Task RemoverAutoresAsync(int livroCodigo, List<int> autoresCodigos)
    {
        var livroAutores = await _dbContext.LivroAutor
            .Where(la => la.LivroCodigo == livroCodigo && autoresCodigos.Contains(la.AutorCodigo))
            .ToListAsync();

        _dbContext.LivroAutor.RemoveRange(livroAutores);
    }

    // Métodos para LivroAssunto
    public async Task<List<int>> BuscarAssuntosCodigosAsync(int livroCodigo)
    {
        return await _dbContext.LivroAssunto
            .Where(la => la.LivroCodigo == livroCodigo)
            .Select(la => la.AssuntoCodigo)
            .ToListAsync();
    }

    public async Task AdicionarAssuntosAsync(List<LivroAssunto> livroAssuntos)
    {
        await _dbContext.LivroAssunto.AddRangeAsync(livroAssuntos);
    }

    public async Task RemoverAssuntosAsync(int livroCodigo, List<int> assuntosCodigos)
    {
        var livroAssuntos = await _dbContext.LivroAssunto
            .Where(la => la.LivroCodigo == livroCodigo && assuntosCodigos.Contains(la.AssuntoCodigo))
            .ToListAsync();

        _dbContext.LivroAssunto.RemoveRange(livroAssuntos);
    }

    // Métodos para LivroFormaCompra
    public async Task<List<int>> BuscarFormasCompraCodigosAsync(int livroCodigo)
    {
        return await _dbContext.LivroFormaCompra
            .Where(lfc => lfc.LivroCodigo == livroCodigo)
            .Select(lfc => lfc.FormaCompraCodigo)
            .ToListAsync();
    }

    public async Task AdicionarFormasCompraAsync(List<LivroFormaCompra> livroFormasCompra)
    {
        await _dbContext.LivroFormaCompra.AddRangeAsync(livroFormasCompra);
    }

    public async Task RemoverFormasCompraAsync(int livroCodigo, List<int> formasCompraCodigos)
    {
        var livroFormasCompra = await _dbContext.LivroFormaCompra
            .Where(lfc => lfc.LivroCodigo == livroCodigo && formasCompraCodigos.Contains(lfc.FormaCompraCodigo))
            .ToListAsync();

        _dbContext.LivroFormaCompra.RemoveRange(livroFormasCompra);
    }
}
