using System;

using Microsoft.Extensions.Logging;

using WAVE.lib.Applications.Containers;

namespace WAVE.lib
{
    internal class ApplicationAnomalyEngine : BaseWaveClass
    {
        public event EventHandler<ApplicationAnomaliesItem> OnAnomalyEvent;

        public ApplicationAnomalyEngine(ILogger logger = null) : base(logger) { }
    
        public void StartEngine()
        {
            LogDebug("Starting Anomaly Engine");
        }

        public void StopEngine()
        {
            LogDebug("Stopping Anomaly Engine");
        }
    }
}