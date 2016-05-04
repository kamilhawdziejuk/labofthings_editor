using HomeOS.Hub.Common;
using PetrinetTool;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace EnvironmentMonitor
{
    public class RulesManager
    {
        /// <summary>
        /// Rules in the format of component, state_A, component2, state_B
        /// </summary>
        //private List<string> _rules = new List<string>();

        private List<string> _modules = new List<string>();
        private List<string> _states = new List<string>();

        private List<HomeModule> _homeModules = new List<HomeModule>();

        Petrinet _petriNet = new Petrinet();
        Page _page = new Page();


        public RulesManager()
        {
            _petriNet.Pages.Add("current", _page);
        }

        public void AddRule(string mod1, string state1, string mod2, string state2)
        {
            HomeModule moduleFrom = new HomeModule() { Name = mod1, StateDesc = state1 };
            HomeModule moduleTo = new HomeModule() { Name = mod2, StateDesc = state2 };

            if (!_homeModules.Contains(moduleFrom))
            {
                _homeModules.Add(moduleFrom);
            }

            if (!_homeModules.Contains(moduleTo))
            {
                _homeModules.Add(moduleTo);
            }
            
            HomeRule homeRule = new HomeRule() { FromModule = moduleFrom, ToModule = moduleTo };
            string name = string.Format("Rules/{0}.xml", homeRule.Name);
            XmlWriter writer = XmlWriter.Create(name);

            writer.WriteStartDocument();

            writer.WriteStartElement("scxml");
            XNamespace ns = "http://www.w3.org/2005/07/scxml";

            //writer.WriteAttributeString("xmlns", ns.ToString());
            writer.WriteAttributeString("version", "1.0");
            writer.WriteAttributeString("initial", "ready");

            homeRule.Export(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            AddToPetriNet(homeRule);

            //PN.Save("D://testNet.xml");
        }

        private void AddToPetriNet(HomeRule homeRule)
        {
            _page.Places.Add(homeRule.FromModule.Place);
            _page.Places.Add(homeRule.ToModule.Place);

            _page.Transitions.Add(homeRule.Transition);
            _page.Arcs.Add(homeRule.Arc1);
            _page.Arcs.Add(homeRule.Arc2);
        }

        //private void TestOfUsingStateMachineLibrary()
        //{
        //    /*
        //    Stopped->Runnig (play)
        //    Loaded->Stopped (stop)
        //    Running->Paused (pause)
        //    Paused->Running (play)
        //    */

        //    StateMachineTemplate t = new StateMachineTemplate();
        //    t.Region("Stopped", false);
        //    t.State("Stopped");
        //    t.Transition("T1", "Running", "Play");
        //    t.EndState();
        //    t.State("Loaded", StartMotor, StopMotor);
        //    t.Transition("T2", "Stopped", "Stop");
        //    t.Region("Running", false);
        //    t.State("Running", EngageHead, ReleaseHead);
        //    t.Transition("T3", "Paused", "Pause");
        //    t.EndState();
        //    t.State("Paused");
        //    t.Transition("T4", "Running", "Play");
        //    t.EndState();
        //    t.EndRegion();
        //    t.EndState();
        //    t.EndRegion();

        //    StateMachine stateMachine = t.CreateStateMachine();

        //    stateMachine.Startup();
        //    stateMachine.SendTriggerEvent("Play");
        //    stateMachine.SendTriggerEvent("Pause");
        //    stateMachine.SendTriggerEvent("Stop");
        //    stateMachine.Finish();
        //}

        //private void StartMotor(StateMachine stateMachine, object triggerEvent, EventArgs eventArgs)
        //{
        //    System.Console.WriteLine("StartMotor");
        //}

        //private void StopMotor(StateMachine stateMachine, object triggerEvent, EventArgs eventArgs)
        //{
        //    System.Console.WriteLine("StopMotor");
        //}

        //private void EngageHead(StateMachine stateMachine, object triggerEvent, EventArgs eventArgs)
        //{
        //    System.Console.WriteLine("EngageHead");
        //}

        //private void ReleaseHead(StateMachine stateMachine, object triggerEvent, EventArgs eventArgs)
        //{
        //    System.Console.WriteLine("ReleaseHead");
        //}

        //private void SaveHomeEnvAsPN()
        //{
        //    #region --- places ---

        //    //clock
        //    var pClockNight = new Place() { Name = "night", Id = "pClockNight" };
        //    var pClockDay = new Place() { Name = "day", Id = "pClockDay" };

        //    //speaker
        //    var pSpeakerMin = new Place() { Name = "min_loudness", Id = "pSpeakerMin" };
        //    var pSpeakerHalf = new Place() { Name = "half_loudness", Id = "pSpeakerHalf" };
        //    var pSpeakerMax = new Place() { Name = "Mmx_loudness", Id = "pSpeakerMax" };

        //    //light_sensor
        //    var pLightDark = new Place() { Name = "dark", Id = "pLightDark" };
        //    var pLightBright = new Place() { Name = "bright", Id = "pLightBright" };

        //    #endregion

        //    #region --- transitions ---

        //    //day->night
        //    var t6am = new Transition() { Name = "6am", Id = "t6am" };
        //    var a1 = new Arc() { SourceID = "pClockNight", TargetID = "t6am", Weight = 1, Id = "a1" };
        //    var a2 = new Arc() { SourceID = "t6am", TargetID = "pClockDay", Weight = 1, Id = "a2" };
        //    //night->day
        //    var t10pm = new Transition() { Name = "10pm", Id = "t10pm" };
        //    var a3 = new Arc() { SourceID = "pClockDay", TargetID = "t10pm", Weight = 1, Id = "a3" };
        //    var a4 = new Arc() { SourceID = "t10pm", TargetID = "pClockNight", Weight = 1, Id = "a4" };

        //    //++light
        //    var tMoreLight = new Transition() { Name = "moreLight", Id = "tMoreLight" };
        //    var a5 = new Arc() { SourceID = "pLightDark", TargetID = "tMoreLight", Weight = 1, Id = "a5" };
        //    var a6 = new Arc() { SourceID = "tMoreLight", TargetID = "pLightBright", Weight = 1, Id = "a6" };
        //    //--light
        //    var tLessLight = new Transition() { Name = "lessLight", Id = "tLessLight" };
        //    var a7 = new Arc() { SourceID = "pLightBright", TargetID = "tLessLight", Weight = 1, Id = "a7" };
        //    var a8 = new Arc() { SourceID = "tLessLight", TargetID = "pLightDark", Weight = 1, Id = "a8" };


        //    #endregion

        //    #region --- adding everything ---

        //    Page page = new Page();
        //    page.Places.Add(pClockNight);
        //    page.Places.Add(pClockDay);
        //    page.Places.Add(pSpeakerMin);
        //    page.Places.Add(pSpeakerHalf);
        //    page.Places.Add(pSpeakerMax);
        //    page.Places.Add(pLightBright);
        //    page.Places.Add(pLightDark);

        //    page.Transitions.Add(t6am);
        //    page.Transitions.Add(t10pm);
        //    page.Transitions.Add(tMoreLight);
        //    page.Transitions.Add(tLessLight);

        //    page.Arcs.Add(a1);
        //    page.Arcs.Add(a2);
        //    page.Arcs.Add(a3);
        //    page.Arcs.Add(a4);
        //    page.Arcs.Add(a5);
        //    page.Arcs.Add(a6);
        //    page.Arcs.Add(a7);
        //    page.Arcs.Add(a8);

        //    #endregion

        //    Petrinet PN = new Petrinet();
        //    PN.Pages.Add("current", page);

        //    //PN.Save("D://testNet.xml");
        //}
    }
}
