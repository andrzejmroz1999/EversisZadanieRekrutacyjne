using EversisZadanieRekrutacyjne.DAL;
using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

            // Utworzenie instancji IDataLoader i IDatabaseSelector
            var dataLoader = new CsvDataLoader();
            var databaseSelector = new SqlDatabaseSelector();
            var dbContext = new EmployesDbContext();
            // Utworzenie instancji MainViewModel z wstrzykniętymi zależnościami
            var viewModel = new MainViewModel(dataLoader, databaseSelector, dbContext);

            // Utworzenie instancji MainWindow i przypisanie ViewModel jako DataContext
            var mainWindow = new MainWindow(viewModel);

            // Uruchomienie aplikacji z MainWindow
            mainWindow.Show();
        }
    }
}
