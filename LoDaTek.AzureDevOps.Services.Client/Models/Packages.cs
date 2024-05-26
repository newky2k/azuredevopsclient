using System.Text.Json.Serialization;

namespace LoDaTek.AzureDevOps.Services.Client.Models 
{

    /// <summary>
    /// Packages model
    /// </summary>
    public class Packages
    {
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

}