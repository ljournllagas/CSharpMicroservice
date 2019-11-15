using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Events
{
    public class AccountCreated : Event
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientEmail { get; set; }
        public string BranchBranchCode { get; set; }
        public string TypeOfAccount { get; set; }
        public string AccountNumber { get; set; }
        public double InitialBalance { get; set; }
    }
}
