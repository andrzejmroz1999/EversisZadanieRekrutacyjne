using Autofac;
using Autofac.Core;
using EversisZadanieRekrutacyjne.DAL;
using EversisZadanieRekrutacyjne.Helpers;
using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Repositories;
using EversisZadanieRekrutacyjne.Services;
using EversisZadanieRekrutacyjne.ViewModels;
using EversisZadanieRekrutacyjne.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace EversisZadanieRekrutacyjne
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                //Pobranie oraz zaszyfrowanie connection strin z app.config    
                string connectionString = ConnectionStringEncryptor.EncryptConnectionString(ConfigurationManager.ConnectionStrings["EmployesConnectionString"].ConnectionString);

                CreateDatabase(connectionString);
                ConfigureMainWindow(connectionString);
            }
            catch (Exception ex)
            {
                // Obsługa ogólnych błędów aplikacji
                MessageBox.Show("Wystąpił błąd podczas uruchamiania aplikacji: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void CreateDatabase(string connectionString)
        {
            DatabaseCreator databaseCreator = new DatabaseCreator(connectionString);
            databaseCreator.CreateDatabase();
        }

        private void ConfigureMainWindow(string connectionString)
        {
            try
            {        
                           
                var dataLoader = new CsvDataLoader();
                var databaseSelector = new SqlDatabaseSelector();
                var employeeRepository = new EmployeeRepository(connectionString);
                var employeeService = new EmployeeService(employeeRepository);
                var mainViewModel = new MainViewModel(dataLoader, databaseSelector, employeeRepository, employeeService);
                var mainWindow = new MainWindow(mainViewModel);
                mainWindow.Show();
            }
            catch (Exception ex)
            {              
                MessageBox.Show("Błąd podczas konfigurowania głównego okna: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}