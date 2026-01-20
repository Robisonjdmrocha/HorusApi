namespace HorusV2.HorusIntegration.DTO.Response;

public record Protocol(
    string CodigoIbge,
    DateTime DataProtocolo,
    int Protocolo,
    int Situacao,
    int TipoOperacao,
    int TipoServico,
    string UsuarioEnvio
);

public record SearchProtocolsResponseDTO(
    IReadOnlyList<Protocol> Content,
    int NumberOfElements,
    int PageNumber,
    int PageSize,
    int TotalElements,
    int TotalPages
);