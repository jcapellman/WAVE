using System;
using System.IO;
using System.Linq;
using System.Text.Json;

using WAVE.lib;

namespace WAVE.cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var apps = new WAVELayer().GetInstalledApplications();

            File.WriteAllText("inventory.json", string.Join(Environment.NewLine, apps.Select(a => JsonSerializer.Serialize(a))));

            foreach (var app in apps)
            {
                Console.WriteLine(JsonSerializer.Serialize(app));
            }
        }
    }
}