﻿using System.Collections.Generic;
using System.Linq;

using WAVE.lib.Updates.Base;

using WUApiLib;

namespace WAVE.lib.Windows
{
    public class WindowsUpdates : BaseUpdateCheck
    {
        public override List<string> GetUpdateNameOnlyList()
        {
            var uSession = new UpdateSession();
            var uSearcher = uSession.CreateUpdateSearcher();

            uSearcher.Online = false;

            var sResult = uSearcher.Search("IsInstalled=0 And IsHidden=0");

            return (from IUpdate update in sResult.Updates select update.Title).ToList();
        }
    }
}