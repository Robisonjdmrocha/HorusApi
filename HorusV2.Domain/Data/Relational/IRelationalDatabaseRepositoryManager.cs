using HorusV2.Core.UnitOfWork;
using HorusV2.Domain.Data.Repositories;

namespace HorusV2.Domain.Data.Relational;

public interface IRelationalDatabaseRepositoryManager : IUnitOfWork
{
    IStreamingRequestHistoryRepository StreamingRequestHistoryRepository { get; }
    IHorusIntegrationRepository HorusIntegrationRepository { get; }
    IStreamingMovementRepository StreamingMovementRepository { get; }
}