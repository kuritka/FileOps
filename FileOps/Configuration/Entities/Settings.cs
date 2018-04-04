using Newtonsoft.Json.Linq;

namespace FileOps.Configuration.Entities
{
    internal class Settings : ISettings
    {
        public bool Enabled { get; set; }

        public Step[] Pipe { get; set; }

        public CommonRecord[] Common { get; set; }

        public string Identifier { get; set; }

        public string GroupIdentifier { get; set; }

        public class Step
        {
            public string StepName { get; set; }

            public JObject StepSettings { get; set; }

            public string Reference { get; set; }
        }

        public class CommonRecord
        {
            public string Name { get; set; }

            public JObject StepSettings { get; set; }
        }
    }

}
