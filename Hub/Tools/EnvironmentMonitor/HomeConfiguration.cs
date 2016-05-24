using System;
using System.Collections.Generic;
using System.Linq;
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

            this.client = new DocumentClient(new Uri(HomeConfigurationAzureDocumentDb.EndpointUri), HomeConfigurationAzureDocumentDb.PrimaryKey);
        }

        public async Task CreateDocumentIfNotExists(string databaseName, string collectionName, HomeRuleDbEntry entry)
        {
            try
            {
                await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), entry);
            }
            catch (Exception ex)
            {   
                throw;
            }
            
        }

        public List<string> ExecuteSimpleQuery(string databaseName, string collectionName)
        {
            try
            {
                var results = new List<string>();

                var dataExist = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName))
                       .AsEnumerable()
                       .Any();

                if (dataExist)
                {
                    var data = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName)).AsEnumerable();
                    foreach (dynamic rule in data)
                    {
                       results.Add(rule.Action); 
                    }
                }

                return results;

                // Set some common query options
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = 10};

                // Here we find the Andersen family via its LastName
                IQueryable<HomeRuleDbEntry> familyQuery = this.client.CreateDocumentQuery<HomeRuleDbEntry>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions);
                //.Where(f => f.LastName == "Andersen");

                // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
                Console.WriteLine("Running LINQ query...");
                foreach (HomeRuleDbEntry family in familyQuery)
                {
                    Console.WriteLine("\tRead {0}", family);
                }

                // Now execute the same query via direct SQL
                IQueryable<HomeRuleDbEntry> familyQueryInSql = this.client.CreateDocumentQuery<HomeRuleDbEntry>(
                        UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                        "SELECT * FROM rules",//" WHERE Family.lastName = 'Andersen'",
                        queryOptions);

                Console.WriteLine("Running direct SQL query...");
                foreach (HomeRuleDbEntry family in familyQueryInSql)
                {
                    Console.WriteLine("\tRead {0}", family);
                }

                Console.WriteLine("Press any key to continue ...");
                Console.ReadKey();
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