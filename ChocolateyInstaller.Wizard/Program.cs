using System;
using System.IO.Pipes;
using System.Windows.Forms;

namespace ChocolateyInstaller.Wizard
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "/install")
            {
                AnonymousPipeClientStream pipe = (args.Length > 1) ? new AnonymousPipeClientStream(args[1]) : null;
                BatchInstaller installer = new BatchInstaller(pipe);
                installer.Run();

                byte[] doneMsg = System.Text.Encoding.UTF8.GetBytes("DONE\n");
                if (pipe != null) pipe.Write(doneMsg, 0, doneMsg.Length);

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
