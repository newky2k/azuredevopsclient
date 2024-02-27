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
    /// <summary>
    /// HttpClient for feed management
    /// Implements the <see cref="DSoft.AzureDevOps.Services.Client.DevOpsHttpClientBase" />
    /// </summary>
    /// <seealso cref="DSoft.AzureDevOps.Services.Client.DevOpsHttpClientBase" />
    public sealed class FeedManagmentHttpClient : DevOpsHttpClientBase
    {
        #region Properties

        /// <summary>
        /// Gets the test URL.
        /// </summary>
        /// <value>The test URL.</value>
        internal override string TestUrl => $"{ApiUrl}packaging/feeds?api-version=5.0-preview.1";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedManagmentHttpClient"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        internal FeedManagmentHttpClient(DevOpsConnectionBase connection) : base(connection, ApiType.Feeds)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get feeds as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="DSoft.AzureDevOps.Services.Client.Exceptions.RequestFailureException">Unable to find feeds</exception>
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

        /// <summary>
        /// Gets the packages asynchronous.
        /// </summary>
        /// <param name="feedId">The feed identifier.</param>
        /// <returns>Task&lt;List&lt;Package&gt;&gt;.</returns>
        public Task<List<Package>> GetPackagesAsync(string feedId) => GetPackagesAsync(Guid.Parse(feedId));

        /// <summary>
        /// Get packages as an asynchronous operation.
        /// </summary>
        /// <param name="feedId">The feed identifier.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="DSoft.AzureDevOps.Services.Client.Exceptions.RequestFailureException">Unable to fetch packages</exception>
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

        #endregion

    }
}
