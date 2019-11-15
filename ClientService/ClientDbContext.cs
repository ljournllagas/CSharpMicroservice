using ClientService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientService
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> options)
        : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
    }
}
