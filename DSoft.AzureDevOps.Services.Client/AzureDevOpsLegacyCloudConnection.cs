using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.AzureDevOps.Services.Client
{
    /// <summary>
    /// Azure DevOps Cloud Services connection for organisations with VisualStudio.com urls
    /// Implements the <see cref="DSoft.AzureDevOps.Services.Client.DevOpsConnectionBase" />
    /// </summary>
    /// <seealso cref="DSoft.AzureDevOps.Services.Client.DevOpsConnectionBase" />
    public sealed class AzureDevOpsLegacyCloudConnection : DevOpsConnectionBase
    {
        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        protected override string BaseUrl => $"https://{OrganisationName}.visualstudio.com/";

        /// <summary>
        /// Gets the common URL.
        /// </summary>
        /// <value>The common URL.</value>
        internal override string CommonUrl => BaseUrl;

        /// <summary>
        /// Gets the feeds API URL.
        /// </summary>
        /// <value>The feeds API URL.</value>
        internal override string FeedsApiUrl => $"https://{OrganisationName}.feeds.visualstudio.com/_apis/";

        /// <summary>
        /// Gets the nuget packages API URL.
        /// </summary>
        /// <value>The nuget packages API URL.</value>
        internal override string NugetPackagesApiUrl => $"https://{OrganisationName}.pkgs.visualstudio.com/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsLegacyCloudConnection"/> class.
        /// </summary>
        /// <param name="organisationName">Name of the organisation.</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public AzureDevOpsLegacyCloudConnection(string organisationName, string personalAccessToken) : base(organisationName, personalAccessToken)
        {

        }
    }
}
