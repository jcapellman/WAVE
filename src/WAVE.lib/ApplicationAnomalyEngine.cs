using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Logging;

using WAVE.lib.AnomalyEngine;
using WAVE.lib.Applications.Containers;

namespace WAVE.lib
{
    internal class ApplicationAnomalyEngine : BaseWaveClass
    {
        public event EventHandler<ApplicationAnomaliesItem> OnAnomalyEvent;

        private readonly List<AnomalyListener> _listeners = new List<AnomalyListener>();

        private const int LOOP_WAIT_SECONDS = 60;

        public ApplicationAnomalyEngine(ILogger logger = null) : base(logger) { }

        public bool InitializeListeners(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException(folderPath);
            }

            var initializedWithoutError = true;

            var files = Directory.GetFiles(folderPath);

            foreach (var file in files)
            {
                try
                {
                    var content = File.ReadAllText(file);

                    var listener = new AnomalyListener(content);

                    _listeners.Add(listener);
                } catch (Exception e)
                {
                    LogError($"Failure to initialize listener from file ({file}), exception: {e}");
                }
            }

            return initializedWithoutError;
        }

        public void StartEngine(int loopWaitSeconds = LOOP_WAIT_SECONDS)
        {
            LogDebug("Starting Anomaly Engine");

            while (true)
            {
                foreach (var listener in _listeners)
                {
                    AnomalyListener.Run(listener);
                }

                System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(loopWaitSeconds));
            }
        }

        public void StopEngine()
        {
            LogDebug("Stopping Anomaly Engine");

            foreach (var listener in _listeners)
            {
                AnomalyListener.Stop(listener);
            }
        }
    }
}