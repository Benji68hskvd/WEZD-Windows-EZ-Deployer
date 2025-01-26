using System;
using System.Diagnostics;
using System.Security.Principal;

namespace WEZD
{
    public static class ProgramLauncher
    {
        public static void LaunchAsAdmin(string filePath, string arguments = "")
        {
            try
            {
                // Vérifie si l'application est déjà lancée en tant qu'administrateur
                if (!IsRunAsAdmin())
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = filePath,         // Chemin du programme à exécuter
                        Arguments = arguments,       // Arguments à passer au programme
                        UseShellExecute = true,      // Nécessaire pour demander l'élévation
                        Verb = "runas"               // Demande d'élévation des privilèges
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    Console.WriteLine("L'application est déjà lancée en tant qu'administrateur.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du lancement en tant qu'administrateur : {ex.Message}");
            }
        }

        public static bool IsRunAsAdmin()
        {
            // Vérifie si le processus actuel est exécuté avec des privilèges administrateur
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}