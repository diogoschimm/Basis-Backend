using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.SetupApp;

public static class ModelValidationExtension
{
    public static IMvcBuilder AddModelValidationConfiguration(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Erro de validação",
                    Status = StatusCodes.Status400BadRequest
                };
                return new BadRequestObjectResult(problemDetails);
            };
        });

        return builder;
    }
}

