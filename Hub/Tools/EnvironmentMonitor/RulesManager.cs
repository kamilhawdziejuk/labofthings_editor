using System.Linq;
using HomeOS.Hub.Common;
using HomeOS.Hub.Tools.EnvironmentMonitor.Validators;
using PetrinetTool;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using PetrinetTool.AnalysisTools;

namespace EnvironmentMonitor
{
    public class RulesManager
    {
        /// <summary>
        /// Rules in the format of component, state_A, component2, state_B
        /// </summary>
        //private List<string> _rules = new List<string>();

        private List<string> _modules = new List<string>();
        private List<string> _states = new List<string>();

        private List<HomeModule> _homeModules = new List<HomeModule>();

        Petrinet _petriNet = new Petrinet();
        Page _page = new Page();

        PetriNetValidator petriNetValidator = new PetriNetValidator();

        public RulesManager()
        {
            _petriNet.Pages.Add("current", _page);
        }

        public void AddRule(string mod1, string state1, string mod2, string state2)
        {
            HomeModule moduleFrom = new HomeModule() { Name = mod1, StateDesc = state1 };
            HomeModule moduleTo = new HomeModule() { Name = mod2, StateDesc = state2 };

            if (!_homeModules.Contains(moduleFrom))
            {
                _homeModules.Add(moduleFrom);
            }

            if (!_homeModules.Contains(moduleTo))
            {
                _homeModules.Add(moduleTo);
            }
            
            HomeRule homeRule = new HomeRule() { FromModule = moduleFrom, ToModule = moduleTo };
            this.Export(homeRule);

            AddToPetriNet(homeRule);

            Result deadlockExistance = petriNetValidator.CheckDeadlock(_petriNet);
            Result boundnessExistance = petriNetValidator.CheckBoundness(_petriNet);
           
        }

        private void AddToPetriNet(HomeRule homeRule)
        {
            _page.Places.Add(homeRule.FromModule.Place);
            _page.Places.Add(homeRule.ToModule.Place);

            _page.Transitions.Add(homeRule.Transition);
            _page.Arcs.Add(homeRule.Arc1);
            _page.Arcs.Add(homeRule.Arc2);
        }

        private void Export(HomeRule homeRule)
        {
            string name = string.Format("Rules/{0}.xml", homeRule.Name);
            XmlWriter writer = XmlWriter.Create(name);

            writer.WriteStartDocument();

            writer.WriteStartElement("scxml");
            XNamespace ns = "http://www.w3.org/2005/07/scxml";

            //writer.WriteAttributeString("xmlns", ns.ToString());
            writer.WriteAttributeString("version", "1.0");
            writer.WriteAttributeString("initial", "ready");

            homeRule.Export(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

        }
    }
}
