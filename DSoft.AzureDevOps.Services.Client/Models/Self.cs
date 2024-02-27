using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{ 

    public class Self
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

}