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
            var appNames = new WindowsApplicationCheck().GetInstalledApplicationsNameOnly();

            Assert.True(appNames.Any());
        }
    }
}
