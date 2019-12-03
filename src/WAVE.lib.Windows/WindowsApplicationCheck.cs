using Microsoft.Win32;

using System.Collections.Generic;

using WAVE.lib.Applications.Base;

namespace WAVE.lib.Windows
{
    public class WindowsApplicationCheck : BaseApplicationCheck
    {
        public override List<string> GetInstalledApplicationsNameOnly()
        {
            const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            var appNames = new List<string>();

            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                appNames.AddRange(key?.GetSubKeyNames());
            }

            return appNames;
        }
    }
}