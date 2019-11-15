using BankAPI.Dto;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Commands
{
    public class CreateAccountCommand : IRequest<AccountDto>
    {
        public CreateAccountCommand()
        {}

        [JsonConstructor]
        public CreateAccountCommand(int clientId, string typeOfAccount, string accountNumber, double initialBalance)
        {
            ClientId = clientId;
            TypeOfAccount = typeOfAccount;
            AccountNumber = accountNumber;
            InitialBalance = initialBalance;
        }

     
        public int ClientId { get; set; }

        public string BranchCode { get; set; }

        public string TypeOfAccount { get; set; }

        public string AccountNumber { get; set; }

        public double InitialBalance { get; set; }

    }
}
