using ClientAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Repository.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
        void UpdateAccountBalance(Account account, double amount, string transactionType);
    }
}
