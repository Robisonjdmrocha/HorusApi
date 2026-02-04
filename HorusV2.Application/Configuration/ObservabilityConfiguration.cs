using HorusV2.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HorusV2.Application.Configuration;

public static class ObservabilityConfiguration
{
    public static void AddApplicationLog(this IHostBuilder host, IConfiguration configuration)
    {
        LogSettings? logSettings = configuration.GetSection("LogSettings").Get<LogSettings>();

        if (logSettings is null)
            throw new ApplicationException($"Configuration file doesn't contain required key: {nameof(LogSettings)}.");

        host.UseSerilog();

        var logFilePath = (logSettings.LogTextFilePath ?? string.Empty)
            .Replace('\\', System.IO.Path.DirectorySeparatorChar)
            .Replace('/', System.IO.Path.DirectorySeparatorChar);

        var logDir = System.IO.Path.GetDirectoryName(logFilePath);
        if (!string.IsNullOrWhiteSpace(logDir))
            System.IO.Directory.CreateDirectory(logDir);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.File(
                logFilePath,
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {TraceId} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }
}
