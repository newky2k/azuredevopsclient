using System.Text.Json.Serialization; 

namespace DSoft.AzureDevOps.Services.Client.Models{

    /// <summary>
    /// Class Links.
    /// </summary>
    public class Links
    {
        /// <summary>
        /// Gets or sets the self.
        /// </summary>
        /// <value>The self.</value>
        [JsonPropertyName("self")]
        public Self Self { get; set; }

        /// <summary>
        /// Gets or sets the packages.
        /// </summary>
        /// <value>The packages.</value>
        [JsonPropertyName("packages")]
        public Packages Packages { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        [JsonPropertyName("permissions")]
        public Permissions Permissions { get; set; }
    }

}