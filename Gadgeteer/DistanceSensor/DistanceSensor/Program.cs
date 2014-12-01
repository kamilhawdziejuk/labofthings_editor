using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace DistanceSensor
{
    public partial class Program
    {
        HomeOSGadgeteer.HomeOSGadgeteerDevice hgd;
        TimeSpan RemoteControlLedEndTime = TimeSpan.Zero;
        const int CheckPeriod = 5000;

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            hgd = new HomeOSGadgeteer.HomeOSGadgeteerDevice("MicrosoftResearch", "DistanceSensor", "abcdefgh", wifi,
                null, null, /*usbSerial.SerialLine.PortName*/null, null, null, () => { return GT.Timer.GetMachineTime() < RemoteControlLedEndTime; }, true);

            GT.Timer timer = new GT.Timer(CheckPeriod);
            timer.Tick += Timer_Tick;
            timer.Start();

            hgd.SetupWebEvent("distance").WebEventReceived += WebEventReceived;
            Debug.Print("Program Started");
        }

        void Timer_Tick(GT.Timer timer)
        {
            Debug.Print(this.response);
        }

        void WebEventReceived(string path,
            HomeOSGadgeteer.Networking.WebServer.HttpMethod method,
            HomeOSGadgeteer.Networking.Responder responder)
        {
            Debug.Print("Distance web event from " + responder.ClientEndpoint + " - response " + this.response);
            responder.Respond(this.webResponse);
        }

        private string webResponse
        {
            get
            {
                return "{\"DeviceId\":\"" +
                    hgd.IdentifierString + "\","
                    + "\"distance\":" + this.distance_US3.GetDistanceInCentimeters().ToString() 
                    + "\"gas\":" + this.gasSense.ReadVoltage().ToString() +
                    "}";
            }
        }

        private string response
        {
            get
            {
                return "{" +
                "\"distance\" : " + this.distance_US3.GetDistanceInCentimeters().ToString() + "\n" +
                "\"gas\" : " + this.gasSense.ReadVoltage().ToString() + "\n" +
                "\"DeviceIP\" : \"" + this.wifi.NetworkSettings.IPAddress + "\", " +
                 "\"DeviceId\" : \"" + hgd.IdentifierString + "\", " +
                "}";
            }
        }
    }
}
