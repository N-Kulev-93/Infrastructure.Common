using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;

namespace Infrastructure.Logger.Monitor
{
    public static class ServiceCollectionLogsMonitorExtensions
    {
        public static IServiceCollection AddLogsMonitorServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DatabaseConnectionString"];
            if(connectionString is null) throw new ArgumentNullException(nameof(connectionString));
            services.AddSerilogUi(builder => builder.UseSqlServer(options => options.WithConnectionString(connectionString).WithTable("DefaultLogs")));

            return services;
        }
        
        public static IApplicationBuilder UseLogsMonitorServices(this IApplicationBuilder builder)
        {
            return builder.UseSerilogUi();
        }
    }
}
