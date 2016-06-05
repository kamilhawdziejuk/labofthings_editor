using System;
using System.Collections.Generic;

namespace HomeOS.Hub.Platform.Views
{
    public interface VModuleCondition : ICloneable
    {
        /// <summary>
        /// Represents actual exact value of the state
        /// </summary>
        double ExactValue
        {
            get;
            set;
        }

        /// <summary>
        /// Represents actual interpreted value of the state (one of the possible interpreted state in Home System Net)
        /// </summary>
        double InterpretedValue
        {
            get;
        }

        /// <summary>
        /// All possible interpreted stated in Home System Net
        /// </summary>
        Dictionary<double, string> PossibleIntepretedValues
        {
            get;
        }

        string GetDescription(string hint);
    }
}
