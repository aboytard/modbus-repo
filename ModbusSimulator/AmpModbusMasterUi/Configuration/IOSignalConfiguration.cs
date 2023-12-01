using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace AmpModbusMasterUi.Configuration
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class IOSignalConfiguration
    {
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }

        // THIS IS ADDITIONAL IF I WANT TO COMMUNICATE VIA RABBIT
        
        //[JsonProperty(Required = Required.Always)]
        //public ServiceInterfaceDescriptors IOSignalDescriptors { get; set; }

        [JsonProperty(Required = Required.Default)]
        public IEnumerable<IOSignalMappingConfiguration> InputSignals { get; set; }

        [JsonProperty(Required = Required.Default)]
        public IEnumerable<IOSignalMappingConfiguration> OutputSignals { get; set; }
    }
}
