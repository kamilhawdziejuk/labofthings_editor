using System;
using System.Windows.Documents;
using Newtonsoft.Json;

namespace EnvironmentMonitor
{
    public class HomeRuleDbEntry
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Action { get; set; }
        public string StateFrom { get; set; }

        public string StateTo { get; set; }


        public HomeRuleDbEntry(HomeRule rule)
        {
            Id = Guid.NewGuid().ToString();
            StateFrom = rule.FromModule.Description;
            StateTo = rule.ToModule.Description;
            Action = rule.Transition.Name;
        }
        public HomeRuleDbEntry(string id, string action, string stateFrom, string stateTo)
        {
            Id = id;
            Action = action;
            StateFrom = stateFrom;
            StateTo = stateTo;
        }
    }
}