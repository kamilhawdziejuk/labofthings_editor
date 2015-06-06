using HomeOS.Hub.Common;
namespace HomeOS.Hub.Tools.EnvironmentMonitor.Modules
{
    public class LightBulpSimulation : ModuleBase, ModuleCondition
    {
        public LightBulpSimulation()
        {
            //moduleInfo = new ModuleInfo("Light bulp", "LightBulpSimulation", null, null, false, null);
        }

        public override string GetDescription(string hint)
        {
            return "Light bulp";
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

        public System.Collections.Generic.Dictionary<double, string> PossibleIntepretedValues
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
