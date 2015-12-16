namespace ChocolateyInstaller.Wizard
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label9;
            this.WizardControl = new AeroWizard.WizardControl();
            this.IntroductionPage = new AeroWizard.WizardPage();
            this.ChocolateyWebsiteLinkLabel = new ChocolateyInstaller.Wizard.FixedLinkLabel();
            this.WhyChocolateyLinkLabel = new ChocolateyInstaller.Wizard.FixedLinkLabel();
            this.LicensesPage = new AeroWizard.WizardPage();
            this.LicenseControlPanel = new System.Windows.Forms.Panel();
            this.LicensesAgreedCheckBox = new System.Windows.Forms.CheckBox();
            this.InstallOptionsPage = new AeroWizard.WizardPage();
            this.ChooseInstallLocationButton = new System.Windows.Forms.Button();
            this.InstallLocationTextBox = new System.Windows.Forms.TextBox();
            this.InstallingPage = new AeroWizard.WizardPage();
            this.StepDescriptionLabel = new System.Windows.Forms.Label();
            this.InstallProgressBar = new System.Windows.Forms.ProgressBar();
            this.InstallSucceededPage = new AeroWizard.WizardPage();
            this.label8 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.WizardControl)).BeginInit();
            this.IntroductionPage.SuspendLayout();
            this.LicensesPage.SuspendLayout();
            this.InstallOptionsPage.SuspendLayout();
            this.InstallingPage.SuspendLayout();
            this.InstallSucceededPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.Name = "label9";
            // 
            // WizardControl
            // 
            resources.ApplyResources(this.WizardControl, "WizardControl");
            this.WizardControl.Name = "WizardControl";
            this.WizardControl.Pages.Add(this.IntroductionPage);
            this.WizardControl.Pages.Add(this.LicensesPage);
            this.WizardControl.Pages.Add(this.InstallOptionsPage);
            this.WizardControl.Pages.Add(this.InstallingPage);
            this.WizardControl.Pages.Add(this.InstallSucceededPage);
            this.WizardControl.Cancelling += new System.ComponentModel.CancelEventHandler(this.WizardControl_Cancelling);
            this.WizardControl.Finished += new System.EventHandler(this.WizardControl_Finished);
            // 
            // IntroductionPage
            // 
            this.IntroductionPage.AllowBack = false;
            this.IntroductionPage.Controls.Add(this.ChocolateyWebsiteLinkLabel);
            this.IntroductionPage.Controls.Add(label5);
            this.IntroductionPage.Controls.Add(label4);
            this.IntroductionPage.Controls.Add(label3);
            this.IntroductionPage.Controls.Add(label2);
            this.IntroductionPage.Controls.Add(label1);
            this.IntroductionPage.Controls.Add(this.WhyChocolateyLinkLabel);
            this.IntroductionPage.Name = "IntroductionPage";
            this.IntroductionPage.NextPage = this.LicensesPage;
            resources.ApplyResources(this.IntroductionPage, "IntroductionPage");
            // 
            // ChocolateyWebsiteLinkLabel
            // 
            resources.ApplyResources(this.ChocolateyWebsiteLinkLabel, "ChocolateyWebsiteLinkLabel");
            this.ChocolateyWebsiteLinkLabel.Name = "ChocolateyWebsiteLinkLabel";
            this.ChocolateyWebsiteLinkLabel.TabStop = true;
            this.ChocolateyWebsiteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChocolateyWebsiteLinkLabel_LinkClicked);
            // 
            // WhyChocolateyLinkLabel
            // 
            resources.ApplyResources(this.WhyChocolateyLinkLabel, "WhyChocolateyLinkLabel");
            this.WhyChocolateyLinkLabel.Name = "WhyChocolateyLinkLabel";
            this.WhyChocolateyLinkLabel.TabStop = true;
            this.WhyChocolateyLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WhyChocolateyLinkLabel_LinkClicked);
            // 
            // LicensesPage
            // 
            this.LicensesPage.AllowNext = false;
            this.LicensesPage.Controls.Add(this.LicenseControlPanel);
            this.LicensesPage.Controls.Add(this.LicensesAgreedCheckBox);
            this.LicensesPage.Name = "LicensesPage";
            this.LicensesPage.NextPage = this.InstallOptionsPage;
            resources.ApplyResources(this.LicensesPage, "LicensesPage");
            // 
            // LicenseControlPanel
            // 
            resources.ApplyResources(this.LicenseControlPanel, "LicenseControlPanel");
            this.LicenseControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LicenseControlPanel.Name = "LicenseControlPanel";
            // 
            // LicensesAgreedCheckBox
            // 
            resources.ApplyResources(this.LicensesAgreedCheckBox, "LicensesAgreedCheckBox");
            this.LicensesAgreedCheckBox.Name = "LicensesAgreedCheckBox";
            this.LicensesAgreedCheckBox.UseVisualStyleBackColor = true;
            this.LicensesAgreedCheckBox.CheckedChanged += new System.EventHandler(this.LicensesAgreedCheckBox_CheckedChanged);
            // 
            // InstallOptionsPage
            // 
            this.InstallOptionsPage.Controls.Add(this.ChooseInstallLocationButton);
            this.InstallOptionsPage.Controls.Add(this.InstallLocationTextBox);
            this.InstallOptionsPage.Controls.Add(label6);
            this.InstallOptionsPage.Name = "InstallOptionsPage";
            this.InstallOptionsPage.NextPage = this.InstallingPage;
            resources.ApplyResources(this.InstallOptionsPage, "InstallOptionsPage");
            this.InstallOptionsPage.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.InstallOptionsPage_Commit);
            this.InstallOptionsPage.Enter += new System.EventHandler(this.InstallOptionsPage_Enter);
            this.InstallOptionsPage.Leave += new System.EventHandler(this.InstallOptionsPage_Leave);
            // 
            // ChooseInstallLocationButton
            // 
            resources.ApplyResources(this.ChooseInstallLocationButton, "ChooseInstallLocationButton");
            this.ChooseInstallLocationButton.Name = "ChooseInstallLocationButton";
            this.ChooseInstallLocationButton.UseVisualStyleBackColor = true;
            this.ChooseInstallLocationButton.Click += new System.EventHandler(this.ChooseInstallLocationButton_Click);
            // 
            // InstallLocationTextBox
            // 
            resources.ApplyResources(this.InstallLocationTextBox, "InstallLocationTextBox");
            this.InstallLocationTextBox.Name = "InstallLocationTextBox";
            // 
            // InstallingPage
            // 
            this.InstallingPage.AllowBack = false;
            this.InstallingPage.AllowCancel = false;
            this.InstallingPage.AllowNext = false;
            this.InstallingPage.Controls.Add(this.StepDescriptionLabel);
            this.InstallingPage.Controls.Add(this.InstallProgressBar);
            this.InstallingPage.Name = "InstallingPage";
            this.InstallingPage.NextPage = this.InstallSucceededPage;
            this.InstallingPage.ShowNext = false;
            resources.ApplyResources(this.InstallingPage, "InstallingPage");
            this.InstallingPage.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.InstallingPage_Initialize);
            // 
            // StepDescriptionLabel
            // 
            resources.ApplyResources(this.StepDescriptionLabel, "StepDescriptionLabel");
            this.StepDescriptionLabel.Name = "StepDescriptionLabel";
            // 
            // InstallProgressBar
            // 
            resources.ApplyResources(this.InstallProgressBar, "InstallProgressBar");
            this.InstallProgressBar.Name = "InstallProgressBar";
            // 
            // InstallSucceededPage
            // 
            this.InstallSucceededPage.AllowBack = false;
            this.InstallSucceededPage.AllowCancel = false;
            this.InstallSucceededPage.Controls.Add(label9);
            this.InstallSucceededPage.Controls.Add(label7);
            this.InstallSucceededPage.IsFinishPage = true;
            this.InstallSucceededPage.Name = "InstallSucceededPage";
            resources.ApplyResources(this.InstallSucceededPage, "InstallSucceededPage");
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.WizardControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.WizardControl)).EndInit();
            this.IntroductionPage.ResumeLayout(false);
            this.IntroductionPage.PerformLayout();
            this.LicensesPage.ResumeLayout(false);
            this.LicensesPage.PerformLayout();
            this.InstallOptionsPage.ResumeLayout(false);
            this.InstallOptionsPage.PerformLayout();
            this.InstallingPage.ResumeLayout(false);
            this.InstallingPage.PerformLayout();
            this.InstallSucceededPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl WizardControl;
        private AeroWizard.WizardPage IntroductionPage;
        private FixedLinkLabel WhyChocolateyLinkLabel;
        private FixedLinkLabel ChocolateyWebsiteLinkLabel;
        private AeroWizard.WizardPage InstallOptionsPage;
        private System.Windows.Forms.Button ChooseInstallLocationButton;
        private System.Windows.Forms.TextBox InstallLocationTextBox;
        private AeroWizard.WizardPage InstallingPage;
        private System.Windows.Forms.ProgressBar InstallProgressBar;
        private System.Windows.Forms.Label StepDescriptionLabel;
        private AeroWizard.WizardPage LicensesPage;
        private System.Windows.Forms.CheckBox LicensesAgreedCheckBox;
        private System.Windows.Forms.Panel LicenseControlPanel;
        private AeroWizard.WizardPage InstallSucceededPage;
        private System.Windows.Forms.Label label8;
    }
}

