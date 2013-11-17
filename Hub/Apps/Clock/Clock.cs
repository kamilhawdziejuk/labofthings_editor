using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Common;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Apps.Clock
{
    /// <summary>
    /// A clock module that simulate time at home
    /// </summary>
    [System.AddIn.AddIn("HomeOS.Hub.Apps.Clock")]
    public class Clock : ModuleBase, ModuleCondition
    {
        private DateTime dateTime;

        public override void Start()
        {
            logger.Log("Started: {0}", ToString());

            //..... get the list of current ports from the platform
            IList<VPort> allPortsList = GetAllPortsFromPlatform();

            dateTime = DateTime.Now;
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
                System.Threading.Thread.Sleep(1 * 1000);
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
    }
}
