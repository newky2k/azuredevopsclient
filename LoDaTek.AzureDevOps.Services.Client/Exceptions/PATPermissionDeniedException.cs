using LoDaTek.AzureDevOps.Services.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client.Exceptions
{
    /// <summary>
    /// PAT Permission Denied Exception.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    public class PATPermissionDeniedException : Exception
    {
        private string _area;
        private string _type;
        private DevOpsOrganisation _organisation;

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value>The message.</value>
        public override string Message => $"{base.Message} - {_area} - {_type}";

        /// <summary>
        /// Initializes a new instance of the <see cref="PATPermissionDeniedException"/> class.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        /// <param name="area">The area.</param>
        /// <param name="type">The type.</param>
        /// <param name="innerException">The inner exception.</param>
        public PATPermissionDeniedException(DevOpsOrganisation organisation, string area, string type, Exception innerException) : base("Permission Denied", innerException)
        {
            _area = area;
            _type = type;
            _organisation = organisation;
        }
    }
}
