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
using SchedulingDesktopApp.Classes;
using SchedulingDesktopApp.Database;

namespace SchedulingDesktopAppv2.Forms
{
    public partial class AppointmentManagementForm : Form
    {
        private int LoggedInUserID;
        public AppointmentManagementForm(int userId)
        {
            InitializeComponent();
            LoggedInUserID = userId;
            LoadAppointments();
            LoadCustomersIntoComboBox();
        }

        private void LoadAppointments()
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = "SELECT appointmentId, customerId, type, start, end FROM appointment";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            row["start"] = ConvertToLocalTime((DateTime)row["start"]);
                            row["end"] = ConvertToLocalTime((DateTime)row["end"]);
                        }
                        dataGridViewAppointments.DataSource = dt;

                        dataGridViewAppointments.Columns["start"].Width = 150;
                        dataGridViewAppointments.Columns["end"].Width = 150;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}");
            }
        }

        private void LoadCustomersIntoComboBox()
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = "SELECT customerId, customerName FROM customer";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            DataRow blankRow = dt.NewRow();
                            blankRow["customerId"] = -1;
                            blankRow["customerName"] = "--Select a Customer--";
                            dt.Rows.InsertAt(blankRow, 0);

                            comboBoxCustomer.DisplayMember = "customerName";
                            comboBoxCustomer.ValueMember = "customerId";
                            comboBoxCustomer.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}");
            }
        }

        private int GetCurrentUserId()
        {
            return LoggedInUserID;
        }

        private bool IsWithinBusinessHours(DateTime start, DateTime end)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estStart = TimeZoneInfo.ConvertTime(start, est);
            DateTime estEnd = TimeZoneInfo.ConvertTime(end, est);

            if (estStart.TimeOfDay < new TimeSpan(9, 0, 0) || estEnd.TimeOfDay > new TimeSpan(17, 0, 0))
            {
                return false;
            }

            if (estStart.DayOfWeek == DayOfWeek.Saturday || estStart.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            return true;
        }

        private bool IsOverlappingAppointment(int customerId, DateTime start, DateTime end, int? appointmentId = null)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        "SELECT COUNT(*) FROM appointment WHERE customerId = @customerId AND (@start < end AND @end > start)";
                    if (appointmentId.HasValue)
                    {
                        query += "AND appointmentId != @appointmentId";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);
                        if (appointmentId.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@appointmentId", appointmentId.Value);
                        }

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking overlapping appointments: {ex.Message}");
                return true;
            }
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            string type = txtType.Text.Trim();
            DateTime start = ConvertToUtcTime(dateTimePickerStart.Value);
            DateTime end = ConvertToUtcTime(dateTimePickerEnd.Value);
            int customerId = (int)comboBoxCustomer.SelectedValue;

            if (comboBoxCustomer.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            if (customerId == -1)
            {
                MessageBox.Show("Please select a valid customer.");
                return;
            }

            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Please enter an appointment type.");
                return;
            }

            if (start >= end)
            {
                MessageBox.Show("The start time must be earlier than the end time.");
                return;
            }

            if (!IsWithinBusinessHours(start, end))
            {
                MessageBox.Show("Appointments must be scheduled between 9:00 AM and 5:00 PM, Monday to Friday (EST).");
                return;
            }

            if (IsOverlappingAppointment(customerId, start, end))
            {
                MessageBox.Show("The appointment overlaps with an existing appointment. Please choose another time.");
                return;
            }

            int userId = GetCurrentUserId();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        "INSERT INTO appointment (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                        "VALUES (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, NOW(), @createdBy, NOW(), @lastUpdateBy);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@userID", userId);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);
                        cmd.Parameters.AddWithValue("@createdBy", LoggedInUserID);
                        cmd.Parameters.AddWithValue("@lastUpdateBy", LoggedInUserID);
                        cmd.Parameters.AddWithValue("@title", "not needed");
                        cmd.Parameters.AddWithValue("@description", "not needed");
                        cmd.Parameters.AddWithValue("@location", "not needed");
                        cmd.Parameters.AddWithValue("@contact", "not needed");
                        cmd.Parameters.AddWithValue("@url", "not needed");
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Appointment added successfully.");
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding appointment: {ex.Message}");
            }
        }

        private void btnUpdateAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to update.");
                return;
            }

            int appointmentId = Convert.ToInt32(dataGridViewAppointments.SelectedRows[0].Cells["appointmentId"].Value);
            string type = txtType.Text.Trim();
            DateTime start = ConvertToUtcTime(dateTimePickerStart.Value);
            DateTime end = ConvertToUtcTime(dateTimePickerEnd.Value);
            int customerId = (int)comboBoxCustomer.SelectedValue;

            if (string.IsNullOrEmpty(type) || start >= end || comboBoxCustomer.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all fields correctly.");
                return;
            }

            try
            {
                if (!IsWithinBusinessHours(start, end))
                {
                    MessageBox.Show("Appointments must be scheduled between 9:00 AM and 5:00 PM, Monday to Friday (EST).");
                    return;
                }

                if (IsOverlappingAppointment(customerId, start, end))
                {
                    MessageBox.Show("The appointment overlaps with an existing appointment. Please choose another time.");
                    return;
                }

                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        "UPDATE appointment SET customerId = @customerId, type = @type, start = @start, end = @end, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy " +
                        "WHERE appointmentId = @appointmentId;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);
                        cmd.Parameters.AddWithValue("@lastUpdateBy", LoggedInUserID);
                        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Appointment updated successfully.");
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}");
            }
        }

        private void btnDeleteAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            int appointmentId = Convert.ToInt32(dataGridViewAppointments.SelectedRows[0].Cells["appointmentId"].Value);

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this appointment?",
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

                    string query = "DELETE FROM appointment WHERE appointmentId = @appointmentId;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Appointment deleted successfully.");
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointment: {ex.Message}");
            }
        }

        private void dataGridViewAppointments_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewAppointments.SelectedRows[0];
                txtType.Text = selectedRow.Cells["type"].Value.ToString();
                dateTimePickerStart.Value = Convert.ToDateTime(selectedRow.Cells["start"].Value);
                dateTimePickerEnd.Value = Convert.ToDateTime(selectedRow.Cells["end"].Value);
                comboBoxCustomer.SelectedValue = Convert.ToInt32(selectedRow.Cells["customerId"].Value);
            }
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            txtType.Clear();
            dateTimePickerStart.ResetText();
            dateTimePickerEnd.ResetText();
            comboBoxCustomer.SelectedIndex = 0;
        }

        private DateTime ConvertToUtcTime(DateTime localTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localTime);
        }

        private DateTime ConvertToLocalTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.Local);
        }

        private string GetCustomerName(int customerId)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = "SELECT customerName FROM customer WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        return cmd.ExecuteScalar()?.ToString() ?? "Unknown Customer";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching customer name: {ex.Message}");
                return "Unknown Customer";
            }
        }
    }
}
