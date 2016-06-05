using System.Collections.Generic;
using Newtonsoft.Json;

namespace EnvironmentMonitor
{
    public class HomeModuleDbEntry
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Name { get; set; }
        public List<string> States { get; set; }
    }
}