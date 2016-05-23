using System;
using System.Windows.Documents;
using System.Xml;
using PetrinetTool;

namespace EnvironmentMonitor
{
    public class HomeRule : IXmlProvider
    {
        public HomeModule FromModule;
        public HomeModule ToModule;

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
                    SourceID = FromModule.Id,
                    TargetID = Transition.Id,
                    Weight = 1,
                    Id = String.Format("{0}->{1}", FromModule.Id, Transition.Id)
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
                    TargetID = ToModule.Id,
                    Weight = 1,
                    Id = String.Format("{0}->{1}", Transition.Id, ToModule.Id)
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
                    Name = String.Format("Event{0}->{1}", FromModule.Name, ToModule.Name),
                    Id = String.Format("{0}_{1}", FromModule.Name, ToModule.Name)
                };
                return transition;
            }
        }

        public void Export(XmlWriter writer)
        {
            writer.WriteStartElement("state");
            writer.WriteAttributeString("id", FromModule.Id);

            writer.WriteStartElement("transition");
            writer.WriteAttributeString("event", Transition.Name);
            writer.WriteAttributeString("target", ToModule.Id);
            
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
