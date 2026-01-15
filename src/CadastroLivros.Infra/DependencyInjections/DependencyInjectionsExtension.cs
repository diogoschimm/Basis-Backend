using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Infra.Bases;
using CadastroLivros.Infra.DbContexts;
using CadastroLivros.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CadastroLivros.Infra.DependencyInjections;

public static class DependencyInjectionsExtension
{
    public static IServiceCollection AddInfraDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LivrosDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAssuntoRepository, AssuntoRepository>();
        services.AddScoped<ILivroRepository, LivroRepository>();
        services.AddScoped<IAutorRepository, AutorRepository>();
        services.AddScoped<IFormaCompraRepository, FormaCompraRepository>();
        
        return services;
    }
}
