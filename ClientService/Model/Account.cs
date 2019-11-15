using System;
using System.Collections.Generic;
using System.Text;

namespace ClientService.Model
{
    public class Account
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public int ClientId { get; set; }
        public string BranchCode { get; set; }
        public double Balance { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
