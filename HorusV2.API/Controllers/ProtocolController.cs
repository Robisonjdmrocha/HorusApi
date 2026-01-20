using HorusV2.Application.Contracts;
using HorusV2.Application.DTO.Parameters;
using HorusV2.Application.DTO.Response;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Application.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HorusV2.API.Controllers;

[Route("api/protocols")]
[ApiController]
[Produces("application/json")]
[ServiceFilter(typeof(BasicAuthorizationFilter))]
public class ProtocolController : ControllerBase
{
    private readonly IProtocolServices _protocolServices;

    public ProtocolController(IProtocolServices protocolServices)
    {
        _protocolServices = protocolServices;
    }

    /// <summary>
    ///     Obtém a lista de protocolos enviados com base nos parâmetros (Suporta paginação).
    /// </summary>
    /// <remarks>
    ///     Exemplo:
    ///     Apenas pageNumber e pageSize são parâmetros obrigatórios para requisição.
    ///     Os demais são todos filtros opcionais.
    ///     O tamanho máximo da página é determinado pelo Hórus.
    ///     GET /protocols?pageNumber=1&#38;pageSize=20&#38;finalDate=4&#38;startDate=2023&#38;operationType=1&#38;
    ///     serviceType=1
    /// </remarks>
    /// <param name="searchParameters">Paginação e filtros desejados</param>
    /// <returns>A lista de requisições com base nos parâmetros</returns>
    /// <response code="200">A lista de protocolos com base nos parâmetros</response>
    /// <response code="401">Caso não tenha sido informado os dados do SIGSM</response>
    /// <response code="500">Caso ocorra algum problema interno do servidor</response>
    /// <response code="503">Caso o serviço do Hórus externo esteja indisponível</response> 
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ErrorResponseDTO))]
    public async Task<IActionResult> SearchProtocols([FromQuery] SearchProtocolsRequestParameters searchParameters)
    {
        searchParameters.Route = $"{Request.QueryString}";

        BaseResponseDTO response = await _protocolServices.SearchProtocols(searchParameters);

        return StatusCode((int) response.StatusCode, response);
    }
    
    /// <summary>
    ///     Obtém os detalhes do protocolo desejado.
    /// </summary>
    /// <remarks>
    ///     Exemplo:
    ///     GET /protocols/{protocolNumber}/details
    /// </remarks>
    /// <param name="protocol">Protocolo desejado</param>
    /// <returns>Os detalhes do protocolo desejado</returns>
    /// <response code="200">Os detalhes do protocolo desejado</response>
    /// <response code="401">Caso não tenha sido informado os dados do SIGSM</response>
    /// <response code="500">Caso ocorra algum problema interno do servidor</response>
    /// <response code="503">Caso o serviço do Hórus externo esteja indisponível</response>
    [HttpGet("{protocol:int}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ErrorResponseDTO))]
    public async Task<IActionResult> GetProtocolDetails([FromRoute] int protocol)
    {
        BaseResponseDTO response = await _protocolServices.GetProtocolDetails(protocol);

        return StatusCode((int) response.StatusCode, response);
    }
    
    /// <summary>
    ///     Obtém a lista de inconsistências (Suporta paginação).
    /// </summary>
    /// <remarks>
    ///     Exemplo:
    ///     Apenas pageNumber e pageSize são parâmetros obrigatórios para requisição.
    ///     O tamanho máximo da página é determinado pelo Hórus.
    ///     GET /protocols?pageNumber=1&#38;pageSize=20&#38;finalDate=4&#38;startDate=2023&#38;operationType=1&#38;
    ///     serviceType=1
    /// </remarks>
    /// <param name="searchParameters">Paginação e filtros desejados</param>
    /// <param name="protocol">Protocolo desejado</param>
    /// <returns>A lista de requisições com base nos parâmetros</returns>
    /// <response code="200">A lista de protocolos com base nos parâmetros</response>
    /// <response code="401">Caso não tenha sido informado os dados do SIGSM</response>
    /// <response code="500">Caso ocorra algum problema interno do servidor</response>
    /// <response code="503">Caso o serviço do Hórus externo esteja indisponível</response>
    [HttpGet("{protocol:int}/inconsistencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponseDTO))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ErrorResponseDTO))]
    public async Task<IActionResult> GetProtocolInconsistencies([FromQuery] SearchProtocolInconsistenciesRequestParameters searchParameters, [FromRoute] int protocol)
    {
        searchParameters.Route = $"{Request.QueryString}";

        BaseResponseDTO response = await _protocolServices.GetProtocolInconsistencies(searchParameters, protocol);

        return StatusCode((int) response.StatusCode, response);
    }

}