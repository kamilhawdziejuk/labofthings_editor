using System;
using EnvironmentMonitor;
using PetrinetTool;
using PetrinetTool.AnalysisTools;

namespace HomeOS.Hub.Tools.EnvironmentMonitor.Validators
{
    public class PetriNetValidator
    {
        public Result CheckDeadlock(Petrinet petriNet)
        {
            var result = new Result() {Name = "Deadlock"};
            var deadlock = new Deadlock(petriNet, 5);
            try
            {
                deadlock.Run();
                result.Success = !deadlock.IsDeadlock;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public Result CheckBoundness(Petrinet petriNet)
        {
            var result = new Result() {Name = "Boundness"};
            var bounded = new Bounded(petriNet, 5);
            try
            {
                bounded.Run();
                result.Success = !bounded.IsBounded;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}