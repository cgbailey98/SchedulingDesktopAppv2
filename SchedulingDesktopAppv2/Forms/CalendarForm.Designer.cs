namespace SchedulingDesktopAppv2.Forms
{
    partial class CalendarForm
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
            this.AppointmentCalendar = new System.Windows.Forms.MonthCalendar();
            this.dataGridViewAppointments = new System.Windows.Forms.DataGridView();
            this.lblCalendar = new System.Windows.Forms.Label();
            this.lblAppointments = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // AppointmentCalendar
            // 
            this.AppointmentCalendar.Location = new System.Drawing.Point(60, 117);
            this.AppointmentCalendar.Name = "AppointmentCalendar";
            this.AppointmentCalendar.TabIndex = 0;
            this.AppointmentCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.AppointmentCalendar_DateChanged);
            // 
            // dataGridViewAppointments
            // 
            this.dataGridViewAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAppointments.Location = new System.Drawing.Point(377, 88);
            this.dataGridViewAppointments.Name = "dataGridViewAppointments";
            this.dataGridViewAppointments.Size = new System.Drawing.Size(411, 226);
            this.dataGridViewAppointments.TabIndex = 1;
            // 
            // lblCalendar
            // 
            this.lblCalendar.AutoSize = true;
            this.lblCalendar.Location = new System.Drawing.Point(85, 95);
            this.lblCalendar.Name = "lblCalendar";
            this.lblCalendar.Size = new System.Drawing.Size(173, 13);
            this.lblCalendar.TabIndex = 2;
            this.lblCalendar.Text = "Select a date to view appointments";
            // 
            // lblAppointments
            // 
            this.lblAppointments.AutoSize = true;
            this.lblAppointments.Location = new System.Drawing.Point(500, 72);
            this.lblAppointments.Name = "lblAppointments";
            this.lblAppointments.Size = new System.Drawing.Size(153, 13);
            this.lblAppointments.TabIndex = 3;
            this.lblAppointments.Text = "Appointments on selected date";
            // 
            // CalendarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblAppointments);
            this.Controls.Add(this.lblCalendar);
            this.Controls.Add(this.dataGridViewAppointments);
            this.Controls.Add(this.AppointmentCalendar);
            this.Name = "CalendarForm";
            this.Text = "CalendarForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar AppointmentCalendar;
        private System.Windows.Forms.DataGridView dataGridViewAppointments;
        private System.Windows.Forms.Label lblCalendar;
        private System.Windows.Forms.Label lblAppointments;
    }
}