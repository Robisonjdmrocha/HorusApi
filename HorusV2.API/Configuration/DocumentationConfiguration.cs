using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HorusV2.API.Configuration;

public static class DocumentationConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Horus Integration API", Version = "v1" });
            options.OperationFilter<BasicAuthorizationOperationFilter>();
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }

    private class BasicAuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "SIGSM_USER_ID",
                In = ParameterLocation.Header,
                Description = "Sigsm User Id",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiInteger(12)
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "SIGSM_IBGE_CITY_CODE",
                In = ParameterLocation.Header,
                Description = "Sigsm IBGE City Code",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiInteger(355410)
                }
            });
        }
    }
}