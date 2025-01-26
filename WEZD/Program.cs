using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace WEZD
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            // Vérifie si l'application est exécutée en tant qu'administrateur
            if (!ProgramLauncher.IsRunAsAdmin())
            {
                // Relance l'application en tant qu'administrateur
                ProgramLauncher.LaunchAsAdmin(Application.ExecutablePath);
                return; // Quitte le processus actuel après avoir lancé l'application avec élévation
            }

            // Code principal de l'application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}