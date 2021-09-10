using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            if (!string.IsNullOrEmpty(displayVersion))
            {
                return displayVersion;
            }

            return string.IsNullOrEmpty(majorVersion) || string.IsNullOrEmpty(minorVersion) ? version : $"{majorVersion}.{minorVersion}";
        }

        private List<ApplicationResponseItem> GetApps(string registryKeyPath)
        {
            var results = new ConcurrentDictionary<string, ApplicationResponseItem>();

            var hives = new RegistryHive[] { 
                RegistryHive.CurrentUser, 
                RegistryHive.LocalMachine 
            };

            var views = new RegistryView[] { 
                RegistryView.Registry32, 
                RegistryView.Registry64 
            };

            foreach (var hive in hives)
            {
                foreach (var view in views)
                {
                    RegistryKey key = null,
                            basekey = null;

                    try
                    {
                        basekey = RegistryKey.OpenBaseKey(hive, view);
                        key = basekey.OpenSubKey(registryKeyPath);
                    }
                    catch (Exception ex) {
                        LogDebug($"Error when attempting to obtain base key and key: {ex}");

                        continue; 
                    }

                    if (basekey == null || key == null)
                    {
                        LogDebug($"Could not obtain base key or key - skipping");

                        continue;
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

                                if (displayNameValue == null || string.IsNullOrEmpty(displayNameValue.ToString()))
                                {
                                    LogDebug($"{subKeyName} had an empty Name - ignoring");

                                    return;
                                }

                                var item = new ApplicationResponseItem
                                {
                                    Name = displayNameValue.ToString(),
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
                }
            }

            return results.Values.ToList();
        }

        public override List<ApplicationResponseItem> GetInstalledApplications()
        {
            const string apps = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            var results = GetApps(apps);

            LogDebug($"{results.Count} Applications found");

            return results.OrderBy(a => a.Name).ToList();
        }
    }
}