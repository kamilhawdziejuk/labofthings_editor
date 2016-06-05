using EnvironmentMonitor;
using HomeOS.Hub.Common;
using System.Collections.Generic;

namespace HomeOS.Hub.Tools.EnvironmentMonitor.Modules
{
    public class ClockSimulation : ModuleBase, ModuleCondition, IModuleLinks
    {
        public ClockSimulation()
        {
            //moduleInfo = new ModuleInfo("Light bulp", "LightBulpSimulation", null, null, false, null);
        }

        #region Simulation


        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public override void PortRegistered(Platform.Views.VPort port)
        {
            throw new System.NotImplementedException();
        }

        public override void PortDeregistered(Platform.Views.VPort port)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public double ExactValue
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public double InterpretedValue
        {
            get { throw new System.NotImplementedException(); }
        }

        public Dictionary<double, string> PossibleIntepretedValues
        {
            get
            {
                var result = new Dictionary<double, string>();
                result.Add(0, "day [6;22]");
                result.Add(1, "night (22;6)");
                return result;
            }
        }

        public override string GetDescription(string hint)
        {
            return "Clock";
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }

        public List<string> Links
        {
            get
            {
                return new List<string>() {"time", "hour", "day"};
            }
        }
    }
}
