using System.Text.Json.Serialization;

namespace HorusV2.HorusIntegration.DTO.Response;

public record ProtocolDetailsResponseDTO(
    IReadOnlyList<ItemProcessado> ItensProcessados,
    Processamento Processamento,
    Protocolo Protocolo
);


public record ItemProcessado(
    int CodigoBnafar,
    string CodigoOrigem,
    int PosicaoEnvio,
    int ProtocoloExclusao,
    bool Sucesso
);

public record Processamento(
    DateTime FimProcessamento,
    DateTime IncioProcessamento,
    int QuantidadeItensInconsistente,
    int QuantidadeItensSucesso,
    int QuantidadeItensTotal
);

public record Protocolo(
    string CodigoIbge,
    DateTime DataProtocolo,
    [property: JsonPropertyName("protocolo")] int NumeroProtocolo,
    int Situacao,
    int TipoOperacao,
    int TipoServico,
    string UsuarioEnvio
);