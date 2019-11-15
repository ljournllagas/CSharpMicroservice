using AuditLogService.Config;
using Messaging;
using Messaging.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AuditLogService
{
    class Program
    {

        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            MigrateDatabase(host);

            await host.RunAsync();
        }

        private static void MigrateDatabase(IHost host)
        {
            Policy
              .Handle<Exception>()
              .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to the Database. Retrying in 5 sec."); })
              .Execute(() =>
              {
                  using var scope = host.Services.CreateScope();
                  var services = scope.ServiceProvider;
                  try
                  {
                      var context = services.GetRequiredService<AuditDbContext>();
                      context.Database.Migrate();
                  }
                  catch (Exception ex)
                  {
                      var logger = services.GetRequiredService<ILogger<Program>>();
                      logger.LogError(ex, "An error has occured during migration");
                  }
              });
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {

                    var sqlConnectionString = hostContext.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];

                    services.AddDbContext<AuditDbContext>(options => options.UseSqlServer(sqlConnectionString));

                    services.AddTransient<IMessageHandler>((svc) =>
                    {
                        var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string rabbitMQHost = rabbitMQConfigSection["Host"];
                        string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                        string rabbitMQPassword = rabbitMQConfigSection["Password"];
                        return new RabbitMQMessageHandler(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "RBank", "Auditlog", ""); ;
                    });


                    services.AddTransient<AuditlogManagerConfig>((svc) =>
                    {
                        var auditlogConfigSection = hostContext.Configuration.GetSection("Auditlog");
                        string logPath = auditlogConfigSection["path"];
                        return new AuditlogManagerConfig { LogPath = logPath };
                    });

                    services.AddHostedService<AuditLogManager>();
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();

            return hostBuilder;
        }
    }

  
}
