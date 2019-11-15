using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Events
{
    public class InquireBalance : Event
    {
        public int AccountId { get; set; }
        public double Balance { get; set; }
        public double TransactionAmount { get; set; }

        public string Status { get; set; }
    }
}
