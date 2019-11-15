using System;
using System.Collections.Generic;
using System.Text;

namespace BankService.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {

        IAccountRepository Account { get;  }

        void Save();
    }
}
