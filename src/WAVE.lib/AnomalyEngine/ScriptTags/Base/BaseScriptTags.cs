namespace WAVE.lib.AnomalyEngine.ScriptTags.Base
{
    public abstract class BaseScriptTags
    {
        private readonly string[] _arguments;

        protected BaseScriptTags(string[] arguments)
        {
            _arguments = arguments;
        }

        public abstract string Run();
    }
}