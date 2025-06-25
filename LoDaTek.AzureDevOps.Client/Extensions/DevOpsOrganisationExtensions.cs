using Microsoft.VisualStudio.Services.Organization.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Extensions for DevOpsOrganisation
    /// </summary>
    public static class DevOpsOrganisationExtensions
    {
        /// <summary>
        /// Gets the URL components.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public static Dictionary<string, string> GetUrlComponents(this DevOpsOrganisation organisation)
        {
            var dict = new Dictionary<string, string>();

            var orgUrl = organisation.Url;

            if (orgUrl.EndsWith(@"/"))
                orgUrl = orgUrl.Substring(0, orgUrl.Length - 1);

            var index = orgUrl.LastIndexOf(@"/");

            var url = orgUrl.Substring(0, index);

            var name = orgUrl.Substring(index + 1);

            dict.Add("url", url);
            dict.Add("name", name);

            return dict;
        }
    }
}
