
using BankService.Model;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options)
          : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
    }
}
