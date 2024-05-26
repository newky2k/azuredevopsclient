using System.Text.Json.Serialization;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{

    /// <summary>
    /// Self model
    /// </summary>
    public class Self
    {
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

}