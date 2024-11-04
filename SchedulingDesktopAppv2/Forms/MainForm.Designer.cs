namespace SchedulingDesktopAppv2.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenCalendar = new System.Windows.Forms.Button();
            this.btnAppointmentTypesByMonth = new System.Windows.Forms.Button();
            this.btnScheduleForEachUser = new System.Windows.Forms.Button();
            this.btnAppointmentsByLocation = new System.Windows.Forms.Button();
            this.listBoxReport = new System.Windows.Forms.ListBox();
            this.btnManageCustomers = new System.Windows.Forms.Button();
            this.btnManageAppointments = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenCalendar
            // 
            this.btnOpenCalendar.Location = new System.Drawing.Point(694, 12);
            this.btnOpenCalendar.Name = "btnOpenCalendar";
            this.btnOpenCalendar.Size = new System.Drawing.Size(94, 23);
            this.btnOpenCalendar.TabIndex = 0;
            this.btnOpenCalendar.Text = "Open Calendar";
            this.btnOpenCalendar.UseVisualStyleBackColor = true;
            this.btnOpenCalendar.Click += new System.EventHandler(this.btnOpenCalendar_Click);
            // 
            // btnAppointmentTypesByMonth
            // 
            this.btnAppointmentTypesByMonth.Location = new System.Drawing.Point(12, 12);
            this.btnAppointmentTypesByMonth.Name = "btnAppointmentTypesByMonth";
            this.btnAppointmentTypesByMonth.Size = new System.Drawing.Size(155, 23);
            this.btnAppointmentTypesByMonth.TabIndex = 1;
            this.btnAppointmentTypesByMonth.Text = "Appointment Types by Month";
            this.btnAppointmentTypesByMonth.UseVisualStyleBackColor = true;
            this.btnAppointmentTypesByMonth.Click += new System.EventHandler(this.btnAppointmentTypesByMonth_Click);
            // 
            // btnScheduleForEachUser
            // 
            this.btnScheduleForEachUser.Location = new System.Drawing.Point(187, 12);
            this.btnScheduleForEachUser.Name = "btnScheduleForEachUser";
            this.btnScheduleForEachUser.Size = new System.Drawing.Size(130, 23);
            this.btnScheduleForEachUser.TabIndex = 2;
            this.btnScheduleForEachUser.Text = "Schedule for Each User";
            this.btnScheduleForEachUser.UseVisualStyleBackColor = true;
            this.btnScheduleForEachUser.Click += new System.EventHandler(this.btnScheduleForEachUser_Click);
            // 
            // btnAppointmentsByLocation
            // 
            this.btnAppointmentsByLocation.Location = new System.Drawing.Point(336, 12);
            this.btnAppointmentsByLocation.Name = "btnAppointmentsByLocation";
            this.btnAppointmentsByLocation.Size = new System.Drawing.Size(154, 23);
            this.btnAppointmentsByLocation.TabIndex = 3;
            this.btnAppointmentsByLocation.Text = "Appointments by Location";
            this.btnAppointmentsByLocation.UseVisualStyleBackColor = true;
            this.btnAppointmentsByLocation.Click += new System.EventHandler(this.btnAppointmentsByLocation_Click);
            // 
            // listBoxReport
            // 
            this.listBoxReport.FormattingEnabled = true;
            this.listBoxReport.Location = new System.Drawing.Point(23, 74);
            this.listBoxReport.Name = "listBoxReport";
            this.listBoxReport.Size = new System.Drawing.Size(442, 147);
            this.listBoxReport.TabIndex = 4;
            // 
            // btnManageCustomers
            // 
            this.btnManageCustomers.Location = new System.Drawing.Point(23, 405);
            this.btnManageCustomers.Name = "btnManageCustomers";
            this.btnManageCustomers.Size = new System.Drawing.Size(116, 23);
            this.btnManageCustomers.TabIndex = 5;
            this.btnManageCustomers.Text = "Manage Customers";
            this.btnManageCustomers.UseVisualStyleBackColor = true;
            this.btnManageCustomers.Click += new System.EventHandler(this.btnManageCustomers_Click);
            // 
            // btnManageAppointments
            // 
            this.btnManageAppointments.Location = new System.Drawing.Point(161, 405);
            this.btnManageAppointments.Name = "btnManageAppointments";
            this.btnManageAppointments.Size = new System.Drawing.Size(133, 23);
            this.btnManageAppointments.TabIndex = 6;
            this.btnManageAppointments.Text = "Manage Appointments";
            this.btnManageAppointments.UseVisualStyleBackColor = true;
            this.btnManageAppointments.Click += new System.EventHandler(this.btnManageAppointments_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(679, 405);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(109, 23);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Log Out";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnManageAppointments);
            this.Controls.Add(this.btnManageCustomers);
            this.Controls.Add(this.listBoxReport);
            this.Controls.Add(this.btnAppointmentsByLocation);
            this.Controls.Add(this.btnScheduleForEachUser);
            this.Controls.Add(this.btnAppointmentTypesByMonth);
            this.Controls.Add(this.btnOpenCalendar);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenCalendar;
        private System.Windows.Forms.Button btnAppointmentTypesByMonth;
        private System.Windows.Forms.Button btnScheduleForEachUser;
        private System.Windows.Forms.Button btnAppointmentsByLocation;
        private System.Windows.Forms.ListBox listBoxReport;
        private System.Windows.Forms.Button btnManageCustomers;
        private System.Windows.Forms.Button btnManageAppointments;
        private System.Windows.Forms.Button btnLogout;
    }
}