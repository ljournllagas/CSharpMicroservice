using BankService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Repository.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
    }
}
