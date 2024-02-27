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
        #region Properties
        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        protected override string BaseUrl => @"https://dev.azure.com/";

        /// <summary>
        /// Gets the feeds API URL.
        /// </summary>
        /// <value>The feeds API URL.</value>
        internal override string FeedsApiUrl => $"https://feeds.dev.azure.com/{OrganisationName}/_apis/";

        /// <summary>
        /// Gets the common URL.
        /// </summary>
        /// <value>The common URL.</value>
        internal override string CommonUrl => $"{BaseUrl}{OrganisationName}/";

        /// <summary>
        /// Gets the nuget packages API URL.
        /// </summary>
        /// <value>The nuget packages API URL.</value>
        internal override string NugetPackagesApiUrl => $"https://pkgs.dev.azure.com/{OrganisationName}/";


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsCloudConnection"/> class.
        /// </summary>
        /// <param name="organisationName">Name of the organisation.</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public AzureDevOpsCloudConnection(string organisationName, string personalAccessToken) : base(organisationName, personalAccessToken)
        {

        }

        #endregion
    }
}

