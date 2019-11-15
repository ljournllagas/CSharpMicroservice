using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Dto
{
    public class DepositDto
    {
        public int TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }
        public string Remarks { get; set; }
    }
}
