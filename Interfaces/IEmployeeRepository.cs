using EversisZadanieRekrutacyjne.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace EversisZadanieRekrutacyjne.Interfaces
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        Employee GetById(int id);
        void Update(Employee employee);
        void Delete(Employee employee);
        void RemoveAll();
        void Save();
        void AddRange(List<Employee> employees);

    }
}