using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Events
{
    public class BranchCreated : Event
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string Address { get; set; }
    }
}
