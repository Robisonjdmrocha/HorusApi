using HorusV2.Core.DomainObjects;
using HorusV2.Domain.Enumerators;

namespace HorusV2.Domain.Entities;

public class StreamingRequestHistory : Entity
{
    public StreamingRequestHistory()
    {
    }

    public StreamingRequestHistory(int day,int month, int year)
    {
        StreamingDay = day;
        StreamingMonth = month;
        StreamingYear = year;
    }

    public StreamingRequestHistory(int sigsmUserId, EStreamingRequestSituation requestSituationId, int ibgeCityCode,
        string? auxiliaryMessage = null, int streamingDay = 0, int streamingMonth = 0, int streamingYear = 0)
    {
        UniqueIdentifier = Guid.NewGuid();
        SigsmUserId = sigsmUserId;
        RequestDate = DateTime.Now;
        RequestSituationId = requestSituationId;
        IbgeCityCode = ibgeCityCode;
        RequestSituation = RequestSituationId.ToString();
        AuxiliaryMessage = auxiliaryMessage;
        StreamingDay = streamingDay;
        StreamingMonth = streamingMonth;
        StreamingYear = streamingYear;
    }

    public int SigsmUserId { get; set; }
    public int IbgeCityCode { get; set; }
    public DateTime RequestDate { get; private set; }
    public DateTime? UpdateDate { get; set; }
    public EStreamingRequestSituation RequestSituationId { get; set; }
    public string RequestSituation { get; set; }
    public string? AuxiliaryMessage { get; set; }
    public int StreamingDay { get; set; }
    public int StreamingMonth { get; set; }
    public int StreamingYear { get; set; }

    private static HashSet<EStreamingRequestSituation> SituationsThatBlockNewRequests { get; } = new()
    {
        EStreamingRequestSituation.Processando,
        EStreamingRequestSituation.TransmitidoComSucesso
    };

    public bool DoesSituationBlockNewRequests()
    {
        return SituationsThatBlockNewRequests.Contains(RequestSituationId);
    }

    public bool IsValidForStream(out string message)
    {
        DateTime currentDate = DateTime.Now;
        bool requestForCurrentYear = StreamingYear == currentDate.Year;
        bool requestForCurrentMonth = requestForCurrentYear && StreamingMonth == currentDate.Month;

        // Validação básica de ano e mês
        bool isRequestValid = StreamingMonth is >= 1 and <= 12 &&
                              StreamingYear <= currentDate.Year &&
                              StreamingYear >= currentDate.Year - 100;

        // Validação de dia
        bool isDayValid = StreamingDay is >= 1 and <=31;

        // Verifica se o dia é válido para o mês e ano específicos
        int daysInMonth = DateTime.DaysInMonth(StreamingYear, StreamingMonth);
        isDayValid = isDayValid && StreamingDay <= daysInMonth;

        // Ajusta regras para o ano e mês atual
        if (requestForCurrentYear)
        {
            if (requestForCurrentMonth)
            {
                // Se for o mês atual, o dia deve ser menor que o dia atual
                isRequestValid = isRequestValid && StreamingDay < currentDate.Day;
            }
            else
            {
                // Se for o ano atual mas não o mês atual, o mês deve ser menor que o mês atual
                isRequestValid = isRequestValid && StreamingMonth < currentDate.Month;
            }
        }

        // Combinando todas as validações
        isRequestValid = isRequestValid && isDayValid;

        message = $"A data desejada para transmissão deve ser no máximo o dia anterior da data atual ({currentDate.AddDays(-1):dd/MM/yyyy}) e deve ser válida.";
        return isRequestValid;
    }

    public void UpdateStatusWithMessage(EStreamingRequestSituation requestSituation, string message)
    {
        UpdateDate = DateTime.Now;
        RequestSituation = requestSituation.ToString();
        RequestSituationId = requestSituation;
        AuxiliaryMessage = message;
    }
}