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
        Task AddEmployeeAsync(Employee employee);
        Task AddEmployeesAsync(List<Employee> employees);
        Task RemoveEmployeeAsync(Employee employee);
        Task RemoveAllEmployeesAsync();
        Task UpdateEmployeeAsync(Employee employee);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}
