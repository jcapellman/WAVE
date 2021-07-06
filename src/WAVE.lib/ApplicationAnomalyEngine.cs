using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using WAVE.lib.AnomalyEngine;
using WAVE.lib.Applications.Containers;

namespace WAVE.lib
{
    internal class ApplicationAnomalyEngine : BaseWaveClass
    {
        public event EventHandler<ApplicationAnomaliesItem> OnAnomalyEvent;

        private List<AnomalyListener> _listeners = new();

        public ApplicationAnomalyEngine(ILogger logger = null) : base(logger) { }
    
        public void StartEngine()
        {
            LogDebug("Starting Anomaly Engine");

            foreach (var listener in _listeners)
            {
                            
            }
        }

        public void StopEngine()
        {
            LogDebug("Stopping Anomaly Engine");
        }
    }
}