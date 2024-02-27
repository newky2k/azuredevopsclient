using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{

    /// <summary>
    /// Class Project.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        /// <value>The visibility.</value>
        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }
    }

}