using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using HomeOS.Hub.Platform.EnvironmentMonitor.Validators;
using HomeOS.Hub.Platform.Views;
//using PetriNetSharp.Engine;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;

namespace HomeOS.Hub.Platform.EnvironmentMonitor
{
    /// <summary>
    /// Class for managing flows between component's states in home enviroment
    /// </summary>
    public class EnvironmentMonitor
    {
        #region Fields

        VLogger logger;
        readonly VPlatform platform;
        SituationSolver solver;
        Dictionary<VModule, List<VModuleCondition>> history = new Dictionary<VModule, List<VModuleCondition>>();  

        List<IValidator> validators = new List<IValidator>();

        /// <summary>
        /// Represents the situation of the running home environment
        /// </summary>
        //PetriNetGraph homeGraph = new PetriNetGraph();

        #endregion

        #region Constructor

        public EnvironmentMonitor(VPlatform platform, VLogger logger)
        {
            this.platform = platform;
            this.logger = logger;
            this.solver = new SituationSolver(logger);
            this.validators.Add(new LimitValidator(-0.2));
            //when building adminstrator should take control of the house
            this.validators.Add(new StagnancyValidator(100));

            //when user should be warned
            this.validators.Add(new StagnancyValidator(15));
        }

        #endregion

        #region --- Flows managing ---

        /// <summary>
        /// Returns states of the running modules in the Home Environment (all modules and compoenents that are considered)
        /// </summary>
        /// <returns></returns>
        private IList<VModuleCondition> GetSystemStates()
        {
            IList<VModule> runningModules = this.platform.GetModules(true);
            List<VModuleCondition> retList = new List<VModuleCondition>();

            foreach (var mod in runningModules)
            {
                if (mod is VModuleCondition)
                {
                    retList.Add(mod as VModuleCondition);
                }
            }

            return retList as IList<VModuleCondition>;
        }

        /*
        private void SaveHomeEnvironmentStateAsPetriNet()
        {
            Place p1 = new Place(new Point(785, 213), "Night", "P1");
            Place p2 = new Place(new Point(419, 228), "Day", "P2");
            Place p3 = new Place(new Point(127, 90), "Min loudness", "P3");
            Place p4 = new Place(new Point(422, 92), "Half loudness", "P4");
            Place p6 = new Place(new Point(448, 451), "Mode_day", "P6");
            Place p7 = new Place(new Point(756, 453), "Mode_night", "P7");

            Transition t1 = new Transition("More light", new Point(607, 125), "T1", 1);
            Transition t2 = new Transition("Less light", new Point(610, 330), "T2", 1);
            Transition t3 = new Transition("Speaker ON", new Point(277, 153), "T3", 1);
            Transition t4 = new Transition("Speaker OFF", new Point(270, 35), "T4", 1);
            Transition t6 = new Transition("22 p.m.", new Point(599, 390), "T6", 1);
            Transition t7 = new Transition("6 a.m.", new Point(602, 500), "T7", 1);

            Arc a1 = new Arc(p1, t1);
            Arc a2 = new Arc(t1, p4);
            Arc a3 = new Arc(t1, p2);
            Arc a4 = new Arc(p2, t2);
            Arc a5 = new Arc(t2, p1);
            Arc a6 = new Arc(p4, t4);
            Arc a7 = new Arc(t4, p3);
            Arc a8 = new Arc(p4, t3);
            Arc a9 = new Arc(t3, p3);
            Arc a10 = new Arc(p7, t7);
            Arc a11 = new Arc(t7, p6);
            Arc a12 = new Arc(p6, t6);
            Arc a13 = new Arc(t6, p7);

            this.homeGraph.AddPlace(p1);
            this.homeGraph.AddPlace(p2);
            this.homeGraph.AddPlace(p3);
            this.homeGraph.AddPlace(p4);
            this.homeGraph.AddPlace(p6);
            this.homeGraph.AddPlace(p7);

            this.homeGraph.AddTransition(t1);
            this.homeGraph.AddTransition(t2);
            this.homeGraph.AddTransition(t3);
            this.homeGraph.AddTransition(t4);
            this.homeGraph.AddTransition(t6);
            this.homeGraph.AddTransition(t7);

            this.homeGraph.AddArc(a1);
            this.homeGraph.AddArc(a2);
            this.homeGraph.AddArc(a3);
            this.homeGraph.AddArc(a4);
            this.homeGraph.AddArc(a5);
            this.homeGraph.AddArc(a6);
            this.homeGraph.AddArc(a7);
            this.homeGraph.AddArc(a8);
            this.homeGraph.AddArc(a9);
            this.homeGraph.AddArc(a10);
            this.homeGraph.AddArc(a11);
            this.homeGraph.AddArc(a12);
            this.homeGraph.AddArc(a13);

            PetriNetSharp.IOSystem ios = new PetriNetSharp.IOSystem();
            ios.SaveAs(this.homeGraph, "D:\\data.pns");
            
            //here comes adding states...
            /*foreach (var state in this.GetSystemStates())
            {
                foreach (var kvp in state.PossibleIntepretedValues)
                {
                    Place place = new Place(
                    this.homeGraph.AddPlace(
                }
                //this.homeGraph.AddState(state);
            }

            //here comes adding possible transitions...
        }
        */

        #endregion

        #region --- private methods ---

        private List<VModuleCondition> GetStatesHistory(VModule _vModule)
        {
            return this.history[_vModule];
        }

        private void AddToHistory(VModule mod)
        {
            VModuleCondition VModuleCondition = mod.GetCondition();
            if (!history.ContainsKey(mod))
            {
                history.Add(mod, new List<VModuleCondition>());
            }
            VModuleCondition VModuleConditionCopy = VModuleCondition.Clone() as VModuleCondition;
            history[mod].Add(VModuleConditionCopy);
        }
    
        /// <summary>
        /// Validates the given state
        /// </summary>
        /// <param name="VModuleCondition"></param>
        /// <returns></returns>
        private bool Validate(VModuleCondition VModuleCondition)
        {
            foreach (IValidator validator in this.validators)
            {
                ProblematicSituation problem = validator.Validate(VModuleCondition);
                if (problem != null)
                {
                    if (!solver.Solve(problem, validator.Priority))
                    {
                        logger.Log("Danger state. Some other solution or solver should be fired here...");
                        return false;
                    }
                }
            }
            return true;
        }

        private void UpdateValidators(VModule _vModule)
        {
            foreach (IValidator validator in this.validators)
            {
                if (validator is HistoryValidator && this.history.ContainsKey(_vModule))
                {
                    (validator as HistoryValidator).History = this.history[_vModule];
                }
            }
        }

        #endregion

        public void Start()
        {
            return;
            System.Threading.Thread.Sleep(5000);
            logger.Log("Environment Monitor agent has started...");
            while (true)
            {
                IList<VModule> list = this.platform.GetModules(true);
                if (list.Count > 0)
                {
                    foreach (VModule mod in list)
                    {
                        VModuleCondition modCondition = mod.GetCondition();
                        if (modCondition != null)
                        {
                            this.AddToHistory(mod);
                            this.UpdateValidators(mod);
                            this.Validate(modCondition);
                        }
                    }
                }
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}


