using ClientAPI.Model;
using ClientAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly ClientAPIDbContext _db;

        public TransactionRepository(ClientAPIDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
