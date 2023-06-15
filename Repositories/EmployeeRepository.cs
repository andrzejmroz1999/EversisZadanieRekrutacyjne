using EversisZadanieRekrutacyjne.DAL;
using EversisZadanieRekrutacyjne.Helpers;
using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EversisZadanieRekrutacyjne.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _ConnectionString;

        public EmployeeRepository(string connectionString)
        {
            _ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task AddAsync(Employee employee)
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employees = db.Set<Employee>();
                    employees.Add(employee);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employees = db.Set<Employee>();
                    return await employees.FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania pracownika o identyfikatorze " + id + ": " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task UpdateAsync(Employee employee)
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employees = db.Set<Employee>();
                    var existingEmployee = await employees.FindAsync(employee.Id);
                    if (existingEmployee != null)
                    {
                        db.Entry(existingEmployee).CurrentValues.SetValues(employee);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task DeleteAsync(Employee employee)
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employees = db.Set<Employee>();
                    employees.Remove(employee);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task RemoveAllAsync()
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employees = db.Set<Employee>();
                    employees.RemoveRange(employees);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania wszystkich pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania zmian: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task AddRangeAsync(List<Employee> employees)
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employeesDbSet = db.Set<Employee>();
                    employeesDbSet.AddRange(employees);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania kolekcji pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employees = db.Set<Employee>();
                    return await employees.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania wszystkich pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Employee>();
            }
        }

        public async Task RemoveAsync(Employee employee)
        {
            try
            {
                string decryptedConnectionString = ConnectionStringEncryptor.DecryptConnectionString(_ConnectionString);
                using (var db = new EmployesDbContext(decryptedConnectionString))
                {
                    var employees = db.Set<Employee>();
                    employees.Remove(employee);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
