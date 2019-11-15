using ClientAPI.Commands;
using ClientAPI.Dto;
using ClientAPI.Events;
using ClientAPI.Model;
using ClientAPI.Repository.IRepository;
using MediatR;
using Messaging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Handlers
{
    public class TransferFundHandler : IRequestHandler<TransferFundCommand, TransferFundDto>
    {
        private readonly ClientAPIDbContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IUnitOfWork _unitOfWork;

        public TransferFundHandler(ClientAPIDbContext dbContext, IMessagePublisher messagePublisher, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransferFundDto> Handle(TransferFundCommand request, CancellationToken cancellationToken)
        {

            var sourceAccount = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.SourceAccount);
            sourceAccount.Balance -= request.Amount;

            var destinationAccount = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.DestinationAccount);
            destinationAccount.Balance += request.Amount;

            var sourceTransaction = new Transaction
            {
                AccountId = sourceAccount.AccountId,
                BranchCode = sourceAccount.BranchCode,
                ClientCode = sourceAccount.ClientId,
                Amount = request.Amount,
                RemainingBalance = sourceAccount.Balance,
                TransactionType = TransactionTypes.TRANSFERFUND,
                TransactionDate = DateTime.Now,
                Remarks = (!string.IsNullOrEmpty(request.Remarks) ? request.Remarks : "")
            };

            var destinationTransaction = new Transaction
            {
                AccountId = destinationAccount.AccountId,
                BranchCode = destinationAccount.BranchCode,
                ClientCode = destinationAccount.ClientId,
                Amount = request.Amount,
                RemainingBalance = destinationAccount.Balance,
                TransactionType = TransactionTypes.RECEIVEFUND,
                TransactionDate = DateTime.Now,
                Remarks = (!string.IsNullOrEmpty(request.Remarks) ? request.Remarks : "")
            };

            _dbContext.Add(sourceTransaction);
            _dbContext.Add(destinationTransaction);

            if (await _dbContext.SaveChangesAsync() == 0)
            {
                throw new ApplicationException();
            }

            await ProcessTransactionCreatedEventForSourceAccount(sourceTransaction, sourceAccount);

            await ProcessTransactionCreatedEventForSourceAccount(destinationTransaction, destinationAccount);

            await ProcessTransferredFundEvent(request, sourceAccount, destinationAccount);

            var transferFundDto = new TransferFundDto
            {
                SourceAccount = sourceAccount.AccountNumber,
                DestinationAccount = destinationAccount.AccountNumber,
                Amount = request.Amount,
                Status = TransactionStatus.SUCCESS,
                Remarks = (string.IsNullOrEmpty(request.Remarks) ? "" : request.Remarks)
            };

            return transferFundDto;
        }

        private async Task ProcessTransferredFundEvent(TransferFundCommand request, Account sourceAccount, Account destinationAccount)
        {
            var transferredFundEvent = new TransferredFund
            {
                SourceAccount = sourceAccount.AccountNumber,
                DestinationAccount = destinationAccount.AccountNumber,
                Amount = request.Amount,
                Status = TransactionStatus.SUCCESS,
                Remarks = (string.IsNullOrEmpty(request.Remarks) ? "" : request.Remarks)
            };

            await _messagePublisher.PublishMessageAsync(transferredFundEvent.MessageType, transferredFundEvent, "");
        }

        private async Task ProcessTransactionCreatedEventForSourceAccount(Transaction sourceTransaction, Account sourceAccount)
        {
            var transactionCreatedEvent = new TransactionCreated
            {
                TransactionId = sourceTransaction.Id,
                AccountId = sourceTransaction.AccountId,
                Amount = sourceTransaction.Amount,
                TransactionType = sourceTransaction.TransactionType,
                Remarks = sourceTransaction.Remarks,
                CurrentBalance = sourceAccount.Balance
            };
            await _messagePublisher.PublishMessageAsync(transactionCreatedEvent.MessageType, transactionCreatedEvent, "");
        }

        private async Task ProcessTransactionCreatedEventForDestination(Transaction destinationTransaction, Account destinationAccount)
        {
            var transactionCreatedEvent = new TransactionCreated
            {
                TransactionId = destinationTransaction.Id,
                AccountId = destinationTransaction.AccountId,
                Amount = destinationTransaction.Amount,
                TransactionType = destinationTransaction.TransactionType,
                Remarks = destinationTransaction.Remarks,
                CurrentBalance = destinationAccount.Balance
            };
            await _messagePublisher.PublishMessageAsync(transactionCreatedEvent.MessageType, transactionCreatedEvent, "");
        }
    }
}
