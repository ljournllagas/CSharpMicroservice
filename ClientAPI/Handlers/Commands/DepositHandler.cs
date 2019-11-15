using AutoMapper;
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

namespace ClientAPI.Handlers.Commands
{
    public class DepositHandler : IRequestHandler<DepositCommand, DepositDto>
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepositHandler(IMessagePublisher messagePublisher, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _messagePublisher = messagePublisher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepositDto> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.AccountNumber);

            _unitOfWork.Account.UpdateAccountBalance(account, request.Amount, TransactionTypes.DEPOSIT);

            var transaction = MapTransaction(request, account);

            _unitOfWork.Transaction.Add(transaction);

            if (await _unitOfWork.SaveAsync() == 0)
            {
                throw new ApplicationException();
            }

            //map and send event to rabbitMQ
            var transactionCreatedEvent = _mapper.Map<TransactionCreated>(transaction);

            await _messagePublisher.PublishMessageAsync(transactionCreatedEvent.MessageType, transactionCreatedEvent, "");

            var depositDto = MapDepositDto(account, transaction);

            return depositDto;
        }

        #region "Mappings"
        private DepositDto MapDepositDto(Account account, Transaction transaction)
        {
            var depositDto = _mapper.Map<DepositDto>(transaction);
            depositDto.AccountNumber = account.AccountNumber;
            return depositDto;
        }

        private Transaction MapTransaction(DepositCommand request, Account account)
        {
            var transaction = _mapper.Map<Transaction>(account);
            transaction.Amount = request.Amount;
            transaction.TransactionType = TransactionTypes.DEPOSIT;
            transaction.TransactionDate = DateTime.Now;
            transaction.Remarks = (!string.IsNullOrEmpty(request.Remarks) ? request.Remarks : "");

            return transaction;
        }
        #endregion
    }
}
