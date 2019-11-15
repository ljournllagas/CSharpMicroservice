using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Events
{
    public class TransactionCreated : Event
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public double Amount { get; set; }
        public double CurrentBalance { get; set; }
        public string TransactionType { get; set; }
        public string Remarks { get; set; }
    }
}
