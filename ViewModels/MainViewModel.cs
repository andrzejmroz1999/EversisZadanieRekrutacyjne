using EversisZadanieRekrutacyjne.Commands;
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
        private ObservableCollection<Employee> _data;
        public ObservableCollection<Employee> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public ICommand LoadCommand { get; }

        private readonly IDataLoader _dataLoader;

        public MainViewModel(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
            LoadCommand = new RelayCommand(LoadData);
        }

        private void LoadData(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                Data = new ObservableCollection<Employee>(_dataLoader.LoadDataFromCsv(filePath));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
