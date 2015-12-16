using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ChocolateyInstaller.Wizard
{
    internal sealed class BatchInstaller
    {
        private const string PowerShellPath = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
        private Stream mStatusPipe;

        private readonly List<Tuple<string, Func<bool>>> mSteps;
        public string LogPath { get; set; } = Path.Combine(Path.GetDirectoryName(typeof(BatchInstaller).Assembly.Location), "install.log");
        public string DestinationDirectory { get; set; } = null;

        public BatchInstaller(Stream pipe)
        {
            mStatusPipe = pipe;

            mSteps = new List<Tuple<string, Func<bool>>>()
            {
                new Tuple<string, Func<bool>>("InstallChocolatey", DoInstallChocolatey),
                new Tuple<string, Func<bool>>("UpgradeChocolatey", DoUpgradeChocolatey),
                new Tuple<string, Func<bool>>("InstallGUI", DoInstallGUI)
            };
        }

        public bool Run()
        {
            int stepCount = mSteps.Count;
            WriteLine($"STEP-COUNT,{stepCount}");
            for (int stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                var step = mSteps[stepIndex];
                WriteLine($"STEP,{stepIndex},{step.Item1}");
                bool success = step.Item2();

                if (!success)
                {
                    WriteLine($"STEP-FAIL,{stepIndex}");
                    return false;
                }
            }

            return true;
        }

        private bool DoInstallChocolatey()
        {
            string myDir = Path.GetDirectoryName(typeof(BatchInstaller).Assembly.Location);
            string chocoInstallModulePath = Path.Combine(myDir, "choco_nupkg", "tools", "chocolateysetup.psm1");

            if (DestinationDirectory == null)
            {
                WriteLine("ERROR_STRING,Install destination not set");
                return false;
            }

            // I must remove the existing chocolateyInstall environment variable, or else
            // Initialize-Chocolatey will ignore my explicit override path in favor of an
            // already-installed copy, if one is present.
            return RunScript($@"
Remove-Item Env:\chocolateyInstall -ErrorAction SilentlyContinue | Out-Null
New-Item -Type Directory ""{DestinationDirectory}"" -ErrorAction SilentlyContinue | Out-Null
Import-Module ""{chocoInstallModulePath}""
Initialize-Chocolatey -chocolateyPath ""{DestinationDirectory}""
");
        }

        private bool DoUpgradeChocolatey()
        {
            if (DestinationDirectory == null)
            {
                WriteLine("ERROR_STRING,Install destination not set");
                return false;
            }

            string chocoPath = Path.Combine(DestinationDirectory, "choco.exe");
            return RunExe(chocoPath, "upgrade", "chocolatey", "-y");
        }

        private bool DoInstallGUI()
        {
            if (DestinationDirectory == null)
            {
                WriteLine("ERROR_STRING,Install destination not set");
                return false;
            }

            string chocoPath = Path.Combine(DestinationDirectory, "choco.exe");
            return RunExe(chocoPath, "install", "chocolateygui", "-y");
        }

        private bool RunExe(string exePath, params string[] argv)
        {
            ProcessStartInfo info = new ProcessStartInfo(exePath, string.Join(" ", argv.Select(x => $"\"{x}\"")));
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            Process child = Process.Start(info);
            child.WaitForExit();
            return child.ExitCode == 0;
        }

        private bool RunScript(string script)
        {
            string path = Path.GetTempFileName();

            // PowerShell scripts must have a .ps1 extension, or else PowerShell.exe won't run them.
            string fixedPath = Path.ChangeExtension(path, "ps1");
            File.WriteAllText(fixedPath, script);

            bool success = RunExe(PowerShellPath, "-ExecutionPolicy", "Bypass", "-Command", fixedPath);
            File.Delete(path);
            File.Delete(fixedPath);
            return success;
        }

        private void WriteLine(string line)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(line + "\n");
            if (mStatusPipe != null)
            {
                mStatusPipe.Write(bytes, 0, bytes.Length);
            }

            using (Stream stream = File.Open(LogPath, FileMode.Append, FileAccess.Write))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
