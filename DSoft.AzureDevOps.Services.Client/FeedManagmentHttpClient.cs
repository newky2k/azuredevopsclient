using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DSoft.AzureDevOps.Services.Client.Exceptions;
using DSoft.AzureDevOps.Services.Client.Models;

namespace DSoft.AzureDevOps.Services.Client
{
    public sealed class FeedManagmentHttpClient : DevOpsHttpClientBase
    {
        internal override string TestUrl => $"{ApiUrl}packaging/feeds?api-version=5.0-preview.1";

        internal FeedManagmentHttpClient(DevOpsConnectionBase connection) : base(connection, ApiType.Feeds)
        {

        }

        public async Task<List<Feed>> GetFeedsAsync()
        {
            using (var client = Client)
            {
                var response = await client.GetAsync("packaging/feeds?api-version=5.0-preview.1");

                //check to see if we have a successful response
                if (response.IsSuccessStatusCode)
                {
                    //set the viewmodel from the content in the response
                    var result = await response.Content.ReadFromJsonAsync<FeedResponse>();

                    if (result != null)
                        return result.Feeds;
                }
            }

            throw new RequestFailureException("Unable to find feeds");
        }

        public Task<List<Package>> GetPackagesAsync(string feedId) => GetPackagesAsync(Guid.Parse(feedId));

        public async Task<List<Package>> GetPackagesAsync(Guid feedId)
        {
            using (var client = Client)
            {
                var response = await client.GetAsync($"packaging/feeds/{feedId}/packages?includeAllVersions=true&api-version=5.0-preview.1");

                //check to see if we have a successful response
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    //set the viewmodel from the content in the response
                    var result = await response.Content.ReadFromJsonAsync<PackagesResponse>();

                    if (result != null)
                        return result.Packages;
                }
            }

            throw new RequestFailureException("Unable to fetch packages");
        }

    }
}
