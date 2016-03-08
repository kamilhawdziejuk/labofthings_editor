using System;
using System.Runtime.Serialization;

namespace HomeOS.Hub.Apps.Clock
{
    [DataContract]
    public class MetricEvent
    {
        [DataMember]
        public string HomeHubId { get; set; }
 
        [DataMember]
        public string SensorName { get; set; }

        [DataMember]
        public string SensorRole { get; set; }

        [DataMember]
        public string SensorData { get; set; }
        
        [DataMember] 
        public DateTime EntryDateTime { get; set; }
    }
}
