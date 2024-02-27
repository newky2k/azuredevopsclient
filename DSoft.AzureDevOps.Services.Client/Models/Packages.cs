using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models 
{

    /// <summary>
    /// Class Packages.
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