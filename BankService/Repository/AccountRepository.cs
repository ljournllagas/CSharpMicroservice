using BankService.Model;
using BankService.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly BankDbContext _db;

        public AccountRepository(BankDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
