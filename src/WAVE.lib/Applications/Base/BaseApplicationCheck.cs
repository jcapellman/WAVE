using System.Collections.Generic;

namespace WAVE.lib.Applications.Base
{
    public abstract class BaseApplicationCheck
    {
        public abstract List<string> GetInstalledApplicationsNameOnly();
    }
}