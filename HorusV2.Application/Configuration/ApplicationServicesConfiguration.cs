using System.Net;
using HorusV2.Application.Contracts;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Application.Services;
using HorusV2.Core.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HorusV2.Application.Configuration;

public static class ApplicationServicesConfiguration
{
    public static void AddMainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IStreamingServices, StreamingServices>();
        serviceCollection.AddScoped<IProtocolServices, ProtocolServices>();
        serviceCollection.AddScoped(typeof(RequestContextStorage<>));
        serviceCollection.AddScoped<RequestHandler>();
    }

    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                const HttpStatusCode errorCode = HttpStatusCode.InternalServerError;

                context.Response.StatusCode = (int)errorCode;
                context.Response.ContentType = "application/json";

                IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    Log.Error($"Ocorreu um problema: {contextFeature.Error}");

                    await context.Response.WriteAsJsonAsync(new ErrorResponseDTO(errorCode,
                        "Error interno do servidor."));
                }
            });
        });
    }
}