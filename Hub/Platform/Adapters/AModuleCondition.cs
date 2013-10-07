using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Pipeline;
using System.Diagnostics.Contracts;
using HomeOS.Hub.Platform.Contracts;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Platform.Adapters
{
    public class ConditionV2C : ContractBase, IModuleCondition
    {
        #region standard stuff DO NOT TOUCH
        private VModuleCondition _condition;

        public ConditionV2C(VModuleCondition condition)
        {
            _condition = condition;
        }

        internal VModuleCondition GetSourceCondition()
        {
            return _condition;
        }
        #endregion

        /// <summary>
        /// Represents actual exact value of the state
        /// </summary>
        public double ExactValue
        {
            get
            {
                return _condition.ExactValue;
            }
            set { _condition.ExactValue = value; }
        }

        /// <summary>
        /// Represents actual interpreted value of the state (one of the possible interpreted state in Home System Net)
        /// </summary>
        public double InterpretedValue
        {
            get
            {
                return _condition.InterpretedValue;
            }
        }

        /// <summary>
        /// All possible interpreted stated in Home System Net
        /// </summary>
        public Dictionary<double, string> PossibleIntepretedValues
        {
            get
            {
                return _condition.PossibleIntepretedValues;
            }
        }

        public object Clone()
        {
            VModuleCondition VModuleConditionCopy = this._condition.Clone() as VModuleCondition;
            return new ConditionV2C(VModuleConditionCopy);
        }
    }

    public class ConditionC2V : VModuleCondition
    {
        #region standard stuff DO NOT TOUCH
        private IModuleCondition _contract;
        private ContractHandle _handle;

        public ConditionC2V(IModuleCondition contract)
        {
            _contract = contract;
            _handle = new ContractHandle(contract);
        }

        internal IModuleCondition GetSourceContract()
        {
            return _contract;
        }
        #endregion


        public double ExactValue
        {
            get { return _contract.ExactValue; }
            set { _contract.ExactValue = value; }
        }

        public double InterpretedValue
        {
            get
            {
                return _contract.InterpretedValue;
            }
        }

        public Dictionary<double, string> PossibleIntepretedValues
        {
            get
            {
                return _contract.PossibleIntepretedValues;
            }
        }

        public object Clone()
        {
            IModuleCondition contractClone = this._contract.Clone() as IModuleCondition;
            return new ConditionC2V(contractClone);
        }
    }

    public class ModuleConditionAdapter
    {
        internal static VModuleCondition C2V(IModuleCondition contract)
        {
            if (!System.Runtime.Remoting.RemotingServices.IsObjectOutOfAppDomain(contract) &&
                (contract.GetType().Equals(typeof(ConditionV2C))))
            {
                return ((ConditionV2C) (contract)).GetSourceCondition();
            }
            else
            {
                return new ConditionC2V(contract);
            }
        }

        internal static IModuleCondition V2C(VModuleCondition condition)
        {
            if (!System.Runtime.Remoting.RemotingServices.IsObjectOutOfAppDomain(condition) &&
                (condition.GetType().Equals(typeof(ConditionC2V))))
            {
                return ((ConditionC2V) (condition)).GetSourceContract();
            }
            else
            {
                return new ConditionV2C(condition);
            }
        }
      
    }

}
