using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

using Microsoft.Win32;

using WAVE.lib.Applications.Base;
using WAVE.lib.Applications.Containers;

namespace WAVE.lib.Windows
{
    [SupportedOSPlatform("windows")]
    public class WindowsApplicationCheck : BaseApplicationCheck
    {
        private static DateTime? ParseDate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value.Contains('/') ? Convert.ToDateTime(value) : DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
        
        private static string ParseVersion(string version, string displayVersion, string majorVersion, string minorVersion)
        {
            if (!string.IsNullOrEmpty(version))
            {
                return version;
            }

            if (!string.IsNullOrEmpty(displayVersion))
            {
                return displayVersion;
            }

            return string.IsNullOrEmpty(majorVersion) ? "" : $"{majorVersion}.{minorVersion}";
        }

        private List<ApplicationResponseItem> GetApps(string registryKeyPath)
        {
            var results = new List<ApplicationResponseItem>();

            using RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyPath);

            if (key == null)
            {
                return results;
            }

            foreach (var subkeyName in key.GetSubKeyNames())
            {
                using var subKey = key.OpenSubKey(subkeyName);

                if (subKey == null)
                {
                    continue;
                }

                var item = new ApplicationResponseItem
                {
                    Name = subKey.GetValue("DisplayName")?.ToString(),
                    Version = ParseVersion(subKey.GetValue("Version")?.ToString(), subKey.GetValue("DisplayVersion")?.ToString(), subKey.GetValue("VersionMajor")?.ToString(), subKey.GetValue("VersionMinor")?.ToString()),
                    Vendor = subKey.GetValue("Publisher")?.ToString(),
                    InstallLocation = subKey.GetValue("InstallLocation")?.ToString(),
                    InstallDate = ParseDate(subKey.GetValue("InstallDate")?.ToString())
                };

                if (item.InstallDate == null && !string.IsNullOrEmpty(item.InstallLocation))
                {
                    if (Directory.Exists(item.InstallLocation))
                    {
                        var directoryInfo = new DirectoryInfo(item.InstallLocation);

                        item.InstallDate = directoryInfo.CreationTime;
                    }
                }

                if (string.IsNullOrEmpty(item.Name))
                {
                    continue;
                }

                results.Add(item);
            }

            return results;
        }

        public override List<ApplicationResponseItem> GetInstalledApplications()
        {
            var apps = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var otherApps = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            var results = GetApps(apps);

            results.AddRange(GetApps(otherApps));

            var filteredResults = new List<ApplicationResponseItem>();

            foreach (var result in results.Where(result => 
                !filteredResults.Any(a => a.Name == result.Name && a.InstallLocation == result.InstallLocation)))
            {
                filteredResults.Add(result);
            }

            return filteredResults.OrderBy(a => a.Name).ToList();
        }
    }
}