using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models 
{ 

    public class Permissions
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

}