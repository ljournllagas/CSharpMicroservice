using ClientAPI.Model;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI
{
    public class ClientAPIDbContext : DbContext
    {
        public ClientAPIDbContext(DbContextOptions<ClientAPIDbContext> options)
        : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}
