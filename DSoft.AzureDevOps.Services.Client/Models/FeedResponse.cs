using System.Text.Json.Serialization; 
using System.Collections.Generic; 

namespace DSoft.AzureDevOps.Services.Client.Models
{ 

    public class FeedResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<Feed> Feeds { get; set; }
    }

}