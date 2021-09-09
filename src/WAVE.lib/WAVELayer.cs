using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

using WAVE.lib.Applications.Containers;

using WAVE.lib.PlatformImplementations.Linux;
using WAVE.lib.PlatformImplementations.MacOS;

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
                _aaEngine.OnAnomalyEvent -= AaEngine_OnAnomalyEvent;
                _aaEngine.StopEngine();

                _aaEngine = null;
            }

            _aaEngine = new ApplicationAnomalyEngine(_logger);

            _aaEngine.OnAnomalyEvent += AaEngine_OnAnomalyEvent;

            _aaEngine.InitializeListeners(anomalyDefinitionPath);

            _aaEngine.StartEngine();
        }

        private void AaEngine_OnAnomalyEvent(object sender, ApplicationAnomaliesItem e)
        {
            OnApplicationAnomaly?.Invoke(this, e);
        }

        public void StopAnomalyDetection()
        {
            _aaEngine.StopEngine();
        }

        public List<ApplicationResponseItem> GetInstalledApplications()
        {
#if NET5_0
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new PlatformImplementations.Windows.WindowsApplicationCheck(_logger).GetInstalledApplications();
            }
#endif
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new LinuxApplicationCheck(_logger).GetInstalledApplications();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new MacOsApplicationCheck(_logger).GetInstalledApplications();
            }

            throw new PlatformNotSupportedException($"{Environment.OSVersion} is not a currently supported operating system");
        }
    }
}