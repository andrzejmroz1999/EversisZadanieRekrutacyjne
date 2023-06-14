using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EversisZadanieRekrutacyjne.DAL
{
    public class CsvDataLoader : IDataLoader
    {
        public List<Employee> LoadDataFromCsv(string filePath)
        {
            var employees = new List<Employee>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    reader.ReadLine(); // Pomijanie nagłówków kolumn

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Employee employee = ParseEmployeeFromCsvLine(line);

                        if (employee != null)
                        {
                            employees.Add(employee);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas wczytywania pliku CSV: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił nieoczekiwany błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return employees;
        }

        private Employee ParseEmployeeFromCsvLine(string line)
        {
            try
            {
                string[] data = line.Split(',');

                int expectedLength = 5; // Oczekiwana długość linii
                if (data.Length != expectedLength)
                {
                    MessageBox.Show($"Nieprawidłowy format danych w linii: {line}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                if (!int.TryParse(data[0], out int id))
                {
                    MessageBox.Show($"Nieprawidłowy format ID w linii: {line}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                return new Employee
                {
                    Id = id,
                    Name = data[1],
                    Surename = data[2],
                    Email = data[3],
                    Phone = data[4]
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas przetwarzania linii CSV: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
