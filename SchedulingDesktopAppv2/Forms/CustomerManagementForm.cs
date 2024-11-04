using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SchedulingDesktopApp.Database;

namespace SchedulingDesktopAppv2.Forms
{
    public partial class CustomerManagementForm : Form
    {
        private int LoggedInUserID;
        public CustomerManagementForm(int userID)
        {
            InitializeComponent();
            LoggedInUserID = userID;
        }

        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();

            try
            {
                comboBoxCity.DataSource = GetCities();
                comboBoxCity.DisplayMember = "city";
                comboBoxCity.ValueMember = "cityId";
                comboBoxCity.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cities: {ex.Message}");
            }
        }

        private DataTable GetCities()
        {
            DataTable cities = new DataTable();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = "SELECT cityId, city FROM city;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(cities);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving cities: {ex.Message}");
            }
            return cities;
        }

        private void LoadCustomers()
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = @"
                            SELECT c.customerId, c.customerName, a.addressId, a.address, a.phone, a.postalCode, ct.city
                            FROM customer c
                            JOIN address a ON c.addressId = a.addressId
                            JOIN city ct ON a.cityId = ct.cityId;";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        System.Data.DataTable customerTable = new System.Data.DataTable();
                        adapter.Fill(customerTable);
                        dataGridViewCustomers.DataSource = customerTable;

                        dataGridViewCustomers.Columns["customerId"].HeaderText = "Customer ID";
                        dataGridViewCustomers.Columns["customerName"].HeaderText = "Customer Name";
                        dataGridViewCustomers.Columns["addressId"].Visible = false;
                        dataGridViewCustomers.Columns["address"].HeaderText = "Address";
                        dataGridViewCustomers.Columns["phone"].HeaderText = "Phone";
                        dataGridViewCustomers.Columns["postalCode"].HeaderText = "Postal Code";
                        dataGridViewCustomers.Columns["city"].HeaderText = "City";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}");
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string name = txtCustomerName.Text.Trim();
            string address = txtCustomerAddress.Text.Trim();
            string address2 = " ";
            string phone = txtCustomerPhone.Text.Trim();
            string postalCode = txtCustomerPostalCode.Text.Trim();
            string createdBy = LoggedInUserID.ToString();
            string lastUpdateBy = LoggedInUserID.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(postalCode) || comboBoxCity.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }

            if (comboBoxCity.SelectedValue == null)
            {
                MessageBox.Show("Please select a city.");
                return;
            }

            int cityId = (int)comboBoxCity.SelectedValue;

            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, "^[0-9-]+$"))
            {
                MessageBox.Show("Phone number can only contain digits and dashes.");
                return;
            }

            if (IsDuplicateCustomer(name, address, phone))
            {
                MessageBox.Show("A customer with the same name, address, and phone number already exists.");
                return;
            }

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string addressQuery = @"
                        INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES (@address, @address2, @cityId, @postalCode, @phone, NOW(), @createdBy, NOW(), @lastUpdateBy);";

                            using (MySqlCommand cmd = new MySqlCommand(addressQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@address", address);
                                cmd.Parameters.AddWithValue("@address2", address2);
                                cmd.Parameters.AddWithValue("@cityId", cityId);
                                cmd.Parameters.AddWithValue("@postalCode", postalCode);
                                cmd.Parameters.AddWithValue("@phone", phone);
                                cmd.Parameters.AddWithValue("@createDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@createdBy", createdBy);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", lastUpdateBy);
                                cmd.ExecuteNonQuery();
                            }

                            int addressId;
                            using (MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn, transaction))
                            {
                                addressId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            string customerQuery = @"
                        INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES (@customerName, @addressId, 1, NOW(), @createdBy, NOW(), @lastUpdateBy);";

                            using (MySqlCommand cmd = new MySqlCommand(customerQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@customerName", name);
                                cmd.Parameters.AddWithValue("@addressId", addressId);
                                cmd.Parameters.AddWithValue("@active", 1);
                                cmd.Parameters.AddWithValue("createDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@createdBy", createdBy);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", lastUpdateBy);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Customer added successfully.");
                            LoadCustomers();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Error adding customer: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}");
            }
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            int customerId = Convert.ToInt32(dataGridViewCustomers.SelectedRows[0].Cells["customerId"].Value);
            string name = txtCustomerName.Text.Trim();
            string address = txtCustomerAddress.Text.Trim();
            string address2 = " ";
            string phone = txtCustomerPhone.Text.Trim();
            string postalCode = txtCustomerPostalCode.Text.Trim();
            string createdBy = LoggedInUserID.ToString();
            string lastUpdateBy = LoggedInUserID.ToString();


            if (comboBoxCity.SelectedValue == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Please fill in all fields and select a valid city.");
                return;
            }

            int cityId = (int)comboBoxCity.SelectedValue;

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string addressQuery = @"
                                UPDATE address
                                SET address = @address, address2 = @address2, phone = @phone, postalCode = @postalCode, cityId = @cityId, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy
                                WHERE addressId = (SELECT addressId FROM customer WHERE customerId = @customerId);";

                            using (MySqlCommand cmd = new MySqlCommand(addressQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@address", address);
                                cmd.Parameters.AddWithValue("@address2", address2);
                                cmd.Parameters.AddWithValue("@phone", phone);
                                cmd.Parameters.AddWithValue("@postalCode", postalCode);
                                cmd.Parameters.AddWithValue("@cityId", cityId);
                                cmd.Parameters.AddWithValue("@customerId", customerId);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", lastUpdateBy);
                                cmd.ExecuteNonQuery();
                            }

                            string customerQuery = @"
                                UPDATE customer
                                SET customerName = @customerName, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy
                                WHERE customerId = @customerId;";

                            using (MySqlCommand cmd = new MySqlCommand(customerQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@customerName", name);
                                cmd.Parameters.AddWithValue("@customerId", customerId);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", lastUpdateBy);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Customer updated successfully.");
                            LoadCustomers();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Error updating customer: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}");
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            int customerId = Convert.ToInt32(dataGridViewCustomers.SelectedRows[0].Cells["customerId"].Value);
            int addressId = Convert.ToInt32(dataGridViewCustomers.SelectedRows[0].Cells["addressId"].Value);

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this customer?",
                "Confirm Deletion", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string checkAppointmentQuery = "SELECT COUNT(*) FROM appointment WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(checkAppointmentQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        int appointmentCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (appointmentCount > 0)
                        {
                            MessageBox.Show(
                                "Cannot delete customer. There are active appointments linked to this customer.");
                            return;
                        }
                    }

                    string customerQuery = "DELETE FROM customer WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(customerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.ExecuteNonQuery();
                    }

                    string addressQuery = "DELETE FROM address WHERE addressId = @addressId;";
                    using (MySqlCommand cmd = new MySqlCommand(addressQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Customer deleted successfully.");
                    LoadCustomers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}");
            }
        }

        private void dataGridViewCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewCustomers.SelectedRows[0];

                txtCustomerName.Text = selectedRow.Cells["customerName"].Value.ToString();
                txtCustomerAddress.Text = selectedRow.Cells["address"].Value.ToString();
                txtCustomerPhone.Text = selectedRow.Cells["phone"].Value.ToString();
                txtCustomerPostalCode.Text = selectedRow.Cells["postalCode"].Value.ToString();

                string cityName = selectedRow.Cells["city"].Value.ToString();
                comboBoxCity.SelectedIndex = comboBoxCity.FindStringExact(cityName);
            }
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();
            txtCustomerAddress.Clear();
            txtCustomerPhone.Clear();
            comboBoxCity.SelectedIndex = -1;
            txtCustomerPostalCode.Clear();
        }

        private bool IsDuplicateCustomer(string name, string address, string phone)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = @"SELECT COUNT(*) 
                             FROM customer c
                             JOIN address a ON c.addressId = a.addressId
                             WHERE c.customerName = @name AND a.address = @address AND a.phone = @phone";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@phone", phone);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for duplicate customer: {ex.Message}");
                return false;
            }
        }
    }
}
