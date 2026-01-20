using HorusV2.Application.DTO.Parameters;
using HorusV2.Application.DTO.Response.Abstracts;

namespace HorusV2.Application.Contracts;

public interface IProtocolServices
{
    Task<BaseResponseDTO> SearchProtocols(SearchProtocolsRequestParameters requestParameters);
    Task<BaseResponseDTO> GetProtocolDetails(int protocol);
    Task<BaseResponseDTO> GetProtocolInconsistencies(SearchProtocolInconsistenciesRequestParameters requestParameters, int protocol);
}