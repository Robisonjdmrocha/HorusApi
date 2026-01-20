using HorusV2.Application.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HorusV2.Application.Configuration;

public static class SecurityConfiguration
{
    public static void AddBasicAuthorization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<BasicAuthorizationFilter>();
    }
}