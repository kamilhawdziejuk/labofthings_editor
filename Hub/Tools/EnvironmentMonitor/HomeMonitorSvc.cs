﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HomeOS.Hub.Common;
using HomeOS.Hub.Platform.Views;
using System.Net;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using HomeOS.Hub.Tools.EnvironmentMonitor.Modules;


namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HomeMonitorSvc : IHomeMonitorServiceWeb
    {
        private VLogger logger;
        private List<VModule> _modules = new List<VModule>();

        public HomeMonitorSvc(VLogger _logger)
        {
            this.logger = _logger;
            InitModules();
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
            List<string> results = new List<string>();
            foreach (var kvp in (module as ModuleCondition).PossibleIntepretedValues)
            {
                results.Add(kvp.Value);
            }
            return results;
        }

        private void InitModules()
        {
            _modules.Add(new LightBulpSimulation());
            _modules.Add(new ThermomentrSimulation());
            _modules.Add(new MotionSensorSimulation());
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