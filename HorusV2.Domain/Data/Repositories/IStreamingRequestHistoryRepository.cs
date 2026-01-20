using HorusV2.Domain.Entities;
using HorusV2.Domain.Queries.Parameters;
using HorusV2.Domain.Queries.Response;

namespace HorusV2.Domain.Data.Repositories;

public interface IStreamingRequestHistoryRepository
{
    Task Add(StreamingRequestHistory streamingRequestHistory);
    Task Update(StreamingRequestHistory streamingRequestHistory);
    Task<IEnumerable<StreamingRequestHistory>> GetByMonthAndYear(int day,int month, int year);
    Task<IEnumerable<SearchStreamingRequestsQueryResponse>> Search(SearchStreamingRequestQuery searchQuery);
}