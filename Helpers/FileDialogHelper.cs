using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EversisZadanieRekrutacyjne.Helpers
{
    public static class FileDialogHelper
    {
        public static string GetFilePathFromUser()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == true)
                {
                    return openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania ścieżki pliku: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }
    }
}