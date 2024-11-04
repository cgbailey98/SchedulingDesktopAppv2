using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SchedulingDesktopApp.Classes;
using SchedulingDesktopApp.Database;
using SchedulingDesktopAppv2.Classes;
using SchedulingDesktopAppv2.Forms;

namespace SchedulingDesktopApp.Forms
{
    public partial class LoginForm : Form
    {
        public int LoggedInUserId { get; private set; }

        private CultureInfo currentCulture;

        public LoginForm()
        {
            InitializeComponent();

            txtPassword.PasswordChar = '*';

            currentCulture = CultureInfo.CurrentCulture;
            string language = currentCulture.TwoLetterISOLanguageName;

            if (language == "es" || currentCulture.Name.StartsWith("es-"))
            {
                SetLanguage("es");
            }
            else
            {
                SetLanguage("en");
            }

            this.AcceptButton = btnLogin;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            SetLanguage(currentCulture.TwoLetterISOLanguageName);
        }

        private void CheckUpcomingAppointments(int loggedInUserId)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    DateTime currentTime = DateTime.UtcNow;
                    DateTime fifteenMinutesLater = currentTime.AddMinutes(15);

                    string query = @"SELECT a.appointmentId, c.customerName, a.start, a.userId
                                   FROM appointment a
                                   INNER JOIN customer c ON a.customerId = c.customerId
                                   WHERE a.userId = @loggedInUserId
                                   AND a.start BETWEEN @currentTime AND @fifteenMinutesLater;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@loggedInUserId", loggedInUserId);
                        cmd.Parameters.AddWithValue("@currentTime", currentTime);
                        cmd.Parameters.AddWithValue("@fifteenMinutesLater", fifteenMinutesLater);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<string> upcomingAppointments = new List<string>();

                            while (reader.Read())
                            {
                                string customerName = reader["customerName"].ToString();
                                DateTime startTimeUtc = Convert.ToDateTime(reader["start"]);
                                DateTime startTimeLocal = ConvertToLocalTime(startTimeUtc);

                                upcomingAppointments.Add($"You have an appointment with {customerName} at {startTimeLocal}.");
                            }

                            upcomingAppointments
                                .Where(appt => !string.IsNullOrEmpty(appt))
                                .ToList()
                                .ForEach(apptMessage => MessageBox.Show(apptMessage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking upcoming appointments: {ex.Message}");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(GetLocalizedString("Username and password cannot be empty."));
                return;
            }

            if (ValidateLogin(username, password))
            {
                MessageBox.Show(GetLocalizedString("Login successful!"));

                LogUserLogin(username);

                this.Hide();
                MainForm mainForm = new MainForm(LoggedInUserId);
                mainForm.Show();

                CheckUpcomingAppointments(LoggedInUserId);
            }
            else
            {
                MessageBox.Show(GetLocalizedString("The username and password do not match."));
            }
        }

        private void LogUserLogin(string username)
        {
            try
            {
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Login_History.txt");
                string logEntry = $"{DateTime.Now}: user '{username}' logged in successfully.";
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error logging user login: {ex.Message}");
            }
        }

        private void SetLanguage(string language)
        {
            if (language == "es")
            {
                lblUsername.Text = "Nombre de usuario";
                lblPassword.Text = "Contraseña";
                btnLogin.Text = "Iniciar sesión";
                lblWelcome.Text = "Bienvenido a la Aplicación de Programación";
            }
            else
            {
                lblUsername.Text = "Username";
                lblPassword.Text = "Password";
                btnLogin.Text = "Login";
                lblWelcome.Text = "Welcome to the Scheduling App";
            }
        }

        private string GetLocalizedString(string message)
        {
            if (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "es")
            {
                switch (message)
                {
                    case "Login successful!":
                        return "¡Inicio de sesión exitoso!";
                    case "The username and password do not match.":
                        return "El nombre de usuario y la contraseña no coinciden.";
                    case "Username and password cannot be empty.":
                        return "El nombre de usuario y la contraseña no pueden estar vacíos.";
                    default:
                        return message;
                }
            }

            return message;
        }

        private bool ValidateLogin(string username, string password)
        {
            bool isValid = false;

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = "SELECT userId FROM user WHERE userName = @username AND password = @password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            LoggedInUserId = Convert.ToInt32(result);
                            isValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to the database: {ex.Message}");
            }

            return isValid;
        }

        private DateTime ConvertToUtcTime(DateTime localTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localTime);
        }

        private DateTime ConvertToLocalTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.Local);
        }
    }
}
