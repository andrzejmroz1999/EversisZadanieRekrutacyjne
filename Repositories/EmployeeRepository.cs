﻿using EversisZadanieRekrutacyjne.Interfaces;
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

        public EmployeeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Employee employee)
        {
            _dbContext.Set<Employee>().Add(employee);
            _dbContext.SaveChanges();
        }

        public Employee GetById(int id)
        {
            return _dbContext.Set<Employee>().Find(id);
        }

        public void Update(Employee employee)
        {
            if (employee != null)
            {
                var existingEmployee = GetById(employee.Id);
                if (existingEmployee != null)
                {
                    _dbContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
                    _dbContext.SaveChanges();
                }
            }
        }

        public void Delete(Employee employee)
        {
            _dbContext.Set<Employee>().Remove(employee);
            _dbContext.SaveChanges();
        }
        public void RemoveAll()
        {
            _dbContext.Set<Employee>().RemoveRange(_dbContext.Set<Employee>());
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public void AddRange(List<Employee> employees)
        {
            _dbContext.Set<Employee>().AddRange(employees);
        }
        public List<Employee> GetAllEmployees()
        {
            return _dbContext.Set<Employee>().ToList();
        }

        public void Remove(Employee employee)
        {
            _dbContext.Set<Employee>().Remove(employee);
        }
    }
}
