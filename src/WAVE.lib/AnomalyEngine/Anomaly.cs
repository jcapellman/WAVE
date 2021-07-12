namespace WAVE.lib.AnomalyEngine
{
    public class AnomalyListener
    {
        public string Name { get; internal set;  }

        public void Run() { }

        public void Stop() { }

        public AnomalyListener(string scriptContent)
        {
            // TODO: Parse JSON
        }
    }
}