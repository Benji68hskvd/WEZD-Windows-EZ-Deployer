using System.Diagnostics;
using HtmlDocument = WEZD.HtmlAgilityPack.HtmlAgilityPack.HtmlDocument;
// ReSharper disable InconsistentNaming

namespace WEZD
{
    public class Functions
    {
        public async Task Install(Form1 form)
        {
            try
            {
                Debug.WriteLine("start install");
                // vérifier directement les checkboxes sur l'instance actuelle du formulaire
                if (form.chrome.Checked)
                {
                    Debug.WriteLine("install chrome");
                    await Install("https://dl.google.com/chrome/install/googlechromestandaloneenterprise64.msi", "", "/", "", "", "chrome_installer.msi", "Chrome");
                }
                if (form.Firefox.Checked)
                {
                    Debug.WriteLine("install firefox");
                    await Install("https://ftp.mozilla.org/pub/firefox/releases/", "/pub/firefox/releases/", "/pub/firefox/releases/", "b", "/win64/fr/", "firefox_installer.msi", "Firefox");
                }
                if (form.CCleaner.Checked)
                {
                    Debug.WriteLine("install ccleaner");
                    await Install("https://bits.avcdn.net/productfamily_CCLEANER/insttype_BUSINESS_32/platform_WIN_MSI/installertype_ONLINE/build_RELEASE/.msi/", "", "/", "", "", "ccleaner_installer.msi", "CCleaner");
                }
                if (form.NovaBench.Checked)
                {
                    Debug.WriteLine("install novabench");
                    await Install("https://cdn.novabench.net/novabench.msi", "", "/", "", "", "novabench_installer.msi", "NovaBench");
                }
                if (form.LibreOffice.Checked)
                {
                    Debug.WriteLine("install libreoffice");
                    await Install("https://miroir.univ-lorraine.fr/documentfoundation/libreoffice/stable/", "", "/", "", "/win/x86_64/", "libreoffice_installer.msi", "LibreOffice");
                }
                if (form.VLC.Checked)
                {
                    Debug.WriteLine("install vlc");
                    await InstallVLC(); // appel spécifique pour VLC
                }
                if (form.TeamViewer.Checked)
                {
                    Debug.WriteLine("install teamviewer");
                    await Install("https://dl.teamviewer.com/download/version_15x/TeamViewer_Setup_x64.exe", "", "/", "", "", "TeamViewer.exe", "TeamViewer");
                }
                // ajout de l'installation d'Office
                if (form.Word.Checked || form.Excel.Checked || form.PowerPoint.Checked || form.Outlook.Checked)
                {
                    Debug.WriteLine("install office");

                    // créer une instance de la classe Office
                    Office officeInstaller = new();

                    // récupérer les options sélectionnées dans le formulaire
                    bool isX64 = form.x64.Checked;
                    bool installWord = form.Word.Checked;
                    bool installExcel = form.Excel.Checked;
                    bool installPowerPoint = form.PowerPoint.Checked;
                    bool installOutlook = form.Outlook.Checked;

                    // appeler la méthode d'installation
                    officeInstaller.Install(isX64, installWord, installExcel, installPowerPoint, installOutlook);
                }
                // activation Windows
                if (form.HWID.Checked)
                {
                    Debug.WriteLine("using hwid");
                    ActivationCommand(form.UseCurDir.Checked, " /HWID");
                }
                if (form.KMS38.Checked)
                {
                    Debug.WriteLine("using kms38");
                    ActivationCommand(form.UseCurDir.Checked, " /KMS38");
                }
                if (form.WinOnlineKMS.Checked)
                {
                    Debug.WriteLine("using online kms windows");
                    ActivationCommand(form.UseCurDir.Checked, " /KMS-Windows /KMS-RenewalTask");
                }
                // activation Office
                if (form.Ohook.Checked)
                {
                    Debug.WriteLine("install ohook");
                    ActivationCommand(form.UseCurDir.Checked, " /Ohook");
                }
                if (form.OfficeOnlineKMS.Checked)
                {
                    Debug.WriteLine("install online kms office");
                    ActivationCommand(form.UseCurDir.Checked, " /KMS-Office /KMS-RenewalTask");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Erreur : {e}");
            }
        }

        //Activation Script ----------------------------------------------------------------------------------------------------------

        //public static async void ActivationCommand(bool useCurrentDirectory, string command)
        //{
        //    Form1 f = new();
        //    f.UpdateStatusLabel("Activate...");
        //    //string url = "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/master/MAS/All-In-One-Version/MAS_AIO-CRC32_31F7FD1E.cmd";
        //    string url = "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/refs/heads/master/MAS/All-In-One-Version-KL/MAS_AIO.cmd";
        //    string currentDirectory = Directory.GetCurrentDirectory();
        //    string ScriptFile = "\\MAS_AIO.cmd";

        //    if (File.Exists(currentDirectory + ScriptFile) == false)
        //    {
        //        System.Net.WebClient myWebClient = new();
        //        myWebClient.DownloadFile(url, currentDirectory + ScriptFile);
        //    }
        //    if (useCurrentDirectory)
        //    {
        //        ProcessStartInfo currentDeleteInfo = new("cmd.exe", "/C " + currentDirectory + ScriptFile + command);
        //        var currentDeleteProcess = Process.Start(currentDeleteInfo);
        //        currentDeleteProcess.WaitForExit();
        //    }
        //    else
        //    {
        //        string FilePath = "C:\\Users\\" + Environment.UserName + "\\Downloads\\" + ScriptFile;

        //        ProcessStartInfo deleteInfo = new("cmd.exe", "/C " + FilePath + command);
        //        var deleteProcess = Process.Start(deleteInfo);
        //        deleteProcess.WaitForExit();
        //    }
        //}

        public static async void ActivationCommand(bool useCurrentDirectory, string command)
        {
            try
            {
                Form1 f = new();
                f.UpdateStatusLabel("Activate...");

                string url = "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/refs/heads/master/MAS/All-In-One-Version-KL/MAS_AIO.cmd";
                string currentDirectory = Directory.GetCurrentDirectory();
                string scriptFile = Path.Combine(currentDirectory, "MAS_AIO.cmd");

                // Télécharge le fichier si non présent
                if (!File.Exists(scriptFile))
                {
                    try
                    {
                        using HttpClient httpClient = new();
                        using HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                        response.EnsureSuccessStatusCode(); // Vérifie le succès de la réponse

                        await using Stream contentStream = await response.Content.ReadAsStreamAsync();
                        await using FileStream fileStream = new(scriptFile, FileMode.Create, FileAccess.Write, FileShare.None);

                        await contentStream.CopyToAsync(fileStream);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors du téléchargement du script : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Exécute le script selon le répertoire choisi
                string filePath = useCurrentDirectory ? scriptFile
                    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "MAS_AIO.cmd");

                try
                {
                    ProcessStartInfo processInfo = new("cmd.exe", $"/C \"{filePath}\" {command}")
                    {
                        UseShellExecute = true, // Nécessaire pour les scripts externes
                        CreateNoWindow = false // Facultatif, mais permet de voir le script s'exécuter si besoin
                    };
                    var process = Process.Start(processInfo);
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'exécution du script : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Erreur : {e.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static async Task CheckInstall(string url, string hrefNodes, string hrefReplace, string ignoreVersionName, string endUrl, string installerName, string packageName)
        {
            //if (isChecked == true)
            //{
                await Install(url, hrefNodes, hrefReplace, ignoreVersionName, endUrl, installerName, packageName);
            //}
        }

        //Installation Script ---------------------------------------------------------------------------------------------------------------
        private static async Task Install(string url, string hrefNodes, string hrefReplace, string ignoreVersionName, string endUrl, string installerName, string packageName)
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
                HttpClient client = new();
                string pageContent = await client.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(pageContent);

                var versionNodes = doc.DocumentNode.SelectNodes($"//a[starts-with(@href, '{hrefNodes}') and contains(@href, '.')]");
                List<string> versions = [];

                if (versionNodes != null)
                {
                    versions = versionNodes
                                .Select(node => node.GetAttributeValue("href", ""))
                                .Select(href => href.Replace(hrefReplace, "").Trim('/'))
                                .Where(href => Version.TryParse(href, out _))
                                .ToList();
                }

                versions = versions.Where(version => !version.Contains(ignoreVersionName)).ToList();
                versions.Sort((x, y) => new Version(x).CompareTo(new Version(y)));
                string latestVersion = versions.LastOrDefault();

                url = url + latestVersion + endUrl;
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                MessageBox.Show(@"Erreur de connexion internet: Veuillez vérifier votre connexion et réessayer.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Erreur lors de la récupération des versions: {ex.Message}");
                return;
            }

            try
            {
                HttpClient client = new();
                string pageContent = await client.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(pageContent);

                var nodes = doc.DocumentNode.SelectSingleNode("//a[contains(@href, '.msi')]");

                string originalHref = nodes.GetAttributeValue("href", "");
                string fullFileUrl = new Uri(new Uri(url), originalHref).ToString();
                string modifiedHref = fullFileUrl.Replace(" ", "%20");

                url = modifiedHref;

                await InstallPackage(url, downloadPath, packageName, installerName);
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                MessageBox.Show(@"Erreur de connexion internet: Veuillez vérifier votre connexion et réessayer.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Erreur lors du téléchargement: {ex.Message}");
            }
        }

        private static async Task InstallVLC()
        {
            Form1 f = new();
            f.UpdateStatusLabel("Download VLC...");

            var downloadPath = "C:\\Users\\" + Environment.UserName + "\\Downloads\\";
            var baseVlcUrl = "https://download.videolan.org/vlc/";
            var installerName = "vlc_installer.msi";
            var packageName = "VLC";

            try
            {
                HttpClient client = new();
                // récupérer la liste des versions disponibles
                string pageContent = await client.GetStringAsync(baseVlcUrl);
                var doc = new HtmlDocument();
                doc.LoadHtml(pageContent);
                var versionNodes = doc.DocumentNode.SelectNodes("//a[starts-with(@href, '3.0.') and contains(@href, '/')]");
                List<string> versions = new();

                if (versionNodes != null)
                {
                    versions = versionNodes
                                .Select(node => node.GetAttributeValue("href", "").Trim('/'))
                                .Where(href => Version.TryParse(href, out _))
                                .ToList();
                }

                // trier les versions par ordre décroissant
                versions.Sort((x, y) => new Version(y).CompareTo(new Version(x)));

                // vérifier chaque version pour trouver un fichier MSI valide
                foreach (var version in versions)
                {
                    string versionUrl = $"{baseVlcUrl}{version}/win64/";
                    try
                    {
                        string versionPageContent = await client.GetStringAsync(versionUrl);
                        var versionDoc = new HtmlDocument();
                        versionDoc.LoadHtml(versionPageContent);

                        var msiNode = versionDoc.DocumentNode.SelectSingleNode($"//a[contains(@href, 'vlc-{version}-win64.msi')]");

                        if (msiNode != null)
                        {
                            string originalHref = msiNode.GetAttributeValue("href", "");
                            string fullFileUrl = new Uri(new Uri(versionUrl), originalHref).ToString();

                            // téléchargement et installation
                            await InstallPackage(fullFileUrl, downloadPath, packageName, installerName);
                            return;
                        }
                    }
                    catch
                    {
                        // ignorer les erreurs et passer à la version suivante
                        continue;
                    }
                }

                // si aucune version valide n'a été trouvée
                MessageBox.Show("Impossible de trouver un fichier MSI valide pour VLC.");
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                MessageBox.Show("Erreur de connexion internet: Veuillez vérifier votre connexion et réessayer.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche des versions de VLC: {ex.Message}");
            }
        }

        //private static async Task InstallPackage(string url, string downloadPath, string packageName, string installerName)
        //{
        //    Form1 f = new();
        //    f.UpdateStatusLabel($"Download {packageName}...");

        //    System.Net.WebClient myWebClient = new();
        //    await myWebClient.DownloadFileTaskAsync(url, downloadPath + installerName);

        //    string filePath = downloadPath + installerName;

        //    bool containsExe = url.Contains(".exe");

        //    if (containsExe)
        //    {
        //        f.UpdateStatusLabel($"Install {packageName}...");
        //        ProcessStartInfo installInfo = new("cmd.exe", "/C " + filePath);
        //        var installProcess = Process.Start(installInfo);
        //        installProcess.WaitForExit();
        //    }
        //    else
        //    {
        //        f.UpdateStatusLabel($"Install {packageName}...");

        //        string msiPath = downloadPath + installerName;

        //        string arguments = $"/passive /i \"{msiPath}\"";

        //        ProcessStartInfo startInfo = new("msiexec.exe", arguments);

        //        var process = Process.Start(startInfo);
        //        process.WaitForExit();
        //    }

        //    ProcessStartInfo deleteInfo = new($"cmd.exe", "/C " + $"del \"{filePath}\"");
        //    var deleteProcess = Process.Start(deleteInfo);
        //    deleteProcess.WaitForExit();
        //}

        //private static async Task InstallPackage(string url, string downloadPath, string packageName, string installerName)
        //{
        //    Form1 f = new();
        //    f.UpdateStatusLabel($"Download {packageName}...");
        //    System.Net.WebClient myWebClient = new();
        //    await myWebClient.DownloadFileTaskAsync(url, Path.Combine(downloadPath, installerName));
        //    string filePath = Path.Combine(downloadPath, installerName);

        //    // Installation silencieuse spécifique pour TeamViewer
        //    if (packageName.Equals("TeamViewer", StringComparison.OrdinalIgnoreCase))
        //    {
        //        f.UpdateStatusLabel($"Install {packageName} en arrière-plan...");
        //        ProcessStartInfo psInfo = new()
        //        {
        //            FileName = "powershell",
        //            Arguments = $"-Command \"Start-Process -FilePath '{filePath}' -ArgumentList '/S' -Wait\"",
        //            UseShellExecute = false,
        //            CreateNoWindow = true
        //        };
        //        var psProcess = Process.Start(psInfo);
        //        psProcess.WaitForExit();
        //    }
        //    else
        //    {
        //        // Installation classique pour les autres packages
        //        bool containsExe = url.Contains(".exe");
        //        if (containsExe)
        //        {
        //            f.UpdateStatusLabel($"Install {packageName}...");
        //            ProcessStartInfo installInfo = new("cmd.exe", "/C " + filePath);
        //            var installProcess = Process.Start(installInfo);
        //            installProcess.WaitForExit();
        //        }
        //        else
        //        {
        //            f.UpdateStatusLabel($"Install {packageName}...");
        //            string msiPath = downloadPath + installerName;
        //            string arguments = $"/passive /i \"{msiPath}\"";
        //            ProcessStartInfo startInfo = new("msiexec.exe", arguments);
        //            var process = Process.Start(startInfo);
        //            process.WaitForExit();
        //        }
        //    }
        //    // Supprimer l'installateur après installation
        //    f.UpdateStatusLabel($"Cleaning up {packageName} installer...");
        //    ProcessStartInfo deleteInfo = new($"cmd.exe", "/C " + $"del \"{filePath}\"");
        //    var deleteProcess = Process.Start(deleteInfo);
        //    deleteProcess.WaitForExit();
        //}

        private static async Task InstallPackage(string url, string downloadPath, string packageName, string installerName)
        {
            Form1 f = new();
            f.UpdateStatusLabel($"Downloading {packageName}...");

            // Chemin complet pour l'installateur
            string filePath = Path.Combine(downloadPath, installerName);
            try
            {
                // Téléchargement du fichier avec HttpClient
                using HttpClient h = new();
                using HttpResponseMessage r = await h.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                r.EnsureSuccessStatusCode(); // Vérifie que la requête est réussie
                await using Stream c = await r.Content.ReadAsStreamAsync();
                await using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

                await c.CopyToAsync(fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du téléchargement de {packageName} : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Installation silencieuse pour TeamViewer
            if (packageName.Equals("TeamViewer", StringComparison.OrdinalIgnoreCase))
            {
                f.UpdateStatusLabel($"Installing {packageName} in the background...");
                ProcessStartInfo ps = new()
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"Start-Process -FilePath '{filePath}' -ArgumentList '/S' -Wait\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var psp = Process.Start(ps);
                psp.WaitForExit();
            }
            else
            {
                // Installation classique pour les autres packages
                bool containsExe = filePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase);

                if (containsExe)
                {
                    f.UpdateStatusLabel($"Installing {packageName}...");
                    ProcessStartInfo i = new("cmd.exe", "/C " + filePath)
                    {
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    var p = Process.Start(i);
                    p.WaitForExit();
                }
                else
                {
                    f.UpdateStatusLabel($"Installing {packageName}...");
                    string a = $"/passive /i \"{filePath}\"";
                    ProcessStartInfo s = new("msiexec.exe", a)
                    {
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    var p = Process.Start(s);
                    p.WaitForExit();
                }
            }
            // Supprime l'installateur après installation
            try
            {
                f.UpdateStatusLabel($"Cleaning up {packageName} installer...");
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Erreur lors de la suppression de l'installateur de {packageName} : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
