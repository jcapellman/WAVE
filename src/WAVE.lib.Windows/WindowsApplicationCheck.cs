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

            try
            {
                return value.Contains('/')
                    ? Convert.ToDateTime(value)
                    : DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                // TODO: LOG Invalid Date

                return null;
            }
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

            return string.IsNullOrEmpty(majorVersion) || string.IsNullOrEmpty(minorVersion) ? "" : $"{majorVersion}.{minorVersion}";
        }

        private static List<ApplicationResponseItem> GetApps(string registryKeyPath)
        {
            var results = new List<ApplicationResponseItem>();

            using var key = Registry.LocalMachine.OpenSubKey(registryKeyPath);

            if (key == null)
            {
                return results;
            }

            foreach (var subKeyName in key.GetSubKeyNames())
            {
                try
                {
                    using var subKey = key.OpenSubKey(subKeyName);

                    if (subKey == null)
                    {
                        continue;
                    }

                    var item = new ApplicationResponseItem
                    {
                        Name = subKey.GetValue("DisplayName")?.ToString(),
                        Version = ParseVersion(subKey.GetValue("Version")?.ToString(),
                            subKey.GetValue("DisplayVersion")?.ToString(), subKey.GetValue("VersionMajor")?.ToString(),
                            subKey.GetValue("VersionMinor")?.ToString()),
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when parsing {subKeyName}: {ex}");
                }
            }

            return results;
        }

        public override List<ApplicationResponseItem> GetInstalledApplications()
        {
            const string apps = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            var results = GetApps(apps);

            if (Environment.Is64BitOperatingSystem)
            {
                const string thirtyTwoBitApps = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

                results.AddRange(GetApps(thirtyTwoBitApps));
            }

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