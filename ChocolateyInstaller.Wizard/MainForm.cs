using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
    }
}
