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
                WriteLine("ERROR_STRING,CHOCO_DESTINATION variable not set");
                return false;
            }

            return RunScript($@"
New-Item -Type Directory ""{DestinationDirectory}"" -ErrorAction SilentlyContinue | Out-Null
Import-Module ""{chocoInstallModulePath}""
Initialize-Chocolatey -chocolateyPath ""{DestinationDirectory}""
");
        }

        private bool DoUpgradeChocolatey()
        {
            if (DestinationDirectory == null)
            {
                WriteLine("ERROR_STRING,CHOCO_DESTINATION variable not set");
                return false;
            }

            string chocoPath = Path.Combine(DestinationDirectory, "choco.exe");
            return RunExe(chocoPath, "upgrade", "chocolatey", "-y");
        }

        private bool DoInstallGUI()
        {
            if (DestinationDirectory == null)
            {
                WriteLine("ERROR_STRING,CHOCO_DESTINATION variable not set");
                return false;
            }

            string chocoPath = Path.Combine(DestinationDirectory, "choco.exe");
            return RunExe(chocoPath, "install", "chocolateygui", "-y");
        }

        private bool RunExe(string exePath, params string[] argv)
        {
            Process child = Process.Start(exePath, string.Join(" ", argv.Select(x => $"\"{x}\"")));
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
