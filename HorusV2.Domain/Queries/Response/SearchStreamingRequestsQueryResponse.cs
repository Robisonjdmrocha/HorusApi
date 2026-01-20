using HorusV2.Domain.Enumerators;

namespace HorusV2.Domain.Queries.Response;

public class SearchStreamingRequestsQueryResponse
{
    public SearchStreamingRequestsQueryResponse()
    {
    }

    public SearchStreamingRequestsQueryResponse(Guid requestUID,
        int sigsmUserId,
        string sigsmUsername,
        DateTime requestDate,
        EStreamingRequestSituation requestSituationId,
        string requestSituation,
        string? auxiliaryMessage,
        int streamingDay,
        int streamingMonth,
        int streamingYear,
        int totalItens)
    {
        RequestUID = requestUID;
        SigsmUserId = sigsmUserId;
        SigsmUsername = sigsmUsername;
        RequestDate = requestDate;
        RequestSituationId = requestSituationId;
        RequestSituation = requestSituation;
        AuxiliaryMessage = auxiliaryMessage;
        StreamingDay = streamingDay;
        StreamingMonth = streamingMonth;
        StreamingYear = streamingYear;
        TotalItens = totalItens;
    }

    public Guid RequestUID { get; set; }
    public int SigsmUserId { get; set; }
    public string SigsmUsername { get; set; }
    public DateTime RequestDate { get; set; }
    public EStreamingRequestSituation RequestSituationId { get; set; }
    public string RequestSituation { get; set; }
    public string? AuxiliaryMessage { get; set; }
    public int StreamingDay { get; set; }
    public int StreamingMonth { get; set; }
    public int StreamingYear { get; set; }
    public int TotalItens { get; set; }
}