using ClientAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Dto
{
    public class TransactionHistoryDto
    {
        public int ClientId { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public List<TransactionHistory> Transactions { get; set; }
    }

    public class TransactionHistory
    {
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public double RemainingBalance { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
    }
}
