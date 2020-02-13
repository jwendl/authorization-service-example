using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class Thing
        : BaseDatabaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Identifier { get; set; }

        [JsonIgnore]
        public ICollection<ThingAttribute> ThingAttributes { get; } = new List<ThingAttribute>();

        [JsonIgnore]
        public ICollection<ThingPolicy> ThingPolicies { get; } = new List<ThingPolicy>();
    }
}
