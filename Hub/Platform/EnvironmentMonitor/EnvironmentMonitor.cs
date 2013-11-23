using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Platform.EnvironmentMonitor.Validators;
using HomeOS.Hub.Platform.Views;

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
        //HomeGraph homeGraph = new HomeGraph();

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

        private void InitializeHomeEnvironment()
        {
            //here comes adding states...
            foreach (var state in this.GetSystemStates())
            {
                //this.homeGraph.AddState(state);
            }

            //here comes adding possible transitions...
        }

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
            System.Threading.Thread.Sleep(5000);
            return;
            logger.Log("Conflicts Manager has started...");
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


