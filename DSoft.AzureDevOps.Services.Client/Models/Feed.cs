using System.Text.Json.Serialization; 
using System.Collections.Generic; 

namespace DSoft.AzureDevOps.Services.Client.Models
{ 

    public class Feed
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("_links")]
        public Links Links { get; set; }

        [JsonPropertyName("hideDeletedPackageVersions")]
        public bool HideDeletedPackageVersions { get; set; }

        [JsonPropertyName("defaultViewId")]
        public string DefaultViewId { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("viewId")]
        public object ViewId { get; set; }

        [JsonPropertyName("viewName")]
        public object ViewName { get; set; }

        [JsonPropertyName("fullyQualifiedName")]
        public string FullyQualifiedName { get; set; }

        [JsonPropertyName("fullyQualifiedId")]
        public string FullyQualifiedId { get; set; }

        [JsonPropertyName("upstreamSources")]
        public List<UpstreamSource> UpstreamSources { get; set; }

        [JsonPropertyName("capabilities")]
        public string Capabilities { get; set; }

        [JsonPropertyName("project")]
        public Project Project { get; set; }

        [JsonPropertyName("upstreamEnabled")]
        public bool? UpstreamEnabled { get; set; }
    }

}