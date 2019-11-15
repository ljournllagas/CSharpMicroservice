using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankAPI.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Account { get; }

        IClientRepository Client { get; }

        IBranchRepository Branch { get; }

        void Save();

        Task<int> SaveAsync();
    }
}
