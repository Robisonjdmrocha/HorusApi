using System.Net;

namespace HorusV2.Application.DTO.Response.Abstracts;

public abstract record BaseResponseDTO
{
    protected BaseResponseDTO(HttpStatusCode statusCode,
        string message)
    {
        StatusCode = statusCode;
        Message = message;
        ResponseTime = DateTime.Now;
    }

    public HttpStatusCode StatusCode { get; init; }
    public string Message { get; init; }
    public DateTime ResponseTime { get; init; }
    public abstract bool Succeeded { get; init; }
}