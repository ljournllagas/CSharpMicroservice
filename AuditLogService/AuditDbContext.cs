using AuditLogService.Model;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuditLogService
{
    public class AuditDbContext : DbContext
    {
        public AuditDbContext(DbContextOptions<AuditDbContext> options)
       : base(options)
        { }

        public DbSet<AuditLog> AuditLog { get; set; }

    }
}
