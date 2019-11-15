using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Events
{
    public class TransactionCreated : Event
    {
 
        public int AccountId { get; set; }
        public double CurrentBalance { get; set; }

    }
}
