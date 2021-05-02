using WAVE.lib.Windows;

using System.Linq;
using System.Runtime.Versioning;
using Xunit;

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
        }
    }
}