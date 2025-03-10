namespace WEZD
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            label1 = new Label();
            panel1 = new Panel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 48F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(-1, 0);
            label1.Name = "label1";
            label1.Size = new Size(262, 106);
            label1.TabIndex = 0;
            label1.Text = "WEZD";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label1);
            panel1.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 125);
            panel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(306, 45);
            label2.Name = "label2";
            label2.Size = new Size(444, 54);
            label2.TabIndex = 2;
            label2.Text = "Windows Easy Deployer";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(30, 172);
            label3.Name = "label3";
            label3.Size = new Size(380, 160);
            label3.TabIndex = 3;
            label3.Text = resources.GetString("label3.Text");
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(229, 361);
            label4.Name = "label4";
            label4.Size = new Size(559, 80);
            label4.TabIndex = 4;
            label4.Text = "WEZD v1.3.0\r\nLicence : Open Source  (C#)\r\nDeveloped by : Benji68hskvd and Melvin\r\nGithub Repository : https://github.com/Benji68hskvd/WEZD-Windows-EZ-Deployer";
            label4.TextAlign = ContentAlignment.BottomRight;
            // 
            // About
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(751, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(panel1);
            Name = "About";
            Text = "About Windows Easy Deployer";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}