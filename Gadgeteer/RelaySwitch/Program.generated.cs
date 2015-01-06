//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LightSensor {
    using Gadgeteer;
    using GTM = Gadgeteer.Modules;
    
    
    public partial class Program : Gadgeteer.Program {
        
        /// <summary>The WiFi_RS21 (Premium) module using socket 6 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.WiFi_RS21 wifi;
        
        /// <summary>The Relay X1 module using socket 4 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Relay_X1 relay_X1;
        
        /// <summary>This property provides access to the Mainboard API. This is normally not necessary for an end user program.</summary>
        protected new static GHIElectronics.Gadgeteer.FEZSpider Mainboard {
            get {
                return ((GHIElectronics.Gadgeteer.FEZSpider)(Gadgeteer.Program.Mainboard));
            }
            set {
                Gadgeteer.Program.Mainboard = value;
            }
        }
        
        /// <summary>This method runs automatically when the device is powered, and calls ProgramStarted.</summary>
        public static void Main() {
            // Important to initialize the Mainboard first
            Program.Mainboard = new GHIElectronics.Gadgeteer.FEZSpider();
            Program p = new Program();
            p.InitializeModules();
            p.ProgramStarted();
            // Starts Dispatcher
            p.Run();
        }
        
        private void InitializeModules() {
            this.wifi = new GTM.GHIElectronics.WiFi_RS21(6);
            this.relay_X1 = new GTM.GHIElectronics.Relay_X1(4);
        }
    }
}
