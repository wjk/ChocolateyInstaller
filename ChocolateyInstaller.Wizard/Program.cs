using System;
using System.Threading;
using System.Windows.Forms;
using PSTaskDialog;

namespace ChocolateyInstaller.Wizard
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            const string LockName = "Chocolatey Installer Singleton Lock";

            Mutex singletonMutex;
            bool mutexExists = Mutex.TryOpenExisting(LockName, out singletonMutex);
            if (mutexExists)
            {
                VistaTaskDialog td = new VistaTaskDialog();
                // td.MainIcon = VistaTaskDialogIcon.Error;
                td.MainInstruction = "Only one instance of Chocolatey Installer can run simultaneously.";
                td.Content = "Close all other instances of Chocolatey Installer and try again.";
                td.WindowTitle = "Chocolatey Installer";
                td.CommonButtons = VistaTaskDialogCommonButtons.Ok;
                td.AllowDialogCancellation = true;
                td.Show();

                singletonMutex.Close();
                return;
            }

            try
            {
                singletonMutex = new Mutex(true, LockName);
                Application.Run(new MainForm());
            }
            finally
            {
                singletonMutex.ReleaseMutex();
                singletonMutex.Close();
            }
        }
    }
}
