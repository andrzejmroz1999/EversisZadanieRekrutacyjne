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
            // Konfiguruj połączenie do bazy danych SQL
            DatabaseConnectionDialog DbConnDialog = new DatabaseConnectionDialog();
            if (DbConnDialog.ShowDialog() == true)
            {
                return DbConnDialog.ConnectionString;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
