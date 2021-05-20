using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using WAVE.lib.Applications.Containers;

namespace WAVE.lib.Applications.Base
{
    public abstract class BaseApplicationCheck
    {
        protected ILogger _logger;

        protected BaseApplicationCheck(ILogger logger = null)
        {
            _logger = logger;
        }

        protected void LogError(string message)
        {
            _logger?.LogError(message);
        }

        protected void LogDebug(string message)
        {
            _logger?.LogDebug(message);
        }

        public abstract List<ApplicationResponseItem> GetInstalledApplications();
    }
}