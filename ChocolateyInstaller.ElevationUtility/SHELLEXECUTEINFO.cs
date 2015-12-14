#pragma warning disable 0649
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ChocolateyInstaller.ElevationUtility
{
    internal struct SHELLEXECUTEINFOW
    {
        #region fMask Values
        public const uint SEE_MASK_DEFAULT = 0x00000000;
        public const uint SEE_MASK_CLASSNAME = 0x00000001;
        public const uint SEE_MASK_CLASSKEY = 0x00000003;
        public const uint SEE_MASK_IDLIST = 0x00000004;
        public const uint SEE_MASK_INVOKEIDLIST = 0x0000000c;
        public const uint SEE_MASK_ICON = 0x00000010;
        public const uint SEE_MASK_HOTKEY = 0x00000020;
        public const uint SEE_MASK_NOCLOSEPROCESS = 0x00000040;
        public const uint SEE_MASK_CONNECTNETDRV = 0x00000080;
        public const uint SEE_MASK_NOASYNC = 0x00000100;
        public const uint SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC;
        public const uint SEE_MASK_DOENVSUBST = 0x00000200;
        public const uint SEE_MASK_FLAG_NO_UI = 0x00000400;
        public const uint SEE_MASK_UNICODE = 0x00004000;
        public const uint SEE_MASK_NO_CONSOLE = 0x00008000;
        public const uint SEE_MASK_ASYNCOK = 0x00100000;
        public const uint SEE_MASK_HMONITOR = 0x00200000;
        public const uint SEE_MASK_NOZONECHECKS = 0x00800000;
        public const uint SEE_MASK_NOQUERYCLASSSTORE = 0x01000000;
        public const uint SEE_MASK_WAITFORINPUTIDLE = 0x02000000;
        public const uint SEE_MASK_FLAG_LOG_USAGE = 0x04000000;
        #endregion

        public int cbSize;
        public uint fMask;
        public IntPtr hWnd;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpVerb;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpParameters;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpDirectory;
        public int nShow;
        public IntPtr hInstApp;
        public IntPtr lpIDList;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpClass;
        public IntPtr hkeyClass;
        public int dwHotKey;
        public IntPtr hMonitor;
        public IntPtr hProcess;
    }
}
