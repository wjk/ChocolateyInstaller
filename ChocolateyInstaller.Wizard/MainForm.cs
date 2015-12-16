using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PSTaskDialog;

namespace ChocolateyInstaller.Wizard
{
    public partial class MainForm : Form
    {
        #region Configuration Properties

        public string InstallRoot
        {
            get
            {
                return InstallLocationTextBox?.Text;
            }

            set
            {
                if (InstallLocationTextBox != null) InstallLocationTextBox.Text = value;
            }
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();
            InstallRoot = "C:\\ProgramData\\chocolatey";
        }

        private void WizardControl_Cancelling(object sender, CancelEventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LicensesControl view = new LicensesControl();
            LicenseControlPanel.Controls.Add(view);
        }

        private void WhyChocolateyLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/chocolatey/choco/wiki/Why#what-is-chocolatey");
        }

        private void ChocolateyWebsiteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://chocolatey.org/");
        }

        private void WizardControl_Finished(object sender, EventArgs e)
        {
            Close();
        }

        private void InstallOptionsPage_Enter(object sender, EventArgs e)
        {
            InstallLocationTextBox.Text = InstallRoot;
            WizardControl.NextButtonShieldEnabled = true;
            WizardControl.NextButtonText = "I&nstall";
        }

        private void ChooseInstallLocationButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select a folder to install Chocolatey into.";
            dlg.ShowNewFolderButton = true;
            dlg.RootFolder = Environment.SpecialFolder.MyComputer;

            var result = dlg.ShowDialog(this);
            if (result == DialogResult.OK) InstallRoot = dlg.SelectedPath;
        }

        private void InstallOptionsPage_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            if (Directory.Exists(InstallRoot) && Directory.GetFileSystemEntries(InstallRoot).Length != 0)
            {
                VistaTaskDialog td = new VistaTaskDialog();
                td.WindowTitle = "Chocolatey Installer";
                td.MainInstruction = "The selected directory already contains files.";
                td.Content = "This program will not install Chocolatey into a folder that contains files. Please choose another location.";
                td.CommonButtons = VistaTaskDialogCommonButtons.Ok;
                td.AllowDialogCancellation = true;
                td.Show(this);
                e.Cancel = true;
            }
        }

        private void InstallOptionsPage_Leave(object sender, EventArgs e)
        {
            WizardControl.NextButtonShieldEnabled = false;
            WizardControl.NextButtonText = "&Next";
        }

        private void LicensesAgreedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LicensesPage.AllowNext = LicensesAgreedCheckBox.Checked;
        }

        private void InstallingPage_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            const string pipeName = "Chocolatey Installer\\User Interface Progress";
            NamedPipeServerStream pipe = new NamedPipeServerStream(pipeName, PipeDirection.In);

            ProcessStartInfo processInfo = new ProcessStartInfo(System.Reflection.Assembly.GetEntryAssembly().Location, $"install /destination \"{InstallRoot}\" /statuspipe \"{pipeName}\"");
            processInfo.UseShellExecute = true;
            if (WindowsVersion.IsWindowsVista()) processInfo.Verb = "runas";

            Process child = Process.Start(processInfo);

            Thread thr = new Thread(PipeReadThread);
            thr.Start(pipe);
        }

        private void PipeReadThread(object param)
        {
            NamedPipeServerStream stream = (NamedPipeServerStream)param;
            stream.WaitForConnection();

            using (StreamReader reader = new StreamReader(stream))
            {
                bool continueLoop = true;
                while (continueLoop)
                {
                    string msg = reader.ReadLine();
                    if (msg == null) break; // because null indicates end-of-stream

                    if (msg == "DONE") continueLoop = false;

                    Action invoke = () => ProcessIPCMessage(msg);
                    BeginInvoke(invoke);
                }
            }

            stream.Dispose();
        }

        private void ProcessIPCMessage(string msg)
        {
            string[] words = msg.Split(',');
            if (words[0] == "DONE")
            {

            }
            else if (words[0] == "STEP-COUNT")
            {
                InstallProgressBar.Maximum = int.Parse(words[1]);
                InstallProgressBar.Value = 0;
            }
            else if (words[0] == "STEP")
            {
                InstallProgressBar.Value = int.Parse(words[1]) + 1;
                StepDescriptionLabel.Text = words[2];
                StepDescriptionLabel.Visible = true;
            }
            else if (words[0] == "ERROR_STRING")
            {
                StepDescriptionLabel.Text = $"Subprocess reported error: {words[1]}";
                StepDescriptionLabel.Visible = true;
            }
        }
    }
}
