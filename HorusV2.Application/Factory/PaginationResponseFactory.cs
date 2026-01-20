using System.Text.RegularExpressions;
using HorusV2.Application.DTO.Response;
using HorusV2.Domain.Queries.Parameters;

namespace HorusV2.Application.Factory;

public class PaginationResponseFactory
{
    public static PaginationResponseDTO Create(IPaginationRequest request, int totalRecords)
    {
        float pagesCalc = (float)totalRecords / request.pageSize;

        int totalPages = (int)Math.Ceiling(pagesCalc);

        totalPages = totalPages == 0 ? 1 : totalPages;

        bool hasNextPage = request.pageNumber < totalPages;
        bool hasPreviousPage = request.pageNumber > 1;

        string pageNumberPlaceholder = $"pageNumber={request.pageNumber}";

        PaginationResponseDTO pagionationResponse = new()
        {
            TotalRecords = totalRecords,
            PageSize = request.pageSize,
            PageNumber = request.pageNumber,
            TotalPages = totalPages,
            FirstPageLink =
                Regex.Replace(request.Route, pageNumberPlaceholder, "pageNumber=1", RegexOptions.IgnoreCase),
            LastPageLink = Regex.Replace(request.Route, pageNumberPlaceholder, $"pageNumber={totalPages}",
                RegexOptions.IgnoreCase),
            NextPageLink = hasNextPage
                ? Regex.Replace(request.Route, pageNumberPlaceholder, $"pageNumber={request.pageNumber + 1}",
                    RegexOptions.IgnoreCase)
                : null,
            PreviousPageLink = hasPreviousPage
                ? Regex.Replace(request.Route, pageNumberPlaceholder, $"pageNumber={request.pageNumber - 1}",
                    RegexOptions.IgnoreCase)
                : null
        };

        return pagionationResponse;
    }
}