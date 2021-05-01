using WAVE.lib.Windows;

using System.Linq;

using Xunit;

namespace WAVE.UnitTests
{
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
