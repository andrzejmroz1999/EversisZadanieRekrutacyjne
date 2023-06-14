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
        private string ConnectionString = "data source=LAPTOP-PC1SB9ND\\SQLEXPRESS;initial catalog=Employes;integrated security=True;MultipleActiveResultSets=True;";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
          
            ConfigureDbContext();
            ConfigureMainWindow();
        }

        private void ConfigureDbContext()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EmployesConnectionString"].ConnectionString;
            dbContext = new EmployesDbContext(connectionString);
        }

        private void ConfigureMainWindow()
        {
            var dataLoader = new CsvDataLoader();
            var databaseSelector = new SqlDatabaseSelector();
            var employeeRepository = new EmployeeRepository(dbContext);
            var employeeService = new EmployeeService(employeeRepository);
            var mainViewModel = new MainViewModel(dataLoader, databaseSelector, employeeService,dbContext);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}