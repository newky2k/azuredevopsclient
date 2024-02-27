using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{
    public class ServiceDetails
    {
        [JsonPropertyName("@context")]
        public Context Context { get; set; }

        [JsonPropertyName("resources")]
        public List<Resource> Resources { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

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

    public class Context
    {
        [JsonPropertyName("@vocab")]
        public string Vocab { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }
    }

    public class Resource
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; }

        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
