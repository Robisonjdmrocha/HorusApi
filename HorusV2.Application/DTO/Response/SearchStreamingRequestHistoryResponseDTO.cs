using HorusV2.Domain.Queries.Response;

namespace HorusV2.Application.DTO.Response;

public record SearchStreamingRequestHistoryResponseDTO
{
    public SearchStreamingRequestHistoryResponseDTO()
    {
    }

    public SearchStreamingRequestHistoryResponseDTO(PaginationResponseDTO pagination,
        IEnumerable<SearchStreamingRequestsQueryResponse> itens)
    {
        Pagination = pagination;
        Itens = itens;
    }

    public PaginationResponseDTO Pagination { get; set; }
    public IEnumerable<SearchStreamingRequestsQueryResponse> Itens { get; set; }
}