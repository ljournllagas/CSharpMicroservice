using BankService.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankService.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankDbContext _db;

        public UnitOfWork(BankDbContext db)
        {
            _db = db;
            Account = new AccountRepository(_db);
        }

        public IAccountRepository Account { get; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
