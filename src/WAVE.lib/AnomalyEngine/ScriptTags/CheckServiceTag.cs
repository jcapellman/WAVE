using WAVE.lib.AnomalyEngine.ScriptTags.Base;

namespace WAVE.lib.AnomalyEngine.ScriptTags
{
    public class CheckServiceTag : BaseScriptTags
    {
        protected override string[] ValidArguments => new[] { "servicename" };

        public override string Run(string[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }
}