using System.Linq;

using Xunit;

using WAVE.lib;

namespace WAVE.UnitTests
{
    public class WindowsAppTests
    {
        [Fact]
        public void AppList()
        {
            var appNames = new WAVELayer().GetInstalledApplications();

            Assert.True(appNames.Any());

            Assert.Contains(appNames, a => !string.IsNullOrEmpty(a.Version));

            Assert.Contains(appNames, a => !string.IsNullOrEmpty(a.Vendor));
        }
    }
}