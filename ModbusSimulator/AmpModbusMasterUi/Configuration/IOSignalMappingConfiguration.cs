using AmpModbusMasterUi.Libs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;

namespace AmpModbusMasterUi.Configuration
{
    // I should harcode everything to work locally for now

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class IOSignalMappingConfiguration
    {
        [JsonProperty(Required = Required.Always)]
        public ushort ModbusAddress { get; set; } // how should I define a ModbusAdress

        [JsonProperty(Required = Required.Always)]
        public IOSignalAddress SignalAddress { get; set; } // DO I REALLY WANT TO HARCODE IT THERE = new IOSignalAddress() { Address = "127.0.0.1" }; // --> both are using the same address

        [JsonProperty(Required = Required.Default)]
        [Description("Only relevant for inputs.")]
        public TimeSpan? DebounceTime { get; set; }
    }
}
