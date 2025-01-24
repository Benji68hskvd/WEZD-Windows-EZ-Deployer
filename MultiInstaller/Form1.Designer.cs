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
            CancelButton = new Button();
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
            TeamViewer = new CheckBox();
            Firefox = new CheckBox();
            label3 = new Label();
            panel3 = new Panel();
            radioButton8 = new RadioButton();
            OfficeOnlineKMS = new RadioButton();
            Ohook = new RadioButton();
            UseCurDir = new CheckBox();
            panel4 = new Panel();
            label4 = new Label();
            labelStatus = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // InstallButton
            // 
            InstallButton.Location = new Point(585, 536);
            InstallButton.Margin = new Padding(3, 4, 3, 4);
            InstallButton.Name = "InstallButton";
            InstallButton.Size = new Size(114, 36);
            InstallButton.TabIndex = 0;
            InstallButton.Text = "Apply";
            InstallButton.UseVisualStyleBackColor = true;
            InstallButton.Click += InstallButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(706, 536);
            CancelButton.Margin = new Padding(3, 4, 3, 4);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(114, 36);
            CancelButton.TabIndex = 1;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 47);
            label1.Name = "label1";
            label1.Size = new Size(154, 20);
            label1.TabIndex = 2;
            label1.Text = "Application to Install :";
            // 
            // chrome
            // 
            chrome.AutoSize = true;
            chrome.Location = new Point(43, 21);
            chrome.Margin = new Padding(3, 4, 3, 4);
            chrome.Name = "chrome";
            chrome.Size = new Size(83, 24);
            chrome.TabIndex = 3;
            chrome.Text = "Chrome";
            chrome.UseVisualStyleBackColor = true;
            // 
            // CCleaner
            // 
            CCleaner.AutoSize = true;
            CCleaner.Location = new Point(43, 88);
            CCleaner.Margin = new Padding(3, 4, 3, 4);
            CCleaner.Name = "CCleaner";
            CCleaner.Size = new Size(90, 24);
            CCleaner.TabIndex = 4;
            CCleaner.Text = "CCleaner";
            CCleaner.UseVisualStyleBackColor = true;
            // 
            // NovaBench
            // 
            NovaBench.AutoSize = true;
            NovaBench.Location = new Point(43, 121);
            NovaBench.Margin = new Padding(3, 4, 3, 4);
            NovaBench.Name = "NovaBench";
            NovaBench.Size = new Size(106, 24);
            NovaBench.TabIndex = 5;
            NovaBench.Text = "NovaBench";
            NovaBench.UseVisualStyleBackColor = true;
            // 
            // VLC
            // 
            VLC.AutoSize = true;
            VLC.Location = new Point(43, 188);
            VLC.Margin = new Padding(3, 4, 3, 4);
            VLC.Name = "VLC";
            VLC.Size = new Size(55, 24);
            VLC.TabIndex = 6;
            VLC.Text = "VLC";
            VLC.UseVisualStyleBackColor = true;
            // 
            // LibreOffice
            // 
            LibreOffice.AutoSize = true;
            LibreOffice.Location = new Point(43, 155);
            LibreOffice.Margin = new Padding(3, 4, 3, 4);
            LibreOffice.Name = "LibreOffice";
            LibreOffice.Size = new Size(104, 24);
            LibreOffice.TabIndex = 7;
            LibreOffice.Text = "LibreOffice";
            LibreOffice.UseVisualStyleBackColor = true;
            // 
            // AutoDeleteInstaller
            // 
            AutoDeleteInstaller.AutoSize = true;
            AutoDeleteInstaller.Checked = true;
            AutoDeleteInstaller.CheckState = CheckState.Checked;
            AutoDeleteInstaller.Location = new Point(14, 547);
            AutoDeleteInstaller.Margin = new Padding(3, 4, 3, 4);
            AutoDeleteInstaller.Name = "AutoDeleteInstaller";
            AutoDeleteInstaller.Size = new Size(167, 24);
            AutoDeleteInstaller.TabIndex = 10;
            AutoDeleteInstaller.Text = "Auto Delete Installer";
            AutoDeleteInstaller.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(295, 47);
            label2.Name = "label2";
            label2.Size = new Size(148, 20);
            label2.TabIndex = 12;
            label2.Text = "Windows Activation :";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(34, 21);
            radioButton1.Margin = new Padding(3, 4, 3, 4);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(84, 24);
            radioButton1.TabIndex = 13;
            radioButton1.TabStop = true;
            radioButton1.Text = "Nothing";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // HWID
            // 
            HWID.AutoSize = true;
            HWID.Location = new Point(34, 55);
            HWID.Margin = new Padding(3, 4, 3, 4);
            HWID.Name = "HWID";
            HWID.Size = new Size(70, 24);
            HWID.TabIndex = 14;
            HWID.Text = "HWID";
            HWID.UseVisualStyleBackColor = true;
            // 
            // KMS38
            // 
            KMS38.AutoSize = true;
            KMS38.Location = new Point(34, 88);
            KMS38.Margin = new Padding(3, 4, 3, 4);
            KMS38.Name = "KMS38";
            KMS38.Size = new Size(76, 24);
            KMS38.TabIndex = 15;
            KMS38.Text = "KMS38";
            KMS38.UseVisualStyleBackColor = true;
            // 
            // WinOnlineKMS
            // 
            WinOnlineKMS.AutoSize = true;
            WinOnlineKMS.Location = new Point(34, 121);
            WinOnlineKMS.Margin = new Padding(3, 4, 3, 4);
            WinOnlineKMS.Name = "WinOnlineKMS";
            WinOnlineKMS.Size = new Size(107, 24);
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
            panel1.Location = new Point(290, 59);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(210, 182);
            panel1.TabIndex = 18;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(TeamViewer);
            panel2.Controls.Add(chrome);
            panel2.Controls.Add(Firefox);
            panel2.Controls.Add(CCleaner);
            panel2.Controls.Add(VLC);
            panel2.Controls.Add(NovaBench);
            panel2.Controls.Add(LibreOffice);
            panel2.Location = new Point(41, 59);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(228, 271);
            panel2.TabIndex = 19;
            // 
            // TeamViewer
            // 
            TeamViewer.AutoSize = true;
            TeamViewer.Location = new Point(43, 221);
            TeamViewer.Margin = new Padding(3, 4, 3, 4);
            TeamViewer.Name = "TeamViewer";
            TeamViewer.Size = new Size(112, 24);
            TeamViewer.TabIndex = 8;
            TeamViewer.Text = "TeamViewer";
            TeamViewer.UseVisualStyleBackColor = true;
            // 
            // Firefox
            // 
            Firefox.AutoSize = true;
            Firefox.Location = new Point(43, 55);
            Firefox.Margin = new Padding(3, 4, 3, 4);
            Firefox.Name = "Firefox";
            Firefox.Size = new Size(76, 24);
            Firefox.TabIndex = 9;
            Firefox.Text = "Firefox";
            Firefox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(295, 267);
            label3.Name = "label3";
            label3.Size = new Size(127, 20);
            label3.TabIndex = 20;
            label3.Text = "Office Activation :";
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(radioButton8);
            panel3.Controls.Add(OfficeOnlineKMS);
            panel3.Controls.Add(Ohook);
            panel3.Location = new Point(290, 280);
            panel3.Margin = new Padding(3, 4, 3, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(210, 150);
            panel3.TabIndex = 21;
            // 
            // radioButton8
            // 
            radioButton8.AutoSize = true;
            radioButton8.Checked = true;
            radioButton8.Location = new Point(34, 24);
            radioButton8.Margin = new Padding(3, 4, 3, 4);
            radioButton8.Name = "radioButton8";
            radioButton8.Size = new Size(84, 24);
            radioButton8.TabIndex = 25;
            radioButton8.TabStop = true;
            radioButton8.Text = "Nothing";
            radioButton8.UseVisualStyleBackColor = true;
            // 
            // OfficeOnlineKMS
            // 
            OfficeOnlineKMS.AutoSize = true;
            OfficeOnlineKMS.Location = new Point(34, 91);
            OfficeOnlineKMS.Margin = new Padding(3, 4, 3, 4);
            OfficeOnlineKMS.Name = "OfficeOnlineKMS";
            OfficeOnlineKMS.Size = new Size(107, 24);
            OfficeOnlineKMS.TabIndex = 23;
            OfficeOnlineKMS.Text = "Online KMS";
            OfficeOnlineKMS.UseVisualStyleBackColor = true;
            // 
            // Ohook
            // 
            Ohook.AutoSize = true;
            Ohook.Location = new Point(34, 57);
            Ohook.Margin = new Padding(3, 4, 3, 4);
            Ohook.Name = "Ohook";
            Ohook.Size = new Size(74, 24);
            Ohook.TabIndex = 22;
            Ohook.Text = "Ohook";
            Ohook.UseVisualStyleBackColor = true;
            // 
            // UseCurDir
            // 
            UseCurDir.AutoSize = true;
            UseCurDir.Checked = true;
            UseCurDir.CheckState = CheckState.Checked;
            UseCurDir.Location = new Point(14, 513);
            UseCurDir.Margin = new Padding(3, 4, 3, 4);
            UseCurDir.Name = "UseCurDir";
            UseCurDir.Size = new Size(172, 24);
            UseCurDir.TabIndex = 23;
            UseCurDir.Text = "Use Current Directory";
            UseCurDir.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Location = new Point(520, 59);
            panel4.Margin = new Padding(3, 4, 3, 4);
            panel4.Name = "panel4";
            panel4.Size = new Size(482, 439);
            panel4.TabIndex = 24;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(525, 47);
            label4.Name = "label4";
            label4.Size = new Size(129, 20);
            label4.TabIndex = 0;
            label4.Text = "Office Instalation :";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new Point(92, 410);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(48, 20);
            labelStatus.TabIndex = 25;
            labelStatus.Text = "V1.2.0";
            labelStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1061, 595);
            Controls.Add(labelStatus);
            Controls.Add(label4);
            Controls.Add(panel4);
            Controls.Add(UseCurDir);
            Controls.Add(label3);
            Controls.Add(panel3);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(AutoDeleteInstaller);
            Controls.Add(CancelButton);
            Controls.Add(InstallButton);
            Margin = new Padding(3, 4, 3, 4);
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
        private Button CancelButton;
        private Label label1;
        public CheckBox chrome;
        public CheckBox CCleaner;
        public CheckBox NovaBench;
        public CheckBox VLC;
        public CheckBox LibreOffice;
        private CheckBox AutoDeleteInstaller;
        private Label label2;
        private RadioButton radioButton1;
        public RadioButton HWID;
        public RadioButton KMS38;
        public RadioButton WinOnlineKMS;
        private Panel panel1;
        private Panel panel2;
        private Label label3;
        private Panel panel3;
        private RadioButton radioButton8;
        public RadioButton Ohook;
        public RadioButton OfficeOnlineKMS;
        public CheckBox UseCurDir;
        public CheckBox TeamViewer;
        public CheckBox Firefox;
        private Panel panel4;
        private Label label4;
        private static Label labelStatus;
    }
}
