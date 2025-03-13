using System.Diagnostics;
using System.Security.Principal;

namespace WEZD
{
    public static class ProgramLauncher
    {
        public static void LaunchAsAdmin(string f, string a = "")
        {
            try
            {
                // Vérifie si l'application est déjà lancée en tant qu'administrateur
                if (!IsRunAsAdmin())
                {
                    ProcessStartInfo startInfo = new()
                    {
                        FileName = f,         
                        Arguments = a,       
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
            WindowsIdentity i = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new(i);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}