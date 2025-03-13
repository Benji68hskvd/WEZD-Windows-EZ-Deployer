using System.Diagnostics;
using System.Reflection;

namespace WEZD
{
    public partial class About : Form
    {
        public string SiteURL = "https://github.com/Benji68hskvd/WEZD-Windows-EZ-Deployer";
        public About()
        {
            InitializeComponent();
            Load += About_Load;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

        private void About_Load(object sender, EventArgs e)
        {
            string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "Unknown";
            string infoVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? fileVersion;
            label5.Text = $"WEZD v{infoVersion}";
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
