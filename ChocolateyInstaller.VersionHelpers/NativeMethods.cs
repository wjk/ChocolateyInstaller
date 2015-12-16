using System.Runtime.InteropServices;

namespace ChocolateyInstaller
{
    internal static class NativeMethods
    {
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
