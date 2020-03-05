using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PolicyManager.DataAccess.Models
{
    public class ThingAttribute
        : BaseDatabaseModel
    {
        [JsonProperty("thingId")]
        [JsonPropertyName("thingId")]
        public Guid ThingId { get; set; }

        [JsonProperty("thing")]
        [JsonPropertyName("thing")]
        public Thing Thing { get; set; }

        [JsonProperty("key")]
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
