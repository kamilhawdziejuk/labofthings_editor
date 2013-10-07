using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Platform.EnvironmentMonitor.Solutions
{
    class LoggerSolution : ISolution
    {
        private VLogger logger;

        public LoggerSolution(VLogger _logger)
        {
            this.logger = _logger;
        }

        public bool Apply(ProblematicSituation _problem)
        {
            this.logger.Log("FlowsManager reported a situation that should be warned to the user: ", _problem.Description);
            return true;
        }
    }
}
