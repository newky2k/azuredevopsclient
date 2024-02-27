using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Class PackagesResponse.
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
