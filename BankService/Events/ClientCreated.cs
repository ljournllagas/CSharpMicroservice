﻿using Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankService.Events
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
