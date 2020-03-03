using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class Thing
        : BaseDatabaseModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonIgnore]
        public ICollection<ThingAttribute> ThingAttributes { get; } = new List<ThingAttribute>();

        [JsonIgnore]
        public ICollection<ThingPolicy> ThingPolicies { get; } = new List<ThingPolicy>();
    }
}
