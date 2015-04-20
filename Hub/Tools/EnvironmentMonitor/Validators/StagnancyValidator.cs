using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Tools.EnvironmentMonitor.Problems;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Tools.EnvironmentMonitor.Validators
{
    /// <summary>
    /// Checks last states if they haven't changed
    /// </summary>
    class StagnancyValidator : HistoryValidator
    {
        private double eps = 0.00001;

        /// <summary>
        /// Counter of situations that haven't changed
        /// </summary>
        private int maxStagnacyCounter = 10;

        public StagnancyValidator(int _maxStagnacyCounter)
        {
            this.maxStagnacyCounter = _maxStagnacyCounter;
        }

        public override string Name
        {
            get { return "StagnancyValidator"; }
        }

        public override ProblematicSituation Validate(VModuleCondition vModuleCondition)
        {
            int cnt = 0;
            int n = this.History.Count;
            int max = maxStagnacyCounter;

            //we will remember here state that 'VModuleCondition' is problematic with
            VModuleCondition differentState = null;

            for (int i = n - 1; i >= 0; i--)
            {
                if (Math.Abs(this.History[i].ExactValue - vModuleCondition.ExactValue) < eps && vModuleCondition != this.History[i])
                {
                    cnt++;
                    differentState = this.History[i];
                }
                max--;
                if (max < 0)
                {
                    break;
                }
            }
            if (cnt >= maxStagnacyCounter && differentState != null)
            {
                return new StatesConflictSituation(vModuleCondition, differentState);
            }
            return null;
        }

        public override SituationPriority Priority
        {
            get
            {
                if (maxStagnacyCounter < 50)
                {
                    return SituationPriority.Warning;
                }
                else
                {
                    return SituationPriority.Alarm;
                }
            }
        } 
    }
}
