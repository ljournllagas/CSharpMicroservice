using AutoMapper;
using BankAPI.Commands;
using BankAPI.Dto;
using BankAPI.Events;
using BankAPI.Model;
using BankAPI.Repository.IRepository;
using MediatR;
using Messaging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankAPI.Handlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountDto>
    {

        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountHandler(BankAPIDbContext dbContext, IMapper mapper, IMessagePublisher messagePublisher, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _messagePublisher = messagePublisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {

            var account = _mapper.Map<Account>(request);

            account.BranchId = _unitOfWork.Branch.GetFirstOrDefault(b => b.BranchCode == request.BranchCode).Id;

            _unitOfWork.Account.Add(account);

            if (await _unitOfWork.SaveAsync() == 0)
            {
                throw new ApplicationException();
            }

            //get account with client info
            var accountClientObj = _unitOfWork.Account.GetFirstOrDefault(a => a.Id == account.Id, "Client,Branch");

            //map obj to accountCreatedEvent
            var accountCreatedEvent = _mapper.Map<AccountCreated>(accountClientObj);

            //publish event
            await _messagePublisher.PublishMessageAsync(accountCreatedEvent.MessageType, accountCreatedEvent, "");

            return _mapper.Map<AccountDto>(accountClientObj);
        }
    }
}
