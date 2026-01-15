using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CadastroLivros.Core.DependencyInjections;

public  static class DependencyInjectionsExtension
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<ILivroService, LivroService>();
        services.AddScoped<IAutorService, AutorService>();
        services.AddScoped<IAssuntoService, AssuntoService>();

        return services;
    }
}
