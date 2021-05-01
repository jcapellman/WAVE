using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.Json;

using WAVE.lib.Windows;

namespace WAVE.cmd
{
    class Program
    {
        [SupportedOSPlatform("windows")]
        static void Main(string[] args)
        {
            var apps = new WindowsApplicationCheck().GetInstalledApplications();

            File.WriteAllText("inventory.json", string.Join(Environment.NewLine, apps.Select(a => JsonSerializer.Serialize(a))));

            foreach (var app in apps)
            {
                Console.WriteLine(JsonSerializer.Serialize(app));
            }
        }
    }
}