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
    public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, BranchDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public CreateBranchHandler(IUnitOfWork unitOfWork, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<BranchDto> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = _mapper.Map<Branch>(request);

            _unitOfWork.Branch.Add(branch);

            if (await _unitOfWork.SaveAsync() == 0)
            {
                throw new ApplicationException();
            }

            //map and send event to rabbitMQ
            var branchCreatedEvent = _mapper.Map<BranchCreated>(branch);

            await _messagePublisher.PublishMessageAsync(branchCreatedEvent.MessageType, branchCreatedEvent, "");

            return _mapper.Map<BranchDto>(branch);
        }
    }
}
