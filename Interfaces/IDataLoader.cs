using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Interfaces
{
    public interface IDataLoader
    {
        List<Employee> LoadDataFromCsv(string filePath);
    }
}
