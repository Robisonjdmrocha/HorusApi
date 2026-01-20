namespace HorusV2.Domain.Queries.Parameters;

public interface IPaginationRequest
{
    public int MaxPageSize { get; }
    public int pageNumber { get; set; }

    public int pageSize { get; set; }
    public string Route { get; }

    public int CalculateOffset();
}