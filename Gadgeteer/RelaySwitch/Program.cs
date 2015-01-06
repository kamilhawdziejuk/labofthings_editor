﻿using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
//using Gadgeteer.Modules.Seeed;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO.Ports;

namespace LightSensor
{
    public partial class Program
    {
        HomeOSGadgeteer.HomeOSGadgeteerDevice hgd;

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            hgd = new HomeOSGadgeteer.HomeOSGadgeteerDevice("MicrosoftResearch", "RelaySwitch", "abcdefgh", wifi,
                null, null, /*usbSerial.SerialLine.PortName*/null, null, null, () => { return GT.Timer.GetMachineTime() < RemoteControlLedEndTime; }, true);

            hgd.SetupWebEvent("IsOn").WebEventReceived += this.RelayWebEventReceived;

            GT.Timer relayTimer = new GT.Timer(RelayCheckPeriod);
            relayTimer.Tick += new GT.Timer.TickEventHandler(this.relayTimer_Tick);
            relayTimer.Start();

            Debug.Print("Program Started");
        }

        TimeSpan RemoteControlLedEndTime = TimeSpan.Zero;
        #region Device behaviour

        /// <summary>
        /// The time between checking light sensor
        /// </summary>
        const int RelayCheckPeriod = 2000;
        TimeSpan LightBlinkTimeSpan = new TimeSpan(0, 0, 1);

        /// <summary>
        /// This is called when a wifi network connection to a home network (not setup network) is made (or remade) 
        /// </summary>
        private void HomeNetworkConnectionMade()
        {
            // this is just for test
            //WebClient.GetFromWeb("http://research.microsoft.com/en-us/").ResponseReceived += new HttpRequest.ResponseHandler(TestResponseReceived);
        }

        void RelayWebEventReceived(string path, 
            HomeOSGadgeteer.Networking.WebServer.HttpMethod method, 
            HomeOSGadgeteer.Networking.Responder responder)
        {
            Debug.Print("Relay web event from " + responder.ClientEndpoint + " - response " + this.response);
            responder.Respond(this.webResponse);
        }

        void relayTimer_Tick(GT.Timer timer)
        {
            Debug.Print(this.response);
        }

        private string webResponse
        {
            get
            {
                return "{\"DeviceId\":\"" + 
                    hgd.IdentifierString + "\"," 
                    + "\"IsOn\":" + this.relay_X1.Enabled.ToString() + 
                    "}";
            }
        }

        private string response
        {
            get
            {
                return "{" +
                "\"IsOn\" : " + this.relay_X1.Enabled.ToString() + "\n" +
                 "\"DeviceIP\" : \"" + this.wifi.NetworkSettings.IPAddress + "\", " +
                 "\"DeviceId\" : \"" + hgd.IdentifierString + "\", " +
                "}";
            }
        }

        #endregion

    }
}
