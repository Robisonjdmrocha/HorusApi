namespace HorusV2.Application.DTO.Response;

public record PaginationResponseDTO
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? FirstPageLink { get; set; }
    public string? LastPageLink { get; set; }
    public string? NextPageLink { get; set; }
    public string? PreviousPageLink { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}