using System.Collections.Generic;
using System.Diagnostics;

using WAVE.lib.Applications.Base;
using WAVE.lib.Applications.Containers;

namespace WAVE.lib.Linux
{
    public class LinuxApplicationCheck : BaseApplicationCheck
    {
        private const string APT_METHOD = "apt";
        private const string APT_PARAMETERS = "--list installed";

        private static List<string> RunTerminalProcess(string process, string arguments)
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

        public override List<ApplicationResponseItem> GetInstalledApplications()
        {
            var lines = RunTerminalProcess(APT_METHOD, APT_PARAMETERS);

            var applications = new List<ApplicationResponseItem>();

            foreach (var line in lines)
            {
                var split = line.Split(' ');

                var item = new ApplicationResponseItem();

                for (var x = 0; x < split.Length; x++)
                {
                    var element = split[x];

                    switch (x)
                    {
                        case 0:
                            item.Name = element.Split('/')[0];
                            break;
                        case 1:
                            item.Version = element;
                            break;
                        default:
                            continue;
                    }
                }

                applications.Add(item);
            }

            return applications;
        }
    }
}