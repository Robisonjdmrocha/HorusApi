using HorusV2.Domain.Queries.Parameters;
using HorusV2.Domain.Queries.Response;
using HorusV2.HorusIntegration.DTO.Response;

namespace HorusV2.HorusIntegration.Contracts;

public interface IHorusIntegrationServices
{
    Task TransmitDispensingBatch(IEnumerable<DispensationByDateQueryResponse> dispensations);
    Task TransmitEntryBatch(IEnumerable<EntriesByDateQueryResponse> entries);
    Task TransmitExitBatch(IEnumerable<ExitsByDateQueryResponse> exits);
    Task TransmitStockPosition(IEnumerable<StockPositionsByDateQueryResponse> stockPositions);
    Task<SearchProtocolsResponseDTO> SearchProtocols(IPaginationRequest paginationRequest);
    Task<ProtocolDetailsResponseDTO> GetProtocolSituation(int protocol);
    Task<SearchProtocolInconsistenciesResponseDTO> GetProtocolInconsistencies(IPaginationRequest paginationRequest, int protocol);
}