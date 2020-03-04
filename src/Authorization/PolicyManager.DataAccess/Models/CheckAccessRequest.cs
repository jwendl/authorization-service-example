using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class CheckAccessRequest
    {
        [JsonPropertyName("requestIdentifier")]
        public string RequestIdentifier { get; set; }
    }
}
