using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChocolateyInstaller
{
    /// <summary>
    /// Contains methods that interact with the Windows User Account Control security mechanism.
    /// </summary>
    public sealed class ElevationUtility
    {
        private ElevationUtility()
        {
            // This class is not meant to be instantiated.
            throw new NotSupportedException();
        }


        /// <summary>
        /// Determines if the current process is running under administrator privileges.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">
        /// Thrown if the current process is not running on Windows Vista or later.
        /// </exception>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// Thrown if a native operation failed.
        /// </exception>
        public static bool IsProcessElevated
        {
            get
            {
                if (!WindowsVersion.IsWindowsVista())
                {
                    // Elevation is only supported on Windows Vista and later.
                    throw new PlatformNotSupportedException();
                }

                IntPtr tokenHandle;
                if (!NativeMethods.OpenProcessToken(NativeMethods.GetCurrentProcess(), TOKEN_ACCESS_RIGHTS.TOKEN_QUERY, out tokenHandle))
                {
                    throw new System.ComponentModel.Win32Exception("OpenProcessToken failed", new System.ComponentModel.Win32Exception());
                }

                try
                {
                    TOKEN_ELEVATION_TYPE elevationType;
                    uint dontCare;

                    if (!NativeMethods.GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, out elevationType, Convert.ToUInt32(sizeof(TOKEN_ELEVATION_TYPE)), out dontCare))
                    {
                        throw new System.ComponentModel.Win32Exception("GetTokenInformation failed", new System.ComponentModel.Win32Exception());
                    }

                    return elevationType == TOKEN_ELEVATION_TYPE.TokenElevationTypeFull;
                }
                finally
                {
                    NativeMethods.CloseHandle(tokenHandle);
                }
            }
        }

        /// <summary>
        /// Executes a process using administrative privileges, waits for it to terminate,
        /// and returns the exit code of that process. A User Account Control dialog will
        /// be presented to the user.
        /// </summary>
        /// <param name="exePath">
        /// The path to the program to run.
        /// </param>
        /// <param name="argv">
        /// The argument array to be passed to the program.
        /// </param>
        /// <returns>
        /// The exit code returned by the executed program.
        /// </returns>
        public static int ExecuteProcessElevated(string exePath, IList<string> argv)
        {
            string argString = string.Join(" ", argv.Select(x => $"\"{x}\""));

            ProcessStartInfo processInfo = new ProcessStartInfo(exePath, argString);
            processInfo.UseShellExecute = true;
            if (WindowsVersion.IsWindowsVista()) processInfo.Verb = "runas";
            Process child = Process.Start(processInfo);
            child.WaitForExit();
            return child.ExitCode;
        }
    }
}
