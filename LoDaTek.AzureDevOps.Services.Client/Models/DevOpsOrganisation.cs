using LoDaTek.AzureDevOps.Services.Client.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// DevOps Organisation.
    /// </summary>
    public class DevOpsOrganisation
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; }

        /// <summary>
        /// Gets the pat.
        /// </summary>
        /// <value>The pat.</value>
        public string PAT { get; }

        /// <summary>
        /// Gets the type of the organisation.
        /// </summary>
        /// <value>The type of the organisation.</value>
        public ServerType OrganisationType { get; }

    }
}
