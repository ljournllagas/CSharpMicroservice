using ClientAPI.Dto;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Commands
{
    public class DepositCommand : IRequest<DepositDto>
    {
        public DepositCommand()
        {

        }

        [JsonConstructor]
        public DepositCommand(string accountNumber, double amount, string remarks)
        {
            AccountNumber = accountNumber;
            Amount = amount;
            Remarks = remarks;
        }

        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
    }
}
