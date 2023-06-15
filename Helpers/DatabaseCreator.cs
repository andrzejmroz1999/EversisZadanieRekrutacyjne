using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EversisZadanieRekrutacyjne.Helpers
{
    public class DatabaseCreator
    {
        private string _connectionString;

        public DatabaseCreator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateDatabase()
        {
            try
            {
              

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionStringEncryptor.DecryptConnectionString(_connectionString));
                builder.Remove("initial catalog");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    string createDatabaseQuery = Properties.Resources.CreateDatabase;

                    using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    //Console.WriteLine("Baza danych została utworzona lub już istnieje.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas tworzenia bazy danych: " + ex.Message);
            }
        }
    }
}

