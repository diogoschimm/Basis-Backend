using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Infra.DbContexts;

public class LivrosDbContext(DbContextOptions<LivrosDbContext> options) : DbContext(options)
{
    public DbSet<Assunto> Assunto { get; set; }
    public DbSet<Livro> Livro { get; set; }
    public DbSet<Autor> Autor { get; set; }
    public DbSet<FormaCompra> FormaCompra { get; set; }
    public DbSet<LivroAssunto> LivroAssunto { get; set; }
    public DbSet<LivroAutor> LivroAutor { get; set; }
    public DbSet<LivroFormaCompra> LivroFormaCompra { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LivrosDbContext).Assembly);
    }
}
