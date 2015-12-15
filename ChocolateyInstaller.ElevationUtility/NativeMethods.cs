using System;
using System.Runtime.InteropServices;

namespace ChocolateyInstaller
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern void WaitForSingleObject(IntPtr handle, uint timeout = uint.MaxValue);
        [DllImport("kernel32.dll")]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, ref int exitCode);
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr hProcess, uint desiredAccess, out IntPtr hToken);
        [DllImport("advapi32.dll")]
        public static extern IntPtr GetCurrentProcess();
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr hToken, TOKEN_INFORMATION_CLASS informationClass, out TOKEN_ELEVATION_TYPE information, uint informationLength, out uint returnedLength);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);
    }
}
