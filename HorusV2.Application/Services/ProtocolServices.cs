using System.Net;
using HorusV2.Application.Contracts;
using HorusV2.Application.DTO.Parameters;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.HorusIntegration.Contracts;
using HorusV2.HorusIntegration.DTO.Response;

namespace HorusV2.Application.Services;

public class ProtocolServices : IProtocolServices
{
    private readonly IHorusIntegrationServices _horusIntegrationService;

    public ProtocolServices(IHorusIntegrationServices horusIntegrationService)
    {
        _horusIntegrationService = horusIntegrationService;
    }

    public async Task<BaseResponseDTO> SearchProtocols(SearchProtocolsRequestParameters requestParameters)
    {
        try
        {
            SuccessResponseDTO<SearchProtocolsResponseDTO> response = new(HttpStatusCode.OK,
                "Protocolos listados com sucesso", await _horusIntegrationService.SearchProtocols(requestParameters));

            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponseDTO(HttpStatusCode.ServiceUnavailable, "Serviço do Hórus atualmente indisponível");
        }
    }

    public async Task<BaseResponseDTO> GetProtocolDetails(int protocol)
    {
        try
        {
            SuccessResponseDTO<ProtocolDetailsResponseDTO> response = new(HttpStatusCode.OK,
                "Detalhes do protocolo obtidos com sucesso", await _horusIntegrationService.GetProtocolSituation(protocol));

            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponseDTO(HttpStatusCode.ServiceUnavailable, "Serviço do Hórus atualmente indisponível");
        }
    }

    public async Task<BaseResponseDTO> GetProtocolInconsistencies(SearchProtocolInconsistenciesRequestParameters requestParameters, int protocol)
    {
        try
        {
            SuccessResponseDTO<SearchProtocolInconsistenciesResponseDTO> response = new(HttpStatusCode.OK,
                "Inconsistências do protocolo obtidas com sucesso", await _horusIntegrationService.GetProtocolInconsistencies(requestParameters, protocol));

            return response;
        }
        catch (Exception e)
        {
            return new ErrorResponseDTO(HttpStatusCode.ServiceUnavailable, "Serviço do Hórus atualmente indisponível");
        }
    }
}