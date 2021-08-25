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

        protected void LogError(string logMessage) => Log(LogLevel.Error, logMessage);

        protected void LogDebug(string logMessage) => Log(LogLevel.Debug, logMessage);

        private void Log(LogLevel logLevel, string logMessage)
        {
            if (_logger == null)
            {
                return;
            }

#pragma warning disable CA2254 // Template should be a static expression
            _logger.Log(logLevel, logMessage, null);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }
}