using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SchedulingDesktopApp.Database;

namespace SchedulingDesktopAppv2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectB_Click_1(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(constr);

                conn.Open();
                MessageBox.Show("Connection is open.");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void CheckB_Click_1(object sender, EventArgs e)
        {
            MySqlConnection c = DBConnection.GetConnection();

            if (c.State == ConnectionState.Open)
            {
                MessageBox.Show("Connection Open");
            }
        }
    }
}
