using System.Text.Json.Serialization; 
using System.Collections.Generic; 

namespace DSoft.AzureDevOps.Services.Client.Models
{

    /// <summary>
    /// Class FeedResponse.
    /// </summary>
    public class FeedResponse
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the feeds.
        /// </summary>
        /// <value>The feeds.</value>
        [JsonPropertyName("value")]
        public List<Feed> Feeds { get; set; }
    }

}