using System.Collections.Generic;

namespace EnvironmentMonitor
{
    public class HomeConfigurationDb
    {
        public List<HomeRuleDbEntry> Rules;

        public HomeConfigurationDb()
        {
            Rules = new List<HomeRuleDbEntry>();
        }
    }
}