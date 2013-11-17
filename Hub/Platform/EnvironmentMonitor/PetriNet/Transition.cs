//07.05.2013.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Platform.EnvironmentMonitor.PetriNet
{
    /// <summary>
    /// Class that represents flow from sources states to target states
    /// </summary>
    public class Transition
    {
        #region --- constructors ---

        /// <summary>
        /// Simple flow between one state and another
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="_output"></param>
        /// <param name="_tokensNeeded"></param>
        public Transition(VModuleCondition _input, VModuleCondition _output, int _tokensNeeded)
        {
            this.intputStates.Add(_input);
            this.outputStates.Add(_output);
            this.tokensNeeded = _tokensNeeded;
        }

        #endregion

        #region --- private fields ---

        private List<VModuleCondition> intputStates;
        private List<VModuleCondition> outputStates;

        /// <summary>
        /// How many tokens it is needed to fire
        /// </summary>
        private int tokensNeeded;

        private int priority = 1;

        #endregion

        #region --- public properties ---

        public List<VModuleCondition> InputStates
        {
            get { return intputStates; }
            set { intputStates = value; }
        }

        public List<VModuleCondition> OutputStates
        {
            get { return outputStates; }
            set { outputStates = value; }
        }

        public int TokensNeeded
        {
            get { return tokensNeeded; }
            set { tokensNeeded = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        #endregion
    }
}
