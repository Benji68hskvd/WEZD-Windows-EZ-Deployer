using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using HtmlAgilityPack;
using System.Xml.Linq;
using System.Runtime.InteropServices;

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
                await CheckInstall(form.IsChromeChecked(), "https://dl.google.com/chrome/install/googlechromestandaloneenterprise64.msi", "", "/", "", "", "chrome_installer.msi", "Chrome");
                await CheckInstall(form.IsFirefoxChecked(), "https://ftp.mozilla.org/pub/firefox/releases/", "/pub/firefox/releases/", "/pub/firefox/releases/", "b", "/win64/fr/", "firefox_installer.msi", "Firefox");
                await CheckInstall(form.IsCCleanerChecked(), "https://bits.avcdn.net/productfamily_CCLEANER/insttype_BUSINESS_32/platform_WIN_MSI/installertype_ONLINE/build_RELEASE/.msi/", "", "/", "", "", "ccleaner_installer.msi", "CCleaner");
                await CheckInstall(form.IsNovaBenchChecked(), "https://cdn.novabench.net/novabench.msi", "", "/", "", "", "novabench_installer.msi", "NovaBench");
                await CheckInstall(form.IsLibreOfficeChecked(), "https://miroir.univ-lorraine.fr/documentfoundation/libreoffice/stable/", "", "/", "", "/win/x86_64/", "libreoffice_installer.msi", "LibreOffice");
                await CheckInstall(form.IsVLCChecked(), "https://get.videolan.org/vlc/last/win64/", "", "/", "", "", "vlc_installer.msi", "VLC");
                await CheckInstall(form.IsTeamViewerChecked(), "https://dl.teamviewer.com/download/version_15x/TeamViewer_Setup_x64.exe", "", "/", "", "", "TeamViewer.exe", "TeamViewer");

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
                    ActivationCommand(form.IsUseCurDirChecked(), " /KMS-Windows /KMS-RenewalTask");
                }
                if (form.IsOhookChecked() == true)
                {
                    ActivationCommand(form.IsUseCurDirChecked(), " /Ohook");
                }
                if (form.IsOfficeOnlineKMSChecked() == true)
                {
                    ActivationCommand(form.IsUseCurDirChecked(), " /KMS-Office /KMS-RenewalTask");
                }
            }
        }

        static async Task CheckInstall(bool isChecked, string url, string hrefNodes, string hrefReplace, string ignoreVersionName, string endUrl, string installerName, string packageName)
        {
            if (isChecked == true)
            {
                await Install(url, hrefNodes, hrefReplace, ignoreVersionName, endUrl, installerName, packageName);
            }
        }

        //Activation Script ----------------------------------------------------------------------------------------------------------

        static async void ActivationCommand(bool useCurrentDirectory, string command)
        {
            string url = "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/master/MAS/All-In-One-Version/MAS_AIO-CRC32_31F7FD1E.cmd";
            string currentDirectory = Directory.GetCurrentDirectory();
            string ScriptFile = "\\MAS_AIO.cmd";

            if (File.Exists(currentDirectory + ScriptFile) == false)
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                myWebClient.DownloadFile(url, currentDirectory + ScriptFile);
            }

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
        static async Task Install(string url, string hrefNodes, string hrefReplace, string ignoreVersionName, string endUrl, string installerName, string packageName)
        {
            var downloadPath = "C:\\Users\\" + Environment.UserName + "\\Downloads\\";

            bool containsMsi = url.Contains(".msi");
            bool containsExe = url.Contains(".exe");

            if (containsMsi || containsExe)
            {
                await InstallPackage(url, downloadPath, packageName, installerName);
                return;
            }

            try
            {
                HttpClient client = new HttpClient();

                string pageContent = await client.GetStringAsync(url);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageContent);

                var versionNodes = doc.DocumentNode.SelectNodes($"//a[starts-with(@href, '{hrefNodes}') and contains(@href, '.')]");
                List<string> versions = new List<string>();

                if (versionNodes != null)
                {
                    versions = versionNodes
                                .Select(node => node.GetAttributeValue("href", ""))
                                .Select(href => href.Replace(hrefReplace, "").Trim('/'))
                                .Where(href => Version.TryParse(href, out _)) // Vérifiez que href est une version valide
                                .ToList();
                }

                versions = versions.Where(version => !version.Contains($"'{ignoreVersionName}'")).ToList();
                versions.Sort((x, y) => new Version(x).CompareTo(new Version(y)));
                string latestVersion = versions.LastOrDefault();

                url = url + latestVersion + endUrl;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des versions: {ex.Message}");
            }
            try
            {
                HttpClient client = new HttpClient();
                string pageContent = await client.GetStringAsync(url);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageContent);

                var nodes = doc.DocumentNode.SelectSingleNode("//a[contains(@href, '.msi')]");

                string originalHref = nodes.GetAttributeValue("href", "");
                string fullFileUrl = new Uri(new Uri(url), originalHref).ToString();
                string modifiedHref = fullFileUrl.Replace(" ", "%20");

                url = modifiedHref;

                await InstallPackage(url, downloadPath, packageName, installerName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du téléchargement: {ex.Message}");
            }
        }
        static async Task InstallPackage(string url, string downloadPath, string packageName, string installerName)
        {
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            AutoClosingMessageBox.Show(packageName + " Download Started", timeout: 2000);
            myWebClient.DownloadFile(url, downloadPath + installerName);

            AutoClosingMessageBox.Show(packageName + " Download Complete", timeout: 2000);

            string filePath = downloadPath + installerName;

            bool containsExe = url.Contains(".exe");

            if (containsExe == true)
            {
                ProcessStartInfo installInfo = new ProcessStartInfo("cmd.exe", "/C " + filePath);
                var installProcess = Process.Start(installInfo);
                installProcess.WaitForExit();
            }
            else
            {
                string msiPath = downloadPath + installerName;

                string arguments = $"/passive /i \"{msiPath}\"";

                ProcessStartInfo startInfo = new ProcessStartInfo("msiexec.exe", arguments);

                var process = Process.Start(startInfo);
                process.WaitForExit();
            }

            ProcessStartInfo deleteInfo = new ProcessStartInfo($"cmd.exe", "/C " + $"del \"{filePath}\"");
            var deleteProcess = Process.Start(deleteInfo);
            deleteProcess.WaitForExit();
        }
    }
}