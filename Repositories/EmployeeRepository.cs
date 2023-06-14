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
        private readonly DbContext _dbContext;
        private readonly DbSet<Employee> _employees;

        public EmployeeRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _employees = _dbContext.Set<Employee>();
        }

        public void Add(Employee employee)
        {
            try
            {
                _employees.Add(employee);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Employee GetById(int id)
        {
            try
            {
                return _employees.Find(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania pracownika o identyfikatorze " + id + ": " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public void Update(Employee employee)
        {
            try
            {
                var existingEmployee = GetById(employee.Id);
                if (existingEmployee != null)
                {
                    _dbContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Delete(Employee employee)
        {
            try
            {
                _employees.Remove(employee);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveAll()
        {
            try
            {
                _employees.RemoveRange(_employees);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania wszystkich pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania zmian: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddRange(List<Employee> employees)
        {
            try
            {
                _employees.AddRange(employees);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania kolekcji pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public List<Employee> GetAllEmployees()
        {
            try
            {
                return _employees.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania wszystkich pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Employee>();
            }
        }

        public void Remove(Employee employee)
        {
            try
            {
                _employees.Remove(employee);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
