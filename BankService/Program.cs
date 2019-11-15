using BankService.Repository;
using BankService.Repository.IRepository;
using Messaging;
using Messaging.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BankService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddEnvironmentVariables("RBANK_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var sqlConnectionString = hostContext.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];

                    services.AddDbContext<BankDbContext>(options => options.UseSqlServer(sqlConnectionString));

                    //add unitofwork for repository pattern
                    services.AddScoped<IUnitOfWork, UnitOfWork>();

                    services.AddTransient<IMessageHandler>((svc) =>
                    {
                        var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string rabbitMQHost = rabbitMQConfigSection["Host"];
                        string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                        string rabbitMQPassword = rabbitMQConfigSection["Password"];
                        return new RabbitMQMessageHandler(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "RBank", "Banking", ""); ;
                    });


                    services.AddHostedService<BankManager>();
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
