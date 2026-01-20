using System.ComponentModel.DataAnnotations;
using HorusV2.Domain.Queries.Parameters;

namespace HorusV2.Application.DTO.Parameters;

public class SearchProtocolsRequestParameters : IPaginationRequest
{
    private int _pageSize;

    public DateTime? dataFinal { get; set; }
    public DateTime? dataInicial { get; set; }
    public int? tipoOperacao { get; set; }
    public int? tipoServico { get; set; }

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