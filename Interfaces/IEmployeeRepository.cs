using EversisZadanieRekrutacyjne.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee> GetByIdAsync(int id);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);
        Task RemoveAllAsync();
        Task SaveAsync();
        Task AddRangeAsync(List<Employee> employees);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task RemoveAsync(Employee employee);
    }
}