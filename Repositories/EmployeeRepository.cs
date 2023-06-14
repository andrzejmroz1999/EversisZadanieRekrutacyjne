using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Employee> _employees;

        public EmployeeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _employees = _dbContext.Set<Employee>();
        }

        public void Add(Employee employee)
        {
            _employees.Add(employee);
            _dbContext.SaveChanges();
        }

        public Employee GetById(int id)
        {
            return _employees.Find(id);
        }

        public void Update(Employee employee)
        {
            var existingEmployee = GetById(employee.Id);
            if (existingEmployee != null)
            {
                _dbContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Employee employee)
        {
            _employees.Remove(employee);
            _dbContext.SaveChanges();
        }

        public void RemoveAll()
        {
            _employees.RemoveRange(_employees);
            _dbContext.SaveChanges();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void AddRange(List<Employee> employees)
        {
            _employees.AddRange(employees);
            _dbContext.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return _employees.ToList();
        }

        public void Remove(Employee employee)
        {
            _employees.Remove(employee);
        }
    }
}
