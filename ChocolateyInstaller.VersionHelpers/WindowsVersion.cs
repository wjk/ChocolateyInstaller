using System.Runtime.InteropServices;

namespace ChocolateyInstaller.VersionHelpers
{
    /// <summary>
    /// Contains methods useful for determining the verson of Windows that a program is running on.
    /// </summary>
    /// <remarks>
    /// Windows 8.1 or later require a compatibility manifest to report the accurate version.
    /// Without a correct manifest, Windows will always report Windows 8.
    /// </remarks>
    public static class WindowsVersion
    {
        /// <summary>
        /// Determines if a program is running on a certain version of Windows,
        /// or any newer version.
        /// </summary>
        /// <param name="majorVersion">
        /// The major component of the Windows NT version.
        /// </param>
        /// <param name="minorVersion">
        /// The minor component of the Windows NT version.
        /// </param>
        /// <param name="servicePack">
        /// The minimum service pack that must be applied to the system.
        /// </param>
        /// <returns>
        /// <c>true</c> if the version requirement is met, or <c>false</c> otherwise.
        /// </returns>
        public static bool IsVersionOrGreater(short majorVersion, short minorVersion, short servicePack)
        {
            OSVERSIONINFOEXW osvi = new OSVERSIONINFOEXW();
            osvi.dwOSVersionInfoSize = Marshal.SizeOf<OSVERSIONINFOEXW>();
            osvi.dwMajorVersion = majorVersion;
            osvi.dwMinorVersion = minorVersion;
            osvi.wServicePackMajor = servicePack;

            long conditionMask = 0;
            conditionMask = NativeMethods.VerSetConditionMask(conditionMask, NativeMethods.VER_SERVICEPACKMAJOR, NativeMethods.VER_GREATER_EQUAL);
            conditionMask = NativeMethods.VerSetConditionMask(conditionMask, NativeMethods.VER_MINORVERSION, NativeMethods.VER_GREATER_EQUAL);
            conditionMask = NativeMethods.VerSetConditionMask(conditionMask, NativeMethods.VER_MAJORVERSION, NativeMethods.VER_GREATER_EQUAL);

            return NativeMethods.VerifyVersionInfoW(ref osvi, NativeMethods.VER_MAJORVERSION | NativeMethods.VER_MINORVERSION | NativeMethods.VER_SERVICEPACKMAJOR, conditionMask);
        }

        /// <summary>
        /// Determines if the program is running on Windows XP or any newer version of Windows.
        /// </summary>
        /// <param name="requiredServicePack">
        /// The required minimum Service Pack that must be applied to the system
        /// for the requirement to succeed. Defaults to 2 for security reasons.
        /// </param>
        /// <returns>
        /// <c>true</c> if the version requirement is met, or <c>false</c> otherwise.
        /// </returns>
        public static bool IsWindowsXP(short requiredServicePack = 2)
        {
            return IsVersionOrGreater(5, 2, requiredServicePack);
        }

        /// <summary>
        /// Determines if the program is running on Windows Vista or any newer version of Windows.
        /// </summary>
        /// <param name="requiredServicePack">
        /// The required minimum Service Pack that must be applied to the system
        /// for the requirement to succeed.
        /// </param>
        /// <returns>
        /// <c>true</c> if the version requirement is met, or <c>false</c> otherwise.
        /// </returns>
        public static bool IsWindowsVista(short requiredServicePack = 0)
        {
            return IsVersionOrGreater(6, 0, requiredServicePack);
        }

        /// <summary>
        /// Determines if the program is running on Windows 7 or any newer version of Windows.
        /// </summary>
        /// <param name="requiredServicePack">
        /// The required minimum Service Pack that must be applied to the system
        /// for the requirement to succeed.
        /// </param>
        /// <returns>
        /// <c>true</c> if the version requirement is met, or <c>false</c> otherwise.
        /// </returns>
        public static bool IsWindows7(short requiredServicePack = 0)
        {
            return IsVersionOrGreater(6, 1, requiredServicePack);
        }

        /// <summary>
        /// Determines if the program is running on Windows 8 or any newer version of Windows.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the version requirement is met, or <c>false</c> otherwise.
        /// </returns>
        public static bool IsWindows8()
        {
            return IsVersionOrGreater(6, 2, 0);
        }

        /// <summary>
        /// Determines if the program is running on Windows 8.1 or any newer version of Windows.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the version requirement is met, or <c>false</c> otherwise.
        /// </returns>
        public static bool IsWindows8_1()
        {
            return IsVersionOrGreater(6, 3, 0);
        }

        /// <summary>
        /// Determines if the program is running on Windows 10 or any newer version of Windows.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the version requirement is met, or <c>false</c> otherwise.
        /// </returns>
        public static bool IsWindows10()
        {
            return IsVersionOrGreater(10, 0, 0);
        }
    }
}
