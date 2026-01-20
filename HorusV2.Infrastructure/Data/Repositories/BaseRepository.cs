using System.Data;
using Dapper;
using HorusV2.Infrastructure.Data.Relational;

namespace HorusV2.Infrastructure.Data.Repositories;

public abstract class BaseRepository : IBaseRepository
{
    protected BaseRepository(RelationalDatabaseContext relationalDatabaseContext, IDbTransaction databaseTransaction)
    {
        RelationalDatabaseContext = relationalDatabaseContext;
        DatabaseTransaction = databaseTransaction;
    }

    protected RelationalDatabaseContext RelationalDatabaseContext { get; }
    protected IDbTransaction DatabaseTransaction { get; }

    public async Task<int> ExecuteScalarAsync(string sqlCommand, object? parameters = null)
    {
        return await RelationalDatabaseContext.DatabaseConnection.ExecuteScalarAsync<int>(sqlCommand, parameters,
            DatabaseTransaction);
    }

    public async Task ExecuteAsync(string sqlCommand, object? parameters = null)
    {
        await RelationalDatabaseContext.DatabaseConnection.ExecuteAsync(sqlCommand, parameters, DatabaseTransaction);
    }


    public async Task<IEnumerable<TQueryResult>> QueryAsync<TQueryResult>(string sqlQuery, object? parameters = null)
        where TQueryResult : class
    {
        return await RelationalDatabaseContext.DatabaseConnection.QueryAsync<TQueryResult>(sqlQuery, parameters,
            DatabaseTransaction, 1200);
    }

    public async Task<TEntity?> QueryFirstOrDefaultAsync<TEntity>(string sqlQuery, object? parameters = null)
        where TEntity : class
    {
        return await RelationalDatabaseContext.DatabaseConnection.QueryFirstOrDefaultAsync<TEntity>(sqlQuery,
            parameters, DatabaseTransaction);
    }

    public async Task<TEntity> QuerySingleAsync<TEntity>(string sqlQuery, object? parameters = null)
        where TEntity : class
    {
        return await RelationalDatabaseContext.DatabaseConnection.QuerySingleAsync<TEntity>(sqlQuery, parameters,
            DatabaseTransaction);
    }
}