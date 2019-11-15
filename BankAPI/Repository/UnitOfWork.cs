using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankAPIDbContext _db;

        public UnitOfWork(BankAPIDbContext db)
        {
            _db = db;
            Account = new AccountRepository(_db);
            Client = new ClientRepository(_db);
            Branch = new BranchRepository(_db);
        }

        public IAccountRepository Account { get; }
        public IClientRepository Client { get; }
        public IBranchRepository Branch { get; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
