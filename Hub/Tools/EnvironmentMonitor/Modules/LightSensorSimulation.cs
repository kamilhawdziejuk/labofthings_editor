using HomeOS.Hub.Common;
using System.Collections.Generic;
namespace HomeOS.Hub.Tools.EnvironmentMonitor.Modules
{
    public class LightSensorSimulation : ModuleBase, ModuleCondition
    {
        public LightSensorSimulation()
        {
            //moduleInfo = new ModuleInfo("Light bulp", "LightBulpSimulation", null, null, false, null);
        }

        public override string GetDescription(string hint)
        {
            return "Light sensor";
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
                result.Add(0, "< 200 lx");
                result.Add(1, "[200; 700] lx");
                result.Add(2, "> 700 lx");
                return result;
            }
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
