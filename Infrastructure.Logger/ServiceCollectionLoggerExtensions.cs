using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Configuration.Internal;

namespace Infrastructure.Logger
{
    public static class ServiceCollectionLoggerExtensions
    {
        public static IServiceCollection AddLoggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                var serilogConfiguration = new LoggerConfiguration();

                ConfigureMSSqlServerSink(serilogConfiguration, configuration);
                ConfigureConsoleSink(serilogConfiguration, configuration);

                return serilogConfiguration.CreateLogger();
            });

            services.AddSingleton(sp =>
            {
                var serilogLogger = sp.GetService<Serilog.Core.Logger>();
                if (serilogLogger is null) throw new ArgumentNullException(nameof(serilogLogger));

                return LoggerFactory.Create(configure: (logBuilder) => logBuilder.AddSerilog(logger: serilogLogger));
            });

            return services;
        }

        static void ConfigureMSSqlServerSink(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            var connectionString = configuration["DatabaseConnectionString"];
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(nameof(connectionString));

            loggerConfiguration.WriteTo.MSSqlServer(connectionString, sinkOptions: new MSSqlServerSinkOptions()
            {
                AutoCreateSqlTable = true,
                TableName = "DefaultLogs"
            });
        }

        static void ConfigureConsoleSink(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            //TODO:...
            loggerConfiguration.WriteTo.Console();
        }
    }
}
