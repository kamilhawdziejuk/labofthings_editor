//02.01.2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using HomeOS.Hub.Platform.Views;
using HomeOS.Hub.Common;

namespace HomeOS.Hub.Apps.Volume
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class VolumeSvc : ISimplexContract, ModuleCondition
    {
        public static ServiceHost CreateServiceHost(ISimplexContract instance, Uri baseAddress)
        {
            ServiceHost service = new ServiceHost(instance, baseAddress);

            BasicHttpBinding binding = new BasicHttpBinding();

            service.Description.Behaviors.Add(new ServiceMetadataBehavior());
            service.AddServiceEndpoint(typeof(ISimplexContract), binding, "");
            service.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            var behavior = service.Description.Behaviors.Find<ServiceDebugBehavior>();
            behavior.IncludeExceptionDetailInFaults = true;

            return service;
        }

        protected VLogger logger;

        public VolumeSvc(VLogger logger)
        {
            this.logger = logger;
        }

        double minValue;
        double maxValue;


        public string UpdateVolume(int _change)
        {
            string total = "";
            try
            {
                //Instantiate an Enumerator to find audio devices
                NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                //Get all the devices, no matter what condition or status
                NAudio.CoreAudioApi.MMDeviceCollection DevCol = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
                //Loop through all devices
                foreach (NAudio.CoreAudioApi.MMDevice dev in DevCol)
                {
                    try
                    {
                        var range = dev.AudioEndpointVolume.VolumeRange;
                       

                        //Show us the human understandable name of the device
                        System.Diagnostics.Debug.Print(dev.FriendlyName);
                        //Mute it
                        if (_change > 0)
                        {
                            dev.AudioEndpointVolume.Mute = false;
                            dev.AudioEndpointVolume.VolumeStepUp();
                        }
                        else if (_change < 0)
                        {
                            dev.AudioEndpointVolume.Mute = false;
                            dev.AudioEndpointVolume.VolumeStepDown();
                        }
                        else if (_change == 0)
                        {
                            dev.AudioEndpointVolume.Mute = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Do something with exception when an audio endpoint could not be muted
                    }
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
            }

            this.level = this.GetActualVolume();
            this.minValue = this.GetMinVolume();
            this.maxValue = this.GetMaxVolume();
            return total.ToString();

        }

        public float GetMaxVolume()
        {
            try
            {
                //Instantiate an Enumerator to find audio devices
                NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                //Get all the devices, no matter what condition or status
                NAudio.CoreAudioApi.MMDeviceCollection DevCol = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
                //Loop through all devices
                foreach (NAudio.CoreAudioApi.MMDevice dev in DevCol)
                {
                    return dev.AudioEndpointVolume.VolumeRange.MaxDecibels;
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
            }

            return -100;
        }

        public float GetMinVolume()
        {
            try
            {
                //Instantiate an Enumerator to find audio devices
                NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                //Get all the devices, no matter what condition or status
                NAudio.CoreAudioApi.MMDeviceCollection DevCol = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
                //Loop through all devices
                foreach (NAudio.CoreAudioApi.MMDevice dev in DevCol)
                {
                    return dev.AudioEndpointVolume.VolumeRange.MinDecibels;
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
            }

            return -100;
        }

        public float GetActualVolume()
        {
            try
            {
                //Instantiate an Enumerator to find audio devices
                NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                //Get all the devices, no matter what condition or status
                NAudio.CoreAudioApi.MMDeviceCollection DevCol = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
                //Loop through all devices
                foreach (NAudio.CoreAudioApi.MMDevice dev in DevCol)
                {
                    return dev.AudioEndpointVolume.MasterVolumeLevel;
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
            }

            return 0;
        }

        private double level = 0;

        /// <summary>
        /// Represents loudness
        /// </summary>
        public double ExactValue
        {
            get
            {
                return this.GetActualVolume();
                return this.level;
            }
            set
            {
                this.UpdateVolume((int)value);
            }
        }

        /// <summary>
        /// Represents actual interpreted value of the state (one of the possible interpreted state in Home System Net)
        /// </summary>
        public double InterpretedValue
        {
            get
            {
                this.minValue = this.GetMinVolume();
                this.maxValue = this.GetMaxVolume();
                if (this.ExactValue <= minValue)
                {
                    return minValue;
                }
                else if (this.ExactValue >= maxValue)
                {
                    return maxValue;
                }
                else
                {
                    return 0.5 * (maxValue + minValue);
                }
            }
        }

        /// <summary>
        /// All possible interpreted stated in Home System Net
        /// </summary>
        public Dictionary<double, string> PossibleIntepretedValues
        {
            get
            {

                this.minValue = this.GetMinVolume();
                this.maxValue = this.GetMaxVolume();

                double keyMin = this.minValue;
                double keyMedium = 0.5 * (this.minValue + this.maxValue);
                double keyMax = this.maxValue;

                Dictionary<double, string> results = new Dictionary<double, string>();
                results.Add(this.minValue, "minLoudness");
                if (!results.ContainsKey(keyMedium))
                {
                    results.Add(keyMedium, "mediumLoudness");
                }
                if (!results.ContainsKey(keyMax))
                {
                    results.Add(keyMax, "maxLoudness");
                }
                return results;
            }
        }

        public object Clone()
        {
            VolumeSvc appVolumeSvcCopy = new VolumeSvc(this.logger);
            appVolumeSvcCopy.level = this.level;
            return appVolumeSvcCopy;
        }
    }

    [ServiceContract]
    public interface ISimplexContract
    {
        [OperationContract]
        string UpdateVolume(int _change);

        [OperationContract]
        float GetMaxVolume();

        [OperationContract]
        float GetMinVolume();

        [OperationContract]
        float GetActualVolume();
    }
}
