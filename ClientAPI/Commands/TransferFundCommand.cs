using ClientAPI.Dto;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Commands
{
    public class TransferFundCommand : IRequest<TransferFundDto>
    {
        public TransferFundCommand()
        {}

        [JsonConstructor]
        public TransferFundCommand(string sourceAccount, string destinationAccount, double amount, string remarks)
        {
            SourceAccount = sourceAccount;
            DestinationAccount = destinationAccount;
            Amount = amount;
            Remarks = remarks;
        }

        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }

    }
}
