using HorusV2.Application.Contracts;
using HorusV2.Application.DTO.Parameters;
using HorusV2.Application.DTO.Request;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Application.Filters;
using HorusV2.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HorusV2.ServerlessFunctions.Controllers;

[Route("api/streamings")]
[ApiController]
[Produces("application/json")]
[ServiceFilter(typeof(BasicAuthorizationFilter))]
public class StreamingController : ControllerBase
{
    private readonly IStreamingServices _streamingServices;

    public StreamingController(IStreamingServices streamingServices)
    {
        _streamingServices = streamingServices;
    }

    [HttpGet]
    public async Task<IActionResult> SearchRequests([FromQuery] SearchStreamingRequestsParameters paginationParameters)
    {
        paginationParameters.Route = $"{Request.Path}{Request.QueryString}";

        BaseResponseDTO response = await _streamingServices.Search(paginationParameters);

        return StatusCode(Convert.ToInt32(response.StatusCode), response);
    }

    [ServiceFilter(typeof(LockMultipleRequestsFilter))]
    [ServiceFilter(typeof(StreamingRequestAuditFilter))]
    [HttpPost]
    public async Task<IActionResult> SendRequest([FromBody] StreamingRequestDto requestDto)
    {
        StreamingRequestHistory request = (Request.HttpContext.Items["request"] as StreamingRequestHistory)!;

        BaseResponseDTO response = await _streamingServices.Send(request);

        return StatusCode(Convert.ToInt32(response.StatusCode), response);
    }
}