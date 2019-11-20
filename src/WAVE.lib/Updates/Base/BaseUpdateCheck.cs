using System.Collections.Generic;

using WAVE.lib.Updates.Containers;

namespace WAVE.lib.Updates.Base
{
    public abstract class BaseUpdateCheck
    {
        public abstract List<string> GetUpdateNameOnlyList();

        public abstract List<UpdateListingResponseItem> GetUpdateList();
    }
}