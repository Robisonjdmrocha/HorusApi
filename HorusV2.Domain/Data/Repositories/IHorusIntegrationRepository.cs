using HorusV2.Domain.Queries.Response;

namespace HorusV2.Domain.Data.Repositories;

public interface IHorusIntegrationRepository
{
    Task<IEnumerable<DispensationByDateQueryResponse>> GetAllDispensationsByDate(int year, int month,int day);
    Task<IEnumerable<EntriesByDateQueryResponse>> GetAllEntriesByDate(int year, int month,int day);
    Task<IEnumerable<ExitsByDateQueryResponse>> GetAllExitsByDate(int year, int month,int day);
    Task<IEnumerable<StockPositionsByDateQueryResponse>> GetAllStockPositionsByDate(int year, int month, int day);
}