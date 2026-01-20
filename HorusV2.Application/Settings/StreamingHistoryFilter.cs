using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Core.Helpers;
using HorusV2.Domain.Data.Relational;
using HorusV2.Domain.Entities;
using HorusV2.Domain.Enumerators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HorusV2.Application.Settings;
public class StreamingHistoryFilter
{
    private readonly IRelationalDatabaseRepositoryManager _repositoryManager;
    private readonly RequestContextStorage<StreamingRequestHistory> _streamingRequestContext;

    public StreamingHistoryFilter(IRelationalDatabaseRepositoryManager repositoryManager,
        RequestContextStorage<StreamingRequestHistory> streamingRequestContext)
    {
        _repositoryManager = repositoryManager;
        _streamingRequestContext = streamingRequestContext;
    }

    public StreamingHistoryFilter(int day, int month, int year)
    {
        StreamingDay = day;
        StreamingMonth = month;
        StreamingYear = year;
    }

    public StreamingHistoryFilter(int sigsmUserId, int ibgeCityCode, int streamingDay = 0, int streamingMonth = 0, int streamingYear = 0)
    {
        SigsmUserId = sigsmUserId;
        IbgeCityCode = ibgeCityCode;
        StreamingDay = streamingDay;
        StreamingMonth = streamingMonth;
        StreamingYear = streamingYear;
    }

    public int SigsmUserId { get; set; }
    public int IbgeCityCode { get; set; }
    public int StreamingDay { get; set; }
    public int StreamingMonth { get; set; }
    public int StreamingYear { get; set; }

    public StreamingHistoryFilter()
    {
        StreamingRequestHistory streamingRequest = new(StreamingDay, StreamingMonth, StreamingYear);

        if (!streamingRequest.IsValidForStream(out string message))
        {   

            return;
        }
    }

    public async Task CheckAndCreateStreamingRequest(int day, int month, int year, int sigsmUserId, int sigsmIbgeCityCode, Func<Task> next, HttpContext context)
    {
        _repositoryManager.Begin();

        IEnumerable<StreamingRequestHistory> dateQuery =
            await _repositoryManager.StreamingRequestHistoryRepository.GetByMonthAndYear(day, month, year);

        if (dateQuery.Any(historyItem => historyItem.DoesSituationBlockNewRequests()))
        {
            string conflictMessage =
                $"Dados do dia: {day} do mês: {month} e ano: {year} já transmitidos ou sendo processados.";

            var result = new ConflictObjectResult(new ErrorResponseDTO(HttpStatusCode.Conflict, conflictMessage));

            var streamingRequest =
                new StreamingRequestHistory(sigsmUserId, EStreamingRequestSituation.ConflitoInterno, sigsmIbgeCityCode, conflictMessage, day, month, year);

            await _repositoryManager.StreamingRequestHistoryRepository.Add(streamingRequest);

            _repositoryManager.Commit();

            return;
        }

        var newStreamingRequest = new StreamingRequestHistory(sigsmUserId, EStreamingRequestSituation.Processando,
            sigsmIbgeCityCode, "Solicitação recebida", day, month, year);

        await _repositoryManager.StreamingRequestHistoryRepository.Add(newStreamingRequest);

        _repositoryManager.Commit();

        _streamingRequestContext.Set(newStreamingRequest);

        context.Items.Add("request", newStreamingRequest);

        await next();
                
    }
}