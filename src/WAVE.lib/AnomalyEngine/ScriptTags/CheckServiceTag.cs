using System;
using System.Runtime.Versioning;
using System.ServiceProcess;

using WAVE.lib.AnomalyEngine.ScriptTags.Base;

namespace WAVE.lib.AnomalyEngine.ScriptTags
{
    public class ArgumentCheckService : ScriptArguments
    {
        public string ServiceName { get; set; }
    }

    public class CheckServiceTag : BaseScriptTags<ArgumentCheckService>
    {
        protected override string RunTag(ArgumentCheckService argumentClass)
        {
            try
            {
                using (var sv = new ServiceController(argumentClass.ServiceName))
                {
                    if (sv.Status == ServiceControllerStatus.Running)
                    {
                        return string.Empty;
                    }

                    return $"Service is not running, it's status is {sv.Status}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception occurred: {ex}";
            }
        }
    }
}