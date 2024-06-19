using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using HtmlAgilityPack;

namespace MultiInstaller
{
    internal static class Program
    {

        [STAThread]
        static async Task Main()
        {
            ApplicationConfiguration.Initialize();
            Form1 form = new Form1();
            Application.Run(form);

            if (form.Install == true)
            {
                await CheckInstall(form.IsChromeChecked(), false, false, "https://dl.google.com/chrome/install/googlechromestandaloneenterprise64.msi", "", "chrome_installer.msi", "Chrome");
                await CheckInstall(form.IsCCleanerChecked(), false, false, "https://bits.avcdn.net/productfamily_CCLEANER/insttype_BUSINESS_32/platform_WIN_MSI/installertype_ONLINE/build_RELEASE", "", "ccleaner_installer.msi", "CCleaner");
                await CheckInstall(form.IsNovaBenchChecked(), false, false, "https://cdn.novabench.net/novabench.msi", "", "novabench_installer.msi", "NovaBench");
                await CheckInstall(form.IsLibreOfficeChecked(), true, true, "https://miroir.univ-lorraine.fr/documentfoundation/libreoffice/stable/", "/win/x86_64/", "libreoffice_installer.msi", "LibreOffice");
                await CheckInstall(form.IsVLCChecked(), false, true, "https://get.videolan.org/vlc/last/win64/", "", "vlc_installer.msi", "VLC");

                if (form.IsHWIDChecked() == true)
                {
                    ActivationCommand(form.IsUseCurDirChecked(), " /HWID");
                }
                if (form.IsKMS38Checked() == true)
                {
                    ActivationCommand(form.IsUseCurDirChecked(), " /KMS38");
                }
                if (form.IsWinOnlineKMSChecked() == true)
                {
                    ActivationCommand(form.IsUseCurDirChecked(), " /KMS-Windows");
                }
                if (form.IsOhookChecked() == true)
                {
                    ActivationCommand(form.IsUseCurDirChecked(), " /Ohook");
                }
                if (form.IsOfficeOnlineKMSChecked() == true)
                {
                    ActivationCommand(form.IsUseCurDirChecked(), " /KMS-Office");
                }
            }
        }

        static async Task CheckInstall(bool isChecked, bool searchVersion, bool search, string url, string endUrl, string installerName, string packageName)
        {
            if (isChecked == true)
            {
                await Install(searchVersion, search, url, endUrl, installerName, packageName);
            }
        }

        static async void ActivationCommand(bool useCurrentDirectory, string command)
        {
            string ScriptFile = "\\MAS_AIO-CRC32_31F7FD1E.cmd";
            string currentDirectory = Directory.GetCurrentDirectory();

            if (useCurrentDirectory == true)
            {
                ProcessStartInfo currentDeleteInfo = new ProcessStartInfo("cmd.exe", "/C " + currentDirectory + ScriptFile + command);
                var currentDeleteProcess = Process.Start(currentDeleteInfo);
                currentDeleteProcess.WaitForExit();
            }
            else
            {
                var FilePath = "C:\\Users\\" + Environment.UserName + "\\Downloads\\" + ScriptFile;

                ProcessStartInfo deleteInfo = new ProcessStartInfo("cmd.exe", "/C " + FilePath + command);
                var deleteProcess = Process.Start(deleteInfo);
                deleteProcess.WaitForExit();
            }
        }

        //Installation Script ---------------------------------------------------------------------------------------------------------------
        static async Task Install(bool searchVersion, bool searchMsi, string url, string endUrl, string installerName, string packageName)
        {
            var DownloadPath = "C:\\Users\\" + Environment.UserName + "\\Downloads\\";

            var downloadUrl = url;

            if (searchVersion == true)
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(response);

                    var links = doc.DocumentNode.SelectNodes("//a[@href]")
                                  .Select(node => node.Attributes["href"].Value)
                                  .Where(href => href.EndsWith("/"))
                                  .Select(href => href.Trim('/'))
                                  .Where(href => Version.TryParse(href, out _))
                                  .Select(href => new Version(href))
                                  .ToList();

                    if (links.Count == 0)
                    {
                        MessageBox.Show("Aucune version trouvée.");
                        return;
                    }

                    var highestVersion = links.Max();

                    downloadUrl = url + highestVersion + endUrl;
                }
            }

            var msiDownloadUrl = url;

            if (searchMsi == true)
            {
                WebClient client = new WebClient();
                string pageContent = client.DownloadString(downloadUrl);

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageContent);

                // Find the link to the MSI file
                var nodes = doc.DocumentNode.SelectNodes("//a[@href]");
                var downloadNode = nodes.FirstOrDefault(n => n.Attributes["href"].Value.EndsWith(".msi"));
                if (downloadNode != null)
                {
                    msiDownloadUrl = downloadUrl + downloadNode.Attributes["href"].Value;
                }
            }

            System.Net.WebClient myWebClient = new System.Net.WebClient();
            AutoClosingMessageBox.Show(packageName + " Download Started", timeout: 2000);
            myWebClient.DownloadFile(msiDownloadUrl, DownloadPath + installerName);

            AutoClosingMessageBox.Show(packageName + " Download Complete", timeout: 2000);

            string msiPath = DownloadPath + installerName;

            string arguments = $"/passive /i \"{msiPath}\"";

            ProcessStartInfo startInfo = new ProcessStartInfo("msiexec.exe", arguments);

            var process = Process.Start(startInfo);
            process.WaitForExit();

            Form1 form = new Form1();

            if (form.IsAutoDeleteInstallerChecked() == true)
            {
                string filePath = DownloadPath + installerName;

                string command = $"del \"{filePath}\"";

                ProcessStartInfo deleteInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
                var deleteProcess = Process.Start(deleteInfo);
                deleteProcess.WaitForExit();
            }
        }
    }
}