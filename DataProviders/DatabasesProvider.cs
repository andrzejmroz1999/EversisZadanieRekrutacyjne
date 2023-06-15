using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EversisZadanieRekrutacyjne.DataProviders
{
    public class DatabasesProvider
    {
        public ObservableCollection<string> GetDatabases(string serverInstance, string username, string password)
        {
            ObservableCollection<string> databases = new ObservableCollection<string>();

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
                    ShowErrorMessage("Błąd podczas pobierania listy baz danych: " + ex.Message);
                }
            }

            return databases;
        }

        private SqlConnection GetSqlConnection(string serverInstance, string username, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (string.IsNullOrEmpty(username))
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

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}