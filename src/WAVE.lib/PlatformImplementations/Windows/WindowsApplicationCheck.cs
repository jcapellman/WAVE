using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Win32;

using WAVE.lib.Applications.Base;
using WAVE.lib.Applications.Containers;

namespace WAVE.lib.PlatformImplementations.Windows
{
    internal class WindowsApplicationCheck : BaseApplicationCheck
    {
        public WindowsApplicationCheck(ILogger logger = null) : base(logger) { }

        private DateTime? ParseDate(string value)
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
            catch (FormatException fex)
            {
                LogError($"Error converting ({value}) to a date: {fex}");

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

        private List<ApplicationResponseItem> GetApps(string registryKeyPath, string appendingSuffix)
        {
            var results = new ConcurrentDictionary<string, ApplicationResponseItem>();

            using (var key = Registry.LocalMachine.OpenSubKey(registryKeyPath)) {

                if (key == null)
                {
                    return results.Values.ToList();
                }

                Parallel.ForEach(key.GetSubKeyNames(), subKeyName =>
                {
                    try
                    {
                        using (var subKey = key.OpenSubKey(subKeyName))
                        {

                            if (subKey == null)
                            {
                                return;
                            }

                            var displayNameValue = subKey.GetValue("DisplayName");

                            if (displayNameValue == null || string.IsNullOrEmpty(displayNameValue.ToString())) {
                                LogDebug($"{subKeyName} had an empty Name - ignoring");

                                return;
                            }

                            var item = new ApplicationResponseItem
                            {
                                Name = $"{displayNameValue} - ({appendingSuffix})",
                                Version = ParseVersion(subKey.GetValue("Version")?.ToString(),
                                    subKey.GetValue("DisplayVersion")?.ToString(), subKey.GetValue("VersionMajor")?.ToString(),
                                    subKey.GetValue("VersionMinor")?.ToString()),
                                Vendor = subKey.GetValue("Publisher")?.ToString(),
                                InstallLocation = subKey.GetValue("InstallLocation")?.ToString(),
                                InstallDate = ParseDate(subKey.GetValue("InstallDate")?.ToString())
                            };

                            if (item.InstallDate == null && !string.IsNullOrEmpty(item.InstallLocation) && Directory.Exists(item.InstallLocation))
                            {
                                try
                                {
                                    var directoryInfo = new DirectoryInfo(item.InstallLocation);

                                    item.InstallDate = directoryInfo?.CreationTime;

                                    LogDebug($"Obtained install date from {item.InstallLocation} ({item.InstallDate ?? null})");
                                }
                                catch (Exception iex)
                                {
                                    LogError($"Error when getting the install date from ({item.InstallLocation}): {iex}");
                                }
                            }

                            results.TryAdd(item.Name, item);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError($"Error when parsing {subKeyName}: {ex}");
                    }
                });

                return results.Values.ToList();
            }
        }

        public override List<ApplicationResponseItem> GetInstalledApplications()
        {
            const string apps = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            var results = GetApps(apps, "x64");

            if (Environment.Is64BitOperatingSystem)
            {
                LogDebug("64bit Operating System Detected - querying 32 bit applications");

                const string thirtyTwoBitApps = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

                results.AddRange(GetApps(thirtyTwoBitApps, "x86"));
            }

            var filteredResults = new List<ApplicationResponseItem>();

            foreach (var result in results.Where(result =>
                !filteredResults.Any(a => a.Name == result.Name && a.InstallLocation == result.InstallLocation)))
            {
                filteredResults.Add(result);
            }

            LogDebug($"{filteredResults.Count} Applications found");

            return filteredResults.OrderBy(a => a.Name).ToList();
        }
    }
}