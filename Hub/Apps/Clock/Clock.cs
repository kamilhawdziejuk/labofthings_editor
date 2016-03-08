using System;
using System.Collections.Generic;
using HomeOS.Hub.Common;
using HomeOS.Hub.Platform.Views;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;


namespace HomeOS.Hub.Apps.Clock
{
    /// <summary>
    /// A clock module that simulate time at home
    /// </summary>
    [System.AddIn.AddIn("HomeOS.Hub.Apps.Clock")]
    public class Clock : ModuleBase, ModuleCondition
    {

        //####################EVENT HUB related code####################################

        static string connectionString = GetServiceBusConnectionString();
        NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
        static string eventHubName = ConfigurationManager.AppSettings["EventHubName"];

        Sender sender = new Sender(eventHubName);

        //###############################################################################

        private DateTime dateTime;
        string gHome_Id;

        public override void Start()
        {
            logger.Log("Started: {0}", ToString());

            //..... get the list of current ports from the platform
            IList<VPort> allPortsList = GetAllPortsFromPlatform();

            dateTime = DateTime.Now;
            gHome_Id = GetConfSetting("HomeId");

            Work();
        }

        public override void Stop()
        {
            Finished();
        }

        public override void PortRegistered(VPort port)
        {

        }

        public override void PortDeregistered(VPort port)
        {

        }

        public override VModuleCondition GetCondition()
        {
            return this as VModuleCondition;
        }

        public void Work()
        {
            while (true)
            {
                //simulate one hour every 1 seconds
                dateTime = dateTime.Add(TimeSpan.FromTicks(TimeSpan.TicksPerHour));
                bool bIsEventSent = sender.SendEvents(gHome_Id, DateTime.Now, "Clock", "Time", DateTime.Now.ToString());

                System.Threading.Thread.Sleep(1 * 5000);
            }
        }

        public double ExactValue
        {
            get
            {
                return (double)this.dateTime.TimeOfDay.TotalSeconds;
            }
            set
            {
                //it is just running, we can not influence on time
            }
        }

        public double InterpretedValue
        {
            get
            {
                if (this.dateTime.Hour > 22 || this.dateTime.Hour < 6)
                {
                    //night
                    return 0;
                }
                else
                {
                    //day
                    return 1;
                }
            }
        }

        public object Clone()
        {
            Clock copy = new Clock();
            copy.dateTime = this.dateTime;
            return copy;
        }


        public Dictionary<double, string> PossibleIntepretedValues
        {
            get
            {
                var dict = new Dictionary<double, string>();
                dict.Add(0, "Night");
                dict.Add(1, "Day");
                return dict;
            }
        }

        private static string GetServiceBusConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Did not find Service Bus connections string in appsettings (app.config)");
                return string.Empty;
            }
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString);
            builder.TransportType = TransportType.Amqp;
            return builder.ToString();
        }
    }
}
