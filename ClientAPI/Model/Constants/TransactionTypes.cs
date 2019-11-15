using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Model
{
    public static class TransactionTypes
    {
        public const string DEPOSIT = "DEPOSIT";
        public const string WITHDRAWAL = "WITHDRAWAL";
        public const string TRANSFERFUND = "TRANSFERFUND";
        public const string RECEIVEFUND = "RECEIVEFUND";
    }
}
