using System.Diagnostics;
using WEZD.HtmlAgilityPack.HtmlAgilityPack;
using static System.Collections.Specialized.BitVector32;
using HtmlDocument = WEZD.HtmlAgilityPack.HtmlAgilityPack.HtmlDocument;
// ReSharper disable InconsistentNaming

namespace WEZD
{
    public class Functions
    {
        public async Task Install(Form1 f)
        {
            try
            {
                Debug.WriteLine("start install");
                // vérifier directement les checkboxes sur l'instance actuelle du formulaire
                if (f.chrome.Checked)
                {
                    Debug.WriteLine("install chrome");
                    await Install("https://dl.google.com/chrome/install/googlechromestandaloneenterprise64.msi", "",
                        "/", "", "", "chrome_installer.msi", "Chrome");
                }

                if (f.Firefox.Checked)
                {
                    Debug.WriteLine("install firefox");
                    await Install("https://ftp.mozilla.org/pub/firefox/releases/", "/pub/firefox/releases/",
                        "/pub/firefox/releases/", "b", "/win64/fr/", "firefox_installer.msi", "Firefox");
                }

                if (f.CCleaner.Checked)
                {
                    Debug.WriteLine("install ccleaner");
                    await Install(
                        "https://bits.avcdn.net/productfamily_CCLEANER/insttype_BUSINESS_32/platform_WIN_MSI/installertype_ONLINE/build_RELEASE/.msi/",
                        "", "/", "", "", "ccleaner_installer.msi", "CCleaner");
                }

                if (f.NovaBench.Checked)
                {
                    Debug.WriteLine("install novabench");
                    await Install("https://cdn.novabench.net/novabench.msi", "", "/", "", "", "novabench_installer.msi",
                        "NovaBench");
                }

                if (f.LibreOffice.Checked)
                {
                    Debug.WriteLine("install libreoffice");
                    await Install("https://miroir.univ-lorraine.fr/documentfoundation/libreoffice/stable/", "", "/", "",
                        "/win/x86_64/", "libreoffice_installer.msi", "LibreOffice");
                }

                if (f.VLC.Checked)
                {
                    Debug.WriteLine("install vlc");
                    await InstallVLC(); // appel spécifique pour VLC
                }

                if (f.TeamViewer.Checked)
                {
                    Debug.WriteLine("install teamviewer");
                    await Install("https://dl.teamviewer.com/download/version_15x/TeamViewer_Setup_x64.exe", "", "/",
                        "", "", "TeamViewer.exe", "TeamViewer");
                }

                // ajout de l'installation d'Office
                if (f.Word.Checked || f.Excel.Checked || f.PowerPoint.Checked || f.Outlook.Checked)
                {
                    Debug.WriteLine("install office");

                    // créer une instance de la classe Office
                    Office officeInstaller = new();

                    // récupérer les options sélectionnées dans le formulaire
                    bool isX64 = f.x64.Checked;
                    bool Word = f.Word.Checked;
                    bool Excel = f.Excel.Checked;
                    bool PowerPoint = f.PowerPoint.Checked;
                    bool Outlook = f.Outlook.Checked;
                    bool Access = f.Access.Checked;
                    bool Teams = f.Teams.Checked;
                    bool OneNote = f.OneNote.Checked;

                    // appeler la méthode d'installation
                    officeInstaller.Install(f, isX64, Word, Excel, PowerPoint, Outlook, Access, Teams, OneNote);
                }

                // activation Windows
                if (f.HWID.Checked)
                {
                    Debug.WriteLine("using hwid");
                    ActivationCommand(f.UseCurDir.Checked, " /HWID");
                }

                if (f.KMS38.Checked)
                {
                    Debug.WriteLine("using kms38");
                    ActivationCommand(f.UseCurDir.Checked, " /KMS38");
                }

                if (f.WinOnlineKMS.Checked)
                {
                    Debug.WriteLine("using online kms windows");
                    ActivationCommand(f.UseCurDir.Checked, " /K-Windows");
                }

                // activation Office
                if (f.Ohook.Checked)
                {
                    Debug.WriteLine("install ohook");
                    ActivationCommand(f.UseCurDir.Checked, " /Ohook");
                }

                if (f.OfficeOnlineKMS.Checked)
                {
                    Debug.WriteLine("install online kms office");
                    ActivationCommand(f.UseCurDir.Checked, " /K-Office");
                }

                if (f.UninstallKMSWindows.Checked || f.UninstallKMSOffice.Checked)
                {
                    Debug.WriteLine("uninstall online kms");
                    ActivationCommand(f.UseCurDir.Checked, "/K-Uninstall");
                }

                if (f.TSforgeOffice.Checked)
                {
                    Debug.WriteLine("use TSforge for Office");
                    ActivationCommand(f.UseCurDir.Checked, "/Z-Office");
                }

                if (f.TSforgeWindows.Checked)
                {
                    Debug.WriteLine("use TSforge for windows");
                    ActivationCommand(f.UseCurDir.Checked, "/Z-Windows");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Erreur : {e}");
            }
        }

        //Activation Script ----------------------------------------------------------------------------------------------------------

        //    string url = "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/master/MAS/All-In-One-Version/MAS_AIO-CRC32_31F7FD1E.cmd";
        //    string url = "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/refs/heads/master/MAS/All-In-One-Version-KL/MAS_AIO.cmd";

        public static async void ActivationCommand(bool useCurrentDirectory, string command)
        {
            try
            {
                Form1 f = new();
                f.UpdateStatusLabel("Activate...");

                string url =
                    "https://raw.githubusercontent.com/massgravel/Microsoft-Activation-Scripts/refs/heads/master/MAS/All-In-One-Version-KL/MAS_AIO.cmd";
                string currentDirectory = Directory.GetCurrentDirectory();
                string scriptFile = Path.Combine(currentDirectory, "MAS_AIO.cmd");

                // Télécharge le fichier si non présent
                if (!File.Exists(scriptFile))
                {
                    try
                    {
                        using HttpClient httpClient = new();
                        using HttpResponseMessage response =
                            await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                        response.EnsureSuccessStatusCode(); // Vérifie le succès de la réponse

                        await using Stream contentStream = await response.Content.ReadAsStreamAsync();
                        await using FileStream fileStream = new(scriptFile, FileMode.Create, FileAccess.Write,
                            FileShare.None);

                        await contentStream.CopyToAsync(fileStream);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors du téléchargement du script : {ex.Message}", "Erreur",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Exécute le script selon le répertoire choisi
                string filePath = useCurrentDirectory
                    ? scriptFile
                    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads",
                        "MAS_AIO.cmd");

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
                    MessageBox.Show($"Erreur lors de l'exécution du script : {ex.Message}", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Erreur : {e.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static async Task CheckInstall(string url, string hrefNodes, string hrefReplace,
            string ignoreVersionName, string endUrl, string installerName, string packageName)
        {
            await Install(url, hrefNodes, hrefReplace, ignoreVersionName, endUrl, installerName, packageName);
        }

        //Installation Script ---------------------------------------------------------------------------------------------------------------
        private static async Task Install(string url, string hrefNodes, string hrefReplace, string ignoreVersionName,
            string endUrl, string installerName, string packageName)
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

                var versionNodes =
                    doc.DocumentNode.SelectNodes($"//a[starts-with(@href, '{hrefNodes}') and contains(@href, '.')]");
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

            var d = "C:\\Users\\" + Environment.UserName + "\\Downloads\\";
            var b = "https://download.videolan.org/vlc/";
            var ie = "vlc_installer.msi";
            var pkg = "VLC";

            try
            {
                HttpClient c = new();
                // récupérer la liste des v disponibles
                string p = await c.GetStringAsync(b);
                var dc = new HtmlDocument();
                dc.LoadHtml(p);
                var vn = dc.DocumentNode.SelectNodes("//a[starts-with(@href, '3.0.') and contains(@href, '/')]");
                List<string> v = new();

                if (vn != null)
                {
                    v = vn.Select(node => node.GetAttributeValue("href", "").Trim('/')).Where(href => Version.TryParse(href, out _)).ToList();
                }

                // trier les v par ordre décroissant
                v.Sort((x, y) => new Version(y).CompareTo(new Version(x)));

                // vérifier chaque ve pour trouver un fichier MSI valide
                foreach (var ve in v)
                {
                    string veUrl = $"{b}{ve}/win64/";
                    try
                    {
                        string vePageContent = await c.GetStringAsync(veUrl);
                        var veDoc = new HtmlDocument();
                        veDoc.LoadHtml(vePageContent);

                        var m = veDoc.DocumentNode.SelectSingleNode($"//a[contains(@href, 'vlc-{ve}-win64.msi')]");

                        if (m != null)
                        {
                            string originalHref = m.GetAttributeValue("href", "");
                            string fullFileUrl = new Uri(new Uri(veUrl), originalHref).ToString();

                            // téléchargement et installation
                            await InstallPackage(fullFileUrl, d, pkg, ie);
                            return;
                        }
                    }
                    catch
                    {
                        // ignorer les erreurs et passer à la ve suivante
                        continue;
                    }
                }

                // si aucune ve valide n'a été trouvée
                MessageBox.Show("Impossible de trouver un fichier MSI valide pour VLC.");
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                MessageBox.Show("Erreur de connexion internet: Veuillez vérifier votre connexion et réessayer.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche des v de VLC: {ex.Message}");
            }
        }

        private static async Task InstallPackage(string u, string d, string pkg, string ie)
        {
            Form1 f = new();
            f.UpdateStatusLabel($"Downloading {pkg}...");

            // Chemin complet pour l'installateur
            string filePath = Path.Combine(d, ie);
            try
            {
                // Téléchargement du fichier avec HttpClient
                using HttpClient h = new();
                using HttpResponseMessage r = await h.GetAsync(u, HttpCompletionOption.ResponseHeadersRead);
                r.EnsureSuccessStatusCode(); // Vérifie que la requête est réussie
                await using Stream c = await r.Content.ReadAsStreamAsync();
                await using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

                await c.CopyToAsync(fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du téléchargement de {pkg} : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Installation silencieuse pour TeamViewer
            if (pkg.Equals("TeamViewer", StringComparison.OrdinalIgnoreCase))
            {
                f.UpdateStatusLabel($"Installing {pkg} in the background...");
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
                bool exe = filePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase);

                if (exe)
                {
                    f.UpdateStatusLabel($"Installing {pkg}...");
                    ProcessStartInfo i = new("cmd.exe", "/C " + filePath)
                    {
                        UseShellExecute = true, CreateNoWindow = true
                    };
                    var p = Process.Start(i);
                    p.WaitForExit();
                }
                else
                {
                    f.UpdateStatusLabel($"Installing {pkg}...");
                    string a = $"/passive /i \"{filePath}\"";
                    ProcessStartInfo s = new("msiexec.exe", a) { UseShellExecute = true, CreateNoWindow = true };
                    var p = Process.Start(s);
                    p.WaitForExit();
                }
            }

            // Supprime l'installateur après installation
            try
            {
                f.UpdateStatusLabel($"Cleaning up {pkg} installer...");
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Erreur lors de la suppression de l'installateur de {pkg} : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void SaveSettings(Form1 f)
        {
            var settings = new Dictionary<string, string>
            {
                { "[Booleans]", "!"},
                { "chrome", f.chrome.Checked.ToString().ToLower() },
                { "firefox", f.Firefox.Checked.ToString().ToLower() },
                { "ccleaner", f.CCleaner.Checked.ToString().ToLower() },
                { "novabench", f.NovaBench.Checked.ToString().ToLower() },
                { "libreoffice", f.LibreOffice.Checked.ToString().ToLower() },
                { "vlc", f.VLC.Checked.ToString().ToLower() },
                { "teamviewer", f.TeamViewer.Checked.ToString().ToLower() },
                { "winnothing", f.winNothing.Checked.ToString().ToLower() },
                { "hwid", f.HWID.Checked.ToString().ToLower() },
                { "kms38", f.KMS38.Checked.ToString().ToLower() },
                { "winonlinekms", f.WinOnlineKMS.Checked.ToString().ToLower() },
                { "winkmsuninstall", f.UninstallKMSWindows.Checked.ToString().ToLower() },
                { "wintsforge", f.TSforgeWindows.Checked.ToString().ToLower() },
                { "officenothing", f.officeNothing.Checked.ToString().ToLower() },
                { "ohook", f.Ohook.Checked.ToString().ToLower() },
                { "officeonlinekms", f.OfficeOnlineKMS.Checked.ToString().ToLower() },
                { "officekmsuninstall", f.UninstallKMSOffice.Checked.ToString().ToLower() },
                { "officetsforge", f.TSforgeOffice.Checked.ToString().ToLower() },
                { "x86", f.x86.Checked.ToString().ToLower() },
                { "x64", f.x64.Checked.ToString().ToLower() },
                { "word", f.Word.Checked.ToString().ToLower() },
                { "excel", f.Excel.Checked.ToString().ToLower() },
                { "powerpoint", f.PowerPoint.Checked.ToString().ToLower() },
                { "outlook", f.Outlook.Checked.ToString().ToLower() },
                { "teams", f.Teams.Checked.ToString().ToLower() },
                { "onenote", f.OneNote.Checked.ToString().ToLower() },
                { "access", f.Access.Checked.ToString().ToLower() },
                { "useprodkey", f.UseProdKey.Checked.ToString().ToLower() },
                { "[Combobox]", "!" },
                { "officeversions", f.officeVersions.SelectedItem?.ToString() ?? "Nothing" },
                { "[Strings]", "!" },
                { "productkey", f.ProductKey.Text }
            };

            using (StreamWriter writer = new(@"./settings.config"))
            {
                foreach (var item in settings)
                {
                    if (item.Value == "!")
                    {
                        writer.WriteLine(item.Key); 
                    }
                    else
                    {
                        writer.WriteLine($"{item.Key}={item.Value}");
                    }
                }
            }

            MessageBox.Show("Settings Save !");
        }
        public static void LoadSettings(Form1 f)
        {
            if (!File.Exists(@"./settings.config"))
            {
                MessageBox.Show("Fichier de configuration introuvable !");
                return;
            }
            else
            {
                File.Create(@"./settings.config");
            }

            string section = "";
            var settings = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(@"./settings.config"))
            {
                if (line.StartsWith("["))
                {
                    section = line.Trim(); // Récupère la section actuelle ([Booleans], [Strings], etc.)
                    continue;
                }

                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    settings[$"{section}.{parts[0]}"] = parts[1]; // Stocke sous forme "Section.Clé"
                }
            }

            // Appliquer les valeurs aux CheckBox
            f.chrome.Checked = settings.ContainsKey("[Booleans].chrome") && bool.Parse(settings["[Booleans].chrome"]);
            f.Firefox.Checked = settings.ContainsKey("[Booleans].firefox") && bool.Parse(settings["[Booleans].firefox"]);
            f.CCleaner.Checked = settings.ContainsKey("[Booleans].ccleaner") && bool.Parse(settings["[Booleans].ccleaner"]);
            f.NovaBench.Checked = settings.ContainsKey("[Booleans].novabench") && bool.Parse(settings["[Booleans].novabench"]);
            f.LibreOffice.Checked = settings.ContainsKey("[Booleans].libreoffice") && bool.Parse(settings["[Booleans].libreoffice"]);
            f.VLC.Checked = settings.ContainsKey("[Booleans].vlc") && bool.Parse(settings["[Booleans].vlc"]);
            f.TeamViewer.Checked = settings.ContainsKey("[Booleans].teamviewer") && bool.Parse(settings["[Booleans].teamviewer"]);
            f.winNothing.Checked = settings.ContainsKey("[Booleans].winnothing") && bool.Parse(settings["[Booleans].winnothing"]);
            f.HWID.Checked = settings.ContainsKey("[Booleans].hwid") && bool.Parse(settings["[Booleans].hwid"]);
            f.KMS38.Checked = settings.ContainsKey("[Booleans].kms38") && bool.Parse(settings["[Booleans].kms38"]);
            f.WinOnlineKMS.Checked = settings.ContainsKey("[Booleans].winonlinekms") && bool.Parse(settings["[Booleans].winonlinekms"]);
            f.UninstallKMSWindows.Checked = settings.ContainsKey("[Booleans].winkmsuninstall") && bool.Parse(settings["[Booleans].winkmsuninstall"]);
            f.TSforgeWindows.Checked = settings.ContainsKey("[Booleans].wintsforge") && bool.Parse(settings["[Booleans].wintsforge"]);
            f.officeNothing.Checked = settings.ContainsKey("[Booleans].officenothing") && bool.Parse(settings["[Booleans].officenothing"]);
            f.OfficeOnlineKMS.Checked = settings.ContainsKey("[Booleans].officeonlinekms") && bool.Parse(settings["[Booleans].officeonlinekms"]);
            f.UninstallKMSOffice.Checked = settings.ContainsKey("[Booleans].officekmsuninstall") && bool.Parse(settings["[Booleans].officekmsuninstall"]);
            f.TSforgeOffice.Checked = settings.ContainsKey("[Booleans].officetsforge") && bool.Parse(settings["[Booleans].officetsforge"]);
            f.Ohook.Checked = settings.ContainsKey("[Booleans].ohook") && bool.Parse(settings["[Booleans].ohook"]);
            f.x86.Checked = settings.ContainsKey("[Booleans].x86") && bool.Parse(settings["[Booleans].x86"]);
            f.x64.Checked = settings.ContainsKey("[Booleans].x64") && bool.Parse(settings["[Booleans].x64"]);
            f.Word.Checked = settings.ContainsKey("[Booleans].word") && bool.Parse(settings["[Booleans].word"]);
            f.Excel.Checked = settings.ContainsKey("[Booleans].excel") && bool.Parse(settings["[Booleans].excel"]);
            f.PowerPoint.Checked = settings.ContainsKey("[Booleans].powerpoint") && bool.Parse(settings["[Booleans].powerpoint"]);
            f.Outlook.Checked = settings.ContainsKey("[Booleans].outlook") && bool.Parse(settings["[Booleans].outlook"]);
            f.Teams.Checked = settings.ContainsKey("[Booleans].teams") && bool.Parse(settings["[Booleans].teams"]);
            f.OneNote.Checked = settings.ContainsKey("[Booleans].onenote") && bool.Parse(settings["[Booleans].onenote"]);
            f.Access.Checked = settings.ContainsKey("[Booleans].access") && bool.Parse(settings["[Booleans].access"]);
            f.UseProdKey.Checked = settings.ContainsKey("[Booleans].useprodkey") && bool.Parse(settings["[Booleans].useprodkey"]);

            //Applique la valeur au ComboBox

            if (settings.ContainsKey("[Combobox].officeversions"))
            {
                f.officeVersions.SelectedItem = settings["[Combobox].officeversions"];
            }

            //Applique la valeur au Strings

            if (f.UseProdKey.Checked)
            {
                if (settings.ContainsKey("[Strings].productkey"))
                {
                    f.ProductKey.Text = settings["[Strings].productkey"];
                }
            }
            else
            {
                f.ProductKey.Clear();
            }

            MessageBox.Show("Settings Load !");
        }

    }
}
