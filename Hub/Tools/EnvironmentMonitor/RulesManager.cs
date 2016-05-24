using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using HomeOS.Hub.Tools.EnvironmentMonitor.Validators;
using PetrinetTool;

namespace EnvironmentMonitor
{
    public class RulesManager
    {
        private readonly HomeConfiguration _configuration;
        private readonly List<HomeModule> _homeModules = new List<HomeModule>();

        private readonly Page _page = new Page();
        private readonly Petrinet _petriNet = new Petrinet();

        private readonly PetriNetValidator _petriNetValidator = new PetriNetValidator();

        public RulesManager()
        {
            _configuration = new HomeConfiguration();
            _petriNet.Pages.Add("current", _page);
        }


        public List<string> GetRules()
        {
            return _configuration.ExecuteSimpleQuery(HomeConfigurationAzureDocumentDb.DatabaseName, HomeConfigurationAzureDocumentDb.CollectionName); 
        }

        public void AddRule(string mod1, string state1, string mod2, string state2)
        {
            var moduleFrom = new HomeModule {Name = mod1, StateDesc = state1};
            var moduleTo = new HomeModule {Name = mod2, StateDesc = state2};

            if (!_homeModules.Contains(moduleFrom))
            {
                _homeModules.Add(moduleFrom);
            }

            if (!_homeModules.Contains(moduleTo))
            {
                _homeModules.Add(moduleTo);
            }

            var homeRule = new HomeRule {FromModule = moduleFrom, ToModule = moduleTo};
            _configuration.AddRule(homeRule);


            XmlWriter writer = XmlWriter.Create("HomeConfiguration.xml");
            _configuration.Export(writer);
            writer.Dispose();


            _configuration.CreateDocumentIfNotExists(HomeConfigurationAzureDocumentDb.DatabaseName, HomeConfigurationAzureDocumentDb.CollectionName, new HomeRuleDbEntry(homeRule));
            
            AddToPetriNet(homeRule);
        }

        public List<Result> GetPetrinetProperties()
        {
            var results = new List<Result>();
            Result deadlockExistance = _petriNetValidator.CheckDeadlock(_petriNet);
            Result boundnessExistance = _petriNetValidator.CheckBoundness(_petriNet);

            results.Add(deadlockExistance);
            results.Add(boundnessExistance);
            return results;
        }

        private void TryAddPlace(Place place)
        {
            try
            {
                _page.Places.Add(place);
            }
            catch (Exception ex)
            {
            }
        }

        private void TryAddTransition(Transition transition)
        {
            try
            {
                _page.Transitions.Add(transition);
            }
            catch (Exception ex)
            {
            }
        }

        private void TryAddArc(Arc arc)
        {
            try
            {
                _page.Arcs.Add(arc);
            }
            catch (Exception ex)
            {
            }
        }

        private void AddToPetriNet(HomeRule homeRule)
        {
            try
            {
                TryAddPlace(homeRule.FromModule.Place);
                TryAddPlace(homeRule.ToModule.Place);

                TryAddTransition(homeRule.Transition);
                TryAddArc(homeRule.Arc1);
                TryAddArc(homeRule.Arc2);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}