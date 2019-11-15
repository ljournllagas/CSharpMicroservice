using ClientAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClientAPIDbContext _db;

        public UnitOfWork(ClientAPIDbContext db)
        {
            _db = db;
            Transaction = new TransactionRepository(_db);
            Account = new AccountRepository(_db);
        }

        public ITransactionRepository Transaction { get; }
        public IAccountRepository Account { get; }

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
