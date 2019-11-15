using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Model
{
    public class Account
    {
        private double _initBalance;

        public int Id { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public int BranchId;

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        //savings or checking
        public string TypeOfAccount { get; set; }

        //12 digits
        public string AccountNumber { get; set; }

        public double InitialBalance {

            get { return _initBalance; } 

            set { 
                _initBalance = value;
                CurrentBalance = value;
            }
        }

        public double CurrentBalance { get; set; }

    }
}
