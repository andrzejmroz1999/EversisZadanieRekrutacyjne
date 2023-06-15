using EversisZadanieRekrutacyjne.Commands;
using EversisZadanieRekrutacyjne.DAL;
using EversisZadanieRekrutacyjne.Helpers;
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

        public MainViewModel(IDataLoader dataLoader, IDatabaseSelector databaseSelector, IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            _dataLoader = dataLoader ?? throw new ArgumentNullException(nameof(dataLoader));
            _databaseSelector = databaseSelector ?? throw new ArgumentNullException(nameof(databaseSelector));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));

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
                ShowErrorMessage("Błąd podczas edycji pracownika", ex.Message);
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
                string filePath = FileDialogHelper.GetFilePathFromUser();
                if (string.IsNullOrEmpty(filePath))
                    return;

                List<Employee> loadedEmployees = _dataLoader.LoadDataFromCsv(filePath);
                UpdateEmployeeData(loadedEmployees);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Błąd podczas ładowania danych", ex.Message);
            }
        }

        private void UpdateEmployeeData(List<Employee> employees)
        {
            try
            {
                _employeeService.RemoveAllEmployeesAsync();
                _employeeService.AddEmployeesAsync(employees);
                RefreshEmployeesCollection();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Błąd podczas aktualizacji danych pracowników", ex.Message);
            }
        }

        private async void RefreshEmployeesCollection()
        {
            try
            {
                Employees = new ObservableCollection<Employee>(await _employeeService.GetAllEmployeesAsync());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Błąd podczas odświeżania kolekcji pracowników", ex.Message);
            }
        }

        private void SelectDatabase(object parameter)
        {
            try
            {
                string connectionString = _databaseSelector.GetConnectionString();
               

                if (!string.IsNullOrEmpty(connectionString))
                {
                    ConfigureEmployeeService(connectionString);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Błąd podczas wyboru bazy danych", ex.Message);
            }
        }

        private void ConfigureEmployeeService(string connectionString)
        {
            try
            {            
                _employeeRepository = new EmployeeRepository(connectionString);
                _employeeService = new EmployeeService(_employeeRepository);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Błąd konfiguracji usługi pracowników", ex.Message);
            }
        }
        private void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }      
    }
}