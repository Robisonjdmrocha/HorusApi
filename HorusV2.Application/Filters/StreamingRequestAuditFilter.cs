using System.Net;
using HorusV2.Application.DTO.Request;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Core.Helpers;
using HorusV2.Domain.Data.Relational;
using HorusV2.Domain.Entities;
using HorusV2.Domain.Enumerators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace HorusV2.Application.Filters;

public class StreamingRequestAuditFilter : IAsyncActionFilter
{
    private readonly IRelationalDatabaseRepositoryManager _repositoryManager;
    private readonly RequestContextStorage<StreamingRequestHistory> _streamingRequestContext;

    public StreamingRequestAuditFilter(IRelationalDatabaseRepositoryManager repositoryManager,
        RequestContextStorage<StreamingRequestHistory> streamingRequestContext)
    {
        _repositoryManager = repositoryManager;
        _streamingRequestContext = streamingRequestContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        /*
         * Auditoria para todas as requisições realizadas de transmissão ao hórus
         */
        try
        {
            int sigsmUserId = Convert.ToInt32(context.HttpContext.Items["sigsmUserId"]);
            int sigsmIbgeCityCode = Convert.ToInt32(context.HttpContext.Items["sigsmIbgeCityCode"]);

            if (context.ActionArguments.FirstOrDefault().Value is StreamingRequestDto requestDto)
            {
                StreamingRequestHistory streamingRequest = new(requestDto.Day,requestDto.Month, requestDto.Year);

                if (!streamingRequest.IsValidForStream(out string message))
                {
                    context.Result = new ConflictObjectResult(new ErrorResponseDTO(HttpStatusCode.Conflict, message));
                    return;
                }

                _repositoryManager.Begin();

                IEnumerable<StreamingRequestHistory> dateQuery =
                    await _repositoryManager.StreamingRequestHistoryRepository.GetByMonthAndYear(requestDto.Day, requestDto.Month,
                        requestDto.Year);
                if (dateQuery.Any(historyItem => historyItem.DoesSituationBlockNewRequests()))
                {
                    string conflictMessage =
                        $"Dados do dia: {requestDto.Day} do mês: {requestDto.Month} e ano: {requestDto.Year} já transmitidos ou sendo processados.";

                    context.Result =
                        new ConflictObjectResult(new ErrorResponseDTO(HttpStatusCode.Conflict, conflictMessage));

                    streamingRequest =
                        new StreamingRequestHistory(sigsmUserId, EStreamingRequestSituation.ConflitoInterno, sigsmIbgeCityCode, conflictMessage, requestDto.Day, requestDto.Month, requestDto.Year);

                    await _repositoryManager.StreamingRequestHistoryRepository.Add(streamingRequest);

                    _repositoryManager.Commit();

                    return;
                }

                streamingRequest = new StreamingRequestHistory(sigsmUserId, EStreamingRequestSituation.Processando,
                    sigsmIbgeCityCode,
                    "Solicitação recebida", requestDto.Day,requestDto.Month, requestDto.Year);

                await _repositoryManager.StreamingRequestHistoryRepository.Add(streamingRequest);

                _repositoryManager.Commit();

                _streamingRequestContext.Set(streamingRequest);

                context.HttpContext.Items.Add("request", streamingRequest);

                await next();
            }
        }
        catch (Exception ex)
        {
            _repositoryManager.Rollback();

            string requestAudit =
                $"Falha ao auditar a requisição no endpoint: {context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}.\nErro: {ex.Message}";

            Log.Error(requestAudit);

            context.Result = new ConflictObjectResult(new ErrorResponseDTO(HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor."));
            return;
        }
    }
}