using HorusV2.Core.Helpers;
using HorusV2.Core.Models;
using HorusV2.Domain.Data.Relational;
using HorusV2.Domain.Entities;
using HorusV2.Domain.Enumerators;
using HorusV2.Domain.Queries.Parameters;
using HorusV2.Domain.Queries.Response;
using HorusV2.HorusIntegration.Contracts;
using HorusV2.HorusIntegration.DTO;
using HorusV2.HorusIntegration.DTO.Response;
using HorusV2.HorusIntegration.Factories;
using HorusV2.HorusIntegration.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;


namespace HorusV2.HorusIntegration.Core;

public class HorusIntegrationServices : IHorusIntegrationServices
{
    private readonly AuthenticationResponseDTO _authentication;
    private readonly HorusIntegrationSettings _horusIntegrationSettings;
    private readonly IRelationalDatabaseRepositoryManager _repositoryManager;
    private readonly RequestContextStorage<StreamingRequestHistory> _streamingRequestContext;
    private readonly RequestHandler _requestHandler;

    public HorusIntegrationServices(
        AccessTokenManager accessTokenManager,
        IOptions<HorusIntegrationSettings> horusIntegrationSettings,
        RequestContextStorage<StreamingRequestHistory> streamingRequestContext,
        IRelationalDatabaseRepositoryManager repositoryManager, 
        RequestHandler requestHandler
    )
    {
        _authentication = accessTokenManager.GetIntegrationAccess().Result;
        _horusIntegrationSettings = horusIntegrationSettings.Value;
        _streamingRequestContext = streamingRequestContext;
        _repositoryManager = repositoryManager;
        _requestHandler = requestHandler;
    }

    #region Stream
    //INTEGRAÇÃO DA DISPENSAÇÃO
    public async Task TransmitDispensingBatch(IEnumerable<DispensationByDateQueryResponse> dispensations)
    {
        try
        {
            _repositoryManager.Begin();

            IEnumerable<IGrouping<dynamic, TransmitDispensingBatchRequestDTO>> protocolGroups =
                dispensations.ConvertToIntegrationFormat();

            HttpRequestModel request = new()
            {
                BaseAddress = _horusIntegrationSettings.BaseUri,
                RequestMethod = "POST",
                RequestUri = $"/produto/ibge/{_streamingRequestContext.Get().IbgeCityCode}/dispensacao-lote/"
            };

            request.AddJwtAuthorization(_authentication.access_token);


            foreach (IGrouping<dynamic, TransmitDispensingBatchRequestDTO> protocolGroup in protocolGroups)
            {
                request.JsonContent = JsonHelper.ToJson(protocolGroup);

                JsonObject response = await HttpRequestHelper.MakeRequest<JsonObject>(request);

                bool sucesso_fl = ((int?)response["protocolo"] is not null ? true : false);

                StreamingMovement movement = new()
                {
                    TransmissionDate = (DateTime?)response["dataProtocolo"],
                    HorusProtocol = (int?)response["protocolo"],
                    UniqueIdentifier = Guid.NewGuid(),
                    TransmissionType = ETransmissionType.Dispensacao,
                    StreamingRequestId = _streamingRequestContext.Get().Id,
                    JsonEnvio = request.JsonContent,
                    JsonRetorno = response.ToString(),
                    Sucesso_fl = sucesso_fl
                };

                await _repositoryManager.StreamingMovementRepository.Add(movement);

            }

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.Processando, $"Transmitido dispensação com sucesso.");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
        catch (Exception ex)
        {
            _repositoryManager.Rollback();

            _repositoryManager.Begin();

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.ErroInterno,
                $"Falha ao transmitir dados ao hórus. Erro: {ex.Message}");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
    }

    //INTEGRAÇÃO DE ENTRADAS
    public async Task TransmitEntryBatch(IEnumerable<EntriesByDateQueryResponse> entries)
    {
        try
        {

            _repositoryManager.Begin();

            IEnumerable<IGrouping<dynamic, TransmitEntriesBatchRequestDTO>> protocolGroups =
                entries.ConvertToIntegrationFormat();

            HttpRequestModel request = new()
            {
                BaseAddress = _horusIntegrationSettings.BaseUri,
                RequestMethod = "POST",
                RequestUri = $"/produto/ibge/{_streamingRequestContext.Get().IbgeCityCode}/entrada-lote/"
            };

            request.AddJwtAuthorization(_authentication.access_token);


            foreach (IGrouping<dynamic, TransmitEntriesBatchRequestDTO> protocolGroup in protocolGroups)
            {
                request.JsonContent = JsonHelper.ToJson(protocolGroup);

                JsonObject response = await HttpRequestHelper.MakeRequest<JsonObject>(request);

                bool sucesso_fl = ((int?)response["protocolo"] is not null ? true : false);

                StreamingMovement movement = new()
                {
                    TransmissionDate = (DateTime?)response["dataProtocolo"],
                    HorusProtocol = (int?)response["protocolo"],
                    UniqueIdentifier = Guid.NewGuid(),
                    TransmissionType = ETransmissionType.Entrada,
                    StreamingRequestId = _streamingRequestContext.Get().Id,
                    JsonEnvio = request.JsonContent,
                    JsonRetorno = response.ToString(),
                    Sucesso_fl = sucesso_fl
                };

                await _repositoryManager.StreamingMovementRepository.Add(movement);
            }

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.Processando, $"Transmitido entradas com sucesso.");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
        catch (Exception ex)
        {
            _repositoryManager.Rollback();

            _repositoryManager.Begin();

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

                streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.ErroInterno,
                $"Falha ao transmitir dados ao hórus. Erro: {ex.Message}");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
    }

    //INTEGRAÇÃO DE SAIDAS
    public async Task TransmitExitBatch(IEnumerable<ExitsByDateQueryResponse> exits)
    {
        try
        {
            _repositoryManager.Begin();

            IEnumerable<IGrouping<dynamic, TransmitExitBatchRequestDTO>> protocolGroups =
                exits.ConvertToIntegrationFormat();

            HttpRequestModel request = new()
            {
                BaseAddress = _horusIntegrationSettings.BaseUri,
                RequestMethod = "POST",
                RequestUri = $"/produto/ibge/{_streamingRequestContext.Get().IbgeCityCode}/saida-lote/"
            };

            request.AddJwtAuthorization(_authentication.access_token);

            foreach (IGrouping<dynamic, TransmitExitBatchRequestDTO> protocolGroup in protocolGroups)
            {

                request.JsonContent = JsonHelper.ToJson(protocolGroup);

                JsonObject response = await HttpRequestHelper.MakeRequest<JsonObject>(request);

                bool sucesso_fl = ((int?)response["protocolo"] is not null ? true : false);

                StreamingMovement movement = new()
                {
                    TransmissionDate = (DateTime?)response["dataProtocolo"],
                    HorusProtocol = (int?)response["protocolo"],
                    UniqueIdentifier = Guid.NewGuid(),
                    TransmissionType = ETransmissionType.Saida,
                    StreamingRequestId = _streamingRequestContext.Get().Id,
                    JsonEnvio = request.JsonContent,
                    JsonRetorno = response.ToString(),
                    Sucesso_fl = sucesso_fl
                };

                await _repositoryManager.StreamingMovementRepository.Add(movement);
            }

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.Processando, $"Transmitido saídas com sucesso.");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
        catch (Exception ex)
        {
            _repositoryManager.Rollback();

            _repositoryManager.Begin();

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.ErroInterno,
                $"Falha ao transmitir dados ao hórus. Erro: {ex.Message}");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
    }

    // INTEGRAÇÃO DE POSIÇÃO DE ESTOQUE
    public async Task TransmitStockPosition(IEnumerable<StockPositionsByDateQueryResponse> stockPositions)
    {
        try
        {
            _repositoryManager.Begin();

            IEnumerable<IGrouping<dynamic, TransmitStockPositionBatchRequestDTO>> protocolGroups =
                stockPositions.ConvertToIntegrationFormat();


            HttpRequestModel request = new()
            {
                BaseAddress = _horusIntegrationSettings.BaseUri,
                RequestMethod = "POST",
                RequestUri = $"/produto/ibge/{_streamingRequestContext.Get().IbgeCityCode}/posicao-estoque-lote/"
            };

            int numberOfGroups = protocolGroups.Count();


            request.AddJwtAuthorization(_authentication.access_token);

            foreach (IGrouping<dynamic, TransmitStockPositionBatchRequestDTO> protocolGroup in protocolGroups)
            {
                request.JsonContent = JsonHelper.ToJson(protocolGroup);

                JsonObject response = await HttpRequestHelper.MakeRequest<JsonObject>(request);

                bool sucesso_fl = ((int?)response["protocolo"] is not null ? true : false);

                StreamingMovement movement = new()
                {
                    TransmissionDate = (DateTime?)response["dataProtocolo"],
                    HorusProtocol = (int?)response["protocolo"],
                    UniqueIdentifier = Guid.NewGuid(),
                    TransmissionType = ETransmissionType.PosicaoEstoque,
                    StreamingRequestId = _streamingRequestContext.Get().Id,
                    JsonEnvio = request.JsonContent,
                    JsonRetorno = response.ToString(),
                    Sucesso_fl = sucesso_fl
                };

                await _repositoryManager.StreamingMovementRepository.Add(movement);
            }

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.Processando, $"Transmitido posição de estoque com sucesso.");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
        catch (Exception ex)
        {
            _repositoryManager.Rollback();

            _repositoryManager.Begin();

            StreamingRequestHistory streamingRequestHistory = _streamingRequestContext.Get();

            streamingRequestHistory.UpdateStatusWithMessage(EStreamingRequestSituation.ErroInterno,
                $"Falha ao transmitir dados ao hórus. Erro: {ex.Message}");

            await _repositoryManager.StreamingRequestHistoryRepository.Update(streamingRequestHistory);

            _repositoryManager.Commit();
        }
    }

    #endregion

    #region Protocols

    public async Task<SearchProtocolsResponseDTO> SearchProtocols(IPaginationRequest paginationRequest)
    {
        HttpRequestModel request = new()
        {
            BaseAddress = _horusIntegrationSettings.BaseUri,
            RequestMethod = "GET",
            RequestUri = $"/protocolo/ibge/{_requestHandler.GetHeaderValue("SIGSM_IBGE_CITY_CODE")}/pesquisar{paginationRequest.Route}"
        };

        request.AddJwtAuthorization(_authentication.access_token);

        return await HttpRequestHelper.MakeRequest<SearchProtocolsResponseDTO>(request);
    }

    public async Task<ProtocolDetailsResponseDTO> GetProtocolSituation(int protocol)
    {
        HttpRequestModel request = new()
        {
            BaseAddress = _horusIntegrationSettings.BaseUri,
            RequestMethod = "GET",
            RequestUri = $"/protocolo/ibge/{_requestHandler.GetHeaderValue("SIGSM_IBGE_CITY_CODE")}/detalhar-processamento/{protocol}"
        };

        request.AddJwtAuthorization(_authentication.access_token);

        return await HttpRequestHelper.MakeRequest<ProtocolDetailsResponseDTO>(request);
    }

    public async Task<SearchProtocolInconsistenciesResponseDTO> GetProtocolInconsistencies(IPaginationRequest paginationRequest, int protocol)
    {
        HttpRequestModel request = new()
        {
            BaseAddress = _horusIntegrationSettings.BaseUri,
            RequestMethod = "GET",
            RequestUri = $"/protocolo/ibge/{_requestHandler.GetHeaderValue("SIGSM_IBGE_CITY_CODE")}/inconsistencias/{protocol}{paginationRequest.Route}"
        };

        request.AddJwtAuthorization(_authentication.access_token);

        return await HttpRequestHelper.MakeRequest<SearchProtocolInconsistenciesResponseDTO>(request);
    }
    
    #endregion
}