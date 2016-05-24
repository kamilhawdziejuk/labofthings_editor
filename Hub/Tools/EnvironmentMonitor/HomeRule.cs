using System;
using System.IO;
using System.Windows.Documents;
using System.Xml;
using Newtonsoft.Json;
using PetrinetTool;

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
    }

    public class HomeRule : IXmlProvider
    {

        public HomeModule FromModule;
        public HomeModule ToModule;

        public HomeRule()
        {
            
        }

        public string Name
        {
            get
            {
                return string.Format("{0}{1}", FromModule.Name, ToModule.Name);
            }
        }

        public Arc Arc1
        {
            get
            {
                var arc1 = new Arc()
                {
                    SourceID = FromModule.Description,
                    TargetID = Transition.Id,
                    Weight = 1,
                    Id = String.Format("{0}->{1}", FromModule.Description, Transition.Id)
                };
                return arc1;
            }
        }

        public Arc Arc2
        {
            get
            {
                var arc2 = new Arc()
                {
                    SourceID = Transition.Id,
                    TargetID = ToModule.Description,
                    Weight = 1,
                    Id = String.Format("{0}->{1}", Transition.Id, ToModule.Description)
                };
                return arc2;
            }
        }

        public PetrinetTool.Transition Transition
        {
            get
            {
                var transition = new PetrinetTool.Transition()
                {
                    Name = String.Format("Event({0}->{1})", FromModule.Name, ToModule.Name),
                    Id = String.Format("{0}_{1}", FromModule.Name, ToModule.Name)
                };
                return transition;
            }
        }

        public string Data
        {
            get
            {
                using (var sw = new StringWriter())
                {
                    using (var xw = XmlWriter.Create(sw))
                    {
                        this.Export(xw);
                    }
                    return sw.ToString();
                }
            }
        }

        public void Export(XmlWriter writer)
        {
            writer.WriteStartElement("state");
            writer.WriteAttributeString("id", FromModule.Description);

            writer.WriteStartElement("transition");
            writer.WriteAttributeString("event", Transition.Name);
            writer.WriteAttributeString("target", ToModule.Description);
            
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
