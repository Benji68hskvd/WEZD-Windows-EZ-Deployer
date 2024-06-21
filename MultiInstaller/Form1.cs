using System.Net.Quic;

namespace MultiInstaller
{
    public partial class Form1 : Form
    {
        public bool Install { get; set; }

        public Form1()
        {
            InitializeComponent();
            Install = false;
        }

        //Program To Install ----------------------------------

        public bool IsChromeChecked()
        {
            return chrome.Checked;
        }

        public bool IsFirefoxChecked()
        {
            return Firefox.Checked;
        }

        public bool IsTeamViewerChecked()
        {
        return TeamViewer.Checked;
        }

        public bool IsCCleanerChecked()
        {
            return CCleaner.Checked;
        }

        public bool IsNovaBenchChecked()
        {
            return NovaBench.Checked;
        }

        public bool IsLibreOfficeChecked()
        {
            return LibreOffice.Checked;
        }

        public bool IsVLCChecked()
        {
            return VLC.Checked;
        }

        //Option -----------------------------------------------

        public bool IsAutoDeleteInstallerChecked()
        {
            return AutoDeleteInstaller.Checked;
        }

        public bool IsUseCurDirChecked()
        {
            return UseCurDir.Checked;
        }

        //Windows Activation -----------------------------------

        public bool IsHWIDChecked()
        {
            return HWID.Checked;
        }

        public bool IsKMS38Checked()
        {
            return KMS38.Checked;
        }

        public bool IsWinOnlineKMSChecked()
        {
            return WinOnlineKMS.Checked;
        }

        //Office Activation ---------------------------------------

        public bool IsOhookChecked()
        {
            return Ohook.Checked;
        }

        public bool IsOfficeOnlineKMSChecked()
        {
            return OfficeOnlineKMS.Checked;
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            Install = true;
            this.Close();
        }

        private void CancelButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
