using BankService.Events;
using BankService.Repository.IRepository;
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

namespace BankService
{
    public class BankManager : IHostedService, IMessageHandlerCallback
    {
        private readonly IMessageHandler _messageHandler;
        private readonly BankDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public BankManager(IMessageHandler messageHandler, BankDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _messageHandler = messageHandler;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
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
                    case "ClientCreated":
                        Handle(messageObject.ToObject<ClientCreated>());
                        break;
                    case "BranchCreated":
                        Handle(messageObject.ToObject<BranchCreated>());
                        break;
                    case "AccountCreated":
                        Handle(messageObject.ToObject<AccountCreated>());
                        break;
                    case "TransactionCreated":
                        await Handle(messageObject.ToObject<TransactionCreated>());
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while handling {messageType} event.");
            }

            return true;
        }

        private async Task<bool> Handle(TransactionCreated transaction)
        {
            Log.Information($"Updating the balance >>> [{transaction.CurrentBalance}] of AccountId [{transaction.AccountId}]");

            var accountToBeUpdated = _unitOfWork.Account.GetFirstOrDefault(a => a.Id == transaction.AccountId);
            accountToBeUpdated.CurrentBalance = transaction.CurrentBalance;

            if (await _dbContext.SaveChangesAsync() == 0)
            {
                throw new ApplicationException();
            }

            return true;
        }

        private void Handle(ClientCreated cr)
        {
            Log.Information("Added Client: {Id}, {Name}, {Address}, {Email}",
                cr.Id, cr.FirstName + " " + cr.LastName, cr.Address, cr.Email);

            //handle ClientCreated event

        }

        private void Handle(BranchCreated br)
        {
            Log.Information("Added Branch: {Id}, {BranchCode}, {Address}",
                br.Id, br.BranchCode, br.Address);

            //handle BranchCreated event

        }

        private void Handle(AccountCreated ac)
        {
            Log.Information("Added Account: {Id}, {AccountName}, {Address}, {Email}, {TypeOfAccount}, {AccountNumber}, {InitialBalance}",
             ac.Id, ac.ClientFirstName + " " + ac.ClientLastName, ac.ClientAddress, ac.ClientEmail, ac.TypeOfAccount, ac.AccountNumber, ac.InitialBalance);

            //handle AccountCreated event
        }

    }
}
