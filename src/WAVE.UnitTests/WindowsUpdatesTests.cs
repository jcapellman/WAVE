using WAVE.lib;

using Xunit;

namespace WAVE.UnitTests
{
    public class WindowsUpdatesTests
    {
        [Fact]
        public void CheckForUpdates()
        {
            var result = WindowsUpdates.CheckForUpdates();

            Assert.InRange(result.Count, 0, int.MaxValue);
        }
    }
}