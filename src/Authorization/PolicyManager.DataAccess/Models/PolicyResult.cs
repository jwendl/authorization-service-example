using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class PolicyResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("result")]
        public PolicyEvaluation Result { get; set; }

        [JsonPropertyName("resultString")]
        public string ResultString
        {
            get
            {
                return Result.ToString();
            }
        }
    }

    public enum PolicyEvaluation
    {
        Deny,
        Allow,
    }
}
