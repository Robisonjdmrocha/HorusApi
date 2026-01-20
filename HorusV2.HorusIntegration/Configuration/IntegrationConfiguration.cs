using HorusV2.HorusIntegration.Contracts;
using HorusV2.HorusIntegration.Core;
using HorusV2.HorusIntegration.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HorusV2.HorusIntegration.Configuration;

public static class IntegrationConfiguration
{
    public static void AddHorusIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AccessTokenManager>();
        services.AddScoped<IHorusIntegrationServices, HorusIntegrationServices>();
        services.Configure<HorusIntegrationSettings>(options =>
            configuration.GetSection("HorusIntegrationSettings").Bind(options));
    }
}