using System.Linq;

namespace WAVE.lib.AnomalyEngine.ScriptTags.Base
{
    public abstract class BaseScriptTags
    {
        private readonly string[] _arguments;

        protected abstract string[] ValidArguments { get; }

        private bool ValidateArguments(string[] arguments)
        {
            for (var x = 0; x < arguments.Length; x+=2)
            {
                if (!ValidArguments.Contains(arguments[x].ToLowerInvariant())) {
                    return false;
                }
            }

            return true;
        }

        public string ParseArguments(string[] arguments)
        {
            if (!ValidateArguments(arguments))
            {
                return null;
            }

            return Run(arguments);
        }

        public abstract string Run(string[] arguments);
    }
}