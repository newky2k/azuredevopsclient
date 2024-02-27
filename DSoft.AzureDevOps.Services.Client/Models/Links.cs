using System.Text.Json.Serialization; 

namespace DSoft.AzureDevOps.Services.Client.Models{ 

    public class Links
    {
        [JsonPropertyName("self")]
        public Self Self { get; set; }

        [JsonPropertyName("packages")]
        public Packages Packages { get; set; }

        [JsonPropertyName("permissions")]
        public Permissions Permissions { get; set; }
    }

}