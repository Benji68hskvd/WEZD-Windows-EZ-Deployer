using System.Diagnostics;

namespace WEZD
{
    public partial class About : Form
    {
        public string SiteURL = "https://github.com/Benji68hskvd/WEZD-Windows-EZ-Deployer";
        public About()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = SiteURL,
                UseShellExecute = true
            });
        }
    }
}
