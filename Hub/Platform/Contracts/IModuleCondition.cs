﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Contract;

namespace HomeOS.Hub.Platform.Contracts
{
    public interface IModuleCondition : IContract, ICloneable
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
    }
}
