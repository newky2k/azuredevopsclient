using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{
	public class Package
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }

		[JsonPropertyName("protocolType")]
		public string ProtocolType { get; set; }

		[JsonPropertyName("versions")]
		public List<PackageVersion> Versions { get; set; }
	}

	public class PackageVersion
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }
	}
}
