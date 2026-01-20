using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HorusV2.HorusIntegration.DTO.Response;

public class BatchResponseDTO
{
    public BatchResponseDTO()
    {
    }

    public BatchResponseDTO(int protocolo, string codigoIbge, string usuarioEnvio, DateTime dataProtocolo,
        int situacao, int tipoServico, int tipoOperacao)
    {
        Protocolo = protocolo;
        CodigoIbge = codigoIbge;
        UsuarioEnvio = usuarioEnvio;
        DataProtocolo = dataProtocolo;
        Situacao = situacao;
        TipoServico = tipoServico;
        TipoOperacao = tipoOperacao;
    }

    public int Protocolo { get; set; }
    public string CodigoIbge { get; set; }
    public string UsuarioEnvio { get; set; }
    public DateTime DataProtocolo { get; set; }
    public int Situacao { get; set; }
    public int TipoServico { get; set; }
    public int TipoOperacao { get; set; }     
}