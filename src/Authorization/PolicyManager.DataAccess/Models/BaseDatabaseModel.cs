using System;
using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class BaseDatabaseModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
