using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PolicyManager.DataAccess.Models
{
    public class BaseDatabaseModel
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
