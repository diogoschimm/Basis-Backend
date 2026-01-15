using CadastroLivros.Infra.DbContexts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace CadastroLivros.Api.SetupApp;

public static class HealthCheckExtension
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("liveness", () => HealthCheckResult.Healthy("Aplicação está viva"), tags: ["liveness"])
            .AddDbContextCheck<LivrosDbContext>(
                name: "database",
                tags: ["readiness", "db", "sql"],
                failureStatus: HealthStatus.Unhealthy);

        return services;
    }

    public static IApplicationBuilder UseHealthCheckConfiguration(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("liveness"),
            ResponseWriter = WriteHealthCheckResponse
        });

        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("readiness"),
            ResponseWriter = WriteHealthCheckResponse
        });

        return app;
    }

    private static Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                exception = entry.Value.Exception?.Message,
                duration = entry.Value.Duration.ToString()
            }),
            totalDuration = report.TotalDuration.ToString()
        }, JsonOptions);

        return context.Response.WriteAsync(result);
    }
}

