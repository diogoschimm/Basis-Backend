using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.SetupApp;

public static class GlobalExceptionManager
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;
                
                var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("GlobalExceptionManager");

                LogException(logger, exception, context);

                var problemDetails = CreateProblemDetails(exception, context);

                context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var json = JsonSerializer.Serialize(problemDetails, JsonOptions);

                await context.Response.WriteAsync(json);
            });
        });
    }

    private static void LogException(ILogger logger, Exception? exception, HttpContext context)
    {
        if (exception == null)
            return;

        var activity = System.Diagnostics.Activity.Current;
        var activityId = activity?.Id ?? "N/A";
        var parentId = activity?.ParentId ?? "N/A";

        logger.LogError(
            exception,
            "[Exception] {Method} {Path} | ActivityId: {ActivityId} | ParentId: {ParentId} | ExceptionType: {ExceptionType}",
            context.Request.Method,
            context.Request.Path,
            activityId,
            parentId,
            exception.GetType().Name
        );
    }

    private static ProblemDetails CreateProblemDetails(Exception? exception, HttpContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = context.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Title = "Ocorreu um erro ao processar sua solicitação.",
            Detail = exception?.Message
        };

        if (exception is ArgumentException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Erro de validação";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        }
        else if (exception is ArgumentNullException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Parâmetro inválido";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        }
        else if (exception is UnauthorizedAccessException)
        {
            problemDetails.Status = StatusCodes.Status401Unauthorized;
            problemDetails.Title = "Não autorizado";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
        }
        else if (exception is KeyNotFoundException || exception?.GetType().Name.Contains("NotFound") == true)
        {
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = "Recurso não encontrado";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
        }

        return problemDetails;
    }
}
