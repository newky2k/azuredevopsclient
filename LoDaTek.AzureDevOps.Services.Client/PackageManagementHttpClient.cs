using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LoDaTek.AzureDevOps.Services.Client.Bases;
using LoDaTek.AzureDevOps.Services.Client.Connections;
using LoDaTek.AzureDevOps.Services.Client.Enums;
using LoDaTek.AzureDevOps.Services.Client.Exceptions;
using LoDaTek.AzureDevOps.Services.Client.Models;

namespace LoDaTek.AzureDevOps.Services.Client
{
    /// <summary>
    /// HttpClient for Package Management
    /// Implements the <see cref="Bases.DevOpsHttpClientBase" />
    /// </summary>
    /// <seealso cref="Bases.DevOpsHttpClientBase" />
    public class PackageManagementHttpClient : DevOpsHttpClientBase
	{
        #region Abstract
        /// <summary>
        /// Gets the test URL.
        /// </summary>
        /// <value>The test URL.</value>
        internal override string TestUrl => $"{Connection.FeedsApiUrl}/packaging/feeds?api-version=5.0-preview.1";
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManagementHttpClient"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        internal PackageManagementHttpClient(DevOpsConnectionBase connection) : base(connection, ApiType.Nuget)
		{

		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Get nuget base path as an asynchronous operation.
        /// </summary>
        /// <param name="feedName">Name of the feed.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
        public async Task<string> GetNugetBasePathAsync(string feedName, string projectName = null)
        {
            using (var wclient = Client)
            {
                var url = GetNuGetUrl(feedName, projectName);
   
                var response = await wclient.GetAsync(url);

                //var queryResult = JsonConvert.DeserializeObject<ServiceDetails>(result);

                if (response.IsSuccessStatusCode)
                {

                    var json = await response.Content.ReadAsStringAsync();

                    //set the viewmodel from the content in the response
                    var queryResult = await response.Content.ReadFromJsonAsync<ServiceDetails>();

                    if (queryResult == null)
                        return null;

                    var queryResource = queryResult["SearchQueryService/3.0.0-beta"];
                    var packageBaseResource = queryResult["PackageBaseAddress/3.0.0"];
                    var feedDetails = queryResult["VssFeedId"];

                    if (queryResource == null || packageBaseResource == null)
                    {
                        return null;
                    }

                    var queryUrl = queryResource.Id;
                    var packageBaseUrl = packageBaseResource.Id;

                    return packageBaseUrl;
                }
            }

            return null;
        }

        /// <summary>
        /// Download nuget package as an asynchronous operation.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="packageVersion">The package version.</param>
        /// <param name="outPutFileName">Name of the out put file.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="LoDaTek.AzureDevOps.Services.Client.Exceptions.RequestFailureException">Unable to fetch packages</exception>
        public async Task DownloadNugetPackageAsync(string baseUrl, string packageName, string packageVersion, string outPutFileName)
        {
            var tries = 0;
            var maxTries = 10;

            using (var wclient = Client)
            {
                var packageUrl  = $"{baseUrl}/{packageName}/{packageVersion}/{packageName}.{packageVersion}.nupkg";

                while (true)
                {
                    try
                    {
                        var response = await wclient.GetAsync(new Uri(packageUrl, UriKind.Absolute));

                        response.EnsureSuccessStatusCode();

                        var data = await response.Content.ReadAsByteArrayAsync();
#if NETSTANDARD2_0
                        File.WriteAllBytes(outPutFileName, data);
#else
                        await File.WriteAllBytesAsync(outPutFileName, data);
#endif
                        return;
                    }
                    catch
                    {
                        if (tries > maxTries)
                            throw;

                        await Task.Delay(5000);

                        tries++;
                    }
                }

            }

            throw new RequestFailureException("Unable to fetch packages");
        }


#endregion

        #region Private Methods
        /// <summary>
        /// Gets the nu get URL.
        /// </summary>
        /// <param name="feedName">Name of the feed.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">Unexpected connection type</exception>
        private string GetNuGetUrl(string feedName,string projectName = null)
		{
			if (Connection is AzureDevOpsCloudConnection)
			{
				var prefix = $"https://pkgs.dev.azure.com/";
				var suffix = $"/nuget/v3/index.json";

				var projectComponent = string.Empty;

				if (!string.IsNullOrWhiteSpace(projectName))
				{
					projectComponent = $"{projectName}/";
				}

				return $"{prefix}{Connection.OrganisationName}/{projectComponent}_packaging/{feedName}{suffix}";
			}
			else if (Connection is AzureDevOpsServerConnection)
			{
				var prefix = Connection.CommonUrl;
				var suffix = $"/nuget/v3/index.json";

				var projectComponent = string.Empty;

				if (!string.IsNullOrWhiteSpace(projectName))
				{
					projectComponent = $"{projectName}/";
				}

				return $"{prefix}/{projectComponent}_packaging/{feedName}{suffix}";
			}
			else
			{
				throw new Exception("Unexpected connection type");
			}

		}

        #endregion
    }
}
