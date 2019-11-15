using ClientAPI.Model;
using ClientAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly ClientAPIDbContext _db;

        public AccountRepository(ClientAPIDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateAccountBalance(Account account, double amount, string transactionType)
        {
            if (transactionType.Equals(TransactionTypes.WITHDRAWAL))
                account.Balance -= amount;
            else if(transactionType.Equals(TransactionTypes.DEPOSIT))
                account.Balance += amount;
        }
    }
}
