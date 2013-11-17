using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Platform.EnvironmentMonitor.PetriNet
{
    /// <summary>
    /// Class for representing states and transitions between them in home environment
    /// </summary>
    public class HomeGraph
    {
        #region --- private fields ---

        /// <summary>
        /// Edges
        /// </summary>
        List<Transition> transitions = new List<Transition>();

        /// <summary>
        /// Nodes
        /// </summary>
        List<VModuleCondition> states = new List<VModuleCondition>();

        #endregion

        #region --- public methods ---

        public bool AddState(VModuleCondition _state)
        {
            if (!this.states.Contains(_state))
            {
                this.states.Add(_state);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add transition (that define event in home environment).
        /// </summary>
        /// <param name="_transition"></param>
        /// <returns>True if all input and output states are defined</returns>
        public bool AddTransition(Transition _transition)
        {
            if (!this.transitions.Contains(_transition))
            {
                foreach (var inputState in _transition.InputStates)
                {
                    if (!this.states.Contains(inputState))
                    {
                        //all states must be defined before
                        return false;
                    }
                }
                foreach (var outputState in _transition.OutputStates)
                {
                    if (!this.states.Contains(outputState))
                    {
                        //all states must be defined before
                        return false;
                    }
                }
            }
            if (this.transitions.Contains(_transition))
            {
                return false;
            }
            else
            {
                this.transitions.Add(_transition);
                return true;
            }
        }

        #endregion
    }
}
