using System;

using Microsoft.Extensions.Logging;

using WAVE.lib.Applications.Containers;

namespace WAVE.lib
{
    public class ApplicationAnomalyEngine
    {
        public event EventHandler<ApplicationAnomaliesItem> OnAnomalyEvent;

        public ApplicationAnomalyEngine(ILogger logger = null)
        {

        }

        public void StartEngine()
        {

        }

        public void StopEngine()
        {

        }
    }
}
