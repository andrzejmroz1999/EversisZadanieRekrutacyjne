using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void AddEmployees(List<Employee> employees)
        {
            _employeeRepository.AddRange(employees);
            _employeeRepository.Save();
        }

        public void RemoveAllEmployees()
        {
            _employeeRepository.RemoveAll();
            _employeeRepository.Save();
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.Update(employee);
            _employeeRepository.Save();
        }

        public void AddEmployee(Employee employee)
        {
            _employeeRepository.Add(employee);
            _employeeRepository.Save();
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

