using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Tools.EnvironmentMonitor.Solutions;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    /// <summary>
    /// Class that deals with situations at home
    /// </summary>
    class SituationSolver
    {
        private LoggerSolution loggerSolution;

        public SituationSolver(VLogger _logger)
        {
            this.loggerSolution = new LoggerSolution(_logger);
        }

        /// <summary>
        /// Main method for dealing with situations. It simply log, warn the user or even change the state of the home.
        /// </summary>
        /// <param name="_situation"></param>
        /// <param name="_priority"></param>
        /// <returns></returns>
        public bool Solve(ProblematicSituation _situation, SituationPriority _priority)
        {
            if (_priority == SituationPriority.Warning)
            {
                return loggerSolution.Apply(_situation);
            }
            else if (_priority == SituationPriority.Alarm)
            {
                //here should go something else than simply log, something that for example take control of the house;
                //return true;
            }
            return false;
        }
    }
}
