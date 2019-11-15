using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Model
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string BranchCode { get; set; }
        public int ClientCode { get; set; }
        public double Amount { get; set; }
        public double RemainingBalance { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
    }
}
