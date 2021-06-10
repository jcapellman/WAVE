using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected static List<string> RunTerminalProcess(string process, string arguments)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = process,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            var result = new List<string>();

            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();

                result.Add(line);
            }

            return result;
        }

        public abstract List<ApplicationResponseItem> GetInstalledApplications();
    }
}