using System.Text.Json.Serialization; 
using System.Collections.Generic; 

namespace DSoft.AzureDevOps.Services.Client.Models
{

    /// <summary>
    /// Class Feed.
    /// </summary>
    public class Feed
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        [JsonPropertyName("_links")]
        public Links Links { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [hide deleted package versions].
        /// </summary>
        /// <value><c>true</c> if [hide deleted package versions]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("hideDeletedPackageVersions")]
        public bool HideDeletedPackageVersions { get; set; }

        /// <summary>
        /// Gets or sets the default view identifier.
        /// </summary>
        /// <value>The default view identifier.</value>
        [JsonPropertyName("defaultViewId")]
        public string DefaultViewId { get; set; }

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
        /// Gets or sets the view identifier.
        /// </summary>
        /// <value>The view identifier.</value>
        [JsonPropertyName("viewId")]
        public object ViewId { get; set; }

        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        [JsonPropertyName("viewName")]
        public object ViewName { get; set; }

        /// <summary>
        /// Gets or sets the name of the fully qualified.
        /// </summary>
        /// <value>The name of the fully qualified.</value>
        [JsonPropertyName("fullyQualifiedName")]
        public string FullyQualifiedName { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified identifier.
        /// </summary>
        /// <value>The fully qualified identifier.</value>
        [JsonPropertyName("fullyQualifiedId")]
        public string FullyQualifiedId { get; set; }

        /// <summary>
        /// Gets or sets the upstream sources.
        /// </summary>
        /// <value>The upstream sources.</value>
        [JsonPropertyName("upstreamSources")]
        public List<UpstreamSource> UpstreamSources { get; set; }

        /// <summary>
        /// Gets or sets the capabilities.
        /// </summary>
        /// <value>The capabilities.</value>
        [JsonPropertyName("capabilities")]
        public string Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [JsonPropertyName("project")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [upstream enabled].
        /// </summary>
        /// <value><c>null</c> if [upstream enabled] contains no value, <c>true</c> if [upstream enabled]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("upstreamEnabled")]
        public bool? UpstreamEnabled { get; set; }
    }

}