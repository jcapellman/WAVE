using System.Collections.Generic;
using System.Linq;

using WUApiLib;

namespace WAVE.lib
{
    public class WindowsUpdates
    {
        public static List<string> GetAvailableUpdateList()
        {
            var uSession = new UpdateSession();
            var uSearcher = uSession.CreateUpdateSearcher();

            uSearcher.Online = false;

            var sResult = uSearcher.Search("IsInstalled=0 And IsHidden=0");

            return (from IUpdate update in sResult.Updates select update.Title).ToList();
        }
    }
}