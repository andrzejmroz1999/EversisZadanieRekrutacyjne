using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.DAL
{
    public class EmployesDbContext : DbContext
    {
        public EmployesDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                // Configure your database mappings here
                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {             
                Console.WriteLine("Błąd konfiguracji modelu bazy danych: " + ex.Message);
            }
        }
    }
}