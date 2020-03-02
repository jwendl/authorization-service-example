using System;
using System.Text.Json.Serialization;

namespace ApiExampleProject.OrderSystem.DataAccess.Models
{
    public abstract class BaseCosmosDocument
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        public virtual string PartitionKey { get; set; }
    }
}
