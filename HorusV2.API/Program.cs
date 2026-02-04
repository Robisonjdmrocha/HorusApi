using HorusV2.API.Configuration;
using HorusV2.Application.Configuration;
using HorusV2.Application.Contracts;
using HorusV2.Application.Services;
using HorusV2.Application.Settings;
using HorusV2.Core.Helpers;
using HorusV2.Domain.Entities;
using HorusV2.HorusIntegration.Configuration;
using Serilog;
using Serilog.Context;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

appBuilder.Configuration
    .SetBasePath(appBuilder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{appBuilder.Environment.EnvironmentName}.json", true, true)
    .AddDotEnvFile(Path.Combine(appBuilder.Environment.ContentRootPath, ".env"))
    .AddEnvironmentVariables();

appBuilder.Services.AddHostedService<Worker>();
appBuilder.Services.AddScoped<MyService>();
appBuilder.Services.AddScoped<StreamingHistoryFilter>();

appBuilder.Services.AddControllers();
// Configura o CORS usando o domínio lido do arquivo JSON
appBuilder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
appBuilder.Services.ConfigureSwagger();
appBuilder.Services.AddBasicAuthorization();
appBuilder.Services.AddAuditFeature();
appBuilder.Services.AddRelationalDatabaseServices(appBuilder.Configuration);
appBuilder.Services.AddMainServices();
appBuilder.Services.AddHorusIntegration(appBuilder.Configuration);
appBuilder.Services.AddHttpContextAccessor();
appBuilder.Host.AddApplicationLog(appBuilder.Configuration);
appBuilder.Services.Configure<HorusSettings>(appBuilder.Configuration.GetSection("HorusIntegrationSettings"));
appBuilder.Services.Configure<HostOptions>(options =>{options.ShutdownTimeout = TimeSpan.FromSeconds(30);});
appBuilder.Services.AddScoped<IStreamingServices, StreamingServices>();
appBuilder.Services.AddScoped<RequestContextStorage<StreamingRequestHistory>>();

WebApplication app = appBuilder.Build();

app.UseCors("AllowAll");
app.Use(async (context, next) =>
{
    using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
    {
        await next();
    }
});
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("./swagger/v1/swagger.json", "Horus Integration API v1");
    options.RoutePrefix = string.Empty;
});


app.ConfigureExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync("<html><body><h1>Aplicação em execução!</h1></body></html>");
});

app.MapGet("/health", () => "Aplicação está em execução!");

app.MapGet("/worker-status", (IServiceProvider serviceProvider) =>
{
    using var scope = serviceProvider.CreateScope();
    var myService = scope.ServiceProvider.GetRequiredService<MyService>();
    return "Worker Service disponível e registrado!";
});

app.Run();
