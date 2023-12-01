using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AmpModbusMasterUi.Configuration
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class MainConfiguration
    {
        [JsonProperty(Required = Required.Always)]
        public string IPAddress { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Port { get; set; }

        [JsonProperty(Required = Required.Default)]
        [DefaultValue(0)]
        public byte SlaveAddress { get; set; } /*= "";*/

        [JsonProperty(Required = Required.Always)]
        public string LogEventOrigin { get; set; }

        [JsonProperty(Required = Required.Always)]
        public TimeSpan PollingInterval { get; set; }

        [JsonProperty(Required = Required.Default)]
        public TimeSpan ReconnectDelay { get; set; } = TimeSpan.FromSeconds(2);

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<IOSignalConfiguration> IOSignals { get; set; }
    }
}
