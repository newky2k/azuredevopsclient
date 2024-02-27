using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DSoft.AzureDevOps.Services.Client.Models
{
	public class PackagesResponse
	{
		[JsonPropertyName("count")]
		public int Count { get; set; }

		[JsonPropertyName("value")]
		public List<Package> Packages { get; set; }

	}
}
