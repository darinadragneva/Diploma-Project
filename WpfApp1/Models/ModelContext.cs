using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.models
{
    class ModelContext : DbContext
    {
        //public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        //{

        //}

        //public ModelContext()
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"server=localhost;database=VSK;trusted_connection=true");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<Divisions> Divisions { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Loans> Loans { get; set; }
        public DbSet<LoanStatus> LoanStatus { get; set; }
        public DbSet<Positions> Positions { get; set; }
        public DbSet<TypeOperations> TypeOperations { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
