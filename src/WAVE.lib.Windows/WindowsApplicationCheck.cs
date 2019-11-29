using Microsoft.Win32;

using System.Collections.Generic;

using WAVE.lib.Applications.Base;

namespace WAVE.lib.Windows
{
    public class WindowsApplicationCheck : BaseApplicationCheck
    {
        public override List<string> GetInstalledApplicationsNameOnly()
        {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            var appNames = new List<string>();

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        appNames.Add(subkey.GetValue("DisplayName").ToString());
                    }
                }
            }

            return appNames;
        }
    }
}