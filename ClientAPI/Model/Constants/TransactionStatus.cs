using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Model
{
    public static class TransactionStatus
    {
        public const string REACH_MAXIMUM_ALLOWABLE_AMOUNT = "REACH_MAXIMUM_ALLOWABLE_AMOUNT";
        public const string INSUFFICIENT_FUNDS = "INSUFFICIENT_FUNDS";
        public const string SUCCESS = "SUCCESS";
    }
}
