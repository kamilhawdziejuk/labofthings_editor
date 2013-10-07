using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeOS.Hub.Platform.EnvironmentMonitor
{
    interface ISolution
    {
        bool Apply(ProblematicSituation _problem);
    }
}
