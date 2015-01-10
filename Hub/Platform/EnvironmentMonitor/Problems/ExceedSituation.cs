using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Tools.EnvironmentMonitor.Problems
{
    class ExceedSituation : ProblematicSituation
    {
        double maxValue;
        double valueForSet;

        public ExceedSituation(VModuleCondition _VModuleCondition, double _maxValue, double _valueForSet)
            : base(_VModuleCondition)
        {
            this.maxValue = _maxValue;
            this.valueForSet = _valueForSet;
        }

        public override string Description
        {
            get
            {
                return "Limit has exceeded. The maximum value have to be below " + this.maxValue + ", but there is a try to set " + valueForSet;
            }
        }
    }
}
