using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.AzureDevOps.Services.Client
{
    /// <summary>
    /// Azure DevOps Cloud Services connection
    /// </summary>
    /// <seealso cref="DSoft.AzureDevOps.Services.Client.DevOpsConnectionBase" />
    public sealed class AzureDevOpsCloudConnection : DevOpsConnectionBase
    {
        protected override string BaseUrl => @"https://dev.azure.com/";

        internal override string FeedsApiUrl => $"https://feeds.dev.azure.com/{OrganisationName}/_apis/";

        internal override string CommonUrl => $"{BaseUrl}{OrganisationName}/";

		internal override string NugetPackagesApiUrl => $"https://pkgs.dev.azure.com/{OrganisationName}/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsCloudConnection"/> class.
        /// </summary>
        /// <param name="organisationName">Name of the organisation.</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public AzureDevOpsCloudConnection(string organisationName, string personalAccessToken) : base(organisationName, personalAccessToken)
        {

        }
    }
}
