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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EversisZadanieRekrutacyjne.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow(MainViewModel viewModel)
        {
            try
            {
                InitializeComponent();
                this.viewModel = viewModel;
                DataContext = viewModel;
            }
            catch (Exception ex)
            {            
                MessageBox.Show("Błąd podczas inicjalizacji głównego okna: " + ex.Message, "Błąd");
            }
        }


    }
}
