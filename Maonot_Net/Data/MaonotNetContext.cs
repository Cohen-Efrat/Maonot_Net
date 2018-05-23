using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maonot_Net.Models;
using Microsoft.EntityFrameworkCore;

namespace Maonot_Net.Data
{
    public class MaonotNetContext : DbContext
    {
        public MaonotNetContext(DbContextOptions<MaonotNetContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ApprovalKit> ApprovalKits { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<FaultForm> FaultForms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<VisitorsLog> VisitorsLogs { get; set; }
        public DbSet<Warning> Warnings { get; set; }


    }
}
