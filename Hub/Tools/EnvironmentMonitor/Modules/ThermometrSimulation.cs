using EnvironmentMonitor;
using HomeOS.Hub.Common;
using System.Collections.Generic;
namespace HomeOS.Hub.Tools.EnvironmentMonitor.Modules
{
    public class ThermometrSimulation : ModuleBase, ModuleCondition, IModuleLinks
    {
        public ThermometrSimulation()
        {
            //moduleInfo = new ModuleInfo("Light bulp", "LightBulpSimulation", null, null, false, null);
        }

        public override string GetDescription(string hint)
        {
            return "Thermometr";
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
                Dictionary<double, string> result = new Dictionary<double, string>();
                result.Add(0, "< 16");
                result.Add(1, "[16; 25]");
                result.Add(2, "> 25");
                return result;
            }
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }


        public List<string> Links
        {
            get
            {
                return new List<string>() { "temp.", "temperature", "hot", "cold", "thermometr" };
            }
        }
    }

  
}
