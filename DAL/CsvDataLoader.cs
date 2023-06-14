using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.DAL
{
    public class CsvDataLoader : IDataLoader
    {
        public ObservableCollection<Employee> LoadDataFromCsv(string filePath)
        {
            // Implementacja wczytywania danych z pliku CSV
            // Zwróć kolekcję danych wczytanych z pliku
            return new ObservableCollection<Employee>();
        }
    }
}
