using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Interfaces
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        void AddEmployees(List<Employee> employees);
        void RemoveEmployee(Employee employee);
        void RemoveAllEmployees();
        void UpdateEmployee(Employee employee);
        Employee GetEmployeeById(int id);
        List<Employee> GetAllEmployees();
    }
}
