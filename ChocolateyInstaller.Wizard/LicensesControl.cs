using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChocolateyInstaller.Wizard
{
    public partial class LicensesControl : UserControl
    {
        public LicensesControl()
        {
            InitializeComponent();
        }

        private void LicenseURLLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.apache.org/licenses/LICENSE-2.0");
        }
    }
}
