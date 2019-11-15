using BankAPI.Model;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI
{
    public class BankAPIDbContext : DbContext
    {
        public BankAPIDbContext(DbContextOptions<BankAPIDbContext> options)
          : base(options)
        { }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}
