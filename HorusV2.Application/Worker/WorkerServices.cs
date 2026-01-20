using HorusV2.Application.Contracts;
using HorusV2.Application.DTO.Response.Abstracts;
using HorusV2.Application.Settings;
using HorusV2.Core.Helpers;
using HorusV2.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker Service iniciado com sucesso às {time}", DateTimeOffset.UtcNow);

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.Now;
            var todayAt0030 = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 30, 0, now.Offset);
            var nextRun = now <= todayAt0030 ? todayAt0030 : todayAt0030.AddDays(1);

            _logger.LogInformation("Próxima execução agendada para {nextRun}", nextRun);
            await Task.Delay(nextRun - now, stoppingToken);

            if (stoppingToken.IsCancellationRequested)
                break;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var myService = scope.ServiceProvider.GetRequiredService<MyService>();
                await myService.ProcessDailyStreaming();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar serviço agendado");
            }
        }
    }
}

public class MyService
{
    private readonly IStreamingServices _streamingServices;
    private readonly RequestContextStorage<StreamingRequestHistory> _contextStorage;
    private readonly ILogger<MyService> _logger;
    private readonly int _ibgecode;
    private readonly int _userId;
    private readonly StreamingHistoryFilter _streamingHistoryFilter;

    public MyService(
        IStreamingServices streamingServices,
        RequestContextStorage<StreamingRequestHistory> contextStorage,
        ILogger<MyService> logger,
        IOptions<HorusSettings> settings,
        StreamingHistoryFilter streamingHistoryFilter)
    {
        _streamingServices = streamingServices;
        _contextStorage = contextStorage;
        _logger = logger;
        _ibgecode = settings.Value.IbgeCode;
        _userId = settings.Value.UserId;
        _streamingHistoryFilter = streamingHistoryFilter;
    }

    public async Task ProcessDailyStreaming()
    {
        try
        {
            _logger.LogInformation("Iniciando transmissão automática diária");

            var yesterday = DateTimeOffset.Now.AddDays(-1);

            await _streamingHistoryFilter.CheckAndCreateStreamingRequest(
                yesterday.Day,
                yesterday.Month,
                yesterday.Year,
                _userId,
                _ibgecode,
                async () =>
                {
                    var streamingRequest = _contextStorage.Get();
                    var streamingResult = await _streamingServices.Send(streamingRequest);

                    if (streamingResult is SuccessResponseDTO)
                    {
                        _logger.LogInformation("Transmissão diária concluída com sucesso");
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Transmissão diária concluída com avisos: {Message}",
                            ((ErrorResponseDTO)streamingResult).Message);
                    }
                },
                new DefaultHttpContext());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar transmissão diária");
        }
    }
}
