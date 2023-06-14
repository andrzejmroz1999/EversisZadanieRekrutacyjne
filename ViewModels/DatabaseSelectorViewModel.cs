using EversisZadanieRekrutacyjne.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using Microsoft.Win32;

namespace EversisZadanieRekrutacyjne.ViewModels
{
    public class DatabaseSelectorViewModel : INotifyPropertyChanged
    {
        private List<string> _serverInstances;
        private string _selectedServerInstance;
        private string _username;
        private string _password;
        private string _selectedDatabase;
        private string _selectedServer;
        public string SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                _selectedServer = value;
                OnPropertyChanged(nameof(SelectedServer));

                // Po zmianie wybranego serwera, pobierz baz danych dla tego serwera
                GetDatabases(SelectedServer, Username, Password);
            }
        }
        public List<string> ServerInstances
        {
            get { return _serverInstances; }
            set
            {
                _serverInstances = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _databases;
        public ObservableCollection<string> Databases
        {
            get { return _databases; }
            set
            {
                _databases = value;
                OnPropertyChanged(nameof(Databases));
            }
        }

        public string SelectedServerInstance
        {
            get { return _selectedServerInstance; }
            set
            {
                _selectedServerInstance = value;
                OnPropertyChanged();
                LoadDatabases();
            }
        }

        public string SelectedDatabase
        {
            get { return _selectedDatabase; }
            set
            {
                _selectedDatabase = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoadServerInstancesCommand { get; }

        public DatabaseSelectorViewModel()
        {
            LoadServerInstancesCommand = new RelayCommand(LoadServerInstances);
        }

        private void LoadServerInstances(object parameter)
        {
            // Pobieranie listy dostępnych serwerów MS SQL
            ServerInstances = GetSqlServerInstances();
        }

        private void LoadDatabases()
        {
            // Pobieranie listy baz danych dla wybranego serwera MS SQL
            GetDatabases(SelectedServerInstance, Username, Password);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public List<string> GetSqlServerInstances()
        {
            List<string> instances = new List<string>();

            string ServerName = Environment.MachineName;
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        instances.Add(ServerName + "\\" + instanceName);
                    }
                }
            }

            return instances;
        }

        private void GetDatabases(string serverInstance, string username, string password)
        {
            List<string> databases = new List<string>();

            // Implementacja logiki pobierania listy baz danych dla danego serwera
            // Użyj parametrów serverInstance, username i password do nawiązania połączenia

            // Przykładowa implementacja (zakładając użycie biblioteki System.Data.SqlClient):
            using (SqlConnection connection = new SqlConnection($"Data Source={serverInstance};User ID={username};Password={password}"))
            {
                try
                {
                    connection.Open();

                    // Pobierz listę baz danych
                    DataTable databasesSchema = connection.GetSchema("Databases");
                    foreach (DataRow row in databasesSchema.Rows)
                    {
                        string databaseName = row.Field<string>("database_name");
                        databases.Add(databaseName);
                    }
                }
                catch (Exception ex)
                {
                    // Obsłuż ewentualne błędy połączenia
                    Console.WriteLine($"Error connecting to server: {ex.Message}");
                }
            }

            Databases = new ObservableCollection<string>(databases);
        }
        #endregion
    }
}