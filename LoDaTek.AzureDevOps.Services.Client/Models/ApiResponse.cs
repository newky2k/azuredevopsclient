using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Generic Api Response with items of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the feeds.
        /// </summary>
        /// <value>The feeds.</value>
        [JsonPropertyName("value")]
        public List<T> Items { get; set; }
    }
}
