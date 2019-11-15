using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Model
{
    public class Branch
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string Address { get; set; }
    }
}
