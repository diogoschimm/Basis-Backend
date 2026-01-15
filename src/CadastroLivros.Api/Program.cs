using CadastroLivros.Api.Filters;
using CadastroLivros.Api.Middlewares;
using CadastroLivros.Api.SetupApp;
using CadastroLivros.Core.DependencyInjections;
using CadastroLivros.Infra.DependencyInjections;
using NLog;
using NLog.Web;

var nlogConfigPath = Path.Combine(AppContext.BaseDirectory, "nlog.config");
var logger = LogManager.Setup()
    .LoadConfigurationFromFile(nlogConfigPath)
    .GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    builder.Services.AddCoreDependencies();
    builder.Services.AddInfraDependencies(builder.Configuration);

    builder.Services.AddOpenTelemetryConfiguration(builder.Configuration);
    builder.Services.AddRateLimitingConfiguration();
    builder.Services.AddHealthCheckConfiguration();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        })
        .AddModelValidationConfiguration();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Basis Backend API",
            Version = "v1",
            Description = "API REST para gerenciamento de cadastro de livros, autores e assuntos",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Basis",
            }
        });

        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }

        // Adicionar exemplos de JSON
        c.SchemaFilter<SwaggerExampleSchemaFilter>();
        
        // Configurar para usar camelCase
        c.DescribeAllParametersInCamelCase();
    });

    var app = builder.Build();
    app.UseGlobalExceptionHandler();
    app.UseRateLimitingConfiguration();
    app.UseMiddleware<HttpLoggerMiddleware>();

    // Swagger is only enabled in development environment
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHealthCheckConfiguration();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
