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
            if (args.Length > 0 && args[0] == "install")
            {
                try
                {
                    string pipe_name = null;
                    string destination_directory = null;

                    bool skip_next = false;
                    int argc = args.Length;
                    for (int i = 1; i < argc; i++)
                    {
                        if (skip_next)
                        {
                            skip_next = false;
                            continue;
                        }

                        if (args[i] == "/destination")
                        {
                            destination_directory = args[i + 1];
                            skip_next = true;
                        }
                        else if (args[i] == "/statuspipe")
                        {
                            pipe_name = args[i + 1];
                            skip_next = true;
                        }
                    }

                    AnonymousPipeClientStream pipe = (pipe_name != null) ? new AnonymousPipeClientStream(PipeDirection.Out, pipe_name) : null;
                    BatchInstaller installer = new BatchInstaller(pipe);
                    installer.DestinationDirectory = destination_directory;

#if DEBUG
                    MessageBox.Show("Pausing for debugger, please attach now.", "Chocolatey Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif

                    installer.Run();

                    byte[] doneMsg = System.Text.Encoding.UTF8.GetBytes("DONE\n");
                    if (pipe != null) pipe.Write(doneMsg, 0, doneMsg.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Caught exception: {ex.Message}\r\n\r\n{ex.StackTrace}", "Chocolatey Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
