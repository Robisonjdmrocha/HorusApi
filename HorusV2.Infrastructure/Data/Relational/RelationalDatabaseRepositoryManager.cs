using System.Data;
using HorusV2.Core.Helpers;
using HorusV2.Domain.Data.Relational;
using HorusV2.Domain.Data.Repositories;
using HorusV2.Infrastructure.Data.Repositories;
using Serilog;

namespace HorusV2.Infrastructure.Data.Relational;

public class RelationalDatabaseRepositoryManager : IRelationalDatabaseRepositoryManager
{
    private readonly RelationalDatabaseContext _relationalDatabaseContext;
    private IDbTransaction _dbTransaction;
    private IHorusIntegrationRepository _horusIntegrationRepository;
    private IStreamingMovementRepository _streamingMovementRepository;
    private IStreamingRequestHistoryRepository _streamingRequestHistoryRepository;

    public RelationalDatabaseRepositoryManager(RelationalDatabaseContext relationalDatabaseContext)
    {
        _relationalDatabaseContext = relationalDatabaseContext;
    }

    public void Begin()
    {
        try
        {
            _relationalDatabaseContext.DatabaseConnection.Open();
            _dbTransaction = _relationalDatabaseContext.DatabaseConnection.BeginTransaction();
            ResetRepositories();
        }
        catch (Exception ex)
        {
            Log.Error($"Erro ao tentar abrir conexão. Erro: {ex.Message} \nConnectionString: {_relationalDatabaseContext.DatabaseConnection.ConnectionString}\n\n");
        };
    }

    public void Commit()
    {
        try
        {
            _dbTransaction?.Commit();
            _relationalDatabaseContext?.DatabaseConnection.Close();
            ResetRepositories();
        } 
        catch (Exception ex)
        {
            Log.Error($"Erro ao tentar comitar transação. Erro: {ex.Message} \nConnectionString: {_relationalDatabaseContext.DatabaseConnection.ConnectionString}\n\n");
        }; 
       

    }

    public void Rollback()
    {
        try
        {
            _dbTransaction?.Rollback();
            _relationalDatabaseContext?.DatabaseConnection.Close();
            ResetRepositories();

        }
        catch (Exception ex)
        {
            Log.Error($"Erro ao tentar dar rollback. Erro: {ex.Message} \nConnectionString: {_relationalDatabaseContext.DatabaseConnection.ConnectionString}\n\n");
        };

    }

    private void ResetRepositories()
    {
        _horusIntegrationRepository = null;
        _streamingMovementRepository = null;
        _streamingRequestHistoryRepository = null;
    }

    public IStreamingRequestHistoryRepository StreamingRequestHistoryRepository =>
        _streamingRequestHistoryRepository ??=
            new StreamingRequestHistoryRepository(_relationalDatabaseContext, _dbTransaction);

    public IHorusIntegrationRepository HorusIntegrationRepository => _horusIntegrationRepository ??=
        new HorusIntegrationRepository(_relationalDatabaseContext, _dbTransaction);

    public IStreamingMovementRepository StreamingMovementRepository => _streamingMovementRepository ??=
        new StreamingMovementRepository(_relationalDatabaseContext, _dbTransaction);
}