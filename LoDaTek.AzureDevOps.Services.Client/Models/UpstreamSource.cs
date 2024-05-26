using System.Text.Json.Serialization;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{

    /// <summary>
    /// Upstream Source Model
    /// </summary>
    public class UpstreamSource
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
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        [JsonPropertyName("location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the display location.
        /// </summary>
        /// <value>The display location.</value>
        [JsonPropertyName("displayLocation")]
        public string DisplayLocation { get; set; }

        /// <summary>
        /// Gets or sets the type of the upstream source.
        /// </summary>
        /// <value>The type of the upstream source.</value>
        [JsonPropertyName("upstreamSourceType")]
        public string UpstreamSourceType { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

}