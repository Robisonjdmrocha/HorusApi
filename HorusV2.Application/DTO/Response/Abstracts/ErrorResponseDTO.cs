using System.Net;

namespace HorusV2.Application.DTO.Response.Abstracts;

public record ErrorResponseDTO : BaseResponseDTO
{
    public ErrorResponseDTO(HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
        Succeeded = false;
    }

    public override bool Succeeded { get; init; }
}

public record ErrorResponseDTO<TResponseDTO> : ErrorResponseDTO where TResponseDTO : class
{
    public ErrorResponseDTO(HttpStatusCode statusCode, string message, TResponseDTO data) : base(statusCode, message)
    {
        Data = data;
    }

    public TResponseDTO Data { get; init; }
}