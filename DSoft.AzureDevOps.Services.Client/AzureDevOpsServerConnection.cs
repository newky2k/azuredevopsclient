using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.AzureDevOps.Services.Client
{
    /// <summary>
    /// On-Premise Azure DevOps Server Connection
    /// </summary>
    /// <seealso cref="DSoft.AzureDevOps.Services.Client.DevOpsConnectionBase" />
    public sealed class AzureDevOpsServerConnection : DevOpsConnectionBase
    {
        private string _instanceUrl;

        protected override string BaseUrl => _instanceUrl;

        internal override string FeedsApiUrl => $"{CommonUrl}/_apis/";

        internal override string CommonUrl => $"{BaseUrl}/{OrganisationName}/";

		internal override string NugetPackagesApiUrl => $"{BaseUrl}/{OrganisationName}/";


        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsServerConnection"/> class.
        /// </summary>
        /// <param name="instanceUrl">The main URL of the On-Pemise Azure DevOps Service instance</param>
        /// <param name="collectionName">Name of the collection</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public AzureDevOpsServerConnection(string instanceUrl, string collectionName, string personalAccessToken) : base(collectionName, personalAccessToken)
        {
            _instanceUrl = instanceUrl;
        }
    }
}
