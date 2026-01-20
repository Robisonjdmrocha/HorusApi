using HorusV2.Core.DomainObjects;
using HorusV2.Domain.Enumerators;
using System.Security.Cryptography.X509Certificates;

namespace HorusV2.Domain.Entities;

public class StreamingMovement : Entity
{
    public StreamingMovement()
    {
    }

    public StreamingMovement(int streamingRequestId, ETransmissionType transmissionType, DateTime transmissionDate,
        int horusProtocol)
    {
        StreamingRequestId = streamingRequestId;
        TransmissionType = transmissionType;
        TransmissionDate = transmissionDate;
        HorusProtocol = horusProtocol;
    }

    public StreamingMovement(Guid uniqueIdentifier, int id, int streamingRequestId, ETransmissionType transmissionType,
        DateTime transmissionDate, int horusProtocol) : base(uniqueIdentifier, id)
    {
        StreamingRequestId = streamingRequestId;
        TransmissionType = transmissionType;
        TransmissionDate = transmissionDate;
        HorusProtocol = horusProtocol;
    }

    public int StreamingRequestId { get; set; }
    public ETransmissionType TransmissionType { get; set; }
    public DateTime? TransmissionDate { get; set; }
    public int? HorusProtocol { get; set; }
    public string? JsonEnvio {get; set;}
    public string? JsonRetorno { get; set; }

    public bool Sucesso_fl { get; set; } = false;

}