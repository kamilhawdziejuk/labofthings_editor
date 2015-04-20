using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Tools.EnvironmentMonitor
{
    /// <summary>
    /// Class that represents problem inside home
    /// </summary>
    abstract class ProblematicSituation
    {
        /// <summary>
        /// State that causes this problem (problematic situation)
        /// </summary>
        protected virtual VModuleCondition Condition
        {
            get
            {
                return this.condition;
            }
        }

        public virtual string Description
        {
            get
            {
                return "Problematic situation";
            }
        }

        protected VModuleCondition condition;

        public ProblematicSituation(VModuleCondition _VModuleCondition)
        {
            this.condition = _VModuleCondition;
        }
    }
}
