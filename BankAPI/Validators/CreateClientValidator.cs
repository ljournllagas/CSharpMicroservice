using BankAPI.Commands;
using BankAPI.Repository.IRepository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Validators
{
    public class CreateClientValidator : AbstractValidator<CreateClientCommand>
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress()
                .Must(NotExistingEmailAddress)
                .WithMessage(e => $"[{e.Email}] already exists in the database");
        }

        private bool NotExistingEmailAddress(string emailAddress)
        {
            var validate = _unitOfWork.Client.GetFirstOrDefault(c => c.Email == emailAddress);

            return (validate == null);
        }
    }
}
