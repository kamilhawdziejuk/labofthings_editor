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


namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HomeMonitorSvc : IHomeMonitorServiceWeb
    {
        private VLogger logger;

        public HomeMonitorSvc(VLogger _logger)
        {
            this.logger = _logger;
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

            // service.Description.Behaviors.Add(new ServiceMetadataBehavior());
            //var metaBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            //metaBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.InheritedFromHost; 
            //service.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

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
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        double GetTestMethod();
    }
}
