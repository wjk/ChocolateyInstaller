using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ChocolateyInstaller.Wizard
{
    [ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class FixedLinkLabel : LinkLabel
    {
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            OverrideCursor = FixedHandCursor;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            OverrideCursor = FixedHandCursor;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hModule, IntPtr cursorId);
        public static readonly Cursor FixedHandCursor = new Cursor(LoadCursor(IntPtr.Zero, new IntPtr(32649)));
    }
}
