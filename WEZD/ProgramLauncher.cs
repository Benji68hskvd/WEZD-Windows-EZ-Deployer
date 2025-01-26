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
                    ProcessStartInfo startInfo = new()
                    {
                        FileName = filePath,         
                        Arguments = arguments,       
                        UseShellExecute = true,      
                        Verb = "runas"               
                    };
                    Process.Start(startInfo);
                }
                else
                {
                    Debug.WriteLine("L'application est déjà lancée en tant qu'administrateur.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors du lancement en tant qu'administrateur : {ex.Message}");
                MessageBox.Show($"Erreur lors du lancement en tant qu'administrateur : {ex.Message}");
            }
        }

        public static bool IsRunAsAdmin()
        {
            // Vérifie si le processus actuel est exécuté avec des privilèges administrateur
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}