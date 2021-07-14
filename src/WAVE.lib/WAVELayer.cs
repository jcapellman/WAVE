using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using WAVE.lib.Applications.Containers;

using WAVE.lib.PlatformImplementations.Linux;
using WAVE.lib.PlatformImplementations.MacOS;
using WAVE.lib.PlatformImplementations.Windows;

namespace WAVE.lib
{
    public class WAVELayer
    {
        private readonly ILogger _logger;

        private ApplicationAnomalyEngine _aaEngine;

        public WAVELayer(ILogger logger = null)
        {
            _logger = logger;
        }

        public EventHandler<ApplicationAnomaliesItem> OnApplicationAnomaly;

        public void StartAnomalyDetection(string anomalyDefinitionPath)
        {
            if (_aaEngine != null)
            {
                _aaEngine.OnAnomalyEvent -= _aaEngine_OnAnomalyEvent;
                _aaEngine.StopEngine();

                _aaEngine = null;
            }

            _aaEngine = new ApplicationAnomalyEngine(_logger);

            _aaEngine.OnAnomalyEvent += _aaEngine_OnAnomalyEvent;

            _aaEngine.InitializeListeners(anomalyDefinitionPath);

            _aaEngine.StartEngine();
        }

        private void _aaEngine_OnAnomalyEvent(object sender, ApplicationAnomaliesItem e)
        {
            OnApplicationAnomaly?.Invoke(this, e);
        }

        public void StopAnomalyDetection()
        {
            _aaEngine.StopEngine();
        }

        public List<ApplicationResponseItem> GetInstalledApplications()
        {
            if (OperatingSystem.IsWindows())
            {
                return new WindowsApplicationCheck(_logger).GetInstalledApplications();
            }

            if (OperatingSystem.IsLinux())
            {
                return new LinuxApplicationCheck(_logger).GetInstalledApplications();
            }

            if (OperatingSystem.IsMacOS())
            {
                return new MacOsApplicationCheck(_logger).GetInstalledApplications();
            }

            throw new PlatformNotSupportedException($"{Environment.OSVersion} is not a currently supported operating system");
        }
    }
}