using System;
using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class ThingPolicy
        : BaseDatabaseModel
    {
        [JsonPropertyName("thingId")]
        public Guid ThingId { get; set; }

        [JsonPropertyName("thing")]
        public Thing Thing { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("expression")]
        public string Expression { get; set; }
    }
}
