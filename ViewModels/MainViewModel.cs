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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EversisZadanieRekrutacyjne.ViewModels
{

    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employes;
        public ObservableCollection<Employee> Employes
        {
            get { return _employes; }
            set
            {
                _employes = value;
                OnPropertyChanged(nameof(Employes));
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand SelectDatabaseCommand { get; }

        private readonly IDataLoader _dataLoader;
        private readonly IDatabaseSelector _databaseSelector;

        public MainViewModel(IDataLoader dataLoader, IDatabaseSelector databaseSelector)
        {
            _dataLoader = dataLoader;
            _databaseSelector = databaseSelector;

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
                Employes = new ObservableCollection<Employee>(_dataLoader.LoadDataFromCsv(filePath));
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
