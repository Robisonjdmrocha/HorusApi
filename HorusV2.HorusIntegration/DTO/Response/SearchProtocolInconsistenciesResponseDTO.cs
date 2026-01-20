namespace HorusV2.HorusIntegration.DTO.Response;
public record Content(
    string CodigoOrigem,
    IReadOnlyList<Inconsistencia> Inconsistencias,
    int PosicaoEnvio
);

public record Inconsistencia(
    string Codigo,
    string Mensagem,
    string ValorRejeitado
);

public record SearchProtocolInconsistenciesResponseDTO(
    IReadOnlyList<Content> Content,
    int NumberOfElements,
    int PageNumber,
    int PageSize,
    int TotalElements,
    int TotalPages
);

