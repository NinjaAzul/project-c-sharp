using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Project_C_Sharp.Shared.Swagger;

public class AddLanguageHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "lang",
            In = ParameterLocation.Query,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Enum = new System.Collections.Generic.List<IOpenApiAny>
                {
                    new OpenApiString("pt-BR"),
                    new OpenApiString("en-US")
                },
                Default = new OpenApiString("pt-BR")
            },
            Description = "Idioma da resposta (pt-BR ou en-US)"
        });
    }
}