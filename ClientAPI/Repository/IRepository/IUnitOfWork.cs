using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientAPI.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ITransactionRepository Transaction { get; }

        IAccountRepository Account { get;  }

        void Save();

        Task<int> SaveAsync();
    }
}
