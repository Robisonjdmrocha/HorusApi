namespace HorusV2.Infrastructure.Data.Repositories;

public interface IBaseRepository
{
    Task<int> ExecuteScalarAsync(string sqlCommand, object? parameters = null);
    Task ExecuteAsync(string sqlCommand, object? parameters = null);

    Task<IEnumerable<TQueryResult>> QueryAsync<TQueryResult>(string sqlQuery, object? parameters = null)
        where TQueryResult : class;

    Task<TEntity?> QueryFirstOrDefaultAsync<TEntity>(string sqlQuery, object? parameters = null) where TEntity : class;
    Task<TEntity> QuerySingleAsync<TEntity>(string sqlQuery, object? parameters = null) where TEntity : class;
}