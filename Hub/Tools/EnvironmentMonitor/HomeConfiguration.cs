using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace EnvironmentMonitor
{
    public class HomeConfiguration : IXmlProvider
    {
        private DocumentClient client;

        private readonly List<HomeRule> _homeRules;

        public HomeConfiguration()
        {
            _homeRules = new List<HomeRule>();
        }

        public async Task CreateDocumentIfNotExists(string databaseName, string collectionName, HomeRuleDbEntry entry)
        {
            try
            {
                this.client = new DocumentClient(new Uri(HomeConfigurationAzureDocumentDb.EndpointUri), HomeConfigurationAzureDocumentDb.PrimaryKey);
                await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), entry);
            }
            catch (Exception ex)
            {   
                throw;
            }
            
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