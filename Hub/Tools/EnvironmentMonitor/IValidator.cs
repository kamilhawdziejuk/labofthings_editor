using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    interface IValidator
    {
        ProblematicSituation Validate(VModuleCondition vModuleCondition);
        string Name { get; }
        SituationPriority Priority { get; }
    }
}
