using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Events
{
    public class AccountCreated : Event
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string AccountNumber { get; set; }
        public string BranchBranchCode { get; set; }
        public double InitialBalance { get; set; }
    }
}
