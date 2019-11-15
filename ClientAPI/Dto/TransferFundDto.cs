using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Dto
{
    public class TransferFundDto
    {
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }
}
