using EversisZadanieRekrutacyjne.Interfaces;
using EversisZadanieRekrutacyjne.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.DAL
{
    public class SqlDatabaseSelector : IDatabaseSelector
    {
        public string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            // Konfiguruj połączenie do bazy danych SQL
            DatabaseConnectionDialog DbConnDialog = new DatabaseConnectionDialog();
            DbConnDialog.ShowDialog();

            return builder.ConnectionString;
        }
    }
}
