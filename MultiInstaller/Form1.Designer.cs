namespace MultiInstaller
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            InstallButton = new Button();
            button2 = new Button();
            label1 = new Label();
            chrome = new CheckBox();
            CCleaner = new CheckBox();
            NovaBench = new CheckBox();
            VLC = new CheckBox();
            LibreOffice = new CheckBox();
            AutoDeleteInstaller = new CheckBox();
            label2 = new Label();
            radioButton1 = new RadioButton();
            HWID = new RadioButton();
            KMS38 = new RadioButton();
            WinOnlineKMS = new RadioButton();
            panel1 = new Panel();
            panel2 = new Panel();
            label3 = new Label();
            panel3 = new Panel();
            radioButton8 = new RadioButton();
            OfficeOnlineKMS = new RadioButton();
            Ohook = new RadioButton();
            UseCurDir = new CheckBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // InstallButton
            // 
            InstallButton.Location = new Point(512, 402);
            InstallButton.Name = "InstallButton";
            InstallButton.Size = new Size(100, 27);
            InstallButton.TabIndex = 0;
            InstallButton.Text = "Apply";
            InstallButton.UseVisualStyleBackColor = true;
            InstallButton.Click += InstallButton_Click;
            // 
            // button2
            // 
            button2.Location = new Point(618, 402);
            button2.Name = "button2";
            button2.Size = new Size(100, 27);
            button2.TabIndex = 1;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 35);
            label1.Name = "label1";
            label1.Size = new Size(122, 15);
            label1.TabIndex = 2;
            label1.Text = "Application to Install :";
            // 
            // chrome
            // 
            chrome.AutoSize = true;
            chrome.Location = new Point(38, 17);
            chrome.Name = "chrome";
            chrome.Size = new Size(69, 19);
            chrome.TabIndex = 3;
            chrome.Text = "Chrome";
            chrome.UseVisualStyleBackColor = true;
            // 
            // CCleaner
            // 
            CCleaner.AutoSize = true;
            CCleaner.Location = new Point(38, 42);
            CCleaner.Name = "CCleaner";
            CCleaner.Size = new Size(74, 19);
            CCleaner.TabIndex = 4;
            CCleaner.Text = "CCleaner";
            CCleaner.UseVisualStyleBackColor = true;
            // 
            // NovaBench
            // 
            NovaBench.AutoSize = true;
            NovaBench.Location = new Point(38, 66);
            NovaBench.Name = "NovaBench";
            NovaBench.Size = new Size(87, 19);
            NovaBench.TabIndex = 5;
            NovaBench.Text = "NovaBench";
            NovaBench.UseVisualStyleBackColor = true;
            // 
            // VLC
            // 
            VLC.AutoSize = true;
            VLC.Location = new Point(38, 117);
            VLC.Name = "VLC";
            VLC.Size = new Size(47, 19);
            VLC.TabIndex = 6;
            VLC.Text = "VLC";
            VLC.UseVisualStyleBackColor = true;
            // 
            // LibreOffice
            // 
            LibreOffice.AutoSize = true;
            LibreOffice.Location = new Point(38, 91);
            LibreOffice.Name = "LibreOffice";
            LibreOffice.Size = new Size(84, 19);
            LibreOffice.TabIndex = 7;
            LibreOffice.Text = "LibreOffice";
            LibreOffice.UseVisualStyleBackColor = true;
            // 
            // AutoDeleteInstaller
            // 
            AutoDeleteInstaller.AutoSize = true;
            AutoDeleteInstaller.Checked = true;
            AutoDeleteInstaller.CheckState = CheckState.Checked;
            AutoDeleteInstaller.Location = new Point(12, 410);
            AutoDeleteInstaller.Name = "AutoDeleteInstaller";
            AutoDeleteInstaller.Size = new Size(132, 19);
            AutoDeleteInstaller.TabIndex = 10;
            AutoDeleteInstaller.Text = "Auto Delete Installer";
            AutoDeleteInstaller.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(260, 35);
            label2.Name = "label2";
            label2.Size = new Size(119, 15);
            label2.TabIndex = 12;
            label2.Text = "Windows Activation :";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(30, 16);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(69, 19);
            radioButton1.TabIndex = 13;
            radioButton1.TabStop = true;
            radioButton1.Text = "Nothing";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // HWID
            // 
            HWID.AutoSize = true;
            HWID.Location = new Point(30, 41);
            HWID.Name = "HWID";
            HWID.Size = new Size(56, 19);
            HWID.TabIndex = 14;
            HWID.Text = "HWID";
            HWID.UseVisualStyleBackColor = true;
            // 
            // KMS38
            // 
            KMS38.AutoSize = true;
            KMS38.Location = new Point(30, 66);
            KMS38.Name = "KMS38";
            KMS38.Size = new Size(61, 19);
            KMS38.TabIndex = 15;
            KMS38.Text = "KMS38";
            KMS38.UseVisualStyleBackColor = true;
            // 
            // WinOnlineKMS
            // 
            WinOnlineKMS.AutoSize = true;
            WinOnlineKMS.Location = new Point(30, 91);
            WinOnlineKMS.Name = "WinOnlineKMS";
            WinOnlineKMS.Size = new Size(87, 19);
            WinOnlineKMS.TabIndex = 16;
            WinOnlineKMS.Text = "Online KMS";
            WinOnlineKMS.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(radioButton1);
            panel1.Controls.Add(HWID);
            panel1.Controls.Add(KMS38);
            panel1.Controls.Add(WinOnlineKMS);
            panel1.Location = new Point(254, 44);
            panel1.Name = "panel1";
            panel1.Size = new Size(184, 137);
            panel1.TabIndex = 18;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(chrome);
            panel2.Controls.Add(CCleaner);
            panel2.Controls.Add(NovaBench);
            panel2.Controls.Add(LibreOffice);
            panel2.Controls.Add(VLC);
            panel2.Location = new Point(36, 44);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 162);
            panel2.TabIndex = 19;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(260, 201);
            label3.Name = "label3";
            label3.Size = new Size(102, 15);
            label3.TabIndex = 20;
            label3.Text = "Office Activation :";
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(radioButton8);
            panel3.Controls.Add(OfficeOnlineKMS);
            panel3.Controls.Add(Ohook);
            panel3.Location = new Point(254, 210);
            panel3.Name = "panel3";
            panel3.Size = new Size(184, 113);
            panel3.TabIndex = 21;
            // 
            // radioButton8
            // 
            radioButton8.AutoSize = true;
            radioButton8.Checked = true;
            radioButton8.Location = new Point(30, 18);
            radioButton8.Name = "radioButton8";
            radioButton8.Size = new Size(69, 19);
            radioButton8.TabIndex = 25;
            radioButton8.TabStop = true;
            radioButton8.Text = "Nothing";
            radioButton8.UseVisualStyleBackColor = true;
            // 
            // OfficeOnlineKMS
            // 
            OfficeOnlineKMS.AutoSize = true;
            OfficeOnlineKMS.Location = new Point(30, 68);
            OfficeOnlineKMS.Name = "OfficeOnlineKMS";
            OfficeOnlineKMS.Size = new Size(87, 19);
            OfficeOnlineKMS.TabIndex = 23;
            OfficeOnlineKMS.Text = "Online KMS";
            OfficeOnlineKMS.UseVisualStyleBackColor = true;
            // 
            // Ohook
            // 
            Ohook.AutoSize = true;
            Ohook.Location = new Point(30, 43);
            Ohook.Name = "Ohook";
            Ohook.Size = new Size(61, 19);
            Ohook.TabIndex = 22;
            Ohook.Text = "Ohook";
            Ohook.UseVisualStyleBackColor = true;
            // 
            // UseCurDir
            // 
            UseCurDir.AutoSize = true;
            UseCurDir.Checked = true;
            UseCurDir.CheckState = CheckState.Checked;
            UseCurDir.Location = new Point(12, 385);
            UseCurDir.Name = "UseCurDir";
            UseCurDir.Size = new Size(139, 19);
            UseCurDir.TabIndex = 23;
            UseCurDir.Text = "Use Current Directory";
            UseCurDir.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(730, 441);
            Controls.Add(UseCurDir);
            Controls.Add(label3);
            Controls.Add(panel3);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(AutoDeleteInstaller);
            Controls.Add(button2);
            Controls.Add(InstallButton);
            Cursor = Cursors.Default;
            Name = "Form1";
            Text = "MultiInstaller";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button InstallButton;
        private Button button2;
        private Label label1;
        private CheckBox chrome;
        private CheckBox CCleaner;
        private CheckBox NovaBench;
        private CheckBox VLC;
        private CheckBox LibreOffice;
        private CheckBox AutoDeleteInstaller;
        private Label label2;
        private RadioButton radioButton1;
        private RadioButton HWID;
        private RadioButton KMS38;
        private RadioButton WinOnlineKMS;
        private Panel panel1;
        private Panel panel2;
        private Label label3;
        private Panel panel3;
        private RadioButton radioButton8;
        private RadioButton Ohook;
        private RadioButton OfficeOnlineKMS;
        private CheckBox UseCurDir;
    }
}
