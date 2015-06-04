using System;
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


namespace HomeOS.Hub.Apps.Light
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HomeMonitorSvc : ISimplexLightNotifierContract
    {
        private VLogger logger;

        public HomeMonitorSvc(VLogger _logger)
        {
            this.logger = _logger;
        }

        public static SafeServiceHost CreateServiceHost(VLogger _logger, ModuleBase _moduleBase,
            ISimplexLightNotifierContract _instance, string _address)
        {
            SafeServiceHost service = new SafeServiceHost(_logger, _moduleBase, _instance, _address);
            var contract = ContractDescription.GetContract(typeof(ISimplexLightNotifierContract));
            var webBinding = new WebHttpBinding();
            var webEndPoint = new ServiceEndpoint(contract, webBinding, new EndpointAddress(service.BaseAddresses()[0]));
            webEndPoint.EndpointBehaviors.Add(new WebHttpBehavior());
            service.AddServiceEndpoint(webEndPoint);
            service.AddServiceMetadataBehavior(new ServiceMetadataBehavior());
            return service;
        }

        public double GetModules()
        {
            return 0;
        }
    }

    [ServiceContract]
    public interface ISimplexLightNotifierContract
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat=WebMessageFormat.Json)]
        double GetModules();
    }
}
