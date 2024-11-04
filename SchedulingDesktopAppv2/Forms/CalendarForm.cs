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
    public partial class CalendarForm : Form
    {
        public CalendarForm()
        {
            InitializeComponent();
        }

        private void AppointmentCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = e.Start;

            var appointments = Appointment.GetAllAppointments()
                .Where(a => a.Start.Date == selectedDate.Date)
                .ToList();

            dataGridViewAppointments.DataSource = appointments;
        }
    }
}
