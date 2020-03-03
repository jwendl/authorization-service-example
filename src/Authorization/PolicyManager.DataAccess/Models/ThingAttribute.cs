using System;
using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class ThingAttribute
        : BaseDatabaseModel
    {
        [JsonPropertyName("thingId")]
        public Guid ThingId { get; set; }

        [JsonPropertyName("thing")]
        public Thing Thing { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
