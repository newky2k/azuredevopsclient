using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{ 

    public class Project
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }
    }

}