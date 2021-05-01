using System.Collections.Generic;

using WAVE.lib.Applications.Containers;

namespace WAVE.lib.Applications.Base
{
    public abstract class BaseApplicationCheck
    {
        public abstract List<ApplicationResponseItem> GetInstalledApplications();
    }
}