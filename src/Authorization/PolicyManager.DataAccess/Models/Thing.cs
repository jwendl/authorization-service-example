using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TextJsonIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;
using NewtonsoftJsonIgnore = Newtonsoft.Json.JsonIgnoreAttribute;

namespace PolicyManager.DataAccess.Models
{
    public class Thing
        : BaseDatabaseModel
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("identifier")]
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [TextJsonIgnore]
        [NewtonsoftJsonIgnore]
        [JsonProperty("thingAttributes")]
        [JsonPropertyName("thingAttributes")]
        public List<ThingAttribute> ThingAttributes { get; } = new List<ThingAttribute>();

        [TextJsonIgnore]
        [NewtonsoftJsonIgnore]
        [JsonProperty("thingPolicies")]
        [JsonPropertyName("thingPolicies")]
        public List<ThingPolicy> ThingPolicies { get; } = new List<ThingPolicy>();
    }
}
