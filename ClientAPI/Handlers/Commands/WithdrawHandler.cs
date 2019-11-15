using ClientAPI.Commands;
using ClientAPI.Dto;
using ClientAPI.Model;
using ClientAPI.Events;
using ClientAPI.Repository.IRepository;
using MediatR;
using Messaging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace ClientAPI.Handlers.Commands
{
    public class WithdrawHandler : IRequestHandler<WithdrawCommand, WithdrawDto>
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WithdrawHandler(IMessagePublisher messagePublisher, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _messagePublisher = messagePublisher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WithdrawDto> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.AccountNumber);

            _unitOfWork.Account.UpdateAccountBalance(account, request.Amount, TransactionTypes.WITHDRAWAL);
   
            var transaction = MapTransaction(request, account);

            _unitOfWork.Transaction.Add(transaction);

            if (await _unitOfWork.SaveAsync() == 0)
            {
                throw new ApplicationException();
            }

            //map and send event to rabbitMQ
            var transactionCreatedEvent = _mapper.Map<TransactionCreated>(transaction);

            await _messagePublisher.PublishMessageAsync(transactionCreatedEvent.MessageType, transactionCreatedEvent, "");

            var withdrawDto = MapWithdrawDto(account, transaction);

            return withdrawDto;
        }

        #region "Mappings"
        private WithdrawDto MapWithdrawDto(Account account, Transaction transaction)
        {
            var withdrawDto = _mapper.Map<WithdrawDto>(transaction);
            withdrawDto.AccountNumber = account.AccountNumber;
            return withdrawDto;
        }

        private Transaction MapTransaction(WithdrawCommand request, Account account)
        {
            var transaction = _mapper.Map<Transaction>(account);
            transaction.Amount = request.Amount;
            transaction.TransactionType = TransactionTypes.WITHDRAWAL;
            transaction.TransactionDate = DateTime.Now;
            transaction.Remarks = (!string.IsNullOrEmpty(request.Remarks) ? request.Remarks : "");
          
            return transaction;
        }
        #endregion
    }
}
