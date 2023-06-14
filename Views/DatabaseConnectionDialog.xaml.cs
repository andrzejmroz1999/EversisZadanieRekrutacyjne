using EversisZadanieRekrutacyjne.Models;
using EversisZadanieRekrutacyjne.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EversisZadanieRekrutacyjne.Views
{
    /// <summary>
    /// Interaction logic for DatabaseConnectionDialog.xaml
    /// </summary>
    public partial class DatabaseConnectionDialog : Window
    {
        public DatabaseConnectionDialog()
        {
            InitializeComponent();

            // Ustawienie DataContext na ViewModel
            DatabaseSelectorViewModel viewModel = new DatabaseSelectorViewModel();
            viewModel.RequestClose += (sender, args) => Close();
            DataContext = viewModel;
        }

        public string ConnectionString { get; private set; }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                var viewModel = DataContext as DatabaseSelectorViewModel;
                viewModel.LoadServerInstancesCommand.Execute(null);
            }
            catch (Exception ex)
            {            
                MessageBox.Show("Błąd podczas ładowania instancji serwera SQL: " + ex.Message, "Błąd");
            }
        }

        private void ComboBox_DropDownOpened_1(object sender, EventArgs e)
        {
            try
            {
                var viewModel = DataContext as DatabaseSelectorViewModel;
                viewModel.LoadDatabasesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania baz danych: " + ex.Message, "Błąd");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var viewModel = DataContext as DatabaseSelectorViewModel;
                if (!viewModel.CanCloseWindow())
                {
                    e.Cancel = true;
                }
                else
                {
                    this.DialogResult = true;
                    this.ConnectionString = viewModel.ConnectionString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zamykania okna: " + ex.Message, "Błąd");
            }
        }
    }
}
