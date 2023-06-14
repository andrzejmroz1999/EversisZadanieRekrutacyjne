using EversisZadanieRekrutacyjne.Models;
using EversisZadanieRekrutacyjne.Repositories;
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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow(Employee employee, Interfaces.IEmployeeService _employeeService)
        {
            try
            {
                InitializeComponent();
                EditViewModel viewModel = new EditViewModel(employee, _employeeService);
                viewModel.RequestClose += (sender, args) => Close();
                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas inicjalizacji okna edycji: " + ex.Message, "Błąd");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!(DataContext as EditViewModel).CanCloseWindow())
                {
                    e.Cancel = true;
                }
                else
                {
                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zamykania okna edycji: " + ex.Message, "Błąd");
            }
        }
    }
}
