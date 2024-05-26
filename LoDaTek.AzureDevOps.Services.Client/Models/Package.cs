using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Package model
    /// </summary>
    public class Package
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
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [JsonPropertyName("url")]
		public string Url { get; set; }

        /// <summary>
        /// Gets or sets the type of the protocol.
        /// </summary>
        /// <value>The type of the protocol.</value>
        [JsonPropertyName("protocolType")]
		public string ProtocolType { get; set; }

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        /// <value>The versions.</value>
        [JsonPropertyName("versions")]
		public List<PackageVersion> Versions { get; set; }
	}

    /// <summary>
    /// Class PackageVersion.
    /// </summary>
    public class PackageVersion
	{
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonPropertyName("id")]
		public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [JsonPropertyName("version")]
		public string Version { get; set; }
	}
}
