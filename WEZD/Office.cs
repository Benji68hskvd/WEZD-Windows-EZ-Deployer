using System;
using System.Diagnostics;
using System.IO;

namespace WEZD
{
    public class Office
    {
        private readonly string _installerPath;
        private readonly string _xmlTemplate = @"
<Configuration>
  <Add OfficeClientEdition=""{0}"" Channel=""PerpetualVL2024"">
    <Product ID=""ProPlus2024Volume"" PIDKEY=""BBBBB-BBBBB-BBBBB-BBBBB-BBBBB"">
      <Language ID=""fr-fr""/>
      {1}
    </Product>
  </Add>
  <Property Name=""SharedComputerLicensing"" Value=""0""/>
  <Property Name=""SCLCacheOverride"" Value=""0""/>
  <Property Name=""AUTOACTIVATE"" Value=""0""/>
  <Property Name=""FORCEAPPSHUTDOWN"" Value=""TRUE""/>
  <Property Name=""DeviceBasedLicensing"" Value=""0""/>
  <Updates Enabled=""TRUE""/>
  <RemoveMSI/>
</Configuration>";

        public Office()
        {
            // chemin du fichier exécutable temporaire
            _installerPath = Path.Combine(Path.GetTempPath(), "OfficeSetup.exe");
            ExtractInstaller();
        }

        private void ExtractInstaller()
        {
            if (!File.Exists(_installerPath))
            {
                //File.WriteAllBytes(_installerPath, OfficeBinary.ExecutableData);
                File.WriteAllBytes(_installerPath, Properties.Resources.setup);
            }
        }

        public void Install(bool isX64, bool installWord, bool installExcel, bool installPowerPoint, bool installOutlook)
        {
            try
            {
                Form1 f = new();
                f.UpdateStatusLabel("Préparation de l'installation d'Office...");

                string architecture = isX64 ? "64" : "32";

                // Générer les exclusions
                string exclusions = GenerateExclusions(installWord, installExcel, installPowerPoint, installOutlook);

                // Générer le contenu XML
                string configXml = string.Format(_xmlTemplate, architecture, exclusions);

                // Écrire le fichier XML temporaire
                string configPath = Path.Combine(Path.GetTempPath(), "OfficeConfig.xml");
                File.WriteAllText(configPath, configXml);

                f.UpdateStatusLabel("Installation d'Office en cours...");

                // Exécuter le programme d'installation
                ProcessStartInfo installProcessInfo = new(_installerPath)
                {
                    Arguments = $"/configure {configPath}",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process? process = Process.Start(installProcessInfo);
                process.WaitForExit();

                f.UpdateStatusLabel("Installation d'Office terminée.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Erreur lors de l'installation d'Office : {ex.Message}");
            }
        }

        private string GenerateExclusions(bool installWord, bool installExcel, bool installPowerPoint, bool installOutlook)
        {
            string exclusions = "";

            if (!installWord)
            {
                exclusions += "<ExcludeApp ID=\"Word\"/>";
            }
            if (!installExcel)
            {
                exclusions += "<ExcludeApp ID=\"Excel\"/>";
            }
            if (!installPowerPoint)
            {
                exclusions += "<ExcludeApp ID=\"PowerPoint\"/>";
            }
            if (!installOutlook)
            {
                exclusions += "<ExcludeApp ID=\"Outlook\"/>";
            }

            // Ajouter d'autres exclusions si nécessaire
            exclusions += "<ExcludeApp ID=\"Groove\"/>";
            exclusions += "<ExcludeApp ID=\"Lync\"/>";
            exclusions += "<ExcludeApp ID=\"Teams\"/>";
            exclusions += "<ExcludeApp ID=\"Access\"/>";

            return exclusions;
        }
    }
}
