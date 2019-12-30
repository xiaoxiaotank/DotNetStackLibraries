using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQL.TransactionIsolationLevel
{
    class MyDbContext : DbContext
    {
        public MyDbContext()
            : base(new DbContextOptionsBuilder().UseSqlServer("Data Source=.;Initial Catalog=TransactionIsolationLevel;Trusted_Connection=True;").Options)
        {
        }

        public DbSet<A> As { get; set; }

        public DbSet<B> Bs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<A>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.HasIndex(e => e.HasUpdated);
            });

            modelBuilder.Entity<B>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
