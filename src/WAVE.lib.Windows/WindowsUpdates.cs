using System.Collections.Generic;
using System.Linq;

using WAVE.lib.Updates.Base;
using WAVE.lib.Updates.Containers;

using WUApiLib;

namespace WAVE.lib.Windows
{
    public class WindowsUpdates : BaseUpdateCheck
    {
        public override List<UpdateListingResponseItem> GetUpdateList()
        {
            var uSession = new UpdateSession();
            var uSearcher = uSession.CreateUpdateSearcher();

            uSearcher.Online = false;

            var sResult = uSearcher.Search("IsInstalled=0 And IsHidden=0");

            return (from IUpdate update in sResult.Updates select new UpdateListingResponseItem { 
                Name = update.Title, 
                UpdateType = update.Type.ToString(), 
                Description = update.Description }).ToList();
        }

        public override List<string> GetUpdateNameOnlyList() => GetUpdateList().Select(a => a.Name).ToList();
    }
}