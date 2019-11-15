using BankAPI.Model;
using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly BankAPIDbContext _db;

        public AccountRepository(BankAPIDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
