using System;

namespace WAVE.lib.Applications.Containers
{
    public class ApplicationResponseItem
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Vendor { get; set; }

        public DateTime? InstallDate { get; set; }

        public string InstallLocation { get; set; }
    }
}