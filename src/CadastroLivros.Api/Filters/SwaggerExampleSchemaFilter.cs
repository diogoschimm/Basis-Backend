using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CadastroLivros.Api.Filters;

public class SwaggerExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;

        // Exemplos para CriarLivroRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.Livros.CriarLivroRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["titulo"] = new OpenApiString("Código Limpo"),
                ["editora"] = new OpenApiString("Prentice Hall"),
                ["edicao"] = new OpenApiInteger(1),
                ["anoPublicacao"] = new OpenApiString("2008"),
                ["autoresCodigos"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2)
                },
                ["assuntosCodigos"] = new OpenApiArray
                {
                    new OpenApiInteger(1),
                    new OpenApiInteger(2),
                    new OpenApiInteger(3)
                },
                ["formasCompra"] = new OpenApiArray
                {
                    new OpenApiObject
                    {
                        ["formaCompraCodigo"] = new OpenApiInteger(1),
                        ["valorCompra"] = new OpenApiDouble(89.90)
                    },
                    new OpenApiObject
                    {
                        ["formaCompraCodigo"] = new OpenApiInteger(2),
                        ["valorCompra"] = new OpenApiDouble(49.90)
                    }
                }
            };
        }

        // Exemplos para FormaCompraItemRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.Livros.FormaCompraItemRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["formaCompraCodigo"] = new OpenApiInteger(1),
                ["valorCompra"] = new OpenApiDouble(89.90)
            };
        }

        // Exemplos para AtualizarLivroRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.Livros.AtualizarLivroRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["titulo"] = new OpenApiString("Código Limpo - 2ª Edição"),
                ["editora"] = new OpenApiString("Prentice Hall"),
                ["edicao"] = new OpenApiInteger(2),
                ["anoPublicacao"] = new OpenApiString("2020")
            };
        }

        // Exemplos para CriarAutorRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.Autores.CriarAutorRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["nome"] = new OpenApiString("Robert C. Martin")
            };
        }

        // Exemplos para AtualizarAutorRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.Autores.AtualizarAutorRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["nome"] = new OpenApiString("Robert C. Martin")
            };
        }

        // Exemplos para CriarAssuntoRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.Assuntos.CriarAssuntoRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["descricao"] = new OpenApiString("Programação e Desenvolvimento de Software")
            };
        }

        // Exemplos para AtualizarAssuntoRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.Assuntos.AtualizarAssuntoRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["descricao"] = new OpenApiString("Programação e Desenvolvimento de Software - Arquitetura")
            };
        }

        // Exemplos para CriarFormaCompraRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.FormasCompra.CriarFormaCompraRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["descricao"] = new OpenApiString("Balcão")
            };
        }

        // Exemplos para AtualizarFormaCompraRequest
        if (type.Name == nameof(CadastroLivros.Core.DataTransferObjects.Requests.FormasCompra.AtualizarFormaCompraRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["codigo"] = new OpenApiInteger(1),
                ["descricao"] = new OpenApiString("Sef-Service")
            };
        }
    }
}

