using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Platform.EnvironmentMonitor.Problems
{
    class StatesConflictSituation : ProblematicSituation
    {
        /// <summary>
        /// Second state that causes this problematic situation (with state1)
        /// </summary>
        private VModuleCondition condition2;

        public VModuleCondition Condition2
        {
            get
            {
                return this.condition2;
            }
        }

        public StatesConflictSituation(VModuleCondition _condition1, VModuleCondition _condition2)
            : base(_condition1)
        {
            this.condition2 = _condition2;
        }

        public override string Description
        {
            get
            {
                return "Two of the states conflict each other. These are: " + this.Condition.ToString() + " and " + this.Condition2.ToString();
            }
        }
    }
}
