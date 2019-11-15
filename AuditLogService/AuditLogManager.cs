using AuditLogService.Config;
using AuditLogService.Model;
using Messaging.Interface;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuditLogService
{
    public class AuditLogManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        private string _logPath;
        private AuditDbContext _dbContext;

        public AuditLogManager(IMessageHandler messageHandler, AuditlogManagerConfig config, AuditDbContext dbContext)
        {
            _messageHandler = messageHandler;
            _logPath = config.LogPath;
            _dbContext = dbContext;

            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }


        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            await WriteToTextFile(messageType, message);
            await WriteToDatabase(messageType, message);

            // always akcnowledge message - any errors need to be dealt with locally.
            return true;
        }

        private async Task WriteToDatabase(string messageType, string message)
        {
            await Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to the Database. Retrying in 5 sec."); })
                .Execute(async () =>
                {
                    var auditLog = new AuditLog(eventType: messageType, message);

                    _dbContext.Add(auditLog);

                    if (await _dbContext.SaveChangesAsync() == 0)
                    {
                        throw new ApplicationException();
                    }

                });
        }

        private async Task WriteToTextFile(string messageType, string message)
        {
            string logMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff")} - {message}{Environment.NewLine}";

            string logFile = Path.Combine(_logPath, $"{DateTime.Now.ToString("yyyy-MM-dd")}-auditlog.txt");

            await File.AppendAllTextAsync(logFile, logMessage);

            Log.Information("{MessageType} - {Body}", messageType, message);
        }
    }
}
