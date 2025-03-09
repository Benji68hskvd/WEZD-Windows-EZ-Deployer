﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

#pragma warning disable CA1416

namespace WEZD
{
    public class Office
    {
        private readonly string _installerPath;
        private readonly string _xmlTemplate = @"
<Configuration>
  <Add OfficeClientEdition=""{0}"" Channel=""{1}"">
    <Product ID=""{3}""{4}>
      <Language ID=""fr-fr""/>
      <ExcludeApp ID=""OneNote""/>
       {5}
    </Product>
  </Add>
  <Property Name=""AUTOACTIVATE"" Value=""1""/>
  <Property Name=""FORCEAPPSHUTDOWN"" Value=""TRUE""/>
  <Updates Enabled=""TRUE""/>
  <RemoveMSI/>
</Configuration>";

        public Office()
        {
            // Chemin de l'exécutable temporaire
            _installerPath = Path.Combine(Path.GetTempPath(), "OfficeSetup.exe");
            ExtractInstaller();
        }

        private void ExtractInstaller()
        {
            if (!File.Exists(_installerPath))
            {
                // Extraction de l'exécutable d'installation depuis les ressources
                File.WriteAllBytes(_installerPath, Properties.Resources.setup);
            }
        }

        private string GetOfficeChannel(string productId)
        {
            // Vérifie si c'est une version Volume (VL) ou Retail
            if (productId.Contains("Volume"))
            {
                if (productId.Contains("2024")) return "PerpetualVL2024";
                if (productId.Contains("2021")) return "PerpetualVL2021";
                if (productId.Contains("2019")) return "PerpetualVL2019";
                if (productId.Contains("2016")) return "PerpetualVL2016"; // Peut varier selon la version
                return "PerpetualVL"; // Valeur par défaut
            }

            // Sinon, on assume que c'est une version Retail
            return "Current"; // Office 365 / 2021 / 2019 Retail
        }


        public void Install(Form1 f, bool isX64, bool installWord, bool installExcel, bool installPowerPoint, bool installOutlook)
        {
            try
            {
                f.UpdateStatusLabel("Préparation de l'installation d'Office...");

                // Déterminer l'édition et la version sélectionnée
                string productId = GetSelectedProductId(f);
                if (productId == null)
                {
                    MessageBox.Show("Veuillez sélectionner une version et une édition d'Office.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Récupérer et valider la clé de produit
                string productKey = GetProductKey(f);
                if (string.IsNullOrEmpty(productKey))
                {
                    return; // Annule l'installation si la clé est invalide
                }

                string architecture = isX64 ? "64" : "32";

                // Générer les exclusions
                string exclusions = GenerateExclusions(installWord, installExcel, installPowerPoint, installOutlook);

                string pidKeySection = f.UseProdKey.Checked ? $"PIDKEY=\"{productKey}\"" : "";

                string officeChannel = GetOfficeChannel(productId);

                // Générer le fichier XML de configuration
                string configXml = string.Format(_xmlTemplate, architecture, officeChannel, productId.Substring(productId.Length - 4), productId, pidKeySection, exclusions);

                MessageBox.Show(configXml);

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

        /// <summary>
        /// Vérifie quel `RadioButton` est sélectionné et retourne l'ID du produit correspondant.
        /// </summary>
        private string GetSelectedProductId(Form1 f)
        {
            return f.officeVersions.SelectedIndex switch
            {
                0 => "Standard2016Volume",
                1 => "ProPlus2016Volume",
                2 => "Standard2019Volume",
                3 => "ProPlus2019Volume",
                4 => "Standard2021Volume",
                5 => "ProPlus2021Volume",
                6 => "Standard2024Volume",
                7 => "ProPlus2024Volume",
                _ => null // Si aucun choix n'est fait
            };
            //if (f.Std2016.Checked) return "Standard2016Volume";
            //if (f.PPlus2016.Checked) return "ProPlus2016Volume";
            //if (f.Std2019.Checked) return "Standard2019Volume";
            //if (f.PPlus2019.Checked) return "ProPlus2019Volume";
            //if (f.Std2021.Checked) return "Standard2021Volume";
            //if (f.PPlus2021.Checked) return "ProPlus2021Volume";
            //if (f.Std2024.Checked) return "Standard2024Volume";
            //if (f.PPlus2024.Checked) return "ProPlus2024Volume";
            return null; // Aucun bouton sélectionné
        }

        /// <summary>
        /// Récupère et valide la clé de produit.
        /// </summary>
        private string GetProductKey(Form1 f)
        {
            if (f.UseProdKey.Checked) // Si l'utilisateur a choisi une clé personnalisée
            {
                string inputKey = f.ProductKey.Text.Replace("-", "").ToUpper(); // Nettoie et formate la clé
                if (!IsValidProductKey(inputKey))
                {
                    MessageBox.Show(
                        "La clé de produit personnalisée n'est pas valide. Assurez-vous qu'elle respecte le format XXXXX-XXXXX-XXXXX-XXXXX-XXXXX.",
                        "Clé non valide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                // Formater la clé avec des tirets
                return FormatProductKey(inputKey);
            }

            // Utilisation de la clé par défaut
            return "BBBBB-BBBBB-BBBBB-BBBBB-BBBBB";
        }

        /// <summary>
        /// Valide une clé de produit selon les règles de Microsoft.
        /// </summary>
        private bool IsValidProductKey(string key)
        {
            // La clé doit faire 25 caractères et ne contenir que des caractères valides
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            if (key.Length != 25) return false;

            foreach (char c in key)
            {
                if (!validChars.Contains(c)) return false;
            }

            return true;
        }

        /// <summary>
        /// Formate une clé de produit en ajoutant les tirets au bon endroit.
        /// </summary>
        private string FormatProductKey(string key)
        {
            StringBuilder formattedKey = new();
            for (int i = 0; i < key.Length; i++)
            {
                if (i > 0 && i % 5 == 0) formattedKey.Append("-");
                formattedKey.Append(key[i]);
            }
            return formattedKey.ToString();
        }

        /// <summary>
        /// Génère les exclusions pour le fichier XML.
        /// </summary>
        private string GenerateExclusions(bool installWord, bool installExcel, bool installPowerPoint, bool installOutlook)
        {
            StringBuilder exclusions = new();

            if (!installWord) exclusions.AppendLine("<ExcludeApp ID=\"Word\"/>");
            if (!installExcel) exclusions.AppendLine("<ExcludeApp ID=\"Excel\"/>");
            if (!installPowerPoint) exclusions.AppendLine("<ExcludeApp ID=\"PowerPoint\"/>");
            if (!installOutlook) exclusions.AppendLine("<ExcludeApp ID=\"Outlook\"/>");

            // Exclusions par défaut
            exclusions.AppendLine("<ExcludeApp ID=\"Groove\"/>");
            exclusions.AppendLine("<ExcludeApp ID=\"Lync\"/>");
            exclusions.AppendLine("<ExcludeApp ID=\"Teams\"/>");
            exclusions.AppendLine("<ExcludeApp ID=\"Access\"/>");

            return exclusions.ToString();
        }
    }
}
