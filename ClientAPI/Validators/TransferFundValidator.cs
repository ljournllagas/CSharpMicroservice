using ClientAPI.Commands;
using ClientAPI.Events;
using ClientAPI.Model;
using ClientAPI.Model.Constants;
using ClientAPI.Repository.IRepository;
using FluentValidation;
using Messaging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Validators
{
    public class TransferFundValidator : AbstractValidator<TransferFundCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _messagePublisher;
        private const double MAXIMUM_ALLOWABLE_AMOUNT_ON_ACCOUNT = 10000;


        public TransferFundValidator(IUnitOfWork unitOfWork, IMessagePublisher messagePublisher)
        {
            _unitOfWork = unitOfWork;
            _messagePublisher = messagePublisher;

            RuleFor(x => x.SourceAccount)
                .NotEmpty()
                .NotNull()
                .MustAsync((a, cancellationToken) => HasValidAccount(a))
                .WithMessage(a => $"Source Account number [{a.DestinationAccount}] is tagged as {AccountStatus.ACCOUNT_DISABLED}");

            RuleFor(x => x.DestinationAccount)
                .NotEmpty()
                .NotNull()
                .MustAsync((a, cancellationToken) => HasValidAccount(a))
                .WithMessage(a => $"Destination Account number [{a.DestinationAccount}] is tagged as {AccountStatus.ACCOUNT_DISABLED}");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .NotNull()
                .InclusiveBetween(1, 10000);

            RuleFor(x => x.Remarks)
                .MaximumLength(200);

            RuleFor(x => x)
             .MustAsync((request, cancellation) => SourceHasSufficientAmount(request))
             .WithMessage(a => $"Source account number {a.SourceAccount} has insufficient fund to proceed with this fund transfer.");

            RuleFor(x => x)
              .MustAsync((request, cancellation) => DestinationAccountNotReachItsLimit(request))
              .WithMessage(a => $"Destination account number {a.DestinationAccount} has reached its maximum allowable amount of {MAXIMUM_ALLOWABLE_AMOUNT_ON_ACCOUNT}.");
        }

        private async Task<bool> SourceHasSufficientAmount(TransferFundCommand request)
        {
            var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.SourceAccount);

            var inquireBalanceEvent = new InquireBalance
            {
                AccountId = account.AccountId,
                Balance = account.Balance,
                TransactionAmount = request.Amount,
                Status = (account.Balance >= request.Amount ? TransactionStatus.SUCCESS : TransactionStatus.INSUFFICIENT_FUNDS)
            };

            await _messagePublisher.PublishMessageAsync(inquireBalanceEvent.MessageType, inquireBalanceEvent, "");

            return account.Balance >= request.Amount;
        }

        private async Task<bool> DestinationAccountNotReachItsLimit(TransferFundCommand request)
        {
            var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.DestinationAccount);

            var inquireBalanceEvent = new InquireBalance
            {
                AccountId = account.AccountId,
                Balance = account.Balance,
                TransactionAmount = request.Amount,
                Status = (MAXIMUM_ALLOWABLE_AMOUNT_ON_ACCOUNT >= (account.Balance + request.Amount) ? TransactionStatus.SUCCESS : TransactionStatus.REACH_MAXIMUM_ALLOWABLE_AMOUNT)
            };

            await _messagePublisher.PublishMessageAsync(inquireBalanceEvent.MessageType, inquireBalanceEvent, "");

            return MAXIMUM_ALLOWABLE_AMOUNT_ON_ACCOUNT >= (account.Balance + request.Amount);
        }

        private async Task<bool> HasValidAccount(string accountNumber)
        {
            var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == accountNumber);

            var checkAccountValidityEvent = new CheckAccountValidity();
            checkAccountValidityEvent.AccountId = account.AccountId;
            checkAccountValidityEvent.TransactionType = TransactionTypes.TRANSFERFUND;
            checkAccountValidityEvent.Balance = account.Balance;
            checkAccountValidityEvent.Status = (account.IsValid ? AccountStatus.VALID : AccountStatus.ACCOUNT_DISABLED);

            await _messagePublisher.PublishMessageAsync(checkAccountValidityEvent.MessageType, checkAccountValidityEvent, "");

            return account.IsValid;
        }
    }
}
