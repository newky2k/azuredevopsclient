using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{ 

    public class UpstreamSource
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("displayLocation")]
        public string DisplayLocation { get; set; }

        [JsonPropertyName("upstreamSourceType")]
        public string UpstreamSourceType { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

}