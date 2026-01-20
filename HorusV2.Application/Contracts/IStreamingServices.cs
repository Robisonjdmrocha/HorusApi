using HorusV2.Application.DTO.Parameters;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Domain.Entities;

namespace HorusV2.Application.Contracts;

public interface IStreamingServices
{
    Task<BaseResponseDTO> Send(StreamingRequestHistory request);
    Task<BaseResponseDTO> Search(SearchStreamingRequestsParameters paginationParameters);
}