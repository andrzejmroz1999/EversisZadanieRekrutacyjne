using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EversisZadanieRekrutacyjne.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly object _lockObject = new object();

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void AddEmployees(List<Employee> employees)
        {
            _employeeRepository.AddRange(employees);
            _employeeRepository.SaveAsync();
        }

        public async void RemoveAllEmployees()
        {
            await _employeeRepository.RemoveAllAsync();
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await _employeeRepository.Update(employee);
            await _employeeRepository.SaveAsync();
        }

        public void AddEmployee(Employee employee)
        {
            _employeeRepository.Add(employee);
            _employeeRepository.SaveAsync();
        }

        public void RemoveEmployee(Employee employee)
        {
            _employeeRepository.Remove(employee);
        }

        public Employee GetEmployeeById(int id)
        {
            return _employeeRepository.GetById(id);
        }
    }
}

