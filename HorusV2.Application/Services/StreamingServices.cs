using System.Linq.Expressions;
using System.Net;
using HorusV2.Application.Contracts;
using HorusV2.Application.DTO.Parameters;
using HorusV2.Application.DTO.Response;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Application.Factory;
using HorusV2.Core.Helpers;
using HorusV2.Domain.Data.Relational;
using HorusV2.Domain.Entities;
using HorusV2.Domain.Enumerators;
using HorusV2.Domain.Queries.Parameters;
using HorusV2.Domain.Queries.Response;
using HorusV2.HorusIntegration.Contracts;
using Serilog;

namespace HorusV2.Application.Services;

public class StreamingServices : IStreamingServices
{
    private readonly IHorusIntegrationServices _horusIntegrationService;
    private readonly IRelationalDatabaseRepositoryManager _repositoryManager;
    private readonly RequestContextStorage<StreamingRequestHistory> _streamingRequestContext;

    public StreamingServices(IRelationalDatabaseRepositoryManager repositoryManager,
        IHorusIntegrationServices horusIntegrationServices,
        RequestContextStorage<StreamingRequestHistory> streamingRequestContext)
    {
        _repositoryManager = repositoryManager;
        _horusIntegrationService = horusIntegrationServices;
        _streamingRequestContext = streamingRequestContext;
    }

    public async Task<BaseResponseDTO> Search(SearchStreamingRequestsParameters paginationParameters)
    {
        try
        {
            if (paginationParameters.pageSize <= 0 || paginationParameters.pageNumber <= 0)
                return new ErrorResponseDTO(HttpStatusCode.UnprocessableEntity,
                    "O número da página e o tamanho da página devem ser maiores do que zero.");

            SearchStreamingRequestQuery query = new(paginationParameters.Day, 
                paginationParameters.Month,
                paginationParameters.Year,
                paginationParameters.CalculateOffset(),
                paginationParameters.pageSize);

            IEnumerable<SearchStreamingRequestsQueryResponse> searchResponse =
                await _repositoryManager.StreamingRequestHistoryRepository.Search(query);

            searchResponse = searchResponse.ToArray();

            int totalRecords = searchResponse.Any() ? searchResponse.First().TotalItens : 0;

            if (totalRecords <= 0)
                return new ErrorResponseDTO(HttpStatusCode.NotFound,
                    "Não existem registros de acordo com o filtro solicitado.");

            PaginationResponseDTO paginationResponse =
                PaginationResponseFactory.Create(paginationParameters, totalRecords);

            SearchStreamingRequestHistoryResponseDTO responseDto = new()
            {
                Itens = searchResponse,
                Pagination = paginationResponse
            };

            return new SuccessResponseDTO<SearchStreamingRequestHistoryResponseDTO>(HttpStatusCode.OK,
                "Histórico obtido com sucesso.",
                responseDto);
        }
        catch (Exception ex)
        {
            Log.Error(
                $"Erro ao tentar consultar transmissões realizadas. Solicitação: {JsonHelper.ToJson(paginationParameters)} \n\n Erro: {ex.Message}");

            return new ErrorResponseDTO(HttpStatusCode.InternalServerError,
                "Não foi possível processar sua solicitação. Por favor tente novamente mais tarde");
        }
    }

    public async Task<BaseResponseDTO> Send(StreamingRequestHistory request)
    {
        try
        {
            //IEnumerable<DispensationByDateQueryResponse> dispensations = Enumerable.Empty<DispensationByDateQueryResponse>();
            //IEnumerable<EntriesByDateQueryResponse> entries = Enumerable.Empty<EntriesByDateQueryResponse>();
            IEnumerable<ExitsByDateQueryResponse> exits = Enumerable.Empty<ExitsByDateQueryResponse>();
            IEnumerable<StockPositionsByDateQueryResponse> stockPositions = Enumerable.Empty<StockPositionsByDateQueryResponse>();
            
            //dispensations = await _repositoryManager.HorusIntegrationRepository.GetAllDispensationsByDate(request.StreamingYear, request.StreamingMonth, request.StreamingDay);
            //entries = await _repositoryManager.HorusIntegrationRepository.GetAllEntriesByDate(request.StreamingYear, request.StreamingMonth, request.StreamingDay);
            exits = await _repositoryManager.HorusIntegrationRepository.GetAllExitsByDate(request.StreamingYear, request.StreamingMonth, request.StreamingDay);
            stockPositions = await _repositoryManager.HorusIntegrationRepository.GetAllStockPositionsByDate(request.StreamingYear, request.StreamingMonth, request.StreamingDay);

            //await _horusIntegrationService.TransmitEntryBatch(entries);            
            //await _horusIntegrationService.TransmitDispensingBatch(dispensations);
            await _horusIntegrationService.TransmitExitBatch(exits);
            await _horusIntegrationService.TransmitStockPosition(stockPositions);


            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.TransmitidoComSucesso, $"Transmitido com sucesso.");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();

            return new SuccessResponseDTO(HttpStatusCode.Accepted, "Transmissão realizada com sucesso");
        }
        catch (Exception ex)
        {
            Log.Error(
                $"Erro ao tentar transmitir dados. Solicitação: {JsonHelper.ToJson(request)} \n\n Erro: {ex.Message}");
            
            _repositoryManager.Begin();

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.ErroInterno,
                $"Falha ao transmitir dados ao hórus. Erro: {ex.Message}");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
            
            return new ErrorResponseDTO(HttpStatusCode.InternalServerError,
                "Não foi possível processar sua solicitação. Por favor tente novamente mais tarde");
        }
    }
}