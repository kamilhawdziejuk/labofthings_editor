using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.EnvironmentMonitor.Problems;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Platform.EnvironmentMonitor.Validators
{
    /// <summary>
    /// Validates if the limit was exceeded
    /// </summary>
    class LimitValidator : IValidator
    {
        private double _maxValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_percentLimit">Fraction [0,1]</param>
        public LimitValidator(double maxValue)
        {
            this._maxValue = maxValue;
        }

        public ProblematicSituation Validate(VModuleCondition condition)
        {
            if (condition.ExactValue <= this._maxValue)
            {
                return null;
            }
            else
            {
                return new ExceedSituation(condition, this._maxValue, condition.ExactValue);
            }
        }

        public string Name
        {
            get { return "LimitValidator"; }
        }

        public virtual SituationPriority Priority
        {
            get
            {
                return SituationPriority.Warning;
            }
        }
    }
}
