using System;
using System.Collections.Generic;
using System.Text;
using LoDaTek.AzureDevOps.Services.Client.Bases;
using LoDaTek.AzureDevOps.Services.Client.Enums;

namespace LoDaTek.AzureDevOps.Services.Client.Connections
{
    /// <summary>
    /// Azure DevOps Cloud Services connection
    /// </summary>
    /// <seealso cref="DevOpsConnectionBase" />
    public sealed class AzureDevOpsCloudConnection : DevOpsConnectionBase
    {

        #region Properties

        /// <summary>
        /// Gets the type of the server.
        /// </summary>
        /// <value>The type of the organisation.</value>
        public override ServerType ServerType => ServerType.AzureDevOps;

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        protected override string BaseUrl => @"https://dev.azure.com/";

        /// <summary>
        /// Gets the feeds API URL.
        /// </summary>
        /// <value>The feeds API URL.</value>
        public override string FeedsApiUrl => $"https://feeds.dev.azure.com/{OrganisationName}/_apis/";

        /// <summary>
        /// Gets the common URL.
        /// </summary>
        /// <value>The common URL.</value>
        public override string CommonUrl => $"{BaseUrl}{OrganisationName}/";

        /// <summary>
        /// Gets the nuget packages API URL.
        /// </summary>
        /// <value>The nuget packages API URL.</value>
        public override string NugetPackagesApiUrl => $"https://pkgs.dev.azure.com/{OrganisationName}/";

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

