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
            DataContext = new DatabaseSelectorViewModel();
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            var viewModel = DataContext as DatabaseSelectorViewModel;
            viewModel.LoadServerInstancesCommand.Execute(null);
        }

        private void ComboBox_DropDownOpened_1(object sender, EventArgs e)
        {
            var viewModel = DataContext as DatabaseSelectorViewModel;
            viewModel.LoadDatabasesCommand.Execute(null);
        }
    }
}
