using BankAPI.Commands;
using BankAPI.Repository.IRepository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Validators
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ClientId)
                .NotEmpty()
                .Must(ExistingClientId)
                .WithMessage(c => $"Invalid Client Code [{c.ClientId}]");

            RuleFor(x => x.BranchCode)
                .NotEmpty()
                .Must(ExistingBranchCode)
                 .WithMessage(b => $"Invalid Branch Code [{b.BranchCode}]");


            RuleFor(x => x.TypeOfAccount)
                .NotEmpty()
                .Must(IsValidTypeOfAccount)
                .WithMessage("Type of account must only be SAVINGS or CHECKING");

            RuleFor(x => x.AccountNumber)
                .NotEmpty()
                .MaximumLength(15)
                .MinimumLength(12)
                .Must(NotExistingAccountNumber)
                .WithMessage(x => $"[{x.AccountNumber}] has already been used to open an account.");

            RuleFor(x => x.InitialBalance)
                .NotEmpty()
                .InclusiveBetween(1, 10000);
            
        }

        private bool ExistingClientId(int clientId)
        {
            var validate = _unitOfWork.Client.GetFirstOrDefault(c => c.Id == clientId);
            return (validate != null);
        }

        private bool ExistingBranchCode(string branchCode)
        {
            var validate = _unitOfWork.Branch.GetFirstOrDefault(b => b.BranchCode == branchCode);
            return (validate != null);
        }

        private bool NotExistingAccountNumber(string accountNumber)
        {
            var validate = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == accountNumber);
            return (validate == null);
        }

        private bool IsValidTypeOfAccount(string typeOfAccount)
        {
            return (typeOfAccount.ToLower().Trim().Equals("savings") || typeOfAccount.ToLower().Trim().Equals("checking"));
        }

    }
}
