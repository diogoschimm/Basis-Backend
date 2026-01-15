using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace CadastroLivros.Api.SetupApp;

public static class RateLimitingExtension
{
    private const string DefaultPolicy = "DefaultPolicy";
    private const string AuthenticatedPolicy = "AuthenticatedPolicy";

    public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Política padrão para usuários não autenticados
            options.AddFixedWindowLimiter(DefaultPolicy, limiterOptions =>
            {
                limiterOptions.PermitLimit = 100; // 100 requisições
                limiterOptions.Window = TimeSpan.FromMinutes(1); // por minuto
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 10; // permite 10 requisições em fila
            });

            // Política para usuários autenticados (mais permissiva)
            options.AddFixedWindowLimiter(AuthenticatedPolicy, limiterOptions =>
            {
                limiterOptions.PermitLimit = 200; // 200 requisições
                limiterOptions.Window = TimeSpan.FromMinutes(1); // por minuto
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 20; // permite 20 requisições em fila
            });

            // Configuração global de rejeição
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/problem+json";

                var problemDetails = new
                {
                    Type = "https://tools.ietf.org/html/rfc6585#section-4",
                    Title = "Muitas requisições",
                    Status = StatusCodes.Status429TooManyRequests,
                    Detail = "Você excedeu o limite de requisições. Tente novamente mais tarde.",
                    Instance = context.HttpContext.Request.Path
                };

                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            };
        });

        return services;
    }

    public static IApplicationBuilder UseRateLimitingConfiguration(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
        return app;
    }
}

