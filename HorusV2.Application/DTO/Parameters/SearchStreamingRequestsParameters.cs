using System.ComponentModel.DataAnnotations;
using HorusV2.Domain.Queries.Parameters;

namespace HorusV2.Application.DTO.Parameters;

public record SearchStreamingRequestsParameters : IPaginationRequest
{
    private int _pageSize;

    public SearchStreamingRequestsParameters()
    {
    }

    public SearchStreamingRequestsParameters(int pageSize, int pageNumber,
        int? day = null, int? month = null, int? year = null)
    {
        pageSize = pageSize;
        pageNumber = pageNumber;
        Day = day;
        Month = month;
        Year = year;
    }

    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }

    public int MaxPageSize { get; } = 50;

    [Required] public int pageNumber { get; set; }

    [Required]
    public int pageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string Route { get; set; } = string.Empty;

    public int CalculateOffset()
    {
        return (pageNumber - 1) * pageSize;
    }
}