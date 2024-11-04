namespace SchedulingDesktopAppv2
{
    partial class Form1
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
            this.ConnectB = new System.Windows.Forms.Button();
            this.CheckB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectB
            // 
            this.ConnectB.Location = new System.Drawing.Point(367, 133);
            this.ConnectB.Name = "ConnectB";
            this.ConnectB.Size = new System.Drawing.Size(75, 23);
            this.ConnectB.TabIndex = 0;
            this.ConnectB.Text = "Connect";
            this.ConnectB.UseVisualStyleBackColor = true;
            this.ConnectB.Click += new System.EventHandler(this.ConnectB_Click_1);
            // 
            // CheckB
            // 
            this.CheckB.Location = new System.Drawing.Point(363, 214);
            this.CheckB.Name = "CheckB";
            this.CheckB.Size = new System.Drawing.Size(107, 23);
            this.CheckB.TabIndex = 1;
            this.CheckB.Text = "Check Connection";
            this.CheckB.UseVisualStyleBackColor = true;
            this.CheckB.Click += new System.EventHandler(this.CheckB_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CheckB);
            this.Controls.Add(this.ConnectB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ConnectB;
        private System.Windows.Forms.Button CheckB;
    }
}

