using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Tools.EnvironmentMonitor.Validators
{
    class HistoryValidator : IValidator
    {
        private List<VModuleCondition> history = new List<VModuleCondition>();

        public List<VModuleCondition> History
        {
            get { return this.history; }
            set { this.history = value; }
        }

        public HistoryValidator()
        {
            
        }

        public virtual ProblematicSituation Validate(VModuleCondition VModuleCondition)
        {
            return null;
        }

        public virtual string Name
        {
            get { return "HistoryValidator"; }
        }

        public virtual SituationPriority Priority
        {
            get
            {
                return SituationPriority.Normal;
            }
        }
    }
}
