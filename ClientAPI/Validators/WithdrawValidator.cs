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
    public class WithdrawValidator : AbstractValidator<WithdrawCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _messagePublisher;

        public WithdrawValidator(IUnitOfWork unitOfWork, IMessagePublisher messagePublisher)
        {
            _unitOfWork = unitOfWork;
            _messagePublisher = messagePublisher;

            RuleFor(x => x.AccountNumber)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Amount)
                .NotEmpty()
                .NotNull()
                .InclusiveBetween(1, 10000);

            RuleFor(x => x.Remarks)
               .MaximumLength(200);

            RuleFor(x => x)
               .MustAsync((a, cancellation) => HasValidAccount(a))
               .WithMessage(a => $"Your account [{a.AccountNumber}] is currently tagged as disabled.");

            RuleFor(x => x)
               .MustAsync((a, cancellation) => HasValidTransactionAmount(a))
               .WithMessage(a => $"Insufficient funds. You are not allowed to withdraw {string.Format("{0:#.00}", a.Amount)}.");

        }

        private async Task<bool> HasValidAccount(WithdrawCommand request)
        {
            var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.AccountNumber);

            var checkAccountValidityEvent = new CheckAccountValidity
            {
                AccountId = account.AccountId,
                TransactionType = TransactionTypes.WITHDRAWAL,
                Balance = account.Balance,
                Status = (account.IsValid ? AccountStatus.VALID : AccountStatus.ACCOUNT_DISABLED)
            };

            await _messagePublisher.PublishMessageAsync(checkAccountValidityEvent.MessageType, checkAccountValidityEvent, "");

            return account.IsValid;
        }

        private async Task<bool> HasValidTransactionAmount(WithdrawCommand request)
        {

            var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.AccountNumber);

            var inquireBalanceEvent = new InquireBalance
            {
                AccountId = account.AccountId,
                Balance = account.Balance,
                TransactionAmount = request.Amount,
                Status = (request.Amount <= account.Balance ? TransactionStatus.SUCCESS : TransactionStatus.INSUFFICIENT_FUNDS)
            };

            await _messagePublisher.PublishMessageAsync(inquireBalanceEvent.MessageType, inquireBalanceEvent, "");

            return request.Amount <= account.Balance;
        }
    }
}
