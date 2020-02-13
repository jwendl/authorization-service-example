using System;
using Newtonsoft.Json;

namespace ApiExampleProject.OrderSystem.DataAccess.Models
{
    public abstract class BaseCosmosDocument
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public virtual string PartitionKey { get; set; }
    }
}
