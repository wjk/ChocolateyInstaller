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
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

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

        // If this property is true, then the installer window will not close willingly.
        // This is done to avoid interrupting the installation process.
        private bool LockInstaller { get; set; }

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
            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.DefaultDirectory = KnownFolders.ProgramData.Path;
            dlg.IsFolderPicker = true;
            dlg.Title = "Select a folder to install Chocolatey into";
            dlg.Multiselect = false;
            dlg.AllowNonFileSystemItems = false;
            var result = dlg.ShowDialog(Handle);

            if (result == CommonFileDialogResult.Ok) InstallRoot = dlg.FileAsShellObject.ParsingName;
        }

        private void InstallOptionsPage_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            if (Directory.Exists(InstallRoot) && Directory.GetFileSystemEntries(InstallRoot).Length != 0)
            {
                TaskDialog td = new TaskDialog();
                td.Caption = "Chocolatey Installer";
                td.InstructionText = "The selected directory already contains files.";
                td.Text = "This program will not install Chocolatey into a folder that contains files. Please choose another location.";
                td.StandardButtons = TaskDialogStandardButtons.Ok;
                td.Cancelable = true;
                td.Show();
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
            InstallProgressBar.Style = ProgressBarStyle.Marquee;

            const string pipeName = "Chocolatey Installer\\User Interface Progress";
            NamedPipeServerStream pipe = new NamedPipeServerStream(pipeName, PipeDirection.In);

            ProcessStartInfo processInfo = new ProcessStartInfo(System.Reflection.Assembly.GetEntryAssembly().Location, $"install /destination \"{InstallRoot}\" /statuspipe \"{pipeName}\"");
            processInfo.UseShellExecute = true;
            if (WindowsVersion.IsWindowsVista()) processInfo.Verb = "runas";

            try
            {
                Process child = Process.Start(processInfo);
            }
            catch (Win32Exception)
            {
                // This can be raised if the user clicks Cancel in the UAC dialog.
                pipe.Dispose();
                return;
            }

            Thread thr = new Thread(PipeReadThread);
            thr.Start(pipe);
        }

        private void PipeReadThread(object param)
        {
            NamedPipeServerStream stream = (NamedPipeServerStream)param;
            stream.WaitForConnection();

            using (StreamReader reader = new StreamReader(stream))
            {
                while (true)
                {
                    string msg = reader.ReadLine();
                    if (msg == null) break; // because null indicates end-of-stream

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
                InstallProgressBar.Value = InstallProgressBar.Maximum;
                StepDescriptionLabel.Text = "Installation complete";
                InstallingPage.AllowNext = true;
                WizardControl.NextPage();
                LockInstaller = false;
            }
            else if (words[0] == "STEP-COUNT")
            {
                LockInstaller = true;
                InstallProgressBar.Style = ProgressBarStyle.Continuous;
                InstallProgressBar.Maximum = int.Parse(words[1]) + 1;
                InstallProgressBar.Value = 0;
            }
            else if (words[0] == "STEP")
            {
                string description;
                switch (words[2])
                {
                    case "InstallChocolatey": description = "Installing Chocolatey..."; break;
                    case "UpgradeChocolatey": description = "Ensuring Chocolatey is the latest version..."; break;
                    case "InstallGUI": description = "Installing Chocolatey GUI..."; break;
                    default: description = "Processing..."; break;
                }

                InstallProgressBar.Value = int.Parse(words[1]) + 1;
                StepDescriptionLabel.Text = description;
                StepDescriptionLabel.Visible = true;
            }
            else if (words[0] == "ERROR_STRING")
            {
                ReportInstallationError($"The privileged installer process returned the following error: \"{words[1]}\"");
            }
            else if (words[0] == "STEP-FAIL")
            {
                string content;
                switch (words[1])
                {
                    case "InstallChocolatey": content = "The privileged installer process could not install Chocolatey."; break;
                    case "UpgradeChocolatey": content = "The privileged installer process failed while upgrading Chocolatey to the latest version."; break;
                    case "InstallGUI": content = "The privileged installer process could not install ChocolateyGUI."; break;
                    default: content = "The privileged installer process could not complete a required step."; break;
                }

                ReportInstallationError(content);
            }
        }

        private void ReportInstallationError(string content)
        {
            TaskDialog td = new TaskDialog();
            td.Caption = "Chocolatey Installer";
            td.InstructionText = "Chocolatey failed to install. You may need to clean up your system manually.";
            td.Text = content;
            td.Icon = TaskDialogStandardIcon.Error;
            td.StandardButtons = TaskDialogStandardButtons.Close;
            td.Cancelable = true;
            td.Show();

            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LockInstaller)
            {
                System.Media.SystemSounds.Beep.Play();
                e.Cancel = true;
            }
        }
    }
}
