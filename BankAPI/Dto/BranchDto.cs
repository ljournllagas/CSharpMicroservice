using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Dto
{
    public class BranchDto
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string Address { get; set; }
    }
}
