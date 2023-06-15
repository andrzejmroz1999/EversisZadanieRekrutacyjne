using EversisZadanieRekrutacyjne.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Interfaces
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        Employee GetById(int id);
        Task Update(Employee employee);
        void Delete(Employee employee);
        Task RemoveAllAsync();
        Task SaveAsync();
        void AddRange(List<Employee> employees);
        List<Employee> GetAllEmployees();
        void Remove(Employee employee);
    }
}