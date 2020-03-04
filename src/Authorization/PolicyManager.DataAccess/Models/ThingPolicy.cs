using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PolicyManager.DataAccess.Models
{
    public class ThingPolicy
        : BaseDatabaseModel
    {
        [JsonProperty("thingId")]
        [JsonPropertyName("thingId")]
        public Guid ThingId { get; set; }

        [JsonProperty("thing")]
        [JsonPropertyName("thing")]
        public Thing Thing { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("expression")]
        [JsonPropertyName("expression")]
        public string Expression { get; set; }
    }
}
