using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EversisZadanieRekrutacyjne.Helpers
{
    public class ConnectionStringFactory
    {
        public string BuildConnectionString(string serverInstance, string databaseName, string username, string password, bool windowsAuthentication)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (windowsAuthentication)
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

            builder.InitialCatalog = databaseName;
            builder.MultipleActiveResultSets = true;

            string connectionString = builder.ConnectionString;

            // Sprawdzanie połączenia
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Połączenie udane!", "Sukces");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Błąd połączenia: " + ex.Message, "Błąd");
                }
            }

            string encryptedConnectionString = ConnectionStringEncryptor.EncryptConnectionString(connectionString); //Szyfrowanie ConnectionString
            return encryptedConnectionString;
        }
    }
}