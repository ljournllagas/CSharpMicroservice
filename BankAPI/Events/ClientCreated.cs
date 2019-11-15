using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging;

namespace BankAPI.Events
{
    public class ClientCreated : Event
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

    }
}
