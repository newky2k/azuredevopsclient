using LoDaTek.AzureDevOps.Services.Client.Bases;
using LoDaTek.AzureDevOps.Services.Client.Enums;
using LoDaTek.AzureDevOps.Services.Client.Exceptions;
using LoDaTek.AzureDevOps.Services.Client.Models;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Organization.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client
{
    /// <summary>
    /// Client for Secure Files Api
    /// Implements the <see cref="DevOpsHttpClientBase" />
    /// </summary>
    /// <seealso cref="DevOpsHttpClientBase" />
    public sealed class SecureFilesHttpClient : DevOpsHttpClientBase
    {
        internal override string TestUrl => ApiUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureFilesHttpClient"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        internal SecureFilesHttpClient(DevOpsConnectionBase connection) : base(connection, ApiType.Common)
        {

        }


        #region API

        /// <summary>
        /// Get all secure files for the specified project
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="RequestFailureException">Unable to find feeds</exception>
        public async Task<List<SecureFile>> GetAllAsync(TeamProjectReference project)
        {
            using (var client = Client)
            {
                var requestUrl = BuildApiUrl(project);

                var response = await client.GetAsync(requestUrl);

                //check to see if we have a successful response
                if (response.IsSuccessStatusCode)
                {
                    //set the viewmodel from the content in the response
                    var result = await response.Content.ReadAsAsync<VssJsonCollectionWrapper<List<SecureFile>>>(new MediaTypeFormatter[1] { new VssJsonMediaTypeFormatter() });

                    if (result != null)
                    {
                        return result.Value;
                    }
                }
            }

            return [];
        }

        #endregion

        #region Private

        /// <summary>
        /// Builds the API URL.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>System.String.</returns>
        private string BuildApiUrl(TeamProjectReference project)
        {
            var apiUrl = $"{ApiUrl}{project.Id}/_apis/distributedtask/securefiles";

            return apiUrl;
        }
        #endregion
    }
}
