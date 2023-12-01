using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AmpModbusMasterUi.Libs
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public struct IOSignalAddress
    {
        [JsonProperty(Required = Required.Always)]
        public string Address { get; set; }

        public override string ToString()
        {
            return $"IO signal - Address: {Address}";
        }
    }
}
