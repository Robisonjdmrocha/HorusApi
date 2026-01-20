using HorusV2.Application.Contracts;
using HorusV2.Application.DTO.Parameters;
using HorusV2.Application.DTO.Request;
using HorusV2.Application.DTO.Response;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Application.Filters;
using HorusV2.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HorusV2.API.Controllers;

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

    /// <summary>
    ///     Obtém a lista de requisições registradas com base nos parâmetros (Suporta paginação).
    /// </summary>
    /// <remarks>
    ///     Exemplo:
    ///     Apenas pageNumber e pageSize são parâmetros obrigatórios para requisição.
    ///     Os demais são todos filtros opcionais.
    ///     O tamanho máximo da página é de 50 itens.
    ///     GET /streaming?pageNumber=1&#38;pageSize=20&#38;month=4&#38;year=2023&#38;protocolNumber=123456
    /// </remarks>
    /// <param name="searchParameters">Paginação e filtros desejados</param>
    /// <returns>A lista de requisições com base nos parâmetros</returns>
    /// <response code="200">A lista de requisições com base nos parâmetros</response>
    /// <response code="401">Caso não tenha sido informado os dados do SIGSM</response>
    /// <response code="404">Caso não encontre nenhum item com base nos parâmetros</response>
    /// <response code="422">Caso os dados informados sejam inválidos</response>
    /// <response code="500">Caso ocorra algum problema interno do servidor</response>
    /// <response code="503">Caso o serviço do Hórus externo esteja indisponível</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(SuccessResponseDTO<SearchStreamingRequestHistoryResponseDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ErrorResponseDTO))]
    public async Task<IActionResult> SearchRequests([FromQuery] SearchStreamingRequestsParameters searchParameters)
    {
        searchParameters.Route = $"{Request.Path}{Request.QueryString}";

        BaseResponseDTO response = await _streamingServices.Search(searchParameters);

        return StatusCode(Convert.ToInt32(response.StatusCode), response);
    }

    /// <summary>
    ///     Solicita uma nova transmissão.
    /// </summary>
    /// <remarks>
    ///     Exemplo:
    ///     POST /streaming
    ///     {
    ///     "day": 15,
    ///     "month": 4,
    ///     "year": 2023
    ///     }
    /// </remarks>
    /// <param name="requestDto">Paginação e filtros desejados</param>
    /// <returns>A lista de requisições com base nos parâmetros</returns>
    /// <response code="202">
    ///     Caso a transmissão seja realizada com sucesso (Processamento externo pendente - necessário
    ///     consulta futura)
    /// </response>
    /// <response code="401">Caso não tenha sido informado os dados do SIGSM</response>
    /// <response code="409">Caso o mês ou ano solicitado sejam inválidos ou se encontram sendo processado ou já transmitido</response>
    /// <response code="429">Caso já esteja ocorrendo uma transmissão neste servidor</response>
    /// <response code="500">Caso ocorra algum problema interno do servidor</response>
    /// <response code="503">Caso o serviço do Hórus externo esteja indisponível</response> 
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SuccessResponseDTO))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ErrorResponseDTO))]
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