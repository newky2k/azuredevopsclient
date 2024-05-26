using System;
using System.Collections.Generic;
using System.Text;
using LoDaTek.AzureDevOps.Services.Client.Bases;

namespace LoDaTek.AzureDevOps.Services.Client.Connections
{
    /// <summary>
    /// On-Premise Azure DevOps Server Connection
    /// </summary>
    /// <seealso cref="DevOpsConnectionBase" />
    public sealed class AzureDevOpsServerConnection : DevOpsConnectionBase
    {
        #region Fields

        /// <summary>
        /// The instance URL
        /// </summary>
        private string _instanceUrl;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        protected override string BaseUrl => _instanceUrl;

        /// <summary>
        /// Gets the feeds API URL.
        /// </summary>
        /// <value>The feeds API URL.</value>
        internal override string FeedsApiUrl => $"{CommonUrl}/_apis/";

        /// <summary>
        /// Gets the common URL.
        /// </summary>
        /// <value>The common URL.</value>
        internal override string CommonUrl => $"{BaseUrl}/{OrganisationName}/";

        /// <summary>
        /// Gets the nuget packages API URL.
        /// </summary>
        /// <value>The nuget packages API URL.</value>
        internal override string NugetPackagesApiUrl => $"{BaseUrl}/{OrganisationName}/";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsServerConnection" /> class.
        /// </summary>
        /// <param name="instanceUrl">The main URL of the On-Pemise Azure DevOps Service instance</param>
        /// <param name="collectionName">Name of the collection</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public AzureDevOpsServerConnection(string instanceUrl, string collectionName, string personalAccessToken) : base(collectionName, personalAccessToken)
        {
            _instanceUrl = instanceUrl;
        }

        #endregion
    }
}
