using System.Linq;
using System.Runtime.Versioning;
using Xunit;
using WAVE.lib.PlatformImplementations.Windows;

namespace WAVE.UnitTests
{
    [SupportedOSPlatform("windows")]
    public class WindowsAppTests
    {
        [Fact]
        public void AppList()
        {
            var appNames = new WindowsApplicationCheck().GetInstalledApplications();

            Assert.True(appNames.Any());

            Assert.Contains(appNames, a => !string.IsNullOrEmpty(a.Version));

            Assert.Contains(appNames, a => !string.IsNullOrEmpty(a.Vendor));
        }
    }
}