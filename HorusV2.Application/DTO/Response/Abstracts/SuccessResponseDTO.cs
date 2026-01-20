using System.Net;

namespace HorusV2.Application.DTO.Response.Abstracts;

public record SuccessResponseDTO : BaseResponseDTO
{
    public SuccessResponseDTO(HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
        Succeeded = true;
    }

    public override bool Succeeded { get; init; }
}

public record SuccessResponseDTO<TResponseDTO> : SuccessResponseDTO where TResponseDTO : class
{
    public SuccessResponseDTO(HttpStatusCode statusCode, string message, TResponseDTO data) : base(statusCode, message)
    {
        Data = data;
    }

    public TResponseDTO Data { get; init; }
}