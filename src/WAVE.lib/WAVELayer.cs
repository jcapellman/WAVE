using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using WAVE.lib.Applications.Containers;

using WAVE.lib.PlatformImplementations.Linux;
using WAVE.lib.PlatformImplementations.MacOS;
using WAVE.lib.PlatformImplementations.Windows;

namespace WAVE.lib
{
    public static class WAVELayer
    {
        public static List<ApplicationResponseItem> GetApplications(ILogger logger = null)
        {
            if (OperatingSystem.IsWindows())
            {
                return new WindowsApplicationCheck(logger).GetInstalledApplications();
            }

            if (OperatingSystem.IsLinux())
            {
                return new LinuxApplicationCheck(logger).GetInstalledApplications();
            }

            if (OperatingSystem.IsMacOS())
            {
                return new MacOsApplicationCheck(logger).GetInstalledApplications();
            }

            // TODO: Write macOS and Linux Implementations
            return new List<ApplicationResponseItem>();
        }
    }
}