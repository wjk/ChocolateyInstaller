using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ChocolateyInstaller.ElevationUtility
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
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// Thrown if a native operation failed.
        /// </exception>
        public static int ExecuteProcessElevated(string exePath, IList<string> argv)
        {
            string argString = string.Join(" ", argv.Select(x => $"\"{x}\""));

            if (!WindowsVersion.IsWindowsVista())
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(exePath, argString);
                process.WaitForExit();
                return process.ExitCode;
            }

            int exitCode = 0;
            SHELLEXECUTEINFOW shellExecuteInfo = new SHELLEXECUTEINFOW();
            shellExecuteInfo.cbSize = Marshal.SizeOf<SHELLEXECUTEINFOW>();
            shellExecuteInfo.fMask = SHELLEXECUTEINFOW.SEE_MASK_NOCLOSEPROCESS;
            shellExecuteInfo.lpVerb = "runas";
            shellExecuteInfo.lpFile = exePath;
            shellExecuteInfo.lpParameters = argString;
            shellExecuteInfo.lpDirectory = Directory.GetCurrentDirectory();

            bool launchOK = NativeMethods.ShellExecuteExW(ref shellExecuteInfo);
            if (!launchOK) throw new System.ComponentModel.Win32Exception();

            if (shellExecuteInfo.hProcess == IntPtr.Zero) throw new System.ComponentModel.Win32Exception("SHELLEXECUTEINFOW.hProcess is NULL", new System.ComponentModel.Win32Exception());
            
            const int STILL_ALIVE = 259;
            exitCode = STILL_ALIVE;

            do
            {
                NativeMethods.WaitForSingleObject(shellExecuteInfo.hProcess);
                bool success = NativeMethods.GetExitCodeProcess(shellExecuteInfo.hProcess, ref exitCode);
                if (!success) throw new System.ComponentModel.Win32Exception();
            } while (exitCode == STILL_ALIVE);

            NativeMethods.CloseHandle(shellExecuteInfo.hProcess);
            return exitCode;
        }
    }
}
