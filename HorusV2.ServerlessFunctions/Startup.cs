using HorusV2.Application.Configuration;
using HorusV2.HorusIntegration.Configuration;

namespace HorusV2.ServerlessFunctions;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddBasicAuthorization();
        services.AddAuditFeature();
        services.AddRelationalDatabaseServices(Configuration);
        services.AddMainServices();
        services.AddHorusIntegration(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapGet("/",
                async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
        });
    }
}