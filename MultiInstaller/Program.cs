using System.Diagnostics;

namespace MultiInstaller
{
    public class Program
    {

        [STAThread]
        public static void Main()
        {
            //ApplicationConfiguration.Initialize();
            //Form1 form = new();
            //Application.Run(form);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}