﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOS.Hub.Common;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Drivers.Gadgeteer.MicrosoftResearch.TempHumiditySensor
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "DeviceId")]
        public string DeviceId { get; set; }
        [DataMember(Name = "temperature")]
        public double temperature { get; set; }
    }


    [System.AddIn.AddIn("HomeOS.Hub.Drivers.Gadgeteer.MicrosoftResearch.TempHumiditySensor")]
    public class DriverGadgeteerMicrosoftResearchLightSensor : DriverGadgeteerBase
    {
        const int TempThreshold = 1;
        double lastValue = 0;
        VLogger driverLogger;

        protected override List<VRole> GetRoleList()
        {
            return new List<VRole>() { RoleSensor.Instance };
        }

        public override void Start()
        {
            driverLogger = new Logger(moduleInfo.WorkingDir() +"\\" + "module.log");   
            driverLogger.Log("Temperature sensor started");

            base.Start();
        }

        protected override void WorkerThread()
        {
            while (true)
            {
                try
                {
                    string url = string.Format("http://{0}/temperature", deviceIp);

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    Response jsonResponse = objResponse as Response;

                    response.Close();

                    this.Log(jsonResponse.temperature);
                    double newValue = NormalizeTempValue(jsonResponse.temperature);

                    //notify the subscribers
                    if (newValue != lastValue)
                    {
                        IList<VParamType> retVals = new List<VParamType>();
                        retVals.Add(new ParamType(newValue));

                        devicePort.Notify(RoleSensor.RoleName, RoleSensor.OpGetName, retVals);
                    }

                    lastValue = newValue;

                }
                catch (Exception e)
                {
                    //Watchdog.Enabled = true;
                    //Watchdog.Timeout = new TimeSpan(0, 0, 30);
                    //Watchdog.Behavior = WatchdogBehavior.HardReboot;

                    //icrosoft.SPOT.Hardware.PowerState .RebootDevice(false);
                    logger.Log("{0}: couldn't talk to the device. are the arguments correct?\n exception details: {1}", this.ToString(), e.ToString());

                    //lets try getting the IP again
                    deviceIp = GetDeviceIp(deviceId);
                }


                System.Threading.Thread.Sleep(4 * 1000);
            }
        }

        private void Log(double temperature)
        {
            if (temperature > 0)
            {
                logger.Log("Gadgeteer Temperature: {0}", temperature.ToString());
                DateTime date = DateTime.Now;
                driverLogger.Log("Temperature: {0}", temperature.ToString());
            }
        }

        private double NormalizeTempValue(double rawValue)
        {
            return Math.Round(rawValue,2);
        }


        /// <summary>
        /// The demultiplexing routing for incoming
        /// </summary>
        /// <param name="message"></param>
        protected override List<VParamType> OnOperationInvoke(string roleName, String opName, IList<VParamType> parameters)
        {
            switch (roleName.ToLower())
            {
                case RoleSensor.RoleName:
                    {
                        switch (opName.ToLower())
                        {
                            case RoleSensor.OpGetName:
                                {
                                    List<VParamType> retVals = new List<VParamType>();
                                    retVals.Add(new ParamType(lastValue));

                                    return retVals;
                                }
                            default:
                                logger.Log("Unknown operation {0} for role {1}", opName, roleName);
                                return null;
                        }
                    }
                case RoleActuator.RoleName:
                    {
                        switch (opName.ToLower()) 
                        { 
                            case RoleActuator.OpPutName:
                                { 
                                    try 
                                    { 
                                        //TODO:
                                        /*
                                        string url = string.Format("http://{0}/led?low={1}&high={2}", 
                                            deviceIp, (int)parameters[0].Value(), (int)parameters[1].Value());

                                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url); 
                                        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();*/

                                    } 
                                    catch (Exception e) 
                                    { 
                                        logger.Log("{0}: couldn't talk to the device. are the arguments correct?\n exception details: {1}", this.ToString(), e.ToString());
                                        //lets try getting the IP again 
                                        deviceIp = GetDeviceIp(deviceId); 
                                    } 
                                    return new List<VParamType>(); 
                                } 
                            default: 
                                logger.Log("Unknown operation {0} for {1}", opName, roleName); 
                                return null; 
                        }
                    }
                    default: 
                        logger.Log("Unknown role {0}", roleName); 
                        return null;
            }
        }

        public override void Stop()
        {
            if (worker != null)
                worker.Abort();
        }

        //we have nothing to do with other ports
        public override void PortRegistered(VPort port) { }
        public override void PortDeregistered(VPort port) { }

        public override string GetDescription(string hint)
        {
            logger.Log("DriverGadgeteer.GetDescription for {0}", hint);
            return "Gadgeteer";
        }

    }
}
