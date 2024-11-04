using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchedulingDesktopApp.Database
{
    public static class DBConnection
    {
        public static MySqlConnection GetConnection()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                return new MySqlConnection(constr);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error creating a database connection: {ex.Message}");
                return null;
            }
        }
    }
}
