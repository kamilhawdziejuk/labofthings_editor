using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace EnvironmentMonitor
{
    public class HomeConfiguration : IXmlProvider
    {
        private readonly List<HomeRule> _homeRules;

        public HomeConfiguration()
        {
            _homeRules = new List<HomeRule>();
        }

        public void AddRule(HomeRule rule)
        {
            _homeRules.Add(rule);
        }
        public void Export(XmlWriter writer)
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("scxml");
            XNamespace ns = "http://www.w3.org/2005/07/scxml";

            //writer.WriteAttributeString("xmlns", ns.ToString());
            writer.WriteAttributeString("version", "1.0");
            writer.WriteAttributeString("initial", "ready");

            foreach (var homeRule in _homeRules)
            {
                homeRule.Export(writer);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}