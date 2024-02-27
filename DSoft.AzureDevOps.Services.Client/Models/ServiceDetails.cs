using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Class ServiceDetails.
    /// </summary>
    public class ServiceDetails
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        [JsonPropertyName("@context")]
        public Context Context { get; set; }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        [JsonPropertyName("resources")]
        public List<Resource> Resources { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets the <see cref="Resource"/> with the specified resource type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns>Resource.</returns>
        public Resource this[string resourceType]
        {
            get
            {
                if (Resources == null || Resources.Count == 0)
                    return null;

                return Resources.FirstOrDefault(x => x.Type.Equals(resourceType, StringComparison.OrdinalIgnoreCase));
            }
        }
    }

    /// <summary>
    /// Class Context.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Gets or sets the vocab.
        /// </summary>
        /// <value>The vocab.</value>
        [JsonPropertyName("@vocab")]
        public string Vocab { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }

    /// <summary>
    /// Class Resource.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonPropertyName("@id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
