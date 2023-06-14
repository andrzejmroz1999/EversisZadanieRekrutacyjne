using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Helpers
{
    public static class EmployeeFactory
    {
        public static Employee CreateEmployee(int id, string name, string surname, string email, string phone)
        {
            return new Employee
            {
                Id = id,
                Name = name,
                Surename = surname,
                Email = email,
                Phone = phone
            };
        }
    }
}
