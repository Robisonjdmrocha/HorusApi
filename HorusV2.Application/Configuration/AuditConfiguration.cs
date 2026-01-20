using HorusV2.Application.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HorusV2.Application.Configuration;

public static class AuditConfiguration
{
    public static void AddAuditFeature(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<StreamingRequestAuditFilter>();
        serviceCollection.AddScoped<LockMultipleRequestsFilter>();
    }
}