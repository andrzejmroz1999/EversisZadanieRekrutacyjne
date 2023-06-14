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

namespace EversisZadanieRekrutacyjne.DAL
{
    public class CsvDataLoader : IDataLoader
    {
        public List<Employee> LoadDataFromCsv(string filePath)
   {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    // Pomijanie nagłówków kolumn
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] data = line.Split(',');

                        if (data.Length == 5)
                        {
                            if (int.TryParse(data[0], out int id))
                            {
                                Employee employee = new Employee
                                {
                                    Id = id,
                                    Name = data[1],
                                    Surename = data[2],
                                    Email = data[3],
                                    Phone = data[4]
                                };

                                employees.Add(employee);
                            }
                            else
                            {
                                Console.WriteLine($"Nieprawidłowy format ID w linii: {line}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Nieprawidłowy format danych w linii: {line}");
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas wczytywania pliku CSV: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
            }

            return employees;
        }
    }
}
