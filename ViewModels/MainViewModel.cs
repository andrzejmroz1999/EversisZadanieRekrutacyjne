using EversisZadanieRekrutacyjne.Commands;
using EversisZadanieRekrutacyjne.DAL;
using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using EversisZadanieRekrutacyjne.Repositories;
using EversisZadanieRekrutacyjne.Services;
using EversisZadanieRekrutacyjne.Views;
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
using System.Windows;
using System.Windows.Input;

namespace EversisZadanieRekrutacyjne.ViewModels
{

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDataLoader _dataLoader;
        private readonly IDatabaseSelector _databaseSelector;
        private IEmployeeService _employeeService;
        private IEmployeeRepository _employeeRepository;
        private EmployesDbContext _dbContext;

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
        public ICommand EditCommand { get; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
            }
        }

        public MainViewModel(IDataLoader dataLoader, IDatabaseSelector databaseSelector, IEmployeeRepository employeeRepository, IEmployeeService employeeService, EmployesDbContext dbContext)
        {
            _dataLoader = dataLoader;
            _databaseSelector = databaseSelector;
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _dbContext = dbContext;

            LoadCommand = new RelayCommand(LoadData);
            SelectDatabaseCommand = new RelayCommand(SelectDatabase);
            EditCommand = new RelayCommand(EditEmployee, CanEditEmployee);
        }

        private void EditEmployee(object parameter)
        {
            try
            {
                EditWindow editWindow = new EditWindow(SelectedEmployee, _employeeService);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    RefreshEmployeesCollection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas edycji pracownika: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanEditEmployee(object parameter)
        {
            return SelectedEmployee != null;
        }

        private void LoadData(object parameter)
        {
            try
            {
                string filePath = GetFilePathFromUser();
                if (string.IsNullOrEmpty(filePath))
                    return;

                List<Employee> loadedEmployees = _dataLoader.LoadDataFromCsv(filePath);
                UpdateEmployeeData(loadedEmployees);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania danych: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateEmployeeData(List<Employee> employees)
        {
            try
            {
                _employeeService.RemoveAllEmployees();
                _employeeService.AddEmployees(employees);
                RefreshEmployeesCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji danych pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshEmployeesCollection()
        {
            try
            {
                Employees = new ObservableCollection<Employee>(_employeeService.GetAllEmployees());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas odświeżania kolekcji pracowników: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectDatabase(object parameter)
        {
            try
            {
                string connectionString = _databaseSelector.GetConnectionString();

                if (!string.IsNullOrEmpty(connectionString))
                {
                    _dbContext = new EmployesDbContext(connectionString);
                    _employeeRepository = new EmployeeRepository(_dbContext);
                    _employeeService = new EmployeeService(_employeeRepository);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wyboru bazy danych: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string GetFilePathFromUser()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == true)
                {
                    return openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania ścieżki pliku: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }
    }
}