using EversisZadanieRekrutacyjne.Commands;
using EversisZadanieRekrutacyjne.DAL;
using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Models;
using EversisZadanieRekrutacyjne.Repositories;
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
        private readonly IEmployeeService _employeeService;
        private readonly EmployesDbContext _dbContext;

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

        public MainViewModel(IDataLoader dataLoader, IDatabaseSelector databaseSelector, IEmployeeService employeeService, EmployesDbContext dbContext)
        {
            _dataLoader = dataLoader;
            _databaseSelector = databaseSelector;
            _employeeService = employeeService;
            _dbContext = dbContext;

            LoadCommand = new RelayCommand(LoadData);
            SelectDatabaseCommand = new RelayCommand(SelectDatabase);
            EditCommand = new RelayCommand(EditEmployee, CanEditEmployee);
        }

        private void EditEmployee(object parameter)
        {
            // Otwórz okno edycji (EditEmployeeWindow) i przekaż wybranego pracownika
            EditWindow editWindow = new EditWindow(SelectedEmployee, _employeeService);
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {              
                RefreshEmployeesCollection();
            }
        }

        private bool CanEditEmployee(object parameter)
        {
            return SelectedEmployee != null;
        }

        private void LoadData(object parameter)
        {
            string filePath = GetFilePathFromUser();
            if (string.IsNullOrEmpty(filePath))
                return;

            List<Employee> loadedEmployees = _dataLoader.LoadDataFromCsv(filePath);
            UpdateEmployeeData(loadedEmployees);
        }

        private void UpdateEmployeeData(List<Employee> employees)
        {
            _employeeService.RemoveAllEmployees();
            _employeeService.AddEmployees(employees);
            RefreshEmployeesCollection();
        }

        private void RefreshEmployeesCollection()
        {
            Employees = new ObservableCollection<Employee>(_employeeService.GetAllEmployees());
        }

        private void SelectDatabase(object parameter)
        {
            string connectionString = _databaseSelector.GetConnectionString();

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

        private string GetFilePathFromUser()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
    }
}