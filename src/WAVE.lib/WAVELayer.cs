using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using WAVE.lib.Applications.Containers;
using WAVE.lib.Windows;

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

            // TODO: Write macOS and Linux Implementations
            return new List<ApplicationResponseItem>();
        }
    }
}