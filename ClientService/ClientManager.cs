using ClientService.Events;
using ClientService.Model;
using Messaging.Interface;
using Messaging.Utility;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientService
{
    public class ClientManager : IHostedService, IMessageHandlerCallback
    {
        private readonly IMessageHandler _messageHandler;
        private readonly ClientDbContext _dbContext;

        public ClientManager(IMessageHandler messageHandler, ClientDbContext dbContext)
        {
            _messageHandler = messageHandler;
            _dbContext = dbContext;
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
            try
            {
                JObject messageObject = MessageSerializer.Deserialize(message);

                switch (messageType)
                {
                    case "AccountCreated":
                        await Handle(messageObject.ToObject<AccountCreated>());
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while handling {messageType} event.");
            }

            return true;
        }

        private async Task<bool> Handle(AccountCreated ac)
        {
            Log.Information("Added Account: {ClientId}, {BranchCode}, {InitialBalance}",
             ac.ClientId, ac.BranchBranchCode, ac.InitialBalance);

            var account = new Account();
            account.AccountId = ac.Id;
            account.ClientId = ac.ClientId;
            account.BranchCode = ac.BranchBranchCode;
            account.Balance = ac.InitialBalance;
            account.AccountNumber = ac.AccountNumber;
           
            _dbContext.Add(account);

            if (await _dbContext.SaveChangesAsync() == 0)
            {
                throw new ApplicationException();
            }

            return true;
        }
    }
}
