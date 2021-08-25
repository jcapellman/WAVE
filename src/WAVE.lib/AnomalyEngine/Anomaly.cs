using System;

namespace WAVE.lib.AnomalyEngine
{
    public class AnomalyListener
    {
        public string Name { get; internal set;  }

        public static void Run(AnomalyListener listener) {
            if (listener == null)
            {
                return;
            }
        }

        public static void Stop(AnomalyListener listener) { if (listener == null) { return; } }

        public AnomalyListener(string scriptContent)
        {
            if (string.IsNullOrEmpty(scriptContent))
            {
                throw new ArgumentNullException(nameof(scriptContent));
            }
            // TODO: Parse JSON
        }
    }
}