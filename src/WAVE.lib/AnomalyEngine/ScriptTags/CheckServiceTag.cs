using System;
using System.Runtime.Versioning;
using System.ServiceProcess;

using WAVE.lib.AnomalyEngine.ScriptTags.Base;

namespace WAVE.lib.AnomalyEngine.ScriptTags
{
    [SupportedOSPlatform("windows")]
    public class CheckServiceTag : BaseScriptTags
    {
        protected override string[] ValidArguments => new[] { "servicename" };

        public override string Run(string[] arguments)
        {
            try
            {
                using var sv = new ServiceController(arguments[1]);

                if (sv.Status == ServiceControllerStatus.Running)
                {
                    return string.Empty;
                }

                return $"Service is not running, it's status is {sv.Status}";
            }
            catch (Exception ex)
            {
                return $"Exception occurred: {ex}";
            }
        }
    }
}