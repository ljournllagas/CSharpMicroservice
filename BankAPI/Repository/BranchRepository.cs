using BankAPI.Model;
using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Repository
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private readonly BankAPIDbContext _db;

        public BranchRepository(BankAPIDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
