using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.DAL
{
    public class YourDbContext : DbContext
    {
        public YourDbContext() : base("YourDbConnection")
        {
        }

        public DbSet<Employee> DataItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure your database mappings here
            base.OnModelCreating(modelBuilder);
        }
    }
}