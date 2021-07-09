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

        private const int LOOP_WAIT_SECONDS = 60;

        public ApplicationAnomalyEngine(ILogger logger = null) : base(logger) { }

        public void StartEngine(int loopWaitSeconds = LOOP_WAIT_SECONDS)
        {
            LogDebug("Starting Anomaly Engine");

            while (true)
            {
                foreach (var listener in _listeners)
                {
                    listener.Run();
                }

                System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(loopWaitSeconds));
            }
        }

        public void StopEngine()
        {
            LogDebug("Stopping Anomaly Engine");
        }
    }
}