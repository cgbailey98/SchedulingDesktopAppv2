using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SchedulingDesktopApp.Database;

namespace SchedulingDesktopApp.Classes
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public static void AddAppointment(Appointment appointment)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estStartTime = TimeZoneInfo.ConvertTime(appointment.Start, TimeZoneInfo.Local, est);
            DateTime estEndTime = TimeZoneInfo.ConvertTime(appointment.End, TimeZoneInfo.Local, est);

            appointment.Start = estStartTime;
            appointment.End = estEndTime;

            if (appointment.Start.TimeOfDay < new TimeSpan(9, 0, 0) ||
                appointment.End.TimeOfDay > new TimeSpan(17, 0, 0))
            {
                MessageBox.Show(
                    "Appointments must be scheduled between 9:00 a.m. and 5:00 p.m., Monday to Friday (EST).");
                return;
            }

            var existingAppointments = GetAllAppointments();
            if (existingAppointments.Any(a => a.CustomerId == appointment.CustomerId &&
                                              ((a.Start < appointment.End && a.Start >= appointment.Start) ||
                                               (appointment.Start < a.End && appointment.Start >= a.Start))))
            {
                MessageBox.Show("The appointment overlaps with an existing appointment.");
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

                    string query = @"INSERT INTO appointment (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                                     VALUES (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, NOW(), @createdBy, NOW(), @lastUpdateBy)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", appointment.CustomerId);
                        cmd.Parameters.AddWithValue("@userId", appointment.UserId);
                        cmd.Parameters.AddWithValue("@title", appointment.Title);
                        cmd.Parameters.AddWithValue("@description", appointment.Description);
                        cmd.Parameters.AddWithValue("@location", appointment.Location);
                        cmd.Parameters.AddWithValue("@contact", appointment.Contact);
                        cmd.Parameters.AddWithValue("@type", appointment.Type);
                        cmd.Parameters.AddWithValue("@url", appointment.Url);
                        cmd.Parameters.AddWithValue("@start", appointment.Start);
                        cmd.Parameters.AddWithValue("@end", appointment.End);
                        cmd.Parameters.AddWithValue("@createDate", appointment.CreateDate);
                        cmd.Parameters.AddWithValue("@createdBy", appointment.CreatedBy);
                        cmd.Parameters.AddWithValue("@lastUpdate", appointment.LastUpdate);
                        cmd.Parameters.AddWithValue("@lastUpdateBy", appointment.LastUpdateBy);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Appointment added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding appointment: {ex.Message}");
            }
        }

        public static List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        @"SELECT appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy FROM appointment";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Appointment appointment = new Appointment
                                {
                                    AppointmentId = reader.GetInt32("appointmentId"),
                                    CustomerId = reader.GetInt32("customerId"),
                                    UserId = reader.GetInt32("userID"),
                                    Title = reader.GetString("title"),
                                    Description = reader.GetString("description"),
                                    Location = reader.GetString("location"),
                                    Contact = reader.GetString("contact"),
                                    Type = reader.GetString("type"),
                                    Url = reader.GetString("url"),
                                    Start = TimeZoneInfo.ConvertTimeFromUtc(reader.GetDateTime("start"), TimeZoneInfo.Local),
                                    End = TimeZoneInfo.ConvertTimeFromUtc(reader.GetDateTime("end"), TimeZoneInfo.Local),
                                    CreateDate = reader.GetDateTime("createDate"),
                                    CreatedBy = reader.GetString("createdBy"),
                                    LastUpdate = reader.GetDateTime("lastUpdate"),
                                    LastUpdateBy = reader.GetString("lastUpdateBy")
                                };

                                appointments.Add(appointment);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving appointments: {ex.Message}");
            }

            return appointments;
        }

        public static void UpdateAppointment(Appointment appointment)
        {
            if (appointment.Start.TimeOfDay < new TimeSpan(9, 0, 0) ||
                appointment.End.TimeOfDay > new TimeSpan(17, 0, 0))
            {
                MessageBox.Show(
                    "Appointments must be scheduled between 9:00 a.m. and 5:00 p.m., Monday to Friday (EST).");
                return;
            }

            var existingAppointments = GetAllAppointments();
            if (existingAppointments.Any(a => a.CustomerId == appointment.CustomerId &&
                                              ((a.Start < appointment.End && a.Start >= appointment.Start) ||
                                               (appointment.Start < a.End && appointment.Start >= a.Start))))
            {
                MessageBox.Show("The appointment overlaps with an existing appointment.");
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

                    string query =
                        @"UPDATE appointment SET customerId = @customerId, userId = @userId, title = @title, description = @description, location = @location, contact = @contact, type = @type, url = @url, 
                                                 start = @start, end = @end, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy WHERE appointmentId = @appointmentId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", appointment.AppointmentId);
                        cmd.Parameters.AddWithValue("@customerId", appointment.CustomerId);
                        cmd.Parameters.AddWithValue("@userId", appointment.UserId);
                        cmd.Parameters.AddWithValue("@title", appointment.Title);
                        cmd.Parameters.AddWithValue("@description", appointment.Description);
                        cmd.Parameters.AddWithValue("@location", appointment.Location);
                        cmd.Parameters.AddWithValue("@contact", appointment.Contact);
                        cmd.Parameters.AddWithValue("type", appointment.Type);
                        cmd.Parameters.AddWithValue("@url", appointment.Url);
                        cmd.Parameters.AddWithValue("@start", appointment.Start);
                        cmd.Parameters.AddWithValue("@end", appointment.End);
                        cmd.Parameters.AddWithValue("@lastUpdateBy", appointment.LastUpdateBy);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Appointment updated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}");
            }
        }

        public static void DeleteAppointment(int appointmentId)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = @"DELETE FROM appointment WHERE appointmentId = @appointmentId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Appointment deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointment: {ex.Message}");
            }
        }

        public static Dictionary<string, int> GetAppointmentTypesByMonth()
        {
            var appointments = GetAllAppointments();
            var appointmentTypes = appointments
                .GroupBy(a => new { a.Start.Month, a.Type })
                .ToDictionary(
                    g => $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)} - {g.Key.Type}",
                    g => g.Count());

            return appointmentTypes;
        }

        public static Dictionary<string, List<Appointment>> GetScheduleForEachUser()
        {
            var appointments = GetAllAppointments();
            return appointments
                .GroupBy(a => a.UserId)
                .ToDictionary(g => g.Key.ToString(), g => g.ToList());
        }

        public static Dictionary<string, int> GetAppointmentsByLocation()
        {
            var appointments = GetAllAppointments();

            var report = appointments
                .GroupBy(a => a.Location)
                .ToDictionary(g => g.Key, g => g.Count());

            return report;
        }
    }
}
