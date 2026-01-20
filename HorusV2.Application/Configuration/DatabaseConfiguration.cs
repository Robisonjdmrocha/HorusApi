using HorusV2.Application.Settings;
using HorusV2.Domain.Data.Relational;
using HorusV2.Infrastructure.Data.Relational;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HorusV2.Application.Configuration;

public static class DatabaseConfiguration
{
    public static void AddRelationalDatabaseServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        RelationalDatabaseSettings? relationalDatabaseSettings =
            configuration.GetSection("RelationalDatabaseSettings").Get<RelationalDatabaseSettings>();

        if (relationalDatabaseSettings is null)
            throw new ApplicationException(
                $"Configuration file doesn't contain required key: {nameof(RelationalDatabaseSettings)}.");

        serviceCollection.AddScoped(services =>
            new RelationalDatabaseContext(relationalDatabaseSettings.ConnectionString));
        serviceCollection.AddTransient<IRelationalDatabaseRepositoryManager, RelationalDatabaseRepositoryManager>();
    }
}