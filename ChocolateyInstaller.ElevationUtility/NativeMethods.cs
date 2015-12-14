using System;
using System.Runtime.InteropServices;

namespace ChocolateyInstaller.ElevationUtility
{
    internal static class NativeMethods
    {
        [DllImport("shell32.dll")]
        public static extern bool ShellExecuteExW(ref SHELLEXECUTEINFOW info);
        [DllImport("kernel32.dll")]
        public static extern void WaitForSingleObject(IntPtr handle, uint timeout = uint.MaxValue);
        [DllImport("kernel32.dll")]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, ref int exitCode);
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool VerifyVersionInfoW(ref OSVERSIONINFOEXW osvi, int flags, long conditionalMask);
        [DllImport("kernel32.dll")]
        public static extern long VerSetConditionMask(long orig, int key, int op);

        public const int VER_MAJORVERSION = 0x2;
        public const int VER_MINORVERSION = 0x1;
        public const int VER_SERVICEPACKMAJOR = 0x20;
        public const int VER_GREATER_EQUAL = 3;

    }
}
