using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Packages Response model
    /// </summary>
    public class PackagesResponse
	{
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [JsonPropertyName("count")]
		public int Count { get; set; }

        /// <summary>
        /// Gets or sets the packages.
        /// </summary>
        /// <value>The packages.</value>
        [JsonPropertyName("value")]
		public List<Package> Packages { get; set; }

	}
}
