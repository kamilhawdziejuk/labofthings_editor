using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using HomeOS.Hub.Tools.EnvironmentMonitor.Validators;
using HomeOS.Hub.Platform.Views;
//using PetriNetSharp.Engine;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;
using PetrinetTool;

namespace HomeOS.Hub.Tools.EnvironmentMonitor
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

        private void SaveHomeEnvAsPN()
        {
            #region --- places ---

            //clock
            var pClockNight = new Place() { Name = "night", Id = "pClockNight" };
            var pClockDay = new Place() { Name = "day", Id = "pClockDay" };

            //speaker
            var pSpeakerMin = new Place() { Name = "min_loudness", Id = "pSpeakerMin" };
            var pSpeakerHalf = new Place() { Name = "half_loudness", Id = "pSpeakerHalf" };
            var pSpeakerMax = new Place() { Name = "Mmx_loudness", Id = "pSpeakerMax" };

            //light_sensor
            var pLightDark = new Place() { Name = "dark", Id = "pLightDark" };
            var pLightBright = new Place() { Name = "bright", Id = "pLightBright" };

            #endregion

            #region --- transitions ---

            //day->night
            var t6am = new Transition() { Name = "6am", Id = "t6am" };
            var a1 = new Arc() { SourceID = "pClockNight", TargetID = "t6am", Weight = 1, Id = "a1" };
            var a2 = new Arc() { SourceID = "t6am", TargetID = "pClockDay", Weight = 1, Id = "a2" };
            //night->day
            var t10pm = new Transition() { Name = "10pm", Id = "t10pm" };
            var a3 = new Arc() { SourceID = "pClockDay", TargetID = "t10pm", Weight = 1, Id = "a3" };
            var a4 = new Arc() { SourceID = "t10pm", TargetID = "pClockNight", Weight = 1, Id = "a4" };
            
            //++light
            var tMoreLight = new Transition() { Name = "moreLight", Id = "tMoreLight" };
            var a5 = new Arc() { SourceID = "pLightDark", TargetID = "tMoreLight", Weight = 1, Id = "a5" };
            var a6 = new Arc() { SourceID = "tMoreLight", TargetID = "pLightBright", Weight = 1, Id = "a6" };
            //--light
            var tLessLight = new Transition() { Name = "lessLight", Id = "tLessLight" };
            var a7 = new Arc() { SourceID = "pLightBright", TargetID = "tLessLight", Weight = 1, Id = "a7" };
            var a8 = new Arc() { SourceID = "tLessLight", TargetID = "pLightDark", Weight = 1, Id = "a8" };
            

            #endregion

            #region --- adding everything ---

            Page page = new Page();
            page.Places.Add(pClockNight);
            page.Places.Add(pClockDay);
            page.Places.Add(pSpeakerMin);
            page.Places.Add(pSpeakerHalf);
            page.Places.Add(pSpeakerMax);
            page.Places.Add(pLightBright);
            page.Places.Add(pLightDark);

            page.Transitions.Add(t6am);
            page.Transitions.Add(t10pm);
            page.Transitions.Add(tMoreLight);
            page.Transitions.Add(tLessLight);

            page.Arcs.Add(a1);
            page.Arcs.Add(a2);
            page.Arcs.Add(a3);
            page.Arcs.Add(a4);
            page.Arcs.Add(a5);
            page.Arcs.Add(a6);
            page.Arcs.Add(a7);
            page.Arcs.Add(a8);

            #endregion

            Petrinet PN = new Petrinet();
            PN.Pages.Add("current", page);

            //PN.Save("D://testNet.xml");
        }

        /*
        private void SaveHomeEnvironmentStateAsPetriNet()
        {
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
            this.SaveHomeEnvAsPN();
            //return;
            //System.Threading.Thread.Sleep(5000);
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
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}


