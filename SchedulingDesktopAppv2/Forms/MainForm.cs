using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchedulingDesktopApp.Classes;

namespace SchedulingDesktopAppv2.Forms
{
    public partial class MainForm : Form
    {
        private int LoggedInUserId;
        public MainForm(int userId)
        {
            InitializeComponent();
            LoggedInUserId = userId;
        }

        private void btnOpenCalendar_Click(object sender, EventArgs e)
        {
            CalendarForm calendarForm = new CalendarForm();
            calendarForm.Show();
        }

        private void btnAppointmentTypesByMonth_Click(object sender, EventArgs e)
        {
            var report = Appointment.GetAppointmentTypesByMonth();
            listBoxReport.Items.Clear();

            foreach (var entry in report)
            {
                listBoxReport.Items.Add($"{entry.Key}: {entry.Value}");
            }
        }

        private void btnScheduleForEachUser_Click(object sender, EventArgs e)
        {
            var report = Appointment.GetScheduleForEachUser();
            listBoxReport.Items.Clear();

            foreach (var entry in report)
            {
                listBoxReport.Items.Add($"User {entry.Key}:");
                foreach (var appointment in entry.Value)
                {
                    listBoxReport.Items.Add($" - {appointment.Title} at {appointment.Start}");
                }
            }
        }

        private void btnAppointmentsByLocation_Click(object sender, EventArgs e)
        {
            var report = Appointment.GetAppointmentsByLocation();
            listBoxReport.Items.Clear();

            foreach (var entry in report)
            {
                listBoxReport.Items.Add($"{entry.Key}: {entry.Value}");
            }
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            CustomerManagementForm customerForm = new CustomerManagementForm(LoggedInUserId);
            customerForm.Show();
        }

        private void btnManageAppointments_Click(object sender, EventArgs e)
        {
            AppointmentManagementForm appointmentForm = new AppointmentManagementForm(LoggedInUserId);
            appointmentForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
