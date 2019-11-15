using BankAPI.Commands;
using BankAPI.Repository.IRepository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Validators
{

    public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public CreateBranchValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.BranchCode)
                .NotEmpty()
                .MaximumLength(8)
                .Must(NotExistingBranchCode)
                .WithMessage(b => $"[{b.BranchCode}] is already existing in the database.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(20);
        }

        private bool NotExistingBranchCode(string branchCode)
        {
            var validate = _unitOfWork.Branch.GetFirstOrDefault(b => b.BranchCode == branchCode);

            return (validate == null);
        }
    }
}
