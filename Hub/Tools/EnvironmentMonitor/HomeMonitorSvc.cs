using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using EnvironmentMonitor;
using HomeOS.Hub.Common;
using HomeOS.Hub.Platform.Views;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using HomeOS.Hub.Tools.EnvironmentMonitor.Modules;
using Newtonsoft.Json;

namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HomeMonitorSvc : IHomeMonitorServiceWeb
    {
        private VLogger logger;
        private RulesManager _rulesManager;
        private List<VModule> _modules = new List<VModule>();

        public HomeMonitorSvc(VLogger _logger)
        {
            this.logger = _logger;
            _rulesManager = new RulesManager();
            InitModules();
        }

        public List<string> AddRule(string rule)
        {
            string[] data = rule.Split(',');
            string mod1 = data[0];
            string state1 = data[1];
            string mod2 = data[2];
            string state2 = data[3];

            _rulesManager.AddRule(mod1, state1, mod2, state2);

            return new List<string>();
        }

        public string GetRules()
        {
            HomeConfigurationDb config = _rulesManager.GetRules();
            var result = JsonConvert.SerializeObject(config);
            return result;
        }

        public List<string> GetModuleNames()
        {
            List<string> names = new List<string>();
            foreach (VModule module in _modules)
            {
                names.Add(module.GetDescription(null));
            }
            return names;
        }

        public List<string> GetModuleStates(string name)
        {
            VModule module = _modules.Where(m => m.GetDescription(null).Equals(name)).SingleOrDefault();
            var results = new List<string>();
            foreach (var kvp in (module as ModuleCondition).PossibleIntepretedValues)
            {
                results.Add(kvp.Value);
            }
            return results;
        }

        public string ValidateStates()
        {
            var results = new ValidationsResponse();

            foreach (var p in _rulesManager.GetPetrinetProperties())
            {
                var res = new ValidationResponse();
                res.Name = p.Name;
                res.IsValid = p.Success;

                results.Validations.Add(res);
            }

            var result = JsonConvert.SerializeObject(results);
            return result;
        }

        public List<string> GetModuleLinks(string name)
        {
            VModule module = _modules.Where(m => m.GetDescription(null).Equals(name)).SingleOrDefault();
            return ((IModuleLinks) module).Links;
        }

        private string GetModuleNrInRule(string ruleText, List<string> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                var links = GetModuleLinks(names[i]);
                if (links.Any(l => ruleText.Contains(l.ToLower())))
                {
                    return i.ToString();
                }
            }
            return (-1).ToString();
        }

        public List<string> CheckRule(string ruleText)
        {
            var results = new List<string>();
            var moduleNames = this.GetModuleNames();

            int thenIndex = ruleText.ToLower().IndexOf("then");
            if (thenIndex != -1)
            {
                var first = ruleText.ToLower().Substring(0, thenIndex);
                var second = ruleText.ToLower().Substring(thenIndex + 4);

                var module1 = GetModuleNrInRule(first, moduleNames);
                var module2 = this.GetModuleNrInRule(second, moduleNames);

                results.Add(module1);
                results.Add(module2);
            }
            return results;
        }

        public List<string> GetModuleAttribValues(string name)
        {
            return null;
            //VModule module = _modules.Where(m => m.GetDescription(null).Equals(name)).SingleOrDefault();
            //List<string> results = new List<string>();
            //foreach (var kvp in (module as ModuleCondition).PossibleIntepretedValues)
            //{
            //    results.Add(kvp.Value);
            //}
            //return results;            
        }

        private void InitModules()
        {
            _modules.Add(new LightBulpSimulation());
            _modules.Add(new ThermomentrSimulation());
            _modules.Add(new MotionSensorSimulation());
            _modules.Add(new SpeakerSimulation());
            _modules.Add(new ClockSimulation());
            _modules.Add(new LightSensorSimulation());
        }

        public static ServiceHost CreateServiceHost(IHomeMonitorServiceWeb instance, Uri baseAddress)
        {
            ServiceHost service = new ServiceHost(instance, baseAddress);
            var contract = ContractDescription.GetContract(typeof(IHomeMonitorServiceWeb));

            var webBinding = new WebHttpBinding();

            var webEndPoint = new ServiceEndpoint(contract, webBinding, new EndpointAddress(baseAddress));
            WebHttpBehavior webBehaviour = new WebHttpBehavior();
            webBehaviour.HelpEnabled = true;
            webEndPoint.EndpointBehaviors.Add(webBehaviour);

            service.AddServiceEndpoint(webEndPoint);
            return service;
        }

        public double GetTestMethod()
        {
            return 0;
        }
    }

    [ServiceContract]
    public interface IHomeMonitorServiceWeb
    {
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        double GetTestMethod();
    }
}
