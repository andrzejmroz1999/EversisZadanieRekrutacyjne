using Autofac;
using Autofac.Core;
using EversisZadanieRekrutacyjne.DAL;
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
        private EmployesDbContext dbContext;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                ConfigureDbContext();
                ConfigureMainWindow();
            }
            catch (Exception ex)
            {
                // Obsługa ogólnych błędów aplikacji
                MessageBox.Show("Wystąpił błąd podczas uruchamiania aplikacji: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void ConfigureDbContext()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EmployesConnectionString"].ConnectionString;
                dbContext = new EmployesDbContext(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas konfigurowania kontekstu bazy danych: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void ConfigureMainWindow()
        {
            try
            {
                var dataLoader = new CsvDataLoader();
                var databaseSelector = new SqlDatabaseSelector();
                var employeeRepository = new EmployeeRepository(dbContext);
                var employeeService = new EmployeeService(employeeRepository);
                var mainViewModel = new MainViewModel(dataLoader, databaseSelector, employeeRepository, employeeService, dbContext);
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