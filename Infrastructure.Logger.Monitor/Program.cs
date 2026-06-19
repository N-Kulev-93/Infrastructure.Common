using JJConsulting.Infisical.Configuration;
using OpenTelemetry.Metrics;

namespace Infrastructure.Logger.Monitor
{
    /// diff test
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var infisicalConfig = MachineIdentityInfisicalConfig
                .FromConfiguration(builder.Configuration.GetSection("Vault"));

            builder.Host.AddInfisical(infisicalConfig);

            builder.Services
                .AddLogsMonitorServices(builder.Configuration);

            builder.Services.AddOpenTelemetry()
    .WithMetrics(builder => builder.AddPrometheusExporter());

            var app = builder.Build();

            app.MapPrometheusScrapingEndpoint();
            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.UseLogsMonitorServices();

            app.MapControllers();

            app.Run();
        }
    }
}
