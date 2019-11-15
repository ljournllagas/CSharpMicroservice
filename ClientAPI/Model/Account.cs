using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClientAPI.Model
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public int ClientId { get; set; }
        public string BranchCode { get; set; }
        public double Balance { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
