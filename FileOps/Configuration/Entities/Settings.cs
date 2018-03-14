using Newtonsoft.Json.Linq;

namespace FileOps.Configuration.Entities
{
    internal class Settings
    {
        public string Identifier { get; set; }

        public bool Enabled { get; set; }

        public bool TriggerOnStart { get; set; }

        public string Cron { get; set; }

        public Step[] Pipe { get; set; }

        public class Step
        {
            public string StepName { get; set; }

            public JObject StepSettings { get; set; }
        }
    }
}
