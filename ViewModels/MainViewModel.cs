using EversisZadanieRekrutacyjne.Commands;
using EversisZadanieRekrutacyjne.DAL;
using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EversisZadanieRekrutacyjne.ViewModels
{

    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand SelectDatabaseCommand { get; }

        private readonly IDataLoader _dataLoader;
        private readonly IDatabaseSelector _databaseSelector;
        private readonly EmployesDbContext _dbContext;

        public MainViewModel(IDataLoader dataLoader, IDatabaseSelector databaseSelector, EmployesDbContext dbContext)
        {
            _dataLoader = dataLoader;
            _databaseSelector = databaseSelector;
            _dbContext = dbContext;

            LoadCommand = new RelayCommand(LoadData);
            SelectDatabaseCommand = new RelayCommand(SelectDatabase);
        }

        private void LoadData(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                List<Employee> loadedEmployees = _dataLoader.LoadDataFromCsv(filePath);

                // Wyczyść istniejące dane
                _dbContext.Employees.RemoveRange(_dbContext.Employees);
                _dbContext.SaveChanges();

                // Dodaj nowe dane z pliku CSV do bazy danych
                _dbContext.Employees.AddRange(loadedEmployees);
                _dbContext.SaveChanges();

                // Pobierz dane z bazy danych i przypisz do ObservableCollection
                Employees = new ObservableCollection<Employee>(_dbContext.Employees.ToList());
            }
        }

        private void SelectDatabase(object parameter)
        {
            IDatabaseSelector databaseSelector = new SqlDatabaseSelector();

            string connectionString = databaseSelector.GetConnectionString();

            if (!string.IsNullOrEmpty(connectionString))
            {
                // Wykonaj operacje związane z połączeniem do bazy danych
                // np. inicjalizuj DbContext z użyciem connectionString
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}