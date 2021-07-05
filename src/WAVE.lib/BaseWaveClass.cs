using Microsoft.Extensions.Logging;

namespace WAVE.lib
{
    internal class BaseWaveClass
    {
        protected ILogger _logger;

        public BaseWaveClass(ILogger logger = null)
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
    }
}