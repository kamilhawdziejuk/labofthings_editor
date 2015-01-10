using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    interface ISolution
    {
        bool Apply(ProblematicSituation _problem);
    }
}
