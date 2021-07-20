using System;
using System.Linq;

namespace WAVE.lib.AnomalyEngine.ScriptTags.Base
{
    public class ScriptArguments
    {

    }

    public abstract class BaseScriptTags<T> where T : ScriptArguments
    {
        public string RunAndParse(string[] arguments)
        {
            if (arguments.Length % 2 != 0)
            {
                throw new ArgumentException($"Invalid number of arguments ({arguments.Length} provided)");
            }

            var obj = Activator.CreateInstance<T>();

            var properties = obj.GetType().GetProperties();

            for (var x =0; x < arguments.Length; x+=2) {                             
                var property = properties.FirstOrDefault(a => a.Name.Equals(arguments[x], StringComparison.InvariantCultureIgnoreCase));

                if (property == null)
                {
                    // TODO LOG
                    continue;
                }

                property.SetValue(obj, arguments[x + 1]);
            }

            return RunTag(obj);
        }

        protected abstract string RunTag(T argumentClass);
    }
}