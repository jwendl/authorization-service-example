using System.Text.Json.Serialization;

namespace PolicyManager.Models
{
    public class CheckAccessRequest
    {
        [JsonPropertyName("requestIdentifier")]
        public string RequestIdentifier { get; set; }
    }
}
