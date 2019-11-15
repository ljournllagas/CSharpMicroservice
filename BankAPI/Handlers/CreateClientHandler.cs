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
    public class CreateClientHandler : IRequestHandler<CreateClientCommand, ClientDto>
    {

        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientHandler(IMapper mapper, IMessagePublisher messagePublisher, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _messagePublisher = messagePublisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {

            var client = _mapper.Map<Client>(request);

            _unitOfWork.Client.Add(client);

            if (await _unitOfWork.SaveAsync() == 0)
            {
                throw new ApplicationException();
            }

            //map and send event to rabbitMQ
            var clientCreatedEvent = _mapper.Map<ClientCreated>(client);

            await _messagePublisher.PublishMessageAsync(clientCreatedEvent.MessageType, clientCreatedEvent, "");

            return _mapper.Map<ClientDto>(client);
        }
    }
}
