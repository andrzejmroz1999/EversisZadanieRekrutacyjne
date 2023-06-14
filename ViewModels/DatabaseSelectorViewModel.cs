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
using System.Windows;
using System.Net.NetworkInformation;

namespace EversisZadanieRekrutacyjne.ViewModels
{
    public class DatabaseSelectorViewModel : INotifyPropertyChanged
    {
        public event EventHandler RequestClose;
        private List<string> _serverInstances;
        private string _selectedServerInstance;
        private string _username;
        private string _password;
        private string _selectedDatabase;
        private string _selectedServer;
        private bool _windowsAuthentication;

        public string SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                _selectedServer = value;
                OnPropertyChanged(nameof(SelectedServer));

            }
        }
        public bool CanCloseWindow()
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zamknąć okno?", "Zamknij", MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
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

        public bool WindowsAuthentication
        {
            get { return _windowsAuthentication; }
            set
            {
                _windowsAuthentication = value;
                OnPropertyChanged(nameof(WindowsAuthentication));
            }
        }

        public ICommand LoadServerInstancesCommand { get; }
        public ICommand LoadDatabasesCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand CancelCommand { get; }
        public string ConnectionString { get; private set; }

        public DatabaseSelectorViewModel()
        {
            LoadServerInstancesCommand = new RelayCommand(LoadServerInstances);
            LoadDatabasesCommand = new RelayCommand(LoadDatabases);
            ConnectCommand = new RelayCommand(Connect);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel(object obj)
        {
            SelectedServerInstance = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            SelectedDatabase = string.Empty;
            SelectedServer = string.Empty;
            WindowsAuthentication = false;
        }

        private void Connect(object obj)
        {
            ConnectionString = BuildConnectionString(SelectedServerInstance, SelectedDatabase, Username, Password, WindowsAuthentication);
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
        }
        private string BuildConnectionString(string serverInstance, string databaseName, string username, string password, bool windowsAuthentication)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (windowsAuthentication)
            {
                builder.DataSource = serverInstance;
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.DataSource = serverInstance;
                builder.UserID = username;
                builder.Password = password;
            }

            builder.InitialCatalog = databaseName;
            builder.MultipleActiveResultSets = true;

            string connectionString = builder.ConnectionString;

            // Sprawdzanie połączenia
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Połączenie udane!", "Sukces");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Błąd połączenia: " + ex.Message, "Błąd");
                }
            }

            return connectionString;
        }
        private void LoadServerInstances(object parameter)
        {
            // Pobieranie listy dostępnych serwerów MS SQL
            ServerInstances = GetSqlServerInstances();
        }

        private void LoadDatabases(object parameter)
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

        #endregion

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

            using (SqlConnection connection = GetSqlConnection(serverInstance, username, password))
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

        private SqlConnection GetSqlConnection(string serverInstance, string username, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (WindowsAuthentication)
            {
                builder.DataSource = serverInstance;
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.DataSource = serverInstance;
                builder.UserID = username;
                builder.Password = password;
            }

            return new SqlConnection(builder.ConnectionString);
        }
    }
}