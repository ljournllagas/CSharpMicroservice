using BankAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Repository.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
    }
}
