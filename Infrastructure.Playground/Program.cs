using dotenv.net;
using Infrastructure.Logger;
using JJConsulting.Infisical.Configuration;

namespace Infrastructure.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotEnv.Load();
            var builder = WebApplication.CreateBuilder(args);

            var infisicalConfig = MachineIdentityInfisicalConfig
                    .FromConfiguration(builder.Configuration.GetSection("Vault"));
            builder.Host.AddInfisical(infisicalConfig);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(); // Add services to the container.
            builder.Services.AddLoggerServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
