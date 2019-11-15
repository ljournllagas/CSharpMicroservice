using BankAPI.Model;
using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly BankAPIDbContext _db;

        public ClientRepository(BankAPIDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
