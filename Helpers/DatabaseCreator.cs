using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                using (SqlConnection connection = new SqlConnection(ConnectionStringEncryptor.DecryptConnectionString(_connectionString)))
                {
                    connection.Open();

                    string createDatabaseQuery = Properties.Resources.CreateDatabase;

                    using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Baza danych została utworzona lub już istnieje.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas tworzenia bazy danych: " + ex.Message);
            }
        }
    }
}

